using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;

namespace Notify
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadPage();

            Console.ReadLine();
        }

        private static async Task ReadPage()
        {
            var url = "https://jobs.fresnounified.org/ats/job_board?COMPANY_ID=00001115";
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDoc = new HtmlDocument();
            bool JobFound = false;

            htmlDoc.LoadHtml(html);

            var evenDivs = htmlDoc.DocumentNode.Descendants("tr")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("even")).ToList();

            var oddDivs = htmlDoc.DocumentNode.Descendants("tr")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("even")).ToList();

            foreach (var div in evenDivs)
            {
                var jobs = div.Descendants("td").ElementAt(4).InnerText;
                if (jobs.Contains("Programmer") == true)
                {
                    JobFound = true;
                }

            }

            foreach (var div in oddDivs)
            {
                var jobsOdd = div.Descendants("td").ElementAt(4).InnerText;
                if (jobsOdd.Contains("Programmer") == true)
                {
                    JobFound = true;
                }
            }
            if (JobFound == true)
            {
                Console.WriteLine("Yes!");
            }
            else
            {
                Console.WriteLine("False");
            }
        }
    }
}
