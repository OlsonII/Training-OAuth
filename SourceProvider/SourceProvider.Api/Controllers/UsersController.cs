using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using SourceProvider.Domain.Utils;
using SourceProvider.Repository.Repositories;

namespace SourceProvider.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{

    private readonly ILogger<UsersController> _logger;
    private readonly IdentityProviderRepository _identityProviderRepository;

    public UsersController(ILogger<UsersController> logger)
    {
        _logger = logger;
        _identityProviderRepository = new IdentityProviderRepository();
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromHeader(Name = "Authorization")] string authorizationHeader)
    {
        if (authorizationHeader == string.Empty || authorizationHeader.Contains("Basic"))
            return Problem("Invalid Credentials", statusCode: 401);

        var accessToken = authorizationHeader.Split(' ')[1];
        var idToken = await _identityProviderRepository.Authorize(accessToken);
        
        if (idToken == string.Empty)
            return Problem("Access denied", statusCode: 403);

        var user = new JwtSecurityToken(idToken).GetTokenOwner();
        
        if (user == null)
            return Problem("Access denied", statusCode: 403);
        
        return Ok(user);
    }
}