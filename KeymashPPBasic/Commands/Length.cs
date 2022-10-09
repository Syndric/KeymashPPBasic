using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using KeymashPPBasic.Classes;
namespace KeymashPPBasic.Commands
{
    public class Length
    {
        public static void Run(string workdir)
        {
            string[] fileArray = Directory.GetFiles(Path.Combine(workdir, $"WorkingTexts/"), "*.json"); //get all .json files in folder "TextContents"
            List<Text> texts = new List<Text>();
            string stream; //holding variable for reading from files
            foreach (string path in fileArray)
            {
                stream = File.ReadAllText(path); //reads file
                Text text = JsonConvert.DeserializeObject<Text>(stream); //converts stream into a text object
                texts.Add(text);
            }

            foreach (Text text in texts)
            {
                float lengthDiff = -((float)Math.Pow(1.004, -text.Content.Length + 550f)) + 8f; //curve for length, already mapped
                text.LengthR = lengthDiff;
                stream = JsonConvert.SerializeObject(text);
                File.WriteAllText(Path.Combine(workdir, $"WorkingTexts/{text.TextId}.json"), stream);
                Console.WriteLine("Length, completed text " + texts.IndexOf(text));
            }

            //Create leaderboard for length
            texts.Sort((x, y) => x.LengthR.CompareTo(y.LengthR));
            texts.Reverse();
            string iStream = "Leaderboard for lengthR:";
            foreach (Text text in texts)
            {
                if (text.Locale == "en")
                {
                    stream = $"\n{text.TextId}: {text.LengthR}: {text.Content}";
                    iStream += stream;
                }
            }
            File.WriteAllText(Path.Combine(workdir, $"TextRatings/lengthR.txt"), iStream);
        }
    }
}
