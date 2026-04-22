using Projeto_iALL.Models.Enums;

namespace Projeto_iALL.Models
{
    public class RequestHistoryModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int CollaboratorId { get; set; }
        public CollaboratorModel Collaborator { get; set; } = null!;
        public ActionEnum Action { get; set; }

        public int RequestId { get; set; }
        public RequestModel Request { get; set; } = null!;


    }
}
