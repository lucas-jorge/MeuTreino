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
        public async Task<IActionResult> Login([FromBody] JsonElement body)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TB_USUARIO TB_USUARIO = await context.TB_USUARIO.Where(x => x.Nome.Equals(body.GetProperty("nome").GetString()) &&
                                                                                x.Senha.Equals(body.GetProperty("senha").GetString()))
                                                                    .AsNoTracking()
                                                                    .OrderByDescending(x => x.Id)
                                                                    .FirstOrDefaultAsync();

                    if (TB_USUARIO == null)
                        return await Task.Run(() => BadRequest("Usuário não encontrado com os dados informados!"));

                    switch (TB_USUARIO.Status)
                    {
                        case TB_USUARIO.EStatus.Inativo: return await Task.Run(() => BadRequest("Usuário inativo!"));
                        case TB_USUARIO.EStatus.Excluido: return await Task.Run(() => BadRequest("Usuário excluído!"));
                    }

                    return Ok(new
                    {
                        TB_USUARIO.Id,
                        TB_USUARIO.Nome,
                        Token = TokenService.GenerateToken(TB_USUARIO)
                    });
                }
                catch (Exception ex)
                {
                    return await Task.Run(() => BadRequest("Erro ao realizar o login: " + ex.Message));
                }
            }
            else
                return await Task.Run(() => BadRequest(ModelState));
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
                    retorno = context.TB_USUARIO.Where(x => x.Id.Equals(IdUsuario))
                                                .AsNoTracking()
                                                .FirstOrDefault();

                    if (retorno == null)
                        return await Task.Run(() => BadRequest("Usuário não encontrado!"));
                    else
                        return await Task.Run(() => Ok(retorno));
                }
                else
                {
                    return await Task.Run(() => Ok(context.TB_USUARIO
                                                          .AsNoTracking()
                                                          .ToList()));
                }
            }
            else
                return await Task.Run(() => BadRequest(ModelState));
        }
        #endregion

        #region Put
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] JsonElement body)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TB_USUARIO TB_USUARIO = await context.TB_USUARIO.Where(x => x.Id.Equals(body.GetProperty("id").GetInt64()))
                                                                    .FirstOrDefaultAsync();

                    if (TB_USUARIO == null)
                        return await Task.Run(() => BadRequest("Usuário não encontrado com os dados informados!"));

                    TB_USUARIO.Nome = body.GetProperty("nome").GetString();
                    TB_USUARIO.Senha = body.GetProperty("senha").GetString();
                    TB_USUARIO.Status = (TB_USUARIO.EStatus)body.GetProperty("status").GetInt16();

                    await context.SaveChangesAsync();

                    return await Task.Run(() => Ok("Dados atualizados com sucesso"));
                }
                catch (Exception ex)
                {
                    return await Task.Run(() => BadRequest("Erro ao atualizar os dados: " + ex.Message));
                }
            }
            else
                return await Task.Run(() => BadRequest(ModelState));
        }
        #endregion

        #region Verifica se está autenticado
        [HttpPost("Check")]
        public IActionResult Check()
        {
            return Ok("Usuário autenticado!");
        }
        #endregion


        [HttpPost("{id}/exercicios")]
        public IActionResult AddExercicios(long id, List<TB_Exercicios> exercicios)
        {
            var user = context.TB_USUARIO.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            user.Exercicios.AddRange(exercicios);
            context.SaveChanges();

            return Ok();
        }

        [HttpGet("{id}/exercicios")]
        public IActionResult GetExerciciosPorIdUsuario(long id)
        {
            var user = context.TB_USUARIO.Include(u => u.Exercicios).FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.Exercicios);
        }

    }
}
