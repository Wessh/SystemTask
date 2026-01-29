using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<TaskItem> TaskItems { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }
    }
}
/*
 * Caso queira rodar a migration sem fazer a Factory, faça assim:
 * <Comando referente a migration> --project Infrastructure --startup-project Api
 */