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
        }
    }
}
