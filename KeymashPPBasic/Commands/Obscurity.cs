using System;
using System.Collections.Generic;
using KeymashPPBasic.Classes;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace KeymashPPBasic.Commands
{
    public class Obscurity
    {
        internal static string Filter(string s)
        {
            /* This filter ignores all punctuation and spaces. I could have used
             * regexp, but I wanted to make it long just so it was more clear what 
             * I was replacing what with.
             */
            string manip = s;
            manip = Regex.Replace(manip, @"\r\n?|\n", " "); //Remove newlines and all that junk, replace with a space
            manip = manip.Replace("_", ""); //Remove italics
            manip = manip.Replace("--", " "); //En dashes
            manip = manip.Replace("“", "\""); //Remove quote marks
            manip = manip.Replace("”", "\"");
            manip = manip.Replace("’", "\'"); //Remove apostrophes
            manip = manip.Replace("\"", ""); //Remove quote marks
            manip = manip.Replace("\'", ""); //Remove apostrophes
            manip = manip.Replace("(", ""); //Remove parentheses
            manip = manip.Replace(")", "");
            manip = manip.Replace("[", ""); //Remove brackets
            manip = manip.Replace("]", "");
            manip = manip.Replace("/", " "); //Replace dashes with space
            manip = manip.Replace(",", ""); //Remove commas
            manip = manip.Replace(".", ""); //Remove periods
            manip = manip.Replace(";", ""); //Remove semicolons
            manip = manip.Replace(":", ""); //Remove colons
            manip = manip.Replace("?", ""); //Remove question marks
            manip = manip.Replace("!", ""); //Remove exclamation marks
            return manip;
        }

        public static KeyCombo Process(string s, string charbefore)
        {
            // Contains logic used for the obscurity formulation
            Regex r = new Regex("[^a-zA-Z ]+"); //Only letters and spaces
            if (r.IsMatch(s))
            {
                return new KeyCombo("", 1);
            }
            /* LOGIC
            * 1: no spaces contained - process full length
            * 2: spaces contained
            * 2.1: cursor is a space - disregard combo
            * 2.2: cursor is not a space
            * 2.2.1: cursor is at start of word - process full word
            * 2.2.2: cursor is not at start of word - disregard word
            */
            string processed;
            if (s.Contains(" "))
            { //2
                if (s.Substring(0, 1) == " ")
                { //2.1
                    return new KeyCombo("", 1);
                }
                else
                { //2.2
                    if (charbefore == " ")
                    { //2.2.1
                        int pos = s.IndexOf(" ");
                        processed = s.Substring(0, pos);
                    }
                    else
                    { //2.2.2
                        return new KeyCombo("", 1);
                    }
                }
            }
            else
            { //1
                processed = s;
            }
            return new KeyCombo(processed, 1);

        }

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

            stream = File.ReadAllText(Path.Combine(workdir, $"combos.json")); //get the list of all the 5-letter combos.
            List<KeyCombo> combos = JsonConvert.DeserializeObject<List<KeyCombo>>(stream); //parse the combo list
            /* A litte bit more about the combo list:
             * Basically, the combo list is a frequency list of letter combos, up to five letters long. 
             * It ignores all punctuation, spacing, and is case sensitive.
             * A word such as "the" will appear in the combo list as "the".
             * A word such as "however" will appear in the combo list as "howev", "oweve", "wever"
             * 
             * The combos were grabbed from books from the Gutenberg project, which were downloaded randomly.
             * I made sure that they were all english. If you'd like to download some books for yourself, the
             * API call is https://www.gutenberg.org/cache/epub/{num}/pg{num}.txt where num is 1-70000.
             */

            //Now, for the main part of the program.
            foreach (Text text in texts)
            {
                float obscurity;
                string content = Filter(text.Content); //pass the text through the filter
                int comboLength = 5; //only change if you use a different combo set

                List<float> charratings = new List<float>();
                for(int i = 1; i <= content.Length - comboLength; i++)
                {
                    //return a keycombo with Count=1 for the next <comboLength> characters. Previous character is required
                    KeyCombo combo = Process(content.Substring(i, comboLength), content.Substring(i - 1, 1));
                    if (combo.Combo != "") //keycombo is "" if it's invalid
                    {
                        //This part is probably inefficient, but it works and it's fast enough for me
                        if (combos.Exists(x => x.Combo == combo.Combo)) //Find matching combo from the list
                        {
                            KeyCombo c = combos.Find(x => x.Combo == combo.Combo);
                            float a = (float)Math.Pow(c.Count / 400f, 0.10); //You will need to change values if you use your own combo set
                            charratings.Add(a);
                        }
                        else
                        {
                            charratings.Add(0f);
                        }
                    }
                }
                float add = 0f;
                foreach (float rating in charratings)
                {
                    add += rating;
                }
                obscurity = 25 - (add * 14 / charratings.Count);
                /* Maps obscurity
                 * The more "obscure" texts will have a smaller (very small decimal) rating,
                 * while less "obscure" texts will have a bigger (very small decimal) rating.
                 * This method inverses it and maps it to the ratings we want
                 */
                text.Obscurity = obscurity;
                stream = JsonConvert.SerializeObject(text);
                File.WriteAllText(Path.Combine(workdir, $"WorkingTexts/{text.TextId}.json"), stream); //store texts
                Console.WriteLine("Obscurity, completed text " + texts.IndexOf(text));
            }

            //Create leaderboard for obscurity
            texts.Sort((x, y) => x.Obscurity.CompareTo(y.Obscurity));
            texts.Reverse();
            string iStream = "Leaderboard for obscurity:";
            foreach (Text text in texts)
            {
                if (text.Locale == "en")
                {
                    stream = $"\n{text.TextId}: {text.Obscurity}: {text.Content}";
                    iStream += stream;
                }
            }
            File.WriteAllText(Path.Combine(workdir, $"TextRatings/obscurity.txt"), iStream);

        }
    }
}
