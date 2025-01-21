using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace MyApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        // POST: api/business/sendEmail
        [HttpPost("sendEmail")]
        public async Task<ActionResult> SendEmail()
        {
            int criteria;
            //get info about business

            Business business = new Business("Tasevski Marketing", "https://tasevskimarketing.com", "", 21, 3.4);

            //see if business has website
            if (business.Email != "" && (business.Reviews < 20 || business.Avg_Review < 4.4))
            {
                if (business.Reviews < 20)
                    criteria = 4;
                else
                    criteria = 5;

                //---------------------> sending logic
            }
            else
            {
                if (business.Website != null)
                {
                    //scrape it
                    await WebScraper.ScrapeAsync(business);
                    if(business.Email != null){
                    if (business.hasFb == false && business.hasInsta == false)
                    {
                        criteria = 1;
                    }
                    else if (business.hasFb == false)
                        criteria = 2;
                    else if (business.hasInsta == false)
                        criteria = 3;
                    else criteria = 6;
                    // ----------------------> sending logic
                }
                else{
                     return Ok($"No email found for {business.Name} at {business.Website}.");
                }
                }
                else{
                    return Ok($"No website found for {business.Name}.");
                }
            }
            //log in to email service provider & send mail

            return Ok($"Email sent to {business.Email}");
        }
    }
}
