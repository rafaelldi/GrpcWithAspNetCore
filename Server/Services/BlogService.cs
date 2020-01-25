using System.Threading.Tasks;
using Grpc.Core;

namespace Server
{
    public class BlogService : Blog.BlogBase
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

        public override Task<ListOfArticles> List(Empty request, ServerCallContext context)
        {
            var articles = _articleService.Articles;
            var listOfArticles = new ListOfArticles {Articles = { articles }};
            return Task.FromResult(listOfArticles);
        }
    }
}