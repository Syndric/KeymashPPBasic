using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using KeymashPPBasic.Classes;
namespace KeymashPPBasic.Commands
{
    public class Rating
    {
        public static void Run(string workdir, bool includeUnenabled)
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
                if (text.Locale == "en" && (text.Enabled == 1 || includeUnenabled))
                {
                    float calc = (0.45f * text.LengthR) + (0.27f * text.Obscurity) + (0.28f * text.Density);
                    float star = (float)Math.Pow(calc, 1.2);
                    text.StarRating = star;
                    if (text.TextId == 1)
                    {
                        text.StarRating = 4.8f;
                    }
                    stream = JsonConvert.SerializeObject(text);
                    File.WriteAllText(Path.Combine(workdir, $"WorkingTexts/{text.TextId}.json"), stream);
                    Console.WriteLine("Rating, completed text " + texts.IndexOf(text));
                }
            }

            //Create leaderboard for starratings
            texts.Sort((x, y) => x.StarRating.CompareTo(y.StarRating));
            texts.Reverse();
            string iStream = "Leaderboard for star rating:";
            foreach (Text text in texts)
            {
                if (text.Locale == "en" && (text.Enabled == 1 || includeUnenabled))
                {
                    stream = $"\n{text.TextId}: {text.StarRating}: {text.Content}";
                    iStream += stream;
                }
            }
            File.WriteAllText(Path.Combine(workdir, $"TextRatings/starratings.txt"), iStream);

            //Create CSV
            iStream = "textid|text|obscurity|density|lengthr";
            foreach (Text text in texts)
            {
                if (text.Locale == "en" && (text.Enabled == 1 || includeUnenabled))
                {
                    stream = $"\n{text.TextId}|{text.Content}|{text.Obscurity}|{text.Density}|{text.LengthR}";
                    iStream += stream;
                }
            }
            File.WriteAllText(Path.Combine(workdir, $"TextRatings/starratings.csv"), iStream);

            //Create file to work with Dubs' star python program
            iStream = "{";
            foreach (Text text in texts)
            {
                iStream += $"\"{text.TextId}\": {text.StarRating},";
            }
            iStream = iStream.Remove(iStream.Length - 1);
            iStream += "}";
            File.WriteAllText(Path.Combine(workdir, $"TextRatings/starratingsDubs.json"), iStream);
        }
    }
}
