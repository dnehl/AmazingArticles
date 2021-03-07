using System;
using AmazingArticles.Application.Articles.Commands.CreateArticle;
using AmazingArticles.Application.Articles.Commands.DeleteArticle;
using AmazingArticles.Application.Articles.Commands.UpdateArticle;
using AmazingArticles.Application.Articles.Commands.UpdateArticleSalesPrice;
using AmazingArticles.Application.Articles.Queries.GetArticles;
using AmazingArticles.Application.Articles.Queries.Revenues;
using AmazingArticles.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AmazingArticles.WebUI.Controllers
{

    public class ArticlesController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles([FromQuery] GetArticlesQuery query,CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("GetArticleCountPerDay")]
        public async Task<ActionResult<Dictionary<DateTime, int>>> GetArticlesPerDay(
            [FromQuery] AddedArticlesPerDayCountQuery countQuery, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(countQuery, cancellationToken).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpGet("GetDailyRevenue")]
        public async Task<ActionResult<Dictionary<DateTime, double>>> TotalRevenuePerDay([FromQuery] RevenuesPerDayQuery query,
            CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateArticleCommand command, CancellationToken cancellationToken)
        {
            await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> Update(UpdateArticleCommand command, CancellationToken cancellationToken)
        {
            await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
            return Ok();
        }

        [HttpPatch]
        public async Task<ActionResult> ChangesSalesPrice(UpdateArticleSalesPriceCommand command,
            CancellationToken cancellationToken)
        {
            await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(DeleteArticleCommand command, CancellationToken cancellationToken)
        {
            await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
            return Ok();
        }
    }
}