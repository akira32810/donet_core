using System;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using HtmlAgilityPack;
namespace hwapp
{
    class Program
    {
        static void Main(string[] args)
        {
 
            Console.WriteLine(myTime());
            Console.WriteLine(Directory.GetCurrentDirectory());
            // Create a New HttpClient object.
            MainAsync("https://www.google.com").Wait();
        }

        private static async Task<string> MainAsync(string url)
        {
            string sourceCode = string.Empty;
            try
            {

                using (HttpClient client = new HttpClient())
                {
                    
                    sourceCode = await client.GetStringAsync(url);
                }
                Console.WriteLine(Directory.GetCurrentDirectory());
                   File.WriteAllText(Directory.GetCurrentDirectory() + "/data.txt",sourceCode);
               return sourceCode;
            }

            catch (Exception e)
            {
                Console.WriteLine("Something is wrong: " + e.Message);

            }

            return sourceCode;
        }

        private static async Task ParseHtml(string data) 
        {
            HtmlDocument htmldocs = new HtmlDocument();
            await Task.Run(() => htmldocs.LoadHtml(data));
        }


        private static string myTime()
        {
            var time = DateTime.Now.ToString();

            return time;

        }

      
    }
}