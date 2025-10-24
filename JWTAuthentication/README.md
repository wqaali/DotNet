# JWT Authentication API

A complete JWT (JSON Web Token) authentication system built with ASP.NET Core 8.0, featuring token generation, validation, and refresh token functionality.

## 🚀 Features

- **JWT Token Generation** - Create secure access tokens with user claims
- **Refresh Token System** - Cryptographically secure token renewal mechanism
- **Token Validation** - Comprehensive JWT validation with configurable parameters
- **Swagger Integration** - Complete API documentation with interactive testing
- **Claims-based Authorization** - User ID and email claims support
- **Debug Logging** - Detailed authentication event logging

## 🏗️ Project Structure

```
JWTAuthentication/
├── Authentication/
│   ├── AuthBody.cs              # Token response model
│   ├── IJwtAuthentication.cs    # Authentication interface
│   ├── JwtAuthentication.cs     # Core JWT service
│   └── UserModel.cs             # User input model
├── Controllers/
│   └── AuthenticationController.cs  # API endpoints
├── Properties/
│   └── launchSettings.json      # Launch configuration
├── Program.cs                   # Application startup
├── appsettings.json            # Configuration settings
└── JWTAuthentication.csproj    # Project dependencies
```

## 📦 Dependencies

- **Microsoft.AspNetCore.Authentication.JwtBearer** (8.0.19) - JWT authentication middleware
- **System.IdentityModel.Tokens.Jwt** (8.14.0) - JWT token handling
- **Swashbuckle.AspNetCore** (9.0.3) - API documentation and testing

## 🔧 Configuration

### JWT Settings
The JWT signing key is configured in `appsettings.json`:

```json
{
  "JWTPrivateKey": "r7k8DqM3JfT1uWq9xE5sVgZnQhLc2yAb7oHkRjNdUmPwXyZtCg"
}
```

### Token Configuration
- **Algorithm**: HMAC SHA256
- **Expiration**: 1 year (configurable)
- **Claims**: User ID and Email
- **Validation**: Signing key validation enabled

## 🌐 API Endpoints

### 1. Test Endpoint
```http
GET /Authentication/TestApi
```
- **Description**: Public test endpoint (no authentication required)
- **Response**: `"Hello"`

### 2. User Login
```http
POST /Authentication/login
Content-Type: application/json

{
  "userName": "string",
  "password": "string",
  "email": "string"
}
```
- **Description**: Authenticate user and return JWT tokens
- **Response**: 
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "base64EncodedRandomString"
}
```

### 3. Refresh Token
```http
POST /Authentication/RefreshToken
Content-Type: application/json

{
  "accessToken": "expiredJwtToken",
  "refreshToken": "validRefreshToken"
}
```
- **Description**: Generate new tokens using refresh token
- **Response**: New access and refresh tokens

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 or VS Code
- Git

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd JWTAuthentication
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```

4. **Access Swagger UI**
   - Navigate to `https://localhost:5074/swagger`
   - Test the API endpoints interactively

### Development Setup

1. **Update JWT Key** (Optional)
   - Modify `JWTPrivateKey` in `appsettings.json`
   - Use a secure, randomly generated key in production

2. **Configure HTTPS**
   - The application runs on HTTPS by default
   - Update `launchSettings.json` for custom ports

## 🔒 Security Features

- **JWT Bearer Authentication** - Industry-standard token-based authentication
- **Symmetric Key Signing** - Secure token signing with HMAC SHA256
- **Token Validation** - Comprehensive validation of incoming tokens
- **HTTPS Enforcement** - Automatic HTTPS redirection
- **Claims-based Authorization** - Flexible user identification system

## 🧪 Testing

### Using Swagger UI
1. Open `https://localhost:5074/swagger`
2. Click "Authorize" button
3. Enter JWT token: `Bearer <your-jwt-token>`
4. Test protected endpoints

### Using HTTP Client
```http
POST https://localhost:5074/Authentication/login
Content-Type: application/json

{
  "userName": "testuser",
  "password": "testpass",
  "email": "test@example.com"
}
```

## 📝 Current Implementation Notes

### Simplified Authentication
- **Hardcoded User**: Currently uses hardcoded user ID ("125487") and email ("abc@gmail.com")
- **No Database**: Refresh tokens are not persisted to a database
- **No User Validation**: No actual user credential validation against a user store

### Production Readiness
To make this production-ready, consider implementing:
- Database integration for user management
- Password hashing and validation
- Refresh token persistence
- User registration and management
- Role-based authorization
- Token blacklisting for logout

## 🔧 Customization

### Token Expiration
Modify token expiration in `JwtAuthentication.cs`:
```csharp
Expires = validTill ?? DateTime.Now.AddMinutes(15), // 15 minutes
```

### Additional Claims
Add custom claims in `JwtAuthentication.cs`:
```csharp
Subject = new ClaimsIdentity(new[] { 
    new Claim(ClaimTypes.Name, userId), 
    new Claim("emailId", email),
    new Claim("role", "admin") // Custom claim
}),
```

### Validation Parameters
Configure token validation in `Program.cs`:
```csharp
ValidateIssuer = true,
ValidIssuer = "your-issuer",
ValidateAudience = true,
ValidAudience = "your-audience"
```

## 📚 Learning Resources

- [JWT.io](https://jwt.io/) - JWT token decoder and information
- [ASP.NET Core Authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/)
- [JWT Bearer Authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/jwt-authn)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request
---

**Built with ❤️ using ASP.NET Core 8.0**
