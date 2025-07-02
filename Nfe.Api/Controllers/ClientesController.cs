using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nfe.Application.Features.Clientes.Command.Create.Request;
using Nfe.Application.Features.Clientes.Command.Update;
using Nfe.Application.Features.Clientes.Query.GetAllClientes;
using Nfe.Application.Features.Clientes.Query.GetClienteById;

namespace Nfe.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<GetAllClientesResponse>> GetAll()
    {
        try
        {
            var query = new GetAllClientesRequest();
            var response = await _mediator.Send(query);

            if (response is null) return NotFound();

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Erro interno do servidor", details = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetClienteByIdResponse>> GetById(Guid id)
    {
        try
        {
            var query = new GetClienteByIdRequest(id);
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound(new { error = "Cliente não encontrado" });

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Erro interno do servidor", details = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClienteRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _mediator.Send(request, cancellationToken);

            if (response.Id == Guid.Empty)
                return BadRequest(new { error = response.Message });

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Erro interno do servidor", details = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClienteRequest request, CancellationToken cancellationToken)
    {
        try
        {
            request.Id = id;
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Erro interno do servidor", details = ex.Message });
        }
    }
}