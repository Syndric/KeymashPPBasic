using System;
using System.Collections.Generic;
using KeymashPPBasic.Classes;
using System.IO;
using Newtonsoft.Json;

namespace KeymashPPBasic.Commands
{
    public class Update
    {
        public static void Run(string workdir)
        {
            string[] fileArray = Directory.GetFiles(Path.Combine(workdir, $"TextContents/"), "*.json"); //get all .json files in folder "TextContents"
            List<Text> texts = new List<Text>();
            string stream; //holding variable for reading from files
            foreach (string path in fileArray)
            {
                stream = File.ReadAllText(path); //reads file
                Text text = JsonConvert.DeserializeObject<Text>(stream); //converts stream into a text object
                texts.Add(text);
            }

            //delete all texts in workingtexts
            string[] fileArray2 = Directory.GetFiles(Path.Combine(workdir, $"WorkingTexts/"), "*.json"); //get all .json files in folder "TextContents"
            foreach(string path in fileArray2)
            {
                File.Delete(path);
            }

            //write texts
            foreach (Text text in texts)
            {
                stream = JsonConvert.SerializeObject(text);
                File.WriteAllText(Path.Combine(workdir, $"WorkingTexts/{text.TextId}.json"), stream);
                Console.WriteLine("Update, completed text " + texts.IndexOf(text));
                
            }
        }
    }
}
