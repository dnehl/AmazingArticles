using AmazingArticles.Application.Articles.Commands.CreateArticle;
using AmazingArticles.Application.Articles.Commands.DeleteArticle;
using AmazingArticles.Application.Articles.Commands.UpdateArticle;
using AmazingArticles.Application.Articles.Commands.UpdateArticleSalesPrice;
using AmazingArticles.Application.Articles.Queries.GetArticles;
using AmazingArticles.Application.Articles.Queries.Revenues;
using AmazingArticles.Application.Common.Exceptions;
using AmazingArticles.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AmazingArticles.WebUI.Controllers
{

    public class ArticlesController : ApiControllerBase
    {
        private readonly ILogger<ArticlesController> _logger;

        public ArticlesController(ILogger<ArticlesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles([FromQuery] GetArticlesQuery query,CancellationToken cancellationToken)
        {
            return await HandleQuery(query, cancellationToken).ConfigureAwait(false);
        }

        [HttpGet("GetById")]
        public async Task<ActionResult<Article>> GetById([FromQuery] GetArticleByIdQuery query,
            CancellationToken cancellationToken)
        {
            return await HandleQuery(query, cancellationToken).ConfigureAwait(false);
        }

        [HttpGet("GetArticleCountPerDay")]
        public async Task<ActionResult<Dictionary<DateTime, int>>> GetArticlesPerDay(
            [FromQuery] AddedArticlesPerDayCountQuery countQuery, CancellationToken cancellationToken)
        {
            return await HandleQuery(countQuery, cancellationToken).ConfigureAwait(false);
        }

        [HttpGet("GetDailyRevenue")]
        public async Task<ActionResult<Dictionary<DateTime, double>>> TotalRevenuePerDay([FromQuery] RevenuesPerDayQuery query,
            CancellationToken cancellationToken)
        {
            return await HandleQuery(query, cancellationToken).ConfigureAwait(false);
        }

        [HttpGet("TotalRevenuePerArticleNumber")]
        public async Task<ActionResult<Dictionary<string, double>>> TotalRevenuePerArticleNumber(
            [FromQuery] RevenuesPerArticlesQuery query, CancellationToken cancellationToken)
        {
            return await HandleQuery(query, cancellationToken).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateArticleCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
                return CreatedAtAction("GetById", new GetArticleByIdQuery{Id = result});
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

        [HttpPut]
        public async Task<ActionResult> Update(UpdateArticleCommand command, CancellationToken cancellationToken)
        {
            return await HandleCommandWithOk(command, cancellationToken).ConfigureAwait(false);
        }

        [HttpPatch]
        public async Task<ActionResult> ChangesSalesPrice(UpdateArticleSalesPriceCommand command,
            CancellationToken cancellationToken)
        {
            return await HandleCommandWithOk(command, cancellationToken).ConfigureAwait(false);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(DeleteArticleCommand command, CancellationToken cancellationToken)
        {
            return await HandleCommandWithOk(command, cancellationToken).ConfigureAwait(false);
        }

        protected override void Log(LogLevel logLevel, string message)
        {
            _logger.Log(LogLevel.Debug, message);
        }
    }
}