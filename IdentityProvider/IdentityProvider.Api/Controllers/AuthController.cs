using System.IdentityModel.Tokens.Jwt;
using IdentityProvider.Domain.Models;
using IdentityProvider.Domain.Utils;
using IdentityProvider.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProvider.Api.Controllers;

[ApiController]
[Route("[Controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthenticationRepository _authenticationRepository;

    public AuthController()
    {
        _authenticationRepository = new AuthenticationRepository();
    }

    [HttpPost]
    public IActionResult Auth([FromHeader(Name = "Authorization")] string authorization)
    {
        if (authorization == string.Empty)
            return Problem("Invalid Credentials", statusCode: 401);

        if (authorization.Contains("Basic"))
            return ExecuteBasicAuthentication(authorization);

        return ExecuteAccessTokenAuthorization(authorization);
    }
    
    [HttpPut]
    public IActionResult RegisterAccessToken([FromHeader(Name = "Authorization")] string authorizationHeader)
    {
        if (authorizationHeader == string.Empty)
            return Problem("Invalid Credentials", statusCode: 401);

        if (authorizationHeader.Contains("Basic"))
            return GenerateAccessToken(authorizationHeader);

        return Problem(statusCode: 401);
    }

    private IActionResult ExecuteBasicAuthentication(string authorizationHeader)
    {
        var token = authorizationHeader.Split(' ')[1];
        var username = Base64Utils.GetUsernameFromBase64Token(token);
        var authentication = _authenticationRepository.GetAuthentication(username) ?? new Authentication(username);
        
        return Ok(authentication);
    }

    private IActionResult ExecuteAccessTokenAuthorization(string authorizationHeader)
    {
        var token = authorizationHeader.Split(' ')[1];
        var username = new JwtSecurityToken(token).GetTokenOwner();
        var authentication = _authenticationRepository.GetAuthentication(username);

        if (authentication == null)
            return Problem("Invalid Token", statusCode: 403);

        var isValidToken = authentication.IsValidAccessToken(token);
        if (!isValidToken)
            return Problem(statusCode: 403);

        var response = new
        {
            authentication.IdToken
        };
        return Ok(response);
    }

    private IActionResult GenerateAccessToken(string authorizationHeader)
    {
        var token = authorizationHeader.Split(' ')[1];
        var username = Base64Utils.GetUsernameFromBase64Token(token);
        var authentication = _authenticationRepository.GetAuthentication(username);
        if (authentication == null || authentication.IsFirstAuth())
            return Problem(statusCode: 401);
        
        var accessToken = authentication.RegisterAccessToken(username);
        _authenticationRepository.SaveAuthentication(authentication);
        var response = new
        {
            accessToken
        };
        return Ok(response);
    }
    
    [HttpDelete]
    public IActionResult RevokeAccessToken([FromHeader(Name = "Authorization")] string authorizationHeader)
    {
        if (authorizationHeader == string.Empty || authorizationHeader.Contains("Basic"))
            return Problem("Invalid Credentials", statusCode: 401);

        var token = authorizationHeader.Split(' ')[1];
        var username = new JwtSecurityToken(token).GetTokenOwner();
        var authentication = _authenticationRepository.GetAuthentication(username);
        authentication?.RevokeAccessToken(token);
        _authenticationRepository.SaveAuthentication(authentication);
        return Ok();
    }
}