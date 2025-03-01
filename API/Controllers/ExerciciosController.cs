using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using API.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace API.Controllers
{
    [ApiController]
    [Route("api/exercicios")]
    public class ExerciciosController : ControllerBase
    {
        public AppDbContext context { get; set; }

        public ExerciciosController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet("{id}")]
        public ActionResult<TB_Exercicios> GetById(int id)
        {
            try
            {
                TB_Exercicios exercicio = context.TB_EXERCICIOS.Where(x => x.Id.Equals(id))
                                                           .AsNoTracking()
                                                           .FirstOrDefault();
                if (exercicio == null)
                    return NotFound();

                return Ok(exercicio);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<TB_Exercicios>> GetAll()
        {
            try
            {
                return Ok(context.TB_EXERCICIOS.AsNoTracking()
                                               .ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<TB_Exercicios> Create(TB_Exercicios exercicio)
        {
            try
            {
                context.TB_EXERCICIOS.Add(exercicio);
                context.SaveChanges();
                return CreatedAtAction(nameof(GetById), new { id = exercicio.Id }, exercicio);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                TB_Exercicios exercicio = context.TB_EXERCICIOS.Where(x => x.Id.Equals(id))
                                                               .AsNoTracking()
                                                               .FirstOrDefault();
                if (exercicio == null)
                    return NotFound();

                context.TB_EXERCICIOS.Remove(exercicio);
                context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
