using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using KeymashPPBasic.Commands;
using System.IO;
/*
 * Order of calculation process:
 * Update
 * Obscurity
 * Density
 * Length
 * Rating
 */
namespace KeymashPPBasic
{
    class Program
    {
        static void Main(string[] args)
        { 
            string workdir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "KeymashBasic/"); //define the working directory for the program

            Console.WriteLine("Initialized. Use \"help\" for list of commands");
            string userInput;
            while(true)
            {
                Console.WriteLine("Input Command:");
                userInput = Console.ReadLine();
                //logic for parsing user input
                string[] tmp = Regex.Split(userInput,
    "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*) (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                string[] inputArgs = tmp.Select(str => str.Replace("\"", "")).ToArray();

                //main function
                switch(inputArgs[0].ToLower())
                {
                    case "pulltxt":
                        PullTexts.Run(workdir, Convert.ToInt32(inputArgs[1]), Convert.ToInt32(inputArgs[2]), Convert.ToInt32(inputArgs[3]));
                        break;
                    case "fcrating":
                        Update.Run(workdir);
                        Obscurity.Run(workdir);
                        Density.Run(workdir);
                        Length.Run(workdir);
                        Rating.Run(workdir, Convert.ToBoolean(inputArgs[1]));
                        break;
                    case "update":
                        Update.Run(workdir);
                        break;
                    case "obscurity":
                        Obscurity.Run(workdir);
                        break;
                    case "density":
                        Density.Run(workdir);
                        break;
                    case "length":
                        Length.Run(workdir);
                        break;
                    case "rating":
                        Rating.Run(workdir, Convert.ToBoolean(inputArgs[1]));
                        break;
                    case "help":
                        List<string> x = new List<string>();
                        x.Add("");
                        x.Add("Do not include <>");
                        x.Add("pulltxt <start end delay> - pull texts via KeymashAPI. Texts start at 1 and end at 2406 (as of oct 8 22)");
                        x.Add("fcrating <includeUnenabled> - calculate all aspects of star rating and star ratings");
                        x.Add("");
                        x.Add("Other commands:");
                        x.Add("update - clear WorkingTexts, transfer TextContents into WorkingTexts");
                        x.Add("obscurity");
                        x.Add("density");
                        x.Add("length");
                        x.Add("rating <includeUnenabled> - calculate star ratings");
                        x.Add("");
                        Console.WriteLine(String.Join("\n", x));
                        break;

                }
            }
        }
    }
}
