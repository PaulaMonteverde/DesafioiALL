Este projeto consiste na elaboração de um gestor de fluxo de pedidos.
O fluxo consiste em: um colaborador solicita um pedido e deve mandar para aprovação de determinados setores de acordo com o valor total do pedido. Um pedido até 100 reais precisa da aprovação do setor de suprimentos, um pedido maior que 100 reais até 1000 reais precisa da aprovação do setor de suprimentos e da gestão, e um pedido maior que mil reais precisa da aprovação do setor de suprimentos, da gestão e da direção. Em qualquer etapa do fluxo de aprovação o pedido pode ser cancelado ou pode ser posto para revisão. Uma vez que ele é posto para revisão o pedido volta ao colaborador criador para ser editado e passar pelo fluxo de aprovação novamente.

O projeto foi implementado em C# (.NET 10) usando SQL server como banco de dados e o Insomnia para testes da API e o Entity Framework Core para as migrations.
A API possui os métodos POST, para criar pedidos, Itens e Colaboradores. Possui o método GET para retornar os pedidos, historico dos pedidos, colaboradores e Itens. Possui o metodo PUT para editar os pedidos e para analisar os pedidos (Definir o fluxo).

O diagrama do Banco de Dados e os testes do Insomnia estão na pasta assets.

Para executar o projeto, clone o repositório git clone [https://github.com/PaulaMonteverde/DesafioiALL.git](https://github.com/PaulaMonteverde/DesafioiALL.git)
No Package Manages do Visual studio dê update no banco de dados: Update-Database.
Aperte F5 para dar play na aplicação.
Teste o projeto no Insomnia pois como o projeto está no .NET 10, ele não possui suporte para o swagger.

```mermaid
graph TD
    %% Início do Processo
    Start((Início)) --> Create[Colaborador elabora pedido]
    Create --> Calc[Cálculo do valor total]
    
    %% Decisão de Faixa de Valor
    Calc --> ValueSwitch{Valor do Pedido?}

    %% FAIXA 1: Até 100 reais
    ValueSwitch -- "Até R$ 100" --> SupCheck1[Análise Suprimentos]
    SupCheck1 --> Res1{Resultado?}
    Res1 -- Aprovado --> Done((Concluído))
    Res1 -- Cancelado --> Cancel((Cancelado))
    Res1 -- Revisão --> Create

    %% FAIXA 2: 100 a 1000 reais
    ValueSwitch -- "R$ 100 a R$ 1000" --> SupCheck2[Análise Suprimentos]
    SupCheck2 --> Res2{Aprovou?}
    Res2 -- Não/Cancelado --> Cancel
    Res2 -- Revisão --> Create
    Res2 -- Sim --> GestCheck2[Análise Gestão]
    GestCheck2 --> Res3{Resultado?}
    Res3 -- Aprovado --> Done
    Res3 -- Cancelado --> Cancel
    Res3 -- Revisão --> Create

    %% FAIXA 3: Maior que 1000 reais
    ValueSwitch -- "Acima de R$ 1000" --> SupCheck3[Análise Suprimentos]
    SupCheck3 --> Res4{Aprovou?}
    Res4 -- Não/Cancelado --> Cancel
    Res4 -- Revisão --> Create
    Res4 -- Sim --> GestCheck3[Análise Gestão]
    GestCheck3 --> Res5{Aprovou?}
    Res5 -- Não/Cancelado --> Cancel
    Res5 -- Revisão --> Create
    Res5 -- Sim --> DirCheck[Análise Diretor]
    DirCheck --> Res6{Resultado?}
    Res6 -- Aprovado --> Done
    Res6 -- Cancelado --> Cancel
    Res6 -- Revisão --> Create

    %% Estilização para ficar mais bonito
    style Start fill:#f9f,stroke:#333,stroke-width:2px
    style Done fill:#9f9,stroke:#333,stroke-width:2px
    style Cancel fill:#f99,stroke:#333,stroke-width:2px
    style Create fill:#fff,stroke:#333,stroke-dasharray: 5 5
```
```mermaid
classDiagram

    %% ─── Enums ───────────────────────────────────────────────
    class RoleEnum {
        <<enumeration>>
        Supplies = 1
        Manager = 2
        Director = 3
        Employee = 4
    }

    class StatusRequest {
        <<enumeration>>
        Created = 1
        InReview = 2
        Approved = 3
        Cancelled = 4
        Completed = 5
        Resent = 6
    }

    class ActionEnum {
        <<enumeration>>
        Create = 0
        Approve = 1
        ReviewRequest = 2
        Resend = 3
        Conclusion = 4
        Cancel = 5
    }

    %% ─── Models ──────────────────────────────────────────────
    class CollaboratorModel {
        +int Id
        +string Name
        +RoleEnum role
    }

    class ItemModel {
        +int Id
        +string Name
        +decimal Value
    }

    class RequestedItemModel {
        +int Id
        +int ItemId
        +ItemModel? Item
        +int Quantity
        +decimal TotalValue
    }

    class RequestModel {
        +int Id
        +List~RequestedItemModel~ Items
        +decimal TotalValue
        +StatusRequest Status
        +int RequesterId
        +CollaboratorModel? Requester
        +bool IsApprovedBySupplies
        +bool IsApprovedByManager
        +bool IsApprovedByDirector
    }

    class RequestHistoryModel {
        +int Id
        +DateTime Date
        +int CollaboratorId
        +CollaboratorModel Collaborator
        +ActionEnum Action
        +int RequestId
        +RequestModel Request
    }

    %% ─── Services ────────────────────────────────────────────
    class CollaboratorService {
        -AppDbContext _context
        +CreateCollaboratorAsync(CollaboratorModel) Task~CollaboratorModel~
        +GetCollaboratorsAsync() Task~List~CollaboratorModel~~
    }

    class ItemService {
        -AppDbContext _context
        +CreateItemAsync(ItemModel) Task~ItemModel~
        +GetItemsAsync() Task~List~ItemModel~~
    }

    class RequestService {
        -AppDbContext _context
        +UpdateTotalRequestedItem(RequestedItemModel) void
        +UpdateTotalRequestAsync(RequestModel) Task
        +DefineApprovalFlow(RequestModel) void
        +CreateRequestAsync(RequestModel) Task~RequestModel~
        +RequestAnalysisAsync(int, int, ActionEnum) Task
        +EditRequestAsync(int, RequestModel) Task
        +GetRequestByIdAsync(int) Task~RequestModel?~
        +GetAllRequestsAsync() Task~List~RequestModel~~
        +GetHistoryByRequestIdAsync(int) Task~List~RequestHistoryModel~~
    }

    %% ─── Controllers ─────────────────────────────────────────
    class CollaboratorController {
        -CollaboratorService _collaboratorService
        +CreateCollaborator(CollaboratorModel) Task~ActionResult~
        +GetAllCollaborators() Task~ActionResult~
    }

    class ItemController {
        -ItemService _itemService
        +CreateItem(ItemModel) Task~ActionResult~
        +GetAllItems() Task~ActionResult~
    }

    class RequestController {
        -RequestService _requestService
        +CreateRequest(RequestModel) Task~ActionResult~
        +Analyze(int, int, ActionEnum) Task~IActionResult~
        +EditRequest(int, RequestModel) Task~IActionResult~
        +GetAllRequests() Task~ActionResult~
        +GetRequestById(int) Task~ActionResult~
        +GetRequestHistory(int) Task~ActionResult~
    }

    %% ─── Relationships ───────────────────────────────────────

    %% Enum usage
    CollaboratorModel --> RoleEnum : uses
    RequestModel --> StatusRequest : uses
    RequestHistoryModel --> ActionEnum : uses

    %% Model associations
    RequestModel "1" --> "0..*" RequestedItemModel : contains
    RequestModel --> CollaboratorModel : Requester
    RequestedItemModel --> ItemModel : references
    RequestHistoryModel --> CollaboratorModel : performed by
    RequestHistoryModel --> RequestModel : records

    %% Controller → Service
    CollaboratorController --> CollaboratorService : uses
    ItemController --> ItemService : uses
    RequestController --> RequestService : uses

    %% Service → Model
    CollaboratorService ..> CollaboratorModel : manages
    ItemService ..> ItemModel : manages
    RequestService ..> RequestModel : manages
    RequestService ..> RequestedItemModel : manages
    RequestService ..> RequestHistoryModel : manages
```


