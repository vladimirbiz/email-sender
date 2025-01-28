using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;

public class Scraper
{
    private static HashSet<string> visitedUrls = new HashSet<string>();

    // Extract email addresses from HTML content
    private static HashSet<string> ExtractEmailsFromHtml(string html)
    {
        var emailPattern = @"[a-zA-Z0-9._%+-]+@[a-zA-Z]+\.[a-zA-Z]{2,}";
        var emailMatches = Regex.Matches(html, emailPattern);
        var emails = new HashSet<string>();

        foreach (Match match in emailMatches)
        {
            string email = match.Value;

            // Check if email doesn't end with certain domains or file extensions
            if (!email.EndsWith("wixpress.com") && !email.EndsWith("jpg") &&
                !email.EndsWith("jpeg") && !email.EndsWith("JPG") &&
                !email.EndsWith("JPEG") && !email.EndsWith("PNG") && 
                !email.EndsWith("png"))
            {
                emails.Add(email);
            }
        }

        return emails.Count > 0 ? emails : new HashSet<string>();
    }

    // Check if Instagram account is linked
    private static bool ExtractInstagramFromHtml(string html)
    {
        return Regex.IsMatch(html, "instagram.com");
    }

    // Check if Facebook account is linked
    private static bool ExtractFacebookFromHtml(string html)
    {
        return Regex.IsMatch(html, "facebook.com");
    }

    // Fetch the URL content and extract information
    private static async Task<(HashSet<string>, bool, bool)> GetInfoFromUrlAsync(string url)
    {
        using var client = new HttpClient();
        try
        {
            var response = await client.GetStringAsync(url);
            var emails = ExtractEmailsFromHtml(response);
            var hasInstagram = ExtractInstagramFromHtml(response);
            var hasFacebook = ExtractFacebookFromHtml(response);

            return (emails, hasInstagram, hasFacebook);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Error fetching {url}: {e.Message}");
            return (new HashSet<string>(), false, false);
        }
    }

    // Main scrape function
    public static async 
    // Main scrape function
    Task
ScrapeAsync(Business business)
{
    var innerUrls = await InnerUrlsAsync(business.Website, 2);
    Console.WriteLine($"URLs: {string.Join(", ", innerUrls)}");

    bool hasInsta = false;
    bool hasFb = false;
    var emails = new HashSet<string>();

    foreach (var innerUrl in innerUrls)
    {
        Console.WriteLine($"Processing {innerUrl}...");
        var (emailsFromUrl, hasInstaFromUrl, hasFbFromUrl) = await GetInfoFromUrlAsync(innerUrl);
        foreach (var email in emailsFromUrl)
        {
            emails.Add(email);
        }
        if(hasInstaFromUrl)
        hasInsta = true;
        if(hasFbFromUrl)
        hasFb = true;
    }
    if(hasFb)
    business.Facebook = "facebook";
    if(hasInsta)
    business.Instagram = "instagram";
    if(business.Email == "")
    business.Email = emails.ToArray()[0];
}


    // Recursively scrape inner URLs
    private static async Task<HashSet<string>> InnerUrlsAsync(string url, int depth)
    {
        if (depth == 0 || visitedUrls.Contains(url)) return new HashSet<string>();

        visitedUrls.Add(url);
        var innerUrls = new HashSet<string>();

        using var client = new HttpClient();
        try
        {
            var response = await client.GetStringAsync(url);
            var doc = new HtmlDocument();
            doc.LoadHtml(response);

            // Find all anchor tags with href attributes
            var links = doc.DocumentNode.SelectNodes("//a[@href]")?.Select(link => link.GetAttributeValue("href", string.Empty)).ToList() ?? new List<string>();

            // Filter and process valid links
            foreach (var link in links)
            {
                var fullUrl = new Uri(new Uri(url), link).ToString();
                if (fullUrl.StartsWith(url) && !visitedUrls.Contains(fullUrl))
                {
                    innerUrls.Add(fullUrl);
                    var innerUrlsFromThisLink = await InnerUrlsAsync(fullUrl, depth - 1);
                    foreach (var innerUrl in innerUrlsFromThisLink)
                    {
                        innerUrls.Add(innerUrl);
                    }
                }
            }

        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Failed to retrieve {url}: {e.Message}");
        }

        return innerUrls;
    }
}
