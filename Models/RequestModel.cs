using Projeto_iALL.Models.Enums;

namespace Projeto_iALL.Models
{
    public class RequestModel
    {
        public int Id { get; set; }
        public List<RequestedItemModel> Items { get; set; } = new List<RequestedItemModel>();

        public decimal TotalValue { get; set; }
        public StatusRequest Status { get; set; } = StatusRequest.Created;
        public int RequesterId { get; set; }
        public CollaboratorModel? Requester { get; set; }

        public bool IsApprovedBySupplies { get; set; }
        public bool IsApprovedByManager { get; set; }
        public bool IsApprovedByDirector { get; set; }



    }
}
