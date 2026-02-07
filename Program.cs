using System;
using System.Collections.Generic;
using System.IO;

namespace Assignment1
{
    class MainClass
    {
        [STAThread]
        public static void Main(string[] args)
        {
            // If no arguments, run WPF App
            if (args.Length == 0)
            {
                var app = new App();
                app.InitializeComponent();
                app.Run();
                return;
            }

            // Variables and flags
            string inputFile = string.Empty;
            string outputFile = string.Empty;
            bool appendToFile = false;
            bool displayCount = false;
            bool sortEnabled = false;
            string sortColumnName = string.Empty;

            WeaponCollection results = new WeaponCollection();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-h" || args[i] == "--help")
                {
                    Console.WriteLine("-i <path> or --input <path> : loads the input file path specified (required)");
                    Console.WriteLine("-o <path> or --output <path> : saves result in the output file path specified (optional)");
                    Console.WriteLine("-c or --count : displays the number of entries in the input file (optional).");
                    Console.WriteLine("-a or --append : enables append mode when writing to an existing output file (optional)");
                    Console.WriteLine("-s or --sort <column name> : outputs the results sorted by column name");
                    return;
                }
                else if (args[i] == "-i" || args[i] == "--input")
                {
                    if (args.Length > i + 1)
                    {
                        inputFile = args[++i];
                    }
                }
                else if (args[i] == "-s" || args[i] == "--sort")
                {
                    sortEnabled = true;
                    if (args.Length > i + 1)
                    {
                        sortColumnName = args[++i];
                    }
                }
                else if (args[i] == "-c" || args[i] == "--count")
                {
                    displayCount = true;
                }
                else if (args[i] == "-a" || args[i] == "--append")
                {
                    appendToFile = true;
                }
                else if (args[i] == "-o" || args[i] == "--output")
                {
                    if (args.Length > i + 1)
                    {
                        outputFile = args[++i];
                    }
                }
            }

            if (string.IsNullOrEmpty(inputFile))
            {
                Console.WriteLine("No input file specified.");
                return;
            }

            if (!File.Exists(inputFile))
            {
                Console.WriteLine("The file specified does not exist.");
                return;
            }

            // Load using WeaponCollection
            if (!results.Load(inputFile))
            {
                Console.WriteLine("Failed to load input file.");
                return;
            }

            if (sortEnabled)
            {
                if (string.IsNullOrEmpty(sortColumnName))
                {
                     Console.WriteLine("Sorting by Name (Default)");
                     results.SortBy("Name");
                }
                else
                {
                    Console.WriteLine($"Sorting by {sortColumnName}");
                    results.SortBy(sortColumnName);
                }
            }

            if (displayCount)
            {
                Console.WriteLine("There are {0} entries", results.Count);
            }

            // Output logic
            if (results.Count > 0)
            {
                if (!string.IsNullOrEmpty(outputFile))
                {
                    
                    if (appendToFile)
                    {
                         // Manual append using helper or just writing lines
                         using (StreamWriter writer = File.AppendText(outputFile))
                         {
                             foreach (var weapon in results)
                             {
                                 writer.WriteLine(weapon.ToString());
                             }
                         }
                         Console.WriteLine("The file has been saved (Appended).");
                    }
                    else
                    {
                        if (results.Save(outputFile))
                        {
                            Console.WriteLine("The file has been saved.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to save file.");
                        }
                    }
                }
                else
                {
                    // Print to console
                    Console.WriteLine("Name,Type,Image,Rarity,BaseAttack,SecondaryStat,Passive");
                    foreach (var weapon in results)
                    {
                        Console.WriteLine(weapon.ToString());
                    }
                }
            }
            Console.WriteLine("Done!");
        }
    }
}
