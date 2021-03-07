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
        }
    }
}