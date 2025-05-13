using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using STGeorgeReservation.BaseTypes;
using STGeorgeReservation.Contracts.Interfaces.Users;
using STGeorgeReservation.Data;
using STGeorgeReservation.DTOs.ResponseDTOs;
using STGeorgeReservation.Helpers;
using STGeorgeReservation.Localization;
using STGeorgeReservation.Models;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TheProfessionals.Enums;

namespace STGeorgeReservation.Contracts.Services
{
    public class AuthService : IAuth
    {
        private readonly ApplicationDbContext _ApplicationDbContext;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<aspnetroles> _roleManager;
        private readonly JWT _jwt;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IWebHostEnvironment _env;
        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<aspnetroles> roleManager, IOptions<JWT> jwt, ApplicationDbContext applicationDbContext, IWebHostEnvironment env,  IConfiguration Configuration)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _roleManager = roleManager;
            _applicationDbContext = applicationDbContext;
            _env = env;
            _configuration = Configuration;

        }
        public static class PasswordEncryption
        {
            private static readonly string EncryptionKey = "YourEncryptionKeyHere";

            public static string Encrypt(string plainText)
            {
                using var aes = Aes.Create();
                var key = Encoding.UTF8.GetBytes(EncryptionKey.PadRight(32));
                aes.Key = key;
                aes.IV = new byte[16];
                using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using var ms = new MemoryStream();
                using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }
        public async Task<OperationResult<AuthModel>> RegisterAsync(RegisterModel model)
        {

            OperationResult<AuthModel> result = new OperationResult<AuthModel>();

            if (await _userManager.FindByEmailAsync(model.Email) is not null)
            {
                result.StatusCode = HttpStatusCode.Conflict;
                result.ErrorMessageKey = LocalizationKeys.EmailRegisteredBefore;
                return result;
            }
            if (await _userManager.FindByNameAsync(model.UserName) is not null)
            {
                result.StatusCode = HttpStatusCode.NotFound;
                result.ErrorMessageKey = "UserName Already Registered";
                return result;
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.EncryptedPassword); // ✅ Correct

            var createUserResult = await _userManager.CreateAsync(user);


            var jwtSecurityToken = await CreateJwtToken(user);

  
       
            await _applicationDbContext.SaveChangesAsync();

            result.StatusCode = HttpStatusCode.OK;


            return new AuthModel
            {
                UserID = user.Id,
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                //  Roles = rolesList.FirstOrDefault(),
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = user.UserName
            };




        }


        public async Task<OperationResult<AuthModel>> GetTokenAsync(TokenRequestModel model)
        {
            OperationResult<AuthModel> result = new OperationResult<AuthModel>();
            var authModel = new AuthModel();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return new OperationResult<AuthModel>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessageKey = "Email or Password is incorrect",
                    Data = null
                };
            }


            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            // Get roles and associated permissions
            var permissionsList = new List<UserProfileRolePermissionsResponseDTO>();

            foreach (var roleName in rolesList)
            {
                var role = await _applicationDbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
                if (role != null)
                {
                    // Fetch the role claims for permissions
                    var roleClaims = _applicationDbContext.RoleClaims
                                        .Where(a => a.RoleId == role.Id)
                                        .Select(a => a.ClaimValue)
                                        .ToList();

                    foreach (var claim in roleClaims)
                    {
                        // Try to parse the claim value into the Permission enum
                        if (Enum.TryParse(typeof(Permissions), claim, out var permissionEnumValue))
                        {
                            // Cast the parsed value to the Permission enum
                            var permission = (Permissions)permissionEnumValue;

                            // Add the mapped permission directly to the permissions list
                            permissionsList.Add(new UserProfileRolePermissionsResponseDTO
                            {
                                NameEn = permission.ToString(), // Enum name (e.g., "Read", "Write")
                                ClaimValue = claim.ToString()
                            });
                        }
                    }
                }
            }

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.UserName = user.UserName;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.PermissionsList = permissionsList; // Updated to include roles with permissions
            authModel.UserID = user.Id;
            //authModel.EmployeeType = user.EmployeeType;
            authModel.IsSuperAdmin = user.IsSuperAdmin;
            //user.OtpCode = otpCode;
            //user.OtpGenerationDate = DateTime.Now;

            await _applicationDbContext.SaveChangesAsync();

            result.Data = authModel;
            result.StatusCode = HttpStatusCode.OK;
            return result;
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Subject: User's username
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique token ID
        new Claim(JwtRegisteredClaimNames.Email, user.Email), // User's email address
        new Claim(JwtRegisteredClaimNames.Sid, user.Id), // User ID in "sid"
        new Claim("Userid", user.Id) // Custom claim for User ID
    }

            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(4),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

    }
}


