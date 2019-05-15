using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using TLSNewsScraper.Data;

namespace testApi
{
    class Program
    {
        static void Main(string[] args)
        {
            var html = GetTLSHtml();
            var items = GetItems(html);
            foreach (var item in items)
            {
                Console.WriteLine(item.Date);
            }

            Console.ReadKey(true);
        }

        static string GetTLSHtml()
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

        static List<NewsArticle> GetItems(string html)
        {
            var parser = new HtmlParser();
            IHtmlDocument document = parser.ParseDocument(html);
            var itemDivs = document.QuerySelectorAll(".post");
            List<NewsArticle> items = new List<NewsArticle>();
            foreach (var div in itemDivs)
            {
                //Console.WriteLine(div.InnerHtml);
                NewsArticle item = new NewsArticle();
                var imagelink = div.QuerySelectorAll("a.fancybox").FirstOrDefault();
                if (imagelink != null)
                {
                    item.Image = imagelink.Attributes["href"].Value;

                }

                var h2 = div.QuerySelector("h2");
                var a = h2.QuerySelector("a");
                item.Title=a.Attributes["title"].Value.Substring(18);
               // var b = item.Title.Substring(18);
                 //var fullTitle= splitTitle[0];                var splitTitle = fullTitle.Split("Permanent Link to");


                var h22 = div.QuerySelector("h2");
                var a2 = h22.QuerySelector("a");
                item.Link = a.Attributes["href"].Value;



                items.Add(item);
            }

            return items;
        }

    }
}

