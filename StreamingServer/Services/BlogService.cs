using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace StreamingServer.Services
{
    public partial class BlogService : BlogServer.BlogServerBase
    {
        private readonly ArticleService _articleService;

        public BlogService(ArticleService articleService)
        {
            _articleService = articleService;
        }
        
        public override Task<Empty> Create(Article article, ServerCallContext context)
        {
            _articleService.Articles.Add(article);
            return Task.FromResult(new Empty());
        }

        public override async Task List(Empty request, IServerStreamWriter<Article> responseStream, ServerCallContext context)
        {
            var articles = _articleService.Articles;
            foreach (var article in articles)
            {
                if (context.CancellationToken.IsCancellationRequested)
                {
                    break;
                }

                await responseStream.WriteAsync(article);

                await Task.Delay(1000);
            }
        }
    }
}