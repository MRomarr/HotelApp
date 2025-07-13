using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using HotelApp.DTOs.Auth;
using HotelApp.Interface.Services;
using HotelApp.Models;
using HotelApp.Setting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HotelApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWTSetting _jwt;

        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JWTSetting> jwt, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _roleManager = roleManager;
        }

        public async Task<AuthDto> RegisterAsync(RegisterDto registerDto)
        {
            if(await _userManager.FindByEmailAsync(registerDto.Email) is not null)
            {
                return new AuthDto
                {
                    Message = "Email already exists."
                };
            }
            if (await _userManager.FindByNameAsync(registerDto.UserName) is not null)
            {
                return new AuthDto
                {
                    Message = "Username already exists."
                };
            }
            var user = new ApplicationUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                Address = registerDto.Address,
                Name = registerDto.Name
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return new AuthDto
                {
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            await _userManager.AddToRoleAsync(user, "User");
            // Generate JWT token
            var jwtToken = await CreateJwtToken(user);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            // Return the authentication DTO with the token
            return new AuthDto
            {
                IsAuthenticated = true,
                Token = tokenString,
                TokenExpiration = jwtToken.ValidTo,
                Role = (List<string>)await _userManager.GetRolesAsync(user),
                Message = "User registered successfully."
            };

        }
        public async Task<AuthDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user is null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return new AuthDto { Message = "Email or password is incorrect." };

            var token = await CreateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);

            var authDto = new AuthDto
            {
                Message = "Login successful!",
                IsAuthenticated = true,
                Role = roles.ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };

            return authDto;
        }


        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            // Gather user claims and roles
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role));

            // Build claims list
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim("uid", user.Id)
            };
            claims.AddRange(userClaims);
            claims.AddRange(roleClaims);

            // Validate _jwt configuration
            if (_jwt == null)
                throw new InvalidOperationException("JWT configuration is missing.");

            var key = _jwt.Key ?? throw new InvalidOperationException("JWT key is missing.");
            var issuer = _jwt.Issuer ?? throw new InvalidOperationException("JWT issuer is missing.");
            var audience = _jwt.Audience ?? throw new InvalidOperationException("JWT audience is missing.");

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            // Create the JWT token
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.TokenExpirationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public async Task<string> AddRoleAsync(AddRoleDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user is null)
                return "Invalid user ID";

            if (!await _roleManager.RoleExistsAsync(model.RoleName))
                return "Invalid role";

            if (await _userManager.IsInRoleAsync(user, model.RoleName))
                return "User already assigned to this role";

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);
            return result.Succeeded ? string.Empty : "Something went wrong";
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return false;
            // Generate password reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token);
            var resetLink = $"https://localhost:7219/api/auth/reset-password?userId={user.Id}&token={encodedToken}";

            // Send email with reset link

            //await _emailService.SendEmailAsync(
            //user.Email,
            //"Reset your password",
            //$"Click <a href='{resetLink}'>here</a> to reset your password.");
            return true;
        }

        public async Task<bool> ResetPasswordAsync(string userId, string token, string NewPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }
            var result = await _userManager.ResetPasswordAsync(user, token, NewPassword);

            return result.Succeeded;
        }
    }
}
