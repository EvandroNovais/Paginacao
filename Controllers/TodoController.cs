using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Paginacao.Data;
using Paginacao.Models;

namespace Paginacao.Controllers
{
    [ApiController]
    [Route("v1/todos")]
    public class TodoController : ControllerBase
    {
        [HttpGet("load")]
        public async Task<IActionResult> LoadAsync(
            [FromServices] AppDbContext context)
        {
            for (var i = 0; i < 1348; i++)
            {
                var todo = new Todo()
                {
                    Id = i + 1,
                    Done = false,
                    CreatedAt = DateTime.Now,
                    Title = $"Tarefa {i}"
                };
                await context.Todos.AddAsync(todo);
                await context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpGet("skip={skip:int}&take={take:int}")]
        public async Task<IActionResult> GetAsync(
            [FromServices] AppDbContext context, 
            [FromRoute] int skip = 0, 
            [FromRoute] int take = 25)
        {
            var total = await context.Todos.CountAsync();
            var todos = await context.Todos
                .AsNoTracking()
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return Ok(
                new{
                    total,
                    data = todos
                }
            );
        }
    }
}