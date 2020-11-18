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

            while (true)
            {

                gameMaster.PrintHint();
                gameMaster.PrintDescription();
                gameMaster.PrintMisses();
                gameMaster.PrintCurrentGuessState();
                gameMaster.PrintGameMenu();
                gameMaster.PrintGameState();

                string input = Console.ReadLine();
                
                gameMaster.AddInput(input.ToLower());
                
                if (input.Equals("1"))
                {
                    chosenCountry = ChooseCountry(countries);
                    player = new Player();
                    gameMaster = new GameMaster(chosenCountry, player);
                }

                if (input.Equals("2"))
                {
                    break;
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
            string[] files = LoadDataFile("countries_and_capitals.txt");
            
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
            return File.ReadAllLines(@"C:\Users\Wiktor\RiderProjects\HangmanMotorola\HangmanMotorola\resources\" + fileName);
        }
    }

    class GameMaster
    {
        private Country chosenCountry;
        private Player player;
        private List<string> correctInputs = new List<string>();
        private List<string> missesInputs = new List<string>();
        private GameState gameState = GameState.PLAYING;
        
        public GameMaster(Country chosenCountry, Player player)
        {
            this.chosenCountry = chosenCountry;
            this.player = player;
            correctInputs.Add(" ");
        }

        public void PrintHint()
        {
            if (player.LifePoints != 1)
            {
                Console.WriteLine("Hint -> Country name " + chosenCountry.CountryName);
            } 
        }
        public void PrintDescription()
        {
            Console.WriteLine(
                "Hangman game! Guess a capital of a random country. Each dash represents a letter of random capital city.  You have " +
                player.LifePoints + " life points.");
        }
        
        public void PrintCurrentGuessState()
        {
            foreach (var c in chosenCountry.Capital)
            {
                var toPrint = correctInputs.Contains(c.ToString().ToLower()) ? c : '_';
                Console.Write(toPrint);
            }
            Console.WriteLine();
        }

        public void PrintGameMenu()
        {
            PrintAvailableOptions();
            Console.WriteLine("Would you like to guess a letter or a whole word? Enter a word or a letter: ");
        }

        public void PrintAvailableOptions()
        {
            Console.WriteLine("1. restart");
            Console.WriteLine("2. exit");
            Console.WriteLine("3. high scores");
        }

        public void PrintGameState()
        {
            switch (gameState)
            {
                case GameState.WIN:
                    Console.Clear();
                    Console.WriteLine("You won! Capital of " + chosenCountry.CountryName + " is " + chosenCountry.Capital);
                    Console.WriteLine("Please choose one of the below options to proceed");
                    PrintAvailableOptions();
                    break;
                case GameState.LOSE:
                    Console.Clear();
                    Console.WriteLine("You lose! Capital of " + chosenCountry.CountryName + " is " + chosenCountry.Capital);
                    Console.WriteLine("Please choose one of the below options to proceed");
                    PrintAvailableOptions();
                    break;
            }
        }

        public void PrintMisses()
        {
            Console.Write("Misses: ");
            foreach (var miss in missesInputs)
            {
                Console.Write(miss + ", ");
            }
            Console.WriteLine();
        }

        public void AddInput(string input)
        {
            if (input.Length == 1)
            {

                if (correctInputs.Contains(input) || missesInputs.Contains(input))
                    return;

                if (chosenCountry.Capital.ToLower().Contains(input))
                {
                    correctInputs.Add(input);
                    if (checkIfAllLettersInPlace()) gameState = GameState.WIN;
                }
                else
                {
                    player.LifePoints--;
                    missesInputs.Add(input);
                }
            }
            else {
                if (input.Trim().Equals(chosenCountry.Capital.ToLower()))
                    gameState = GameState.WIN;
                else 
                    player.LifePoints -= 2;
            }

            if (player.LifePoints <= 0) gameState = GameState.LOSE;
            
            correctInputs.Add(input.ToLower());
        }

        private bool checkIfAllLettersInPlace()
        {
            foreach (var i in chosenCountry.Capital.ToLower())
            {
                if (!correctInputs.Contains(i.ToString())) return false;
            }

            return true;
        }
        
    }

    enum GameState
    {
        PLAYING, LOSE, WIN
    }
    
    class Player
    {
        public int LifePoints = 5;
        public string Name = null;
        
        
    }

    class Country
    {
        public readonly string CountryName;
        public readonly string Capital;

        public Country(string countryName, string capital)
        {
            CountryName = countryName;
            Capital = capital;
        }
        
    }
}