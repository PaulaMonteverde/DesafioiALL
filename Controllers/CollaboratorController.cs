using Microsoft.AspNetCore.Mvc;
using Projeto_iALL.Models;
using Projeto_iALL.Services.Collaborator;

namespace Projeto_iALL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CollaboratorController : ControllerBase
    {
        private readonly CollaboratorService _collaboratorService;

        public CollaboratorController(CollaboratorService collaboratorService)
        {
            _collaboratorService = collaboratorService;
        }

        [HttpPost]

        public async Task<ActionResult> CreateCollaborator(CollaboratorModel collaborator)
        {
            try
            {
                if (collaborator == null)
                {
                    return BadRequest("Collaborator data is not valid.");
                }
                var createdCollaborator = await _collaboratorService.CreateCollaboratorAsync(collaborator);
                return Ok(createdCollaborator);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]

        public async Task<ActionResult<List<CollaboratorModel>>> GetAllCollaborators()
        {
            try
            {
                var collaborators = await _collaboratorService.GetCollaboratorsAsync();
                return Ok(collaborators);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "DataBase problem" });     //se não conseguiu rodar o get é porque o problema está no database
            }

        }
    }
}

