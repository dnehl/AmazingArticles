using FluentValidation;

namespace AmazingArticles.Application.Articles.Commands.CreateArticle
{
    public class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
    {
        public CreateArticleCommandValidator()
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
