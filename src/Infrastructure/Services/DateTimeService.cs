using AmazingArticles.Application.Common.Interfaces;
using System;

namespace AmazingArticles.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
