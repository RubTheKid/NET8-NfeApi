using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nfe.Application.Features.NotaFiscal.Command.SendNfeToAuthorization;
using Nfe.Application.Features.NotaFiscal.Query.GetNfeById;
using Nfe.Application.Features.NotaFiscal.Query.GetNfeXml;
using Nfe.Application.Services;

namespace Nfe.Api.Controllers;
//[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NfeController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMessageService _messageService;

    public NfeController(IMediator mediator, IMessageService messageService)
    {
        _mediator = mediator;
        _messageService = messageService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetNfeByIdResponse>> GetById(Guid id)
    {
        try
        {
            var query = new GetNfeByIdRequest(id);
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound(new { error = "Nota Fiscal não encontrada" });

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Erro interno do servidor", details = ex.Message });
        }
    }

    [HttpGet("{id}/xml")]
    public async Task<ActionResult<GetNfeXmlResponse>> GetXml(Guid id)
    {
        try
        {
            var query = new GetNfeXmlRequest(id);
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound(new { error = "Nota Fiscal não encontrada" });

            if (!string.IsNullOrEmpty(result.Message))
                return BadRequest(new { error = result.Message });

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Erro interno do servidor", details = ex.Message });
        }
    }

    [HttpPost("nfe")]
    public async Task<ActionResult<SendNfeToAuthorizationResponse>> EnviarSefaz([FromBody] SendNfeToAuthorizationRequest request)
    {
        try
        {
            var result = await _mediator.Send(request);

            if (!result.Success)
                return BadRequest(new { error = result.Message });

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Erro interno do servidor", details = ex.Message });
        }
    }

}