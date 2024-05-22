using Microsoft.EntityFrameworkCore;

namespace MinhaApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string DataDeNascimento { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Empresa { get; set; }
        public string Senha { get; set; }
        public byte[]? Photo { get; set; }
    }
}
