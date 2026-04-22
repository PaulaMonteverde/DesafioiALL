
using Projeto_iALL.Models.Enums;

namespace Projeto_iALL.Models
{
    public class CollaboratorModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public RoleEnum role { get; set; }
    }
}
