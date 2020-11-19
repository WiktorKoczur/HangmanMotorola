using System;
using System.Collections.Generic;

namespace HangmanMotorola
{
    public class GameMaster
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
            if (player.LifePoints == 1)
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

        public void AddScore()
        {
            Console.WriteLine("Add score in the following order:" +
                              " name| date | guessing_time | guessing_tries |guessed_word");
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
            Console.WriteLine("3. Add a high scores");
        }

        public void PrintGameState()
        {
            switch (gameState)
            {
                case GameState.WIN:
                    Console.Clear();
                    Console.WriteLine("You won! Capital of " + chosenCountry.CountryName + " is " + chosenCountry.Capital);
                    Console.WriteLine("Your number of tries = " + player.NumberOfTries);
                    Console.WriteLine("Please choose one of the below options to proceed");
                    PrintAvailableOptions();
                    break;
                case GameState.LOSE:
                    Console.Clear();
                    Console.WriteLine("You lose! Capital of " + chosenCountry.CountryName + " is " + chosenCountry.Capital);
                    Console.WriteLine("Your number of tries = " + player.NumberOfTries);
                    Console.WriteLine("Please choose one of the below options to proceed");
                    PrintAvailableOptions();
                    break;
            }
        }

        public void PaintHangman(int lifePoints)
        {
            switch (lifePoints)
            {
                case 6:
                    Console.WriteLine("   +---+\n  |   |\n  O   |\n      |\n      |\n      |\n=========");
                    break;
                case 5:
                    Console.WriteLine("   +---+\n  |   |\n  O   |\n  |   |\n      |\n      |\n=========");
                    break;
                case 4:
                    Console.WriteLine("   +---+\n  |   |\n  O   |\n  |   |\n      |\n      |\n=========");
                    break;
                case 3: 
                    Console.WriteLine("   +---+\n  |   |\n  O   |\n /|   |\n      |\n      |\n=========");
                    break;
                case 2:
                    Console.WriteLine("   +---+\n  |   |\n  O   |\n /|\\  |\n      |\n      |\n=========");
                    break;
                case 1:
                    Console.WriteLine("   +---+\n  |   |\n  O   |\n /|\\  |\n /    |\n      |\n=========");
                    break;
                case 0:
                    Console.WriteLine("   +---+\n  |   |\n  O   |\n /|\\  |\n / \\  |\n      |\n=========");
                    break;
                default:
                    Console.WriteLine("   +---+\n  |   |\n      |\n      |\n      |\n      |\n=========");
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
            player.NumberOfTries++;

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
}