using System;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

class Program
{
    static async Task Main(string[] args)
    {
        // Mobil User-Agent belirleniyor
        var userAgent = "Mozilla/5.0 (Linux; Android 10; SM-G975F) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.106 Mobile Safari/537.36";

        var url = "https://news.google.com/home?hl=tr&gl=TR&ceid=TR:tr"; // Google Keşfet sayfasının benzeri bir URL
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);

        var html = await httpClient.GetStringAsync(url);

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        // Haberlere ulaşmaya çalışıyoruz
        var headlines = htmlDocument.DocumentNode
            .SelectNodes("//a") // A tag'leri seçilir
            .Where(node => node.InnerText.Trim() != "") // Boş olmayanlar filtrelenir
            .Select(node => new { Title = node.InnerText.Trim(), Link = node.GetAttributeValue("href", string.Empty) })
            .ToList();

        // JSON formatına dönüştür
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(headlines, Newtonsoft.Json.Formatting.Indented);

        // JSON dosyasına yaz
        var filePath = @"C:\Users\ismail.kocademir\Desktop\Project\NewsScraper\NewsScraper.App\haberler.json";

        await System.IO.File.WriteAllTextAsync(filePath, json);

        Console.WriteLine("Haberler JSON dosyasına yazıldı.");
    }

    public async void Scraper()
    {
        var url = "https://www.google.com/search?q=site:news.google.com";
        using var httpClient = new HttpClient();
        var html = await httpClient.GetStringAsync(url);

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        var headlines = htmlDocument.DocumentNode
            .SelectNodes("//a") // A tag'leri seçilir
            .Where(node => node.InnerText.Trim() != "") // Boş olmayanlar filtrelenir
            .Select(node => new { Title = node.InnerText.Trim(), Link = node.GetAttributeValue("href", string.Empty) })
            .ToList();

        // JSON formatına dönüştür
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(headlines, Newtonsoft.Json.Formatting.Indented);

        // JSON dosyasına yaz
        await System.IO.File.WriteAllTextAsync("haberler.json", json);

        Console.WriteLine("Haberler JSON dosyasına yazıldı.");
    }
}