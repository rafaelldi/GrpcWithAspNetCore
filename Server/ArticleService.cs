using System.Collections.Generic;

namespace Server
{
    public class ArticleService
    {
        public readonly List<Article> Articles;

        public ArticleService()
        {
            Articles = new List<Article>();
        }
    }
}