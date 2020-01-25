using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Client
{
    public class Worker : BackgroundService
    {
        private const string CreateCommand = "create";
        private const string GetListCommand = "get-list";
        private readonly Blog.BlogClient _client;
        
        public Worker(Blog.BlogClient client)
        {
            _client = client;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Type a command.");
                var command = Console.ReadLine();
                if (command != CreateCommand && command != GetListCommand)
                {
                    Console.WriteLine("Wrong command. Try again.");
                    continue;
                }
                
                if (command is GetListCommand)
                {
                    var articles = await _client.ListAsync(new Empty());
                    Console.WriteLine($"We have {articles.Articles.Count} articles");
                    foreach (var article in articles.Articles)
                    {
                        Console.WriteLine($"Article '{article.Name}' by {article.Author}");
                    }
                }
                else if (command is CreateCommand)
                {
                    Console.WriteLine("Type a name");
                    var name = Console.ReadLine();
                    Console.WriteLine("Type an author");
                    var author = Console.ReadLine();
                    Console.WriteLine("Type a content");
                    var content = Console.ReadLine();

                    var article = new Article {Name = name, Author = author, Content = content};
                    _ = await _client.CreateAsync(article, cancellationToken: stoppingToken);
                }
            }
        }
    }
}
