namespace Projeto_iALL.Models
{
    public class RequestedItemModel
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public ItemModel? Item { get; set; }
        public int Quantity { get; set; }
        public decimal TotalValue { get; set; }
    }
}
