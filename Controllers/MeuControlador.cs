using Microsoft.AspNetCore.Mvc;

namespace MinhaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeuControlador : ControllerBase
    {
        [HttpGet("soma")]
        public IActionResult Soma(int a, int b)
        {
            int resultado = a + b;
            return Ok(resultado);
        }

        [HttpGet("teste")]
        public IActionResult Teste()
        {
            return Ok("A API está funcionando!");
        }
    }
}
