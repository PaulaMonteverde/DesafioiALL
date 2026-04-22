using Microsoft.EntityFrameworkCore;
using Projeto_iALL.Data;
using Projeto_iALL.Models;
using Projeto_iALL.Models.Enums;

namespace Projeto_iALL.Services
{
    public class RequestService
    {
        private readonly AppDbContext _context;  //apenas para leitura. Preciso acessar o contexto para acessar o banco de dados e obter os dados dos itens para utilizar nos meus metodos

        public RequestService(AppDbContext context) //construtor
        {
            _context = context;
        }

        public void UpdateTotalRequestedItem(RequestedItemModel requestedItem)
        {
            requestedItem.TotalValue = requestedItem.Item.Value * requestedItem.Quantity;
        }

        public async Task UpdateTotalRequestAsync(RequestModel request)     //async porque como temos que acessar o banco de dados para obter os dados dos itens, precisamos utilizar o async para não travar a aplicação enquanto isso acontece
        {
            decimal result = 0;

            foreach (var item in request.Items)
            {
                var DataItem = await _context.Items.FindAsync(item.ItemId); //espera a info do dataBase

                if (DataItem != null)
                {
                    item.Item = DataItem;
                    UpdateTotalRequestedItem((RequestedItemModel)item);
                    result += item.TotalValue;
                }
            }

            request.TotalValue = result;
        }

        public void DefineApprovalFlow(RequestModel request)        //aqui guiamos o pedido para quem ele deve ser analisado
        {
            if (request.TotalValue <= 100)
            {                                           //até 100, precisa apenas da aprovação do Supplies
                request.IsApprovedBySupplies = false;
                request.IsApprovedByManager = true;
                request.IsApprovedByDirector = true;
            }
            else if (request.TotalValue > 100 && request.TotalValue <= 1000)
            {                                           //entre 100 e 1000, precisa da aprovação do gerente e do supplies
                request.IsApprovedBySupplies = false;
                request.IsApprovedByManager = false;
                request.IsApprovedByDirector = true;
            }
            else
            {                                           //acima de 1000, precisa da aprovação de todos
                request.IsApprovedBySupplies = false;
                request.IsApprovedByManager = false;
                request.IsApprovedByDirector = false;
            }
        }

        public async Task<RequestModel> CreateRequestAsync(RequestModel request)     //aqui criamos o pedido, atualizando o valor total e definindo o fluxo de aprovação
        {
            await UpdateTotalRequestAsync(request);     //atualizamos o valor total do pedido.
            DefineApprovalFlow(request);                //aqui o valor total do pedido ja está atualizado, então podemos definir o fluxo de aprovação com base nesse valor.

            _context.Requests.Add(request);                //Depois de att o valor do pedido e definir o fluxo de aprovação, jogamos ele no banco de dados
            await _context.SaveChangesAsync();            //Salvamos as alterações no banco de dados

            //Depois de criar e salvar o pedido no banco de dados temos que colocar no histórico

            var history = new RequestHistoryModel
            {
                Date = DateTime.Now,
                CollaboratorId = request.RequesterId,
                Action = ActionEnum.Create,
                RequestId = request.Id
            };

            _context.RequestHistories.Add(history);      //Adicionamos o histórico no banco de dados
            await _context.SaveChangesAsync();            //Salvamos as alterações no banco de dados

            return request;
        }

        public async Task RequestAnalysisAsync(int requestId, int collaboratorId, ActionEnum action)
        {
            var request = await _context.Requests.FindAsync(requestId);         //usamos o requestId para achar o pedido no banco de dados
            var collaborator = await _context.Collaborators.FindAsync(collaboratorId);      //usamos o collaboratorId para achar o colaborador no banco de dados

            if (request == null || collaborator == null)
            {
                throw new ArgumentException("Request or collaborator not found.");
            }

            if (action == ActionEnum.Approve)
            {
                if (collaborator.role == RoleEnum.Supplies && !request.IsApprovedBySupplies)
                {
                    request.IsApprovedBySupplies = true;
                }
                else if (collaborator.role == RoleEnum.Manager && !request.IsApprovedByManager)
                {
                    request.IsApprovedByManager = true;
                }
                else if (collaborator.role == RoleEnum.Director && !request.IsApprovedByDirector)
                {
                    request.IsApprovedByDirector = true;
                }

                if (request.IsApprovedBySupplies && request.IsApprovedByManager && request.IsApprovedByDirector)    // se obteve as 3 aprovações -> pedido concluido
                {
                    request.Status = StatusRequest.Completed;
                }
            }
            else if (action == ActionEnum.ReviewRequest)
            {
                request.Status = StatusRequest.InReview;
            }
            else if (action == ActionEnum.Resend)
            {
                request.Status = StatusRequest.Resent;
            }
            else if (action == ActionEnum.Cancel)
            {
                request.Status = StatusRequest.Cancelled;
            }

            // botar as modificações no historico
            var history = new RequestHistoryModel
            {
                Date = DateTime.Now,
                CollaboratorId = collaboratorId,
                Action = action,
                RequestId = requestId
            };

            _context.RequestHistories.Add(history);      //Adicionamos o histórico no banco de dados
            await _context.SaveChangesAsync();            //Salvamos as alterações no banco de dados

        }

        public async Task EditRequestAsync(int requestId, RequestModel updatedRequest)
        {
            var existingRequest = await _context.Requests.Include(r => r.Items).FirstOrDefaultAsync(r => r.Id == requestId);    //desta maneira os itens retornados não serão uma cópia como seriam com FindAsync

            if (existingRequest == null)
            {
                throw new Exception("Request not found.");
            }

            if (existingRequest.Status != StatusRequest.InReview)
            {
                throw new Exception("Cannot edit this request.");
            }

            _context.RequestedItems.RemoveRange(existingRequest.Items);     //removemos os itens antigos do pedido
            existingRequest.Items = updatedRequest.Items;     //atualizamos os itens do pedido com os novos itens

            await UpdateTotalRequestAsync(existingRequest);     //faz o mesmo processo que fazemos quando criamos um pedido
            DefineApprovalFlow(existingRequest);

            existingRequest.Status = StatusRequest.Resent;    //agora o pedido tem como status "Reenviado", para constar no historico   
            var history = new RequestHistoryModel
            {
                Date = DateTime.Now,
                CollaboratorId = existingRequest.RequesterId,
                Action = ActionEnum.Resend,
                RequestId = existingRequest.Id
            };
            _context.RequestHistories.Add(history);      //Adicionamos o histórico no banco de dados
            await _context.SaveChangesAsync();
        }

        public async Task<RequestModel?> GetRequestByIdAsync(int requestId)     // o ? serve para se caso houver um pedido vazio
        {
            return await _context.Requests.Include(r => r.Items).ThenInclude(i => i.Item).Include(r => r.Requester).FirstOrDefaultAsync(r => r.Id == requestId);

        }

        public async Task<List<RequestModel>> GetAllRequestsAsync()
        {
            return await _context.Requests.Include(r => r.Items).OrderByDescending(r => r.Id).ToListAsync();  // retorna os mais recentes primeiro

        }

        public async Task<List<RequestHistoryModel>> GetHistoryByRequestIdAsync(int requestId)     // o ? serve para se caso houver um pedido vazio
        {
            return await _context.RequestHistories.Where(h => h.RequestId == requestId).Include(h => h.Collaborator).OrderByDescending(h => h.Date).ToListAsync();  // retorna os mais recentes primeiro

        }
    }
}


