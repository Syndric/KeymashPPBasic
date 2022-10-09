using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using KeymashPPBasic.Classes;
namespace KeymashPPBasic.Commands
{
    public class Density
    {
        public static void Run(string workdir)
        {
            List<Tuple<string, int, bool>> symbols = new List<Tuple<string, int, bool>> {
                //bool cond: apply buff if in series with anything else that's true
                new Tuple<string, int, bool>(",", 1, true),
                new Tuple<string, int, bool>(".", 1, true), //note: case in code to make sure ellipsis does not get buffed three times
                new Tuple<string, int, bool>("'", 1, true),

                new Tuple<string, int, bool>(";", 3, true),
                new Tuple<string, int, bool>("1", 4, false),
                new Tuple<string, int, bool>("2", 4, false),
                new Tuple<string, int, bool>("3", 4, false),
                new Tuple<string, int, bool>("4", 4, false),
                new Tuple<string, int, bool>("5", 4, false),
                new Tuple<string, int, bool>("6", 4, false),
                new Tuple<string, int, bool>("7", 4, false),
                new Tuple<string, int, bool>("8", 4, false),
                new Tuple<string, int, bool>("9", 4, false),
                new Tuple<string, int, bool>("0", 4, false),

                new Tuple<string, int, bool>(":", 5, true),
                new Tuple<string, int, bool>("?", 5, true),
                new Tuple<string, int, bool>("!", 5, true),

                new Tuple<string, int, bool>("-", 7, true),

                new Tuple<string, int, bool>("...", 10, true),

                new Tuple<string, int, bool>("\"", 20, true),

                new Tuple<string, int, bool>("(", 30, true),
                new Tuple<string, int, bool>(")", 30, true),
                new Tuple<string, int, bool>("/", 30, true),
                new Tuple<string, int, bool>("[", 30, true),
                new Tuple<string, int, bool>("]", 30, true),
                new Tuple<string, int, bool>("#", 30, true),
                new Tuple<string, int, bool>("$", 30, true),
                new Tuple<string, int, bool>("%", 30, true),
                new Tuple<string, int, bool>("&", 30, true),

                //offset capital letter buff
                new Tuple<string, int, bool>(" I ", -5, true),
                new Tuple<string, int, bool>(" A ", -5, true),
                //new Tuple<string, int, bool>(char.IsUpper, 5, true),
            };
            string[] fileArray = Directory.GetFiles(Path.Combine(workdir, $"WorkingTexts/"), "*.json"); //get all .json files in folder "TextContents"
            List<Text> texts = new List<Text>();
            string stream; //holding variable for reading from files
            foreach (string path in fileArray)
            {
                stream = File.ReadAllText(path); //reads file
                Text text = JsonConvert.DeserializeObject<Text>(stream); //converts stream into a text object
                texts.Add(text);
            }

            //main calculation function
            foreach (Text text in texts)
            {
                int score = 0;
                int freq;
                //add points for each capital letter
                freq = text.Content.Count(c => char.IsUpper(c));
                score += (freq * 5);

                //apply points for each symbol in list
                foreach (Tuple<string, int, bool> symbol in symbols)
                {
                    freq = text.Content.Split(symbol.Item1).Length - 1;
                    score += freq * symbol.Item2;
                }

                //apply points if two symbols are in a row
                for (int i = 0; i < text.Content.Length - 1; i++)
                {
                    string c = char.ToString(text.Content[i]);
                    string aft = char.ToString(text.Content[i + 1]);
                    if (symbols.Exists(x => x.Item1 == c))
                    {
                        Tuple<string, int, bool> first = symbols.Find(x => x.Item1 == c);
                        if (symbols.Exists(x => x.Item1 == aft))
                        {
                            Tuple<string, int, bool> second = symbols.Find(x => x.Item1 == aft);
                            if (first.Item3 && second.Item3 && (first.Item1 != second.Item1)) //excludes ellipisis
                            {
                                score += 10;
                            }
                        }
                    }
                }
                float density = (float)Math.Pow((float)score / (float)text.Content.Length * 10f, 0.4f) * 3.8f; //Map density rating
                text.Density = density;
                stream = JsonConvert.SerializeObject(text);
                File.WriteAllText(Path.Combine(workdir, $"WorkingTexts/{text.TextId}.json"), stream); //store texts
                Console.WriteLine("Density, completed text " + texts.IndexOf(text));
            }

            //Create leaderboard for density
            texts.Sort((x, y) => x.Density.CompareTo(y.Density));
            texts.Reverse();
            string iStream = "Leaderboard for densities:";
            foreach (Text text in texts)
            {
                if (text.Locale == "en")
                {
                    stream = $"\n{text.TextId}: {text.Density}: {text.Content}";
                    iStream += stream;
                }
            }
            File.WriteAllText(Path.Combine(workdir, $"TextRatings/densities.txt"), iStream);
        }
    }
}
