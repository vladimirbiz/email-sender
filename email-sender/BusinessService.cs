using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class BusinessService
{
    public static async Task ProcessCsv(string csvFilePath, int index, Business x)
{
    Console.WriteLine("In ProcessCsv");  // Confirm the method is being called
    
    if (!File.Exists(csvFilePath))
    {
        Console.WriteLine($"Error: The file at path {csvFilePath} does not exist.");
        return;  // Exit if the file doesn't exist
    }

    var rowList = new List<Business>();
    var rows = new List<string[]>();

    try
    {
        using (var reader = new StreamReader(csvFilePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<dynamic>().ToList();
            Console.WriteLine($"Number of records: {records.Count}");

            foreach (var (row, i) in records.Select((value, i) => (value, i)))
            {
                Console.WriteLine($"Processing row {i}...");  // Debug row index

                var rowDict = row as IDictionary<string, object>;

                if (rowDict == null) continue; // Skip invalid rows

                var website = rowDict.ContainsKey("Website") ? rowDict["Website"]?.ToString() : null;
                var email = rowDict.ContainsKey("Email") ? rowDict["Email"]?.ToString() : null;
                var instagramStatus = rowDict.ContainsKey("Instagram") ? rowDict["Instagram"]?.ToString() : "False";
                var facebookStatus = rowDict.ContainsKey("Facebook") ? rowDict["Facebook"]?.ToString() : "False";

                int reviews = 0;
                if (rowDict.ContainsKey("Google Reviews") && int.TryParse(rowDict["Google Reviews"]?.ToString(), out var parsedReviews))
                {
                    reviews = parsedReviews;
                }

                double avgReview = 0;
                if (rowDict.ContainsKey("Rating") && double.TryParse(rowDict["Rating"]?.ToString(), out var parsedAvgReview))
                {
                    avgReview = parsedAvgReview;
                }

                if (i == 0) // header row
                {
                    rows.Add(rowDict.Values.Cast<string>().ToArray());
                }

                if (i == index)
                {
                    Console.WriteLine("In ProcessCsv3");  // Check if this is reached
                    x.Reviews = reviews;
                    x.Avg_Review = avgReview;
                    x.Website = website;
                    x.Email = email;
                    x.Instagram = instagramStatus;
                    x.Facebook = facebookStatus;
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
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
