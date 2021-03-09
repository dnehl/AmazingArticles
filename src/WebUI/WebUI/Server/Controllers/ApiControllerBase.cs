using AmazingArticles.Application.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmazingArticles.WebUI.Controllers
{
    /// <inheritdoc />
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        private ISender _mediator;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();

        protected abstract void Log(LogLevel logLevel, string message);


        protected async Task<ActionResult<T>> HandleQuery<T>(IRequest<T> query, CancellationToken cancellationToken) 
        {
            try
            {
                var result = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);
                return result != null
                    ? Accepted(result)
                    : NotFound();
            }
            catch (NotFoundException exception)
            {
                Log(LogLevel.Debug, exception.Message);
                return NotFound(exception.Message);
            }
            catch (Exception exception)
            {
                Log(LogLevel.Debug, exception.Message);
                return BadRequest(exception.Message);
            }
        }

        protected async Task<ActionResult> HandleCommandWithOkResponse<T>(IRequest<T> command, CancellationToken cancellationToken)
        {
            try
            {
                await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
                return Ok();
            }
            catch (NotFoundException exception)
            {
                Log(LogLevel.Debug, exception.Message);
                return NotFound(exception.Message);
            }
            catch (ValidationException exception)
            {
                Log(LogLevel.Debug, exception.Message);
                return ValidationProblem(new ValidationProblemDetails(exception.Errors));
            }
            catch (Exception exception)
            {
                Log(LogLevel.Debug, exception.Message);
                return BadRequest(exception.Message);
            }
        }
    }
}
