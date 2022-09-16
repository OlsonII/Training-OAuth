# OAuth Activity

## Purpose
The main objective of this activity is to execute the practice on the topics learned in the previous class.

## Requirements
* .Net 6.0: https://dotnet.microsoft.com/en-us/download

## How to run
In this repository you will find two projects, every project has an *.Api* folder, into this folder you need type the next command to run the application.

```bash
dotnet run
```

And you will see the next logs. 
In this logs you can find the https and http endpoints
```bash
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7271
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5047
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\Users\nrosado\RiderProjects\IdentityProvider\IdentityProvider.Api\
```

# Identity Provider

## URLs
* https://localhost:7271
* http://localhost:5047

## Endpoints

### Authenticate
This endpoint allow you authenticate and authorize access tokens. To authenticate and obtain your tokens please use scheme Basic with your credentials and if you want authorize an access token please use Bearer with your access token.

#### Path: /auth
#### Method: Post
#### Request
* Required Authorization header
### Response
```json
{
  "idToken": "eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJ1c2VybmFtZSI6Im5pbHNvbi5yb3NhZG8ubWFzaXYiLCJsb2NhdGlvbiI6IkNvbG9tYmlhIiwid29yayI6IkRldmVsb3BlciIsImV4cCI6MTY2MzkwODE1NH0.",
  "accesses": [
    {
      "accessToken": "eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJ1c2VybmFtZSI6Im5pbHNvbi5yb3NhZG8ubWFzaXYiLCJleHAiOjE2NjM5MDkwODJ9.",
      "allowed": false
    },
    {
      "accessToken": "eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJ1c2VybmFtZSI6Im5pbHNvbi5yb3NhZG8ubWFzaXYiLCJleHAiOjE2NjM5MDgxNTR9.",
      "allowed": true
    }
  ]
}
```

### Generate New Access Token
This endpoint allow you generate a new access token using your basic credentials

#### Path: /auth
#### Method: Put
#### Request
* Required Authorization header with basic credentials
#### Response
```json
{
  "accessToken": "eyJhbGciOiJub25lIiwidHlwIjoiSldUIn0.eyJ1c2VybmFtZSI6Im5pbHNvbi5yb3NhZG8ubWFzaXYiLCJleHAiOjE2NjM5Mzg0ODZ9."
}
```

### Revoke Access Token
This endpoint allow you revoke access token

#### Path: /auth
#### Method: Delete
#### Request
* Required Authorization header with **access token to revoke**

# Source Provider

## URLs
* https://localhost:7134
* http://localhost:5138

## Endpoints

### Get User Information
This endpoint allow you get the user information using an access token

#### Path: /users
#### Method: Get
#### Request
* Required Authorization header with **access token**
#### Response
```json
{
  "username": "nilson.rosado.masiv",
  "location": "Colombia",
  "work": "Developer"
}
```