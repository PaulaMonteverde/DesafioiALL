using Projeto_iALL.Data;
using Projeto_iALL.Models;
using Microsoft.EntityFrameworkCore;


namespace Projeto_iALL.Services.Item
{
    public class ItemService
    {
        private readonly AppDbContext _context;

        public ItemService (AppDbContext context)
        {
            _context = context;
        }

        public async Task<ItemModel> CreateItemAsync (ItemModel item)
        {
            _context.Items.Add (item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<List<ItemModel>> GetItemsAsync ()
        {
            return await _context.Items.ToListAsync();
        }
    }
}
