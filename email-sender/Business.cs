 public class Business
    {
        public string Name { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public int Reviews {get; set;}
        public double Avg_Review{get; set;}

        public bool hasInsta {get;set;}
        public bool hasFb{get;set;}

        public Business(string Name, string Website, string Email, int Reviews, double Avg_Review){
            this.Name = Name;
            this.Website = Website;
            this.Email = Email;
            this.Reviews = Reviews;
            this.Avg_Review = Avg_Review;
            this.hasFb = false;
            this.hasInsta = false;
        }
    }