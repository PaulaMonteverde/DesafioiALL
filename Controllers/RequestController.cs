using Microsoft.AspNetCore.Mvc;
using Projeto_iALL.Services;
using Projeto_iALL.Models; 
using Projeto_iALL.Models.Enums;
using System.Collections.Generic;

namespace Projeto_iALL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly RequestService _requestService;

        public RequestController(RequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpPost]
        public async Task<ActionResult<RequestModel>> CreateRequest(RequestModel request)
        {
            try
            {
                var createdRequest = await _requestService.CreateRequestAsync(request);
                return Ok(createdRequest);      //retorna 200

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  //a mensagem fala o que está errado no insomnia
            }
        }

        [HttpPut("{id}/analysis")]
        public async Task<IActionResult> Analyze(int id, int collaboratorId, ActionEnum action)
        {
            try
            {
                await _requestService.RequestAnalysisAsync(id, collaboratorId, action);
                return Ok(new { Message = "Request analyzed successfully" });
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/Edit")]
        public async Task<IActionResult> EditRequest(int id, RequestModel updatedRequest)
        {
            try
            {
                await _requestService.EditRequestAsync(id, updatedRequest);
                return Ok(new { message = "Request was edited and resent" });
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]

        public async Task<ActionResult<List<RequestModel>>> GetAllRequests()
        {
            try
            {
                var requests = await _requestService.GetAllRequestsAsync();
                return Ok(requests);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "DataBase problem" });   //como é um get de uma lista, se der erro não é erro do usuário e sim do servidor
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RequestModel>> GetRequestById(int id)
        {
            var request = await _requestService.GetRequestByIdAsync(id);
            if (request == null)
            {
                return NotFound(new { message = $"RequestId {id} not found" });
            }
            return Ok(request);
        }

        [HttpGet("{id}/history")]

        public async Task<ActionResult<List<RequestHistoryModel>>> GetRequestHistory(int id)
        {
            var history = await _requestService.GetHistoryByRequestIdAsync(id);
            if (history == null || history.Count == 0)
            {
                return NotFound(new { message = $"HistoryId {id} not found" });
            }
            return Ok(history);
        }

    }
}   