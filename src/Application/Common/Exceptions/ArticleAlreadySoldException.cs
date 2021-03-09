using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazingArticles.Application.Common.Exceptions
{
    public class ArticleAlreadySoldException : Exception
    {
        public ArticleAlreadySoldException(string message) : base(message)
        {
        }

        public ArticleAlreadySoldException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ArticleAlreadySoldException(string articleName, object key)
            : base($"Article \"{articleName}\" ({key}) is already sold.")
        {
        }
    }
}
