using System.ComponentModel.DataAnnotations;

namespace MinhaApi.Models
{
    public class UserCreate
    {
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string DataDeNascimento { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Telefone { get; set; }
        [Required]
        public string Empresa { get; set; }
        [Required]
        public string Senha { get; set; } // Nova propriedade Senha
    }
}
