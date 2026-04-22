using Microsoft.AspNetCore.Mvc;
using Projeto_iALL.Models;
using Projeto_iALL.Services.Item;

namespace Projeto_iALL.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class ItemController : ControllerBase
    {
        private readonly ItemService _itemService;

        public ItemController(ItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpPost]

        public async Task<ActionResult> CreateItem(ItemModel item)
        {
            try
            {
                var createdItem = await _itemService.CreateItemAsync(item);
                return Ok(createdItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]

        public async Task<ActionResult<List<ItemModel>>> GetAllItems()
        {
            try
            {
                var items = await _itemService.GetItemsAsync();
                return Ok(items);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "DataBase problem" });
            }
        }
    }

}
