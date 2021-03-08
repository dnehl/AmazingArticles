using FluentValidation;

namespace AmazingArticles.Application.Articles.Commands.UpdateArticle
{
    public class UpdateArticleCommandValidator : AbstractValidator<UpdateArticleCommand>
    {
        public UpdateArticleCommandValidator()
        {
            RuleFor(v => v.ArticleNumber)
                .MaximumLength(32)
                .NotEmpty();

            RuleFor(v => v.SalesPrice)
                .GreaterThan(0)
                .WithMessage("Sales Price must be greater than 0");
        }
    }
}