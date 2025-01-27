 public interface IInformationScraper
    {
        Task<(string email, bool instagram, bool facebook)> ScrapeAsync(string websiteUrl);
    }