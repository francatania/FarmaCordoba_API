using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FarmaceuticaBack.Services.Utils
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string username)
        {

            // Configurar la clave de firma
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Configurar el token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],       // Quién emite el token
                audience: _configuration["Jwt:Audience"],   // Quién puede consumir el token
                expires: DateTime.UtcNow.AddHours(1),       // Tiempo de expiración
                signingCredentials: credentials             // Credenciales para firmar el token
            );

            // Devolver el token como string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
