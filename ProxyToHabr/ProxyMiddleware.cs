using HtmlAgilityPack;
using System.Text;

namespace ProxyToHabr
{
    public class ProxyMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly HttpClient _httpClient = new HttpClient();

        public ProxyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var originalUrl = $"https://habr.com{httpContext.Request.Path}{httpContext.Request.QueryString}";

            var originalHtml = await _httpClient.GetStringAsync(originalUrl);

            var doc = new HtmlDocument();

            doc.LoadHtml(originalHtml);

            ModifyHtmlContent(doc);
            //ModifyLinks(doc, _proxyUrl, url);

            httpContext.Response.ContentType = "text/html";
            await httpContext.Response.WriteAsync(doc.DocumentNode.OuterHtml);
        }

        private void ModifyHtmlContent(HtmlDocument doc)
        {
            foreach (var node in doc.DocumentNode.DescendantsAndSelf())
            {
                if (node.NodeType == HtmlNodeType.Text)
                {
                    var text = node.InnerHtml;
                    var words = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < words.Length; i++)
                    {
                        string? word = RemoveSymbols(words[i]);
                        if (word.Length == 6)
                        {
                            words[i] += "™";
                        }
                    }
                    var modifiedText = string.Join(" ", words);
                    node.InnerHtml = modifiedText;
                }
            }
        }

        private string RemoveSymbols(string word)
        {
            HashSet<char> symbols = new HashSet<char>() { '.', ',', '!', '?', ':', '"', '(', ')'};
            var modifiedWord = new StringBuilder();

            foreach (char c in word)
            {
                if (!symbols.Contains(c))
                {
                    modifiedWord.Append(c);
                }
            }
            return modifiedWord.ToString();
        }

    }

    public static class ProxyMiddlewareExtensions
    {
        public static IApplicationBuilder UseProxyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ProxyMiddleware>();
        }
    }
}
