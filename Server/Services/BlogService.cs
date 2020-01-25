using System.Threading.Tasks;
using Grpc.Core;

namespace Server
{
    public class BlogService : Blog.BlogBase
    {
        public override Task<Empty> Create(Article request, ServerCallContext context)
        {
            return base.Create(request, context);
        }

        public override Task<ListOfArticles> List(Empty request, ServerCallContext context)
        {
            return base.List(request, context);
        }
    }
}