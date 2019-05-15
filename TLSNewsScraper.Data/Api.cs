using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace TLSNewsScraper.Data
{
    public class Api
    {
        public static List<NewsArticle> ScrapeTLS(string query)
        {
            var html = GetTLSHtml();
            return GetItems(html);
        }

        public static string GetTLSHtml()
        {
            var handler = new HttpClientHandler();
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("user-agent", "amazon is trash");
                var url = $"https://www.thelakewoodscoop.com";
                var html = client.GetStringAsync(url).Result;
                return html;
            }
        }

        public static List<NewsArticle> GetItems(string html)
        {
            var parser = new HtmlParser();
            IHtmlDocument document = parser.ParseDocument(html);
            var itemDivs = document.QuerySelectorAll(".post");
            List<NewsArticle> items = new List<NewsArticle>();
            foreach (var div in itemDivs)
            {
                NewsArticle item = new NewsArticle();
                //var date = div.QuerySelectorAll("small").FirstOrDefault();
                //if (date!=null)
                //{
                //item.Date = date;
                //}
                var date = div.QuerySelector("div.postmetadata-top");
                item.Date = date.TextContent.Trim();
                var imagelink = div.QuerySelectorAll("a.fancybox").FirstOrDefault();
                if (imagelink != null)
                {
                    item.Image = imagelink.Attributes["href"].Value;

                }

                var h2 = div.QuerySelector("h2");
                var a = h2.QuerySelector("a");
                item.Title = a.Attributes["title"].Value.Substring(18);

                var h22 = div.QuerySelector("h2");
                var a2 = h22.QuerySelector("a");
                item.Link = a.Attributes["href"].Value;

                items.Add(item);
            }

            return items;
        }
    }
}
