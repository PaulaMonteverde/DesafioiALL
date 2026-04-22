using Microsoft.EntityFrameworkCore;
using Projeto_iALL.Models;

namespace Projeto_iALL.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)  //DbContextOptions é para receber opções de conexões e essas opções serão utilizadas dentro do nosso AppDbContext
        {                                                                            //base do construtor são as opções recebidas

        }

        public DbSet<ItemModel> Items { get; set; }
        public DbSet<RequestedItemModel> RequestedItems { get; set; }
        public DbSet<RequestModel> Requests { get; set; }
        public DbSet<CollaboratorModel> Collaborators { get; set; }
        public DbSet<RequestHistoryModel> RequestHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)      //proibe que se um colaborador for deletado, os históricos de requisição relacionados a ele sejam deletados, ou seja, não tem cascade delete
        {
            modelBuilder.Entity<RequestModel>()
                .HasOne(r => r.Requester)
                .WithMany()
                .HasForeignKey(r => r.RequesterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RequestHistoryModel>()
                .HasOne(h => h.Collaborator)
                .WithMany()
                .HasForeignKey(h => h.CollaboratorId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
