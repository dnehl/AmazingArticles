using AmazingArticles.Application.Articles.Commands.CreateArticle;
using AmazingArticles.Application.Articles.Commands.DeleteArticle;
using AmazingArticles.Application.Articles.Commands.UpdateArticle;
using AmazingArticles.Application.Articles.Commands.UpdateArticleSalesPrice;
using AmazingArticles.Application.Articles.Commands.UpdateArticleSold;
using AmazingArticles.Application.Articles.Queries.GetArticles;
using AmazingArticles.Application.Articles.Queries.Revenues;
using AmazingArticles.Application.Common.Exceptions;
using AmazingArticles.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AmazingArticles.WebUI.Controllers
{
    /// <summary>
    ///     Controller to get information about articles,
    ///     add new articles, delete existing articles or updates existing articles
    /// </summary>
    public class ArticlesController : ApiControllerBase
    {
        private readonly ILogger<ArticlesController> _logger;

        /// <summary>
        /// Constructor to inject the logger for this controller
        /// </summary>
        /// <param name="logger"></param>
        public ArticlesController(ILogger<ArticlesController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///     Gets all articles
        /// </summary>
        /// <param name="query">Query request to receive articles </param>
        /// <param name="cancellationToken">Token to cancel operation</param>
        /// <returns>a list of articles</returns>
        /// <response code="200">Response is ok</response>
        /// <response code="404">No items available</response>
        /// <response code="400">Something unexpected went wrong</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles([FromQuery] GetArticlesQuery query,CancellationToken cancellationToken)
        {
            return await HandleQuery(query, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        ///     Gets the article with a specific id
        /// </summary>
        /// <param name="query">Query request with the information about the specific id of an article</param>
        /// <param name="cancellationToken">Token to cancel operation</param>
        /// <returns>the specific article, if exist, otherwise a NotFound result</returns>
        /// <response code="200">Response is ok</response>
        /// <response code="404">Item not available</response>
        /// <response code="400">Something unexpected went wrong</response>
        [HttpGet("GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Article>> GetById([FromQuery] GetArticleByIdQuery query,
            CancellationToken cancellationToken)
        {
            return await HandleQuery(query, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        ///     Gets all articles, which where sold group by the day 
        /// </summary>
        /// <param name="countQuery">Query request to receive the data</param>
        /// <param name="cancellationToken">Token to cancel operation</param>
        /// <returns>A List of all articles with the specific data as key</returns>
        /// <response code="200">Response is ok</response>
        /// <response code="404">Data not available</response>
        /// <response code="400">Something unexpected went wrong</response>
        [HttpGet("GetArticleCountPerDay")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Dictionary<DateTime, int>>> GetArticlesPerDay(
            [FromQuery] SoldArticlesPerDayCountQuery countQuery, CancellationToken cancellationToken)
        {
            return await HandleQuery(countQuery, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        ///     Gets the revenue depending on the date
        /// </summary>
        /// <param name="query">Query request to receive the data</param>
        /// <param name="cancellationToken">Token to cancel operation</param>
        /// <returns>A List of revenues depending on the date</returns>
        /// <response code="200">Response is ok</response>
        /// <response code="404">Data not available</response>
        /// <response code="400">Something unexpected went wrong</response>
        [HttpGet("GetDailyRevenue")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Dictionary<DateTime, double>>> TotalRevenuePerDay([FromQuery] RevenuesPerDayQuery query,
            CancellationToken cancellationToken)
        {
            return await HandleQuery(query, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        ///     Gets the revenue depending on  the article number
        /// </summary>
        /// <param name="query">Query request to receive the data</param>
        /// <param name="cancellationToken">Token to cancel operation</param>
        /// <returns>A List of revenues depending on the date</returns>
        /// <response code="200">Response is ok</response>
        /// <response code="404">Data not available</response>
        /// <response code="400">Something unexpected went wrong</response>
        [HttpGet("TotalRevenuePerArticleNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Dictionary<string, double>>> TotalRevenuePerArticleNumber(
            [FromQuery] RevenuesPerArticlesQuery query, CancellationToken cancellationToken)
        {
            return await HandleQuery(query, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates an article.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Article
        ///     {
        ///        "ArticleNumber": "at000001",
        ///        "SalesPrice": 445,60
        ///     }
        /// 
        /// </remarks>
        /// <param name="command">command request to create a new article</param>
        /// <param name="cancellationToken">Token to cancel operation</param>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">Something unexpected went wrong, or the validation was not correct</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Replace (update) an existing article.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /Article
        ///     {
        ///        "Id": "550d5d27-ebc2-42fc-bb59-99b10f287256",
        ///        "ArticleNumber": "at000001",
        ///        "SalesPrice": 445,60
        ///     }
        /// 
        /// </remarks>
        /// <param name="command">command request to replace an article</param>
        /// <param name="cancellationToken">Token to cancel operation</param>
        /// <response code="200">Response is ok</response>
        /// <response code="400">Something unexpected went wrong, or the validation was not correct</response>
        /// <response code="404">Data not available</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Update(UpdateArticleCommand command, CancellationToken cancellationToken)
        {
            return await HandleCommandWithOkResponse(command, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the SalesPrice of an existing article.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PATCH /Article
        ///     {
        ///        "Id": "550d5d27-ebc2-42fc-bb59-99b10f287256",
        ///        "SalesPrice": 445,60
        ///     }
        /// 
        /// </remarks>
        /// <param name="command">command request to update an article</param>
        /// <param name="cancellationToken">Token to cancel operation</param>
        /// <response code="400">Something unexpected went wrong, or the validation was not correct</response>
        /// <response code="200">Response is ok</response>
        /// <response code="404">Data not available</response>
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ChangesSalesPrice(UpdateArticleSalesPriceCommand command,
            CancellationToken cancellationToken)
        {
            return await HandleCommandWithOkResponse(command, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sets an article as sold.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PATCH /SellArticle
        ///     {
        ///        "Id": "550d5d27-ebc2-42fc-bb59-99b10f287256",
        ///     }
        /// 
        /// </remarks>
        /// <param name="command">command request to update an article</param>
        /// <param name="cancellationToken">Token to cancel operation</param>
        /// <response code="400">Something unexpected went wrong, or the validation was not correct</response>
        /// <response code="200">Response is ok</response>
        /// <response code="404">Data not available</response>
        [HttpPatch("SellArticle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Article>> SellArticle(UpdateArticleSoldCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
                return Ok(result);
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

        /// <summary>
        /// Updates the SalesPrice of an existing article.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE /Article
        ///     {
        ///        "Id": "550d5d27-ebc2-42fc-bb59-99b10f287256",
        ///     }
        /// 
        /// </remarks>
        /// <param name="command">command request to update an article</param>
        /// <param name="cancellationToken">Token to cancel operation</param>
        /// <response code="400">Something unexpected went wrong, or the validation was not correct</response>
        /// <response code="200">Response is ok</response>
        /// <response code="404">Data not available</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(DeleteArticleCommand command, CancellationToken cancellationToken)
        {
            return await HandleCommandWithOkResponse(command, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override void Log(LogLevel logLevel, string message)
        {
            _logger.Log(LogLevel.Debug, message);
        }
    }
}