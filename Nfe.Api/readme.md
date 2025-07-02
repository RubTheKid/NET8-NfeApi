# Endpoint de Autorização de NFe

## Descrição
Endpoint para enviar notas fiscais para autorização via RabbitMQ. O endpoint recebe dados em JSON, converte para XML e envia para a fila de processamento.

## URL
```
POST /api/nfe/send-to-authorization
```

## Request Body
```json
{
  "numeroNota": "000000001",
  "serie": "001",
  "emitenteId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "destinatarioId": "3fa85f64-5717-4562-b3fc-2c963f66afa7",
  "itens": [
    {
      "produtoId": "3fa85f64-5717-4562-b3fc-2c963f66afa8",
      "sequencia": 1,
      "quantidade": 10.0,
      "valorUnitario": 25.50,
      "cfop": "5102",
      "cst": "00"
    }
  ]
}
```

## Campos Obrigatórios
- `numeroNota`: Número da nota fiscal
- `serie`: Série da nota fiscal
- `emitenteId`: ID do cliente emitente (deve existir na base)
- `destinatarioId`: ID do cliente destinatário (deve existir na base)
- `itens`: Array com pelo menos um item
  - `produtoId`: ID do produto
  - `sequencia`: Sequência do item na nota
  - `quantidade`: Quantidade do produto
  - `valorUnitario`: Valor unitário do produto
  - `cfop`: Código Fiscal de Operações e Prestações
  - `cst`: Código de Situação Tributária

## Response de Sucesso (200)
```json
{
  "success": true,
  "nfeId": "guid-da-nfe-criada",
  "numeroNota": "000000001",
  "serie": "001",
  "chaveAcesso": "chave-de-44-digitos",
  "dataEmissao": "2024-01-01T10:00:00Z",
  "status": "EmProcessamento",
  "valorTotal": 334.25,
  "message": "NFe criada e enviada para processamento com sucesso"
}
```

## Response de Erro (400)
```json
{
  "error": "Emitente não encontrado"
}
```

## Funcionalidades
1. **Validação de dados**: Valida campos obrigatórios e existência de emitente/destinatário
2. **Criação da NFe**: Cria a nota fiscal no banco de dados
3. **Conversão para XML**: Converte os dados para formato XML padrão NFe
4. **Envio para RabbitMQ**: Envia para fila `nfe-authorization-queue`
5. **Cálculo automático**: Calcula automaticamente impostos e totais

## Configuração RabbitMQ
- Host: localhost
- Port: 5672
- User: rabbitmq
- Password: rabbitmq
- Queue: nfe-authorization-queue

## Fluxo do Processo
1. Recebe JSON via POST
2. Valida dados de entrada
3. Cria entidade NotaFiscal
4. Adiciona itens e calcula totais
5. Salva no banco de dados
6. Converte para XML
7. Envia mensagem para RabbitMQ
8. Retorna response com dados da NFe

## Exemplo de Uso com cURL
```bash
curl -X POST "https://localhost:7000/api/nfe/send-to-authorization" \
  -H "Content-Type: application/json" \
  -d @exemplo-nfe-request.json
``` 