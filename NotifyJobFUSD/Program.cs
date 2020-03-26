using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Notify
{
    class Program
    {
        DiscordSocketClient _client;

        static void Main(string[] args)
            => new Program().StartAsync().GetAwaiter().GetResult();
         


        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
        public async Task StartAsync()
        {
            _client = new DiscordSocketClient();
            _client.Log += Log;
            await _client.LoginAsync(TokenType.Bot, "NTYyNzQ5MzgxNzUzODMxNDQ4.XnvwFA.UMNoxrV2zuL9q0KHSErg5PWUTD8");
            await _client.StartAsync();
            ReadPage();
            await Task.Delay(-1);

        }
        public async Task ReadPage()
        {
            while (true)
            {
                int counteven = 0;
                int countodd = 0;
                int whereodd = 0;
                int whereeven = 0;
                var url = "https://jobs.fresnounified.org/ats/job_board?COMPANY_ID=00001115";
                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync(url);
                var htmlDoc = new HtmlDocument();
                bool Job = false;
                string date = DateTime.Now.ToString("hh:mm:ss tt");
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
                    counteven++;
                    if (jobs.Contains("Software") == true)
                    {
                        Job = true;
                        whereeven = counteven;
                        break;
                    }

                }
                foreach (var div in oddDivs)
                {
                    var jobsOdd = div.Descendants("td").ElementAt(4).InnerText;
                    countodd++;
                    if (jobsOdd.Contains("Software") == true)
                    {
                        Job = true;
                        whereodd = countodd;
                        break;
                    }
                }
                Console.WriteLine($"{countodd} {counteven} Where at {whereodd} {whereeven} ");
  
                if (Job == true)
                {
                    await _client.GetGuild(692518346809409547).GetTextChannel(692518346809409550).SendMessageAsync(("<@101925060553490432> Job Found!! " + date));
                    await Task.Delay(300000);
                }
                else if(Job == false)
                {
                    await _client.GetGuild(692518346809409547).GetTextChannel(692518346809409550).SendMessageAsync(("Job Not Found! " + date));
                    await Task.Delay(300000);
                }
            }
        }
    }
}
