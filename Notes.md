# Learning Notes

## Topico 1 - Pagination

### Acertos

- paginacao com `Skip` e `Take`
- metodo generico estatico dentro da API `QueryableExtensions` com metodo estatico (`Paginate<T>`)
- usar metadata (`TotalCount`, `TotalPages`, `HasNext`, `HasPrevious`)
- ordenacao com `OrderBy`
- validacao de entrada (`page`, `pageSize`)
- `BadRequest` corretamente (erro do cliente)
- exemplo:

```csharp
return BadRequest(new ProblemDetails
{
    Title = "Invalid pagination parameters",
    Detail = "page and pageSize must be greater than 0.",
    Status = 400
});
```

---

### O que foi feito de forma incorreta ou pode ser melhorado

- Inicialmente:
- Nao havia validacao de `page` e `pageSize`
- Nao havia `OrderBy` (risco de inconsistenca)
- Paginacao era feita em memoria sem perceber
- Uso de `try/catch` desnecessario
- Atual:
- Typo: `HasPrevius` deveria ser `HasPrevious`
- Ainda nao foi mencionado o uso de async (`ToListAsync`, `CountAsync`)
- Nao foi explicado o custo de duas queries (`Count + Data`)

---

### Dicas para nao esquecer

- Sempre usar `OrderBy` antes de paginar
- Validar entrada (`page >= 1`, `pageSize >= 1`)
- Paginacao envolve **2 queries** (Count + Data) e e importante saber explicar
- `IQueryable` nao e necessariamente banco de dados e depende do provider (EF Core vs memoria)
- Em APIs reais, preferir metodos assincronos (`await`)
- Sempre retornar metadata, nao apenas os dados
- Metadata util: `Data`, `TotalCount`, `TotalPages`, `HasNext`, `HasPrevious`

---

## Topico 2 - Status Code

### Enunciado

Seu endpoint `POST` cria um recurso, mas tambem aciona uma tarefa assincrona em segundo plano (como enviar um e-mail de boas-vindas). Qual codigo de status HTTP voce retorna?

### Acertos

- entendimento de que a criacao do recurso e a tarefa em background sao responsabilidades diferentes
- retorno de `Created` (`201`) quando o recurso principal foi criado com sucesso
- inclusao da URI do recurso criado no retorno
- separacao entre a resposta imediata da API e o processamento assincrono que continua depois
- uso de um identificador de processamento (`processingId`) para acompanhar a tarefa em background
- criacao de endpoint para consultar status do processamento
- criacao de endpoint para sinalizar conclusao manual do processamento

---

### Licoes aprendidas

- `201 Created` deve ser usado quando o recurso foi criado com sucesso naquele momento
- a existencia de uma tarefa assincrona em segundo plano nao obriga o endpoint a retornar `202 Accepted`
- `202 Accepted` faz mais sentido quando o processamento principal ainda nao terminou e o recurso final ainda nao esta realmente pronto
- se o recurso ja existe e apenas um trabalho complementar foi disparado, `201 Created` continua sendo uma boa resposta
- quando ha processamento assincrono, vale a pena expor meios de acompanhamento, como status e identificador de processamento
- pensar no status code correto exige separar o que e resultado principal da requisicao e o que e efeito colateral assincrono

---

### Dicas para nao esquecer

- Pergunta-chave: o recurso ja foi criado de fato?
- Se sim, `201 Created` costuma ser a melhor resposta
- Se nao, e o servidor apenas aceitou processar depois, considerar `202 Accepted`
- `POST` que cria recurso deve idealmente informar a localizacao do recurso criado
- background processing nao muda sozinho o status code; o que importa e o estado do resultado principal

---

### Q&A

- When would you use PUT instead of PATCH in a REST API?

“I would use PUT when I need to fully replace a resource, meaning the client sends the complete representation of the entity. PUT is idempotent, so making the same request multiple times results in the same state.

PATCH is used for partial updates, when I only need to modify specific fields without sending the entire resource, which can be more efficient.”
