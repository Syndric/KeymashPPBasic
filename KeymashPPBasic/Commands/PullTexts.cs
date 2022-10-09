using System;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using KeymashPPBasic.Classes;

namespace KeymashPPBasic.Commands
{
    public static class PullTexts
    {
        public static void Run(string workdir, int start, int end, int delay)
        {
            int min = 1;
            int max = 2406;
            string stream;
            for (int i = min; i <= max; i++)
            {
                stream = Request.Get($"https://api.keymash.io/api/v2/texts/get?textId={i}");
                //parsed = JsonConvert.DeserializeObject(stream);
                File.WriteAllText(Path.Combine(workdir, $"TextContents/{i}.json"), stream);
                Thread.Sleep(delay);
                Console.WriteLine($"Logged:{i} Delay:{delay}");
            }

        }
    }
}