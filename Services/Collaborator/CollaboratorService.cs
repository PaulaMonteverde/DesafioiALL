using Projeto_iALL.Data;
using Projeto_iALL.Models;
using Microsoft.EntityFrameworkCore;

namespace Projeto_iALL.Services.Collaborator
{
    public class CollaboratorService
    {
        private readonly AppDbContext _context;

        public CollaboratorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CollaboratorModel> CreateCollaboratorAsync(CollaboratorModel collaborator)
        {
            _context.Collaborators.Add(collaborator);      //adiciona o colaborador ao banco de dados dos colaboradores
            await _context.SaveChangesAsync();             //salva

            return collaborator;        //retorna colaborador
        }

        public async Task<List<CollaboratorModel>> GetCollaboratorsAsync()
        {
            return await _context.Collaborators.ToListAsync<CollaboratorModel>();
        }
    }
}
