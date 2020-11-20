using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace HangmanMotorola
{
    class Program
    {
        static void Main(string[] args)
        {
            var countries = LoadCountries();
            
            var chosenCountry = ChooseCountry(countries);
            var player = new Player();
            var gameMaster = new GameMaster(chosenCountry, player);
            gameMaster.CurrentTime();

            while (true)
            {
                gameMaster.PrintHint();
                gameMaster.PrintDescription();
                gameMaster.PrintMisses();
                gameMaster.PrintCurrentGuessState();
                gameMaster.PrintGameMenu();
                gameMaster.PrintGameState();
                gameMaster.PaintHangman();

                
                string input = Console.ReadLine();

                gameMaster.AddInput(input.ToLower());
                
                if (input.Equals("1"))
                {
                    chosenCountry = ChooseCountry(countries);
                    player = new Player();
                    gameMaster = new GameMaster(chosenCountry, player);
                    gameMaster.CurrentTime();
                }

                if (input.Equals("2"))
                {
                    break;
                }

                if (input.Equals("3"))
                {
                    gameMaster.AddScore();
                    var environment = System.Environment.CurrentDirectory;
                    string projectDirectory = Directory.GetParent(environment).Parent.Parent.FullName;
                    
                    string newInput = Console.ReadLine();
                    using System.IO.StreamWriter file =
                        new System.IO.StreamWriter(projectDirectory + "/resources/out.txt", true);
                    file.WriteLine(newInput);

                    chosenCountry = ChooseCountry(countries);
                    player = new Player();
                    gameMaster = new GameMaster(chosenCountry, player);
                    gameMaster.CurrentTime();
                }

                Console.Clear();
            }
        }
        
        private static Country ChooseCountry(List<Country> countries)
        {
            var rand = new Random();
            var chosenCountryNumber = rand.Next(Int32.MaxValue) % countries.Count;

            return countries[chosenCountryNumber];
        }

        private static List<Country> LoadCountries()
        {
            string[] files = LoadDataFile("resources/countries_and_capitals.txt");
            
            var countryAndCapital = new List<string>(files);

            return countryAndCapital.ConvertAll(ConvertStringToCountry);
        }

        private static Country ConvertStringToCountry(string line)
        {
            var data = line.Split('|');
            return new Country(countryName: data[0].Trim(), capital: data[1].Trim());
        }
        
        private static string[] LoadDataFile(string fileName)
        {
            var environment = System.Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(environment).Parent.Parent.FullName;
            return File.ReadAllLines(Path.Combine(projectDirectory, fileName));
            //return File.ReadAllLines(@"C:\Users\Wiktor\RiderProjects\HangmanMotorola\HangmanMotorola\resources\" + fileName);
        }
    }
}

