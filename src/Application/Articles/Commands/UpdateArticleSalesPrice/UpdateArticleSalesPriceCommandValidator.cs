using AmazingArticles.Application.Articles.Commands.UpdateArticle;
using FluentValidation;

namespace AmazingArticles.Application.Articles.Commands.UpdateArticleSalesPrice
{
    public class UpdateArticleSalesPriceCommandValidator : AbstractValidator<UpdateArticleSalesPriceCommand>
    {
        public UpdateArticleSalesPriceCommandValidator()
        {
            RuleFor(v => v.SalesPrice)
                .GreaterThan(0)
                .WithMessage("Sales Price must be greater than 0");
        }
    }
}
