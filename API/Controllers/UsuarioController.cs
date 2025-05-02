using System;
using System.Linq;
using API.Entities;
using API.Services;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using API.DTOs; // Added using for DTOs
// Removed static using for BCrypt

namespace API.Controllers
{
    /// <summary>
    /// Controller com os métodos de gerenciamento dos usuários
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        public AppDbContext context { get; set; }

        public UsuarioController(AppDbContext context)
        {
            this.context = context;
        }

        #region Login
        /// <summary>
        /// Método Anônimo que realiza a verificação do usuário para realizar o login no sistema.
        /// Valida se o usuário não está bloqueado/excluído.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto) // Use LoginDto
        {
            if (!ModelState.IsValid) // Check model state based on DTO validation
            {
                return BadRequest(ModelState);
            }

            try
            {
                TB_USUARIO TB_USUARIO = await context.USUARIO.Where(x => x.Nome.Equals(loginDto.Nome)) // Use DTO property
                                                                .AsNoTracking()
                                                                .FirstOrDefaultAsync(); // Fetch user by name only

                    if (TB_USUARIO == null)
                        return BadRequest("Usuário ou senha inválidos!"); // Generic message

                    // Verify password hash
                    if (!BCrypt.Net.BCrypt.Verify(loginDto.Senha, TB_USUARIO.Senha)) // Use DTO property
                        return BadRequest("Usuário ou senha inválidos!"); // Generic message

                    switch (TB_USUARIO.Status)
                    {
                        case TB_USUARIO.EStatus.Inativo: return BadRequest("Usuário inativo!");
                        case TB_USUARIO.EStatus.Excluido: return BadRequest("Usuário excluído!");
                    }

                    // Generate token only after successful verification
                    var token = TokenService.GenerateToken(TB_USUARIO);

                    return Ok(new
                    {
                        TB_USUARIO.Id,
                        TB_USUARIO.Nome,
                        Token = token
                    });
            } // End try block
            // TODO: Implement proper logging
            catch (Exception ex)
            {
                return BadRequest("Erro ao realizar o login: " + ex.Message);
            }
            // Removed else block, handled by initial ModelState check
        }
        #endregion

        #region Get
        [HttpGet]
        public async Task<IActionResult> Get(Int64? IdUsuario)
        {
            if (ModelState.IsValid)
            {
                object retorno = null;

                if (IdUsuario != null)
                {
                    retorno = context.USUARIO.Where(x => x.Id.Equals(IdUsuario))
                                                .AsNoTracking()
                                                .FirstOrDefault();

                    if (retorno == null)
                        return BadRequest("Usuário não encontrado!");
                    else
                        return Ok(retorno);
                }
                else
                {
                    // Consider adding pagination or limiting results for large datasets
                    return Ok(await context.USUARIO
                                           .AsNoTracking()
                                           .ToListAsync()); // Use Async version
                }
            }
            else
                return BadRequest(ModelState); // Return BadRequest directly
        }
        #endregion

        #region Put
        // Update route to include ID
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] UpdateUserDto updateUserDto) // Use UpdateUserDto and get id from route
        {
            if (!ModelState.IsValid) // Check model state based on DTO validation
            {
                return BadRequest(ModelState);
            }

            try
            {
                TB_USUARIO TB_USUARIO = await context.USUARIO.Where(x => x.Id.Equals(id)) // Use id from route
                                                                .FirstOrDefaultAsync();

                if (TB_USUARIO == null)
                    return NotFound("Usuário não encontrado com o ID informado!"); // Use NotFound for missing resource

                // Update properties only if they are provided in the DTO
                if (!string.IsNullOrEmpty(updateUserDto.Nome))
                {
                    TB_USUARIO.Nome = updateUserDto.Nome;
                }

                // Hash password only if provided
                if (!string.IsNullOrEmpty(updateUserDto.Senha))
                {
                    TB_USUARIO.Senha = BCrypt.Net.BCrypt.HashPassword(updateUserDto.Senha); // Use fully qualified name
                }

                // Update status only if provided
                if (updateUserDto.Status.HasValue)
                {
                    TB_USUARIO.Status = updateUserDto.Status.Value;
                }

                await context.SaveChangesAsync();

                return Ok("Dados atualizados com sucesso");
            } // End try block
            // TODO: Implement proper logging
            catch (Exception ex)
            {
                return BadRequest("Erro ao atualizar os dados: " + ex.Message);
            }
            // Removed else block, handled by initial ModelState check
        }
        #endregion

        #region Verifica se está autenticado
        [HttpPost("Check")]
        public IActionResult Check()
        {
            return Ok("Usuário autenticado!");
        }
        #endregion

        #region Register
        /// <summary>
        /// Método Anônimo para registrar um novo usuário.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto) // Use RegisterDto
        {
            if (!ModelState.IsValid) // Check model state based on DTO validation
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Check if user already exists
                bool userExists = await context.USUARIO.AnyAsync(x => x.Nome.Equals(registerDto.Nome)); // Use DTO property
                if (userExists)
                {
                    return BadRequest("Nome de usuário já está em uso.");
                }

                // Hash the password
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Senha); // Use DTO property

                var newUser = new TB_USUARIO
                {
                    Nome = registerDto.Nome, // Use DTO property
                    Senha = hashedPassword,
                    Status = TB_USUARIO.EStatus.Ativo // Default status
                    // Map other properties from registerDto if they exist
                };

                context.USUARIO.Add(newUser);
                await context.SaveChangesAsync();

                // Optionally return the created user (without password hash) or just success
                return Ok(new { message = "Usuário registrado com sucesso!" });
            }
            catch (Exception ex)
            {
                // TODO: Implement proper logging
                return BadRequest("Erro ao registrar usuário: " + ex.Message);
            }
        }
        #endregion

        [HttpPost("{id}/exercicios")]
        public async Task<IActionResult> AddExercicios(long id, List<TB_Exercicios> exercicios) // Made async
        {
            var user = context.USUARIO.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            // TODO: Consider checking if exercises already exist or need to be attached
            user.Exercicios.AddRange(exercicios);
            await context.SaveChangesAsync(); // Use async

            return Ok("Exercícios adicionados com sucesso.");
        }

        [HttpGet("{id}/exercicios")]
        public async Task<IActionResult> GetExerciciosPorIdUsuario(long id) // Made async
        {
            var user = await context.USUARIO
                                    .Include(u => u.Exercicios)
                                    .AsNoTracking() // Good practice for read-only queries
                                    .FirstOrDefaultAsync(x => x.Id == id); // Use async

            if (user == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            return Ok(user.Exercicios);
        }

    } // End Class UsuarioController
} // End Namespace API.Controllers
