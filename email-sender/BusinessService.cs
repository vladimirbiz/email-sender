using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class BusinessService
{

    public static async 
    Task
ProcessCsv(string csvFilePath, int index, Business2 x)
    {
        var rowList = new List<Business2>();
        var rows = new List<string[]>();

        using (var reader = new StreamReader(csvFilePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<dynamic>().ToList();
            
            // Iterate through each record
            foreach (var (row, i) in records.Select((value, i) => (value, i)))
            {
                var rowArray = row.ToString().Split(',');

                if (i == 0) // header row
                {
                    rows.Add(rowArray);
                }

                if (i == index)
                {
                    // Logic to scrape data when row at specified index needs to be processed
                    if (!string.IsNullOrEmpty(rowArray[14]) && 
                        (string.IsNullOrEmpty(rowArray[26]) || string.IsNullOrEmpty(rowArray[25]) || string.IsNullOrEmpty(rowArray[23])))
                    {
                        // Scrape data
                        var scrapeResults = await Scraper.ScrapeAsync(rowArray[14]);
                        var emailScrape = scrapeResults.email;
                        var instagramScrape = scrapeResults.instagram;
                        var facebookScrape = scrapeResults.facebook;

                        // Update social media scrape status
                        var instagramStatusScraped = instagramScrape ? "True" : "False";
                        var facebookStatusScraped = facebookScrape ? "True" : "False";

                        rowArray[23] = emailScrape; // Update email in the row
                        rows.Add(rowArray); // Add updated row
                    }
                    else
                    {
                        rows.Add(rowArray); // If no scraping needed, just add the original row
                    }

                    // Determine the status for social media
                    var email = rowArray[23];
                    var instagramStatus = string.IsNullOrEmpty(rowArray[26]) ? "False" : rowArray[26];
                    var facebookStatus = string.IsNullOrEmpty(rowArray[25]) ? "False" : rowArray[25];

                    if (string.IsNullOrEmpty(rowArray[23])) email = rowArray[23]; // Update email if empty

                    // Create Business object and add it to the list
                    var business = new Business2(
                        rowArray[0],  // ID
                        rowArray[9],   // Google Reviews
                        rowArray[8],   // Rating
                        rowArray[14],  // Website
                        email,         // Email
                        instagramStatus, // Instagram status
                        facebookStatus  // Facebook status
                    );

                    x = business;
                }
            }
        }
    }

    // Determines criteria based on the business row and social media status
    private int DetermineCriteria(string[] row, string instagram, string facebook)
    {
        int criteria = 6; // Default "No Socials"

        if (instagram == "False" && facebook == "False")
            criteria = 1;  // No Instagram, No Facebook
        else if (instagram == "False")
            criteria = 2;  // Missing Instagram
        else if (facebook == "False")
            criteria = 3;  // Missing Facebook
        else if (string.IsNullOrEmpty(row[8]) || int.Parse(row[8]) < 30)
            criteria = 4;  // Small rating or missing rating
        else if (string.IsNullOrEmpty(row[9]) || float.Parse(row[9]) < 4.4f)
            criteria = 5;  // Low Google Reviews

        return criteria;
    }
}
