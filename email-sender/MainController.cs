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
        [HttpHead("sendEmail")]
        public async Task<ActionResult> SendEmail()
        {
            int criteria;
            //get info about business

            Business business = new Business();
///////////////////////////////////////////////////////////////////////

            await BusinessService.ProcessCsv("/app/test.csv", IndexDeterminer.index(), business);


/////////////////////////////////////////////////////////////////////////
            
            Console.WriteLine("Business INITIATED");
            Console.WriteLine(business);
            Console.WriteLine();
            //ako ima vebsajt -> scrape -> ako ima email najdi criteria i send 
            if (business.Website != "" && business.Website != null)
            {
                    await Scraper.ScrapeAsync(business);
                    if(business.Email != null && business.Email != "")
                {
                    if (business.Facebook == "" || business.Instagram == "")
                        {
                            criteria = 1;
                        }
                    else if (business.Reviews < 20)
                        criteria = 2;
                    else if(business.Avg_Review < 4.3)
                        criteria = 3;
                    else criteria = 1;
                    EmailSender.SendEmail(business, criteria);
                    return Ok($"Emails sent for {business.Name} to {business.Email}.");
                    // ----------------------> sending logic
                }
                else
                {
                     return Ok($"No email found for {business.Name} at {business.Website}.");
                }
            }
                //ako nema websajt ama ima email -> send
                else if (business.Email != "" && business.Email != null){
                    criteria = 4;
                    EmailSender.SendEmail(business, criteria);
                    return Ok($"Emails sent for {business.Name} to {business.Email}.");
                    // -----------------------> sending logic
                }
                else{
                    return Ok($"No website or email found for {business.Name}.");
                }
        }
    }
}
