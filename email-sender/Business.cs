 public class Business
    {
        public string Name { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public int Reviews {get; set;}
        public double Avg_Review{get; set;}
        public string Id { get; set; }
        public string Instagram { get; set; }
        public string Facebook { get; set; }


        public Business(string Name, string Website, string Email, int Reviews, double Avg_Review){
            this.Name = Name;
            this.Website = Website;
            this.Email = Email;
            this.Reviews = Reviews;
            this.Avg_Review = Avg_Review;
        }

        public Business(string id, int googleReviews, double rating, string website, string email, string instagram, string facebook)
    {
        Id = id;
        Reviews = googleReviews;
        Avg_Review = rating;
        Website = website;
        Email = email;
        Instagram = instagram;
        Facebook = facebook;
    }
        public Business(){}

        public override string ToString()
    {
        return $"Business Name: {Name}\n" +
               $"Website: {Website}\n" +
               $"Email: {Email}\n" +
               $"Reviews: {Reviews}\n" +
               $"Average Rating: {Avg_Review:F1}\n" +
               $"ID: {Id}\n" +
               $"Instagram: {Instagram}\n" +
               $"Facebook: {Facebook}";
    }
    }