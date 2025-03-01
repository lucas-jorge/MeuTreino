using System;
using System.Text;
using API.Entities;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace API.Services
{
    public static class TokenService
    {
        public static byte[] Secret { get { return Encoding.ASCII.GetBytes("7f72cd430cb04f3a9e2c03039c03ac09"); } }

        public static string GenerateToken(TB_USUARIO user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Sid, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Nome.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(8), // Expira em 8 horas
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Secret), SecurityAlgorithms.HmacSha256Signature)

            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static bool GetUserNameFromToken(string token, out string Name)
        {
            if (GetClaimsPrincipal(token, out ClaimsPrincipal Claims))
            {
                Name = Claims.Identity.Name;
                return true;
            }
            else
            {
                Name = null;
                return false;
            }
        }

        public static bool GetUserIdFromToken(string token, out Int64? Id)
        {
            if (GetClaimsPrincipal(token, out ClaimsPrincipal Claims))
            {
                Id = Convert.ToInt64(Claims.FindFirst(ClaimTypes.Sid).Value);
                return true;
            }
            else
            {
                Id = null;
                return false;
            }
        }

        public static bool GetUserTypeFromToken(string token, out Int16? Type)
        {
            if (GetClaimsPrincipal(token, out ClaimsPrincipal Claims))
            {
                Type = Convert.ToInt16(Claims.FindFirst(ClaimTypes.Role).Value);
                return true;
            }
            else
            {
                Type = null;
                return false;
            }
        }

        private static bool GetClaimsPrincipal(string token, out ClaimsPrincipal Claims)
        {
            try
            {
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                TokenValidationParameters validations = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Secret),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                Claims = handler.ValidateToken(token, validations, out var tokenSecure);
                return true;
            }
            catch
            {
                Claims = null;
                return false;
            }
        }
    }
}
