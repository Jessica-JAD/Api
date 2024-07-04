using Contracts;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using RESTApi.Utiles;
using System;

namespace RESTApi.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoggerManager _logger;

        public LoginController(ILoggerManager logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Autenticación
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetTokenByPassword([FromBody] LoginDTO login)
        {
            try
            {
                if (login == null)
                {
                    _logger.LogError("El objeto Login enviado desde el cliente es nulo.");
                    return BadRequest("Login nulo");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Objeto inválido enviado desde el cliente.");
                    return BadRequest("Modelo del objecto inválido ");
                }

                if (General.CheckPassword(General.EncriptarCadena(login.Password)))
                {
                    var token = General.GetToken(); // TokenGenerator.GenerateTokenJwt(loginDTO.Username);
                    _logger.LogInfo($"Token devuelto");
                    return Ok(token);
                }
                else
                {
                    _logger.LogError($"Contraseña incorrecta para obtener token.");
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en proceso de Login: {ex.Message}");
                return StatusCode(500, $"Error interno del servidor {ex.Message}");
            }
        }

    }
}
