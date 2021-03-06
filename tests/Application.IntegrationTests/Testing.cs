using AmazingArticles.Application.Common.Interfaces;
using AmazingArticles.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebUI.Server;

namespace AmazingArticles.Application.IntegrationTests
{
    [SetUpFixture]
    public class Testing
    {
        private static IConfigurationRoot _configuration;
        private static IServiceScopeFactory _scopeFactory;
        private static Mock<IApplicationRepository<Article>> _repositoryMock;
        private static List<Article> _articles;
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();

            var startup = new Startup(_configuration);
            var services = new ServiceCollection();

            services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
                w.EnvironmentName == "Development" &&
                w.ApplicationName == "WebUI.Server"));

            services.AddLogging();

            startup.ConfigureServices(services);
            services.AddSingleton(_ => _repositoryMock.Object);


            // Register testing version
            _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();
            EnsureDatabase();



        }

        private static void EnsureDatabase()
        {
            _articles = new List<Article>();

            _repositoryMock = new Mock<IApplicationRepository<Article>>();
            _repositoryMock.Setup(x => x.GetAll(It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => { return _articles; });
            _repositoryMock.Setup(x => x.Add(It.IsAny<Article>(), It.IsAny<CancellationToken>()))
                .Returns<Article, CancellationToken>((a, _) =>
                {
                    a.Id = Guid.NewGuid();
                    _articles.Add(a);
                    return Task.FromResult(a.Id);
                });

            _repositoryMock.Setup(x => x.Delete(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .Callback<Guid, CancellationToken>((g, _) =>
                {
                    var item = _articles.FirstOrDefault(x => x.Id.Equals(g));

                    if (item != null)
                        _articles.Remove(item);
                });

            _repositoryMock.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .Returns<Guid, CancellationToken>((g, _) =>
                {
                    var item = _articles.FirstOrDefault(x => x.Id.Equals(g));
                    return Task.FromResult(item);
                });

            _repositoryMock.Setup(x => x.Update(It.IsAny<Guid>(), It.IsAny<Article>(), It.IsAny<CancellationToken>()))
                .Returns<Guid, Article, CancellationToken>((g,a, _) =>
                {
                    var item = _articles.FirstOrDefault(x => x.Id.Equals(g));

                    if (item != null)
                    {
                        var index = _articles.IndexOf(item);
                        _articles[index] = a;
                    }

                    return Task.CompletedTask;
                    
                });
        }

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetService<ISender>();

            return await mediator.Send(request);
        }

        public static Article FindArticle(Guid id)
        {
            return _articles.FirstOrDefault(x => x.Id.Equals(id));
        }

        public static void AddArticle(Article a)
        {
            _articles.Add(a);
        }

        public static void ResetArticles()
        {
            _articles = new List<Article>();
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
            _articles = new List<Article>();
        }

        
        public static Task ResetState()
        {
            return Task.CompletedTask;
        }
    }
}
