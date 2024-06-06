using Microsoft.AspNetCore.Mvc;
using MinhaApi.Data;
using MinhaApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace MinhaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult<UserCreate>> CreateUser(UserCreate userCreate)
        {
            var user = new User
            {
                Id = userCreate.Id,
                Nome = userCreate.Nome,
                DataDeNascimento = userCreate.DataDeNascimento,
                Email = userCreate.Email,
                Telefone = userCreate.Telefone,
                Empresa = userCreate.Empresa,
                Senha = userCreate.Senha,
                Photo = null // Inicialmente, sem foto
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, userCreate);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserCreate>>> GetUsers()
        {
            var users = await _context.Users
                .Select(user => new UserCreate
                {
                    Id = user.Id,
                    Nome = user.Nome,
                    DataDeNascimento = user.DataDeNascimento,
                    Email = user.Email,
                    Telefone = user.Telefone,
                    Empresa = user.Empresa,
                    Senha = user.Senha
                })
                .ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserCreate>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userCreate = new UserCreate
            {
                Id = user.Id,
                Nome = user.Nome,
                DataDeNascimento = user.DataDeNascimento,
                Email = user.Email,
                Telefone = user.Telefone,
                Empresa = user.Empresa,
                Senha = user.Senha
            };

            return Ok(userCreate);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserCreate userUpdate)
        {
            if (id != userUpdate.Id)
            {
                return BadRequest();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Nome = userUpdate.Nome;
            user.DataDeNascimento = userUpdate.DataDeNascimento;
            user.Email = userUpdate.Email;
            user.Telefone = userUpdate.Telefone;
            user.Empresa = userUpdate.Empresa;
            user.Senha = userUpdate.Senha;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{id}/photo")]
        public async Task<IActionResult> UploadUserPhoto(int id, IFormFile photo)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            using (var memoryStream = new MemoryStream())
            {
                await photo.CopyToAsync(memoryStream);
                user.Photo = memoryStream.ToArray();
            }

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Foto adicionada com sucesso" });
        }

        [HttpGet("{id}/photo")]
        public async Task<IActionResult> GetUserPhoto(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || user.Photo == null)
            {
                return NotFound(new { message = "Foto não encontrada para este usuário" });
            }

            return File(user.Photo, "image/jpeg");
        }

        // Endpoint para validação de email e senha
        [HttpPost("validate")]
        public async Task<IActionResult> ValidateUser(LoginRequest loginRequest)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginRequest.Email && u.Senha == loginRequest.Senha);

            if (user != null)
            {
                return Ok(new { message = "Login bem-sucedido" });
            }

            return Unauthorized(new { message = "Email ou senha incorretos" });
        }
    }
}
