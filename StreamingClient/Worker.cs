using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Hosting;

namespace StreamingClient
{
    public class Worker : BackgroundService
    {
        private const string CreateCommand = "create";
        private const string GetListCommand = "get-list";
        private readonly BlogServer.BlogServerClient _client;
        
        public Worker(BlogServer.BlogServerClient client)
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
                    using var articles = _client.List(new Empty());
                    await foreach (var article in articles.ResponseStream.ReadAllAsync(stoppingToken))
                    {
                        Console.WriteLine($"Time: {DateTime.Now:HH:mm:ss}");
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
                    await _client.CreateAsync(article, cancellationToken: stoppingToken);
                }
            }
        }
    }
}
