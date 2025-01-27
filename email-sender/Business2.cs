public class Business2
{
    public string Id { get; set; }
    public string GoogleReviews { get; set; }
    public string Rating { get; set; }
    public string Website { get; set; }
    public string Email { get; set; }
    public string Instagram { get; set; }
    public string Facebook { get; set; }

    // Constructor with 8 parameters
    public Business2(string id, string googleReviews, string rating, string website, string email, string instagram, string facebook)
    {
        Id = id;
        GoogleReviews = googleReviews;
        Rating = rating;
        Website = website;
        Email = email;
        Instagram = instagram;
        Facebook = facebook;
    }
    public Business2(){}
}
