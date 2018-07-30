using DataAccess.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Controllers
{
    [Route("api/[Controller]")]
    public class DataAccessController : Controller
    {
        private readonly IHostingEnvironment _hosting;
        // in .net API, the system can provide host setting to recognize path, the same as other configuration
        public DataAccessController(IHostingEnvironment hosting)
        {
            _hosting = hosting;
        }

        private static Dictionary<string, Security> SecurityMaster = new Dictionary<string, Security>();

        [HttpGet("[action]")]
        public async Task<IActionResult> DataFromFile()
        {
            if (SecurityMaster.Count != 0)
            {
                return Ok(SecurityMaster);
            }

            string securityKey = "BABA";

            Security security = new Security(securityKey);
            var x = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            #region Load Security Pricing Data
            //string path = @"D:\Yury\Program\BackTest\DataAccess\DataAccess\DataFile\BABA.txt";
            string path = Path.Combine(_hosting.ContentRootPath, @"DataFile\BABA.txt");
            List <PricingData> ps = new List<PricingData>();

            //Console.WriteLine("Contents of text: ");

            System.IO.File.ReadAllLines(path).ToList().ForEach(line =>
            {
                string[] values = line.Split('\t');
                DateTime date;
                DateTime.TryParse(values[0].Trim(), out date);
                double priceStore;
                double.TryParse(values[1].Trim(), out priceStore);
                ps.Add(new PricingData(date, priceStore));
                Console.WriteLine(ps.Last().Date.ToString() + " " + ps.Last().ClosePrice.ToString());
            });
            #endregion Load Security Pricing Data
            security.SecurityPricingData = ps;

            // securityMaster means add to securityMaster directly
            // SecurityMaster means execute SecurityMaster Get method again, and then add to securityMaster
            SecurityMaster.Add(securityKey, security);

            return Ok(SecurityMaster);
          
        }
    }
}
