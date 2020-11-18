using System.Collections.Generic;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace HangmanMotorola
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] files = LoadDataFile();
            
            var countryAndCapital = new List<string>(files);
            
            var rand = new Random();
            int chosenCountryNumber = rand.Next(Int32.MaxValue) % files.Length;
            int playerLifePoints = 5;
            
            Console.WriteLine(countryAndCapital[chosenCountryNumber]);

            Console.WriteLine(
                "Hangman game! Guess a capital of a random country. Each dash represents a letter of random capital city.  You have for a start " +
                playerLifePoints + " life points.");


            string stringCountry = countryAndCapital[chosenCountryNumber];

            //litery stolic i krajow
            char[] lettersOfCountries = new char[stringCountry.Length];
            char[] lettersOfCapital = new char[stringCountry.Length];
            char[] dashesOfCapital = new char[stringCountry.Length];

            var theCountry = new StringBuilder();
            var theCapital = new StringBuilder();
            var misses = new StringBuilder();

            //taking names of countries and capitals from file to variables
            FileNamesConversion(stringCountry, lettersOfCountries, lettersOfCapital, dashesOfCapital, theCountry,
                theCapital);

            //showing dashes of a capital
            Dashes(dashesOfCapital);

            Console.WriteLine("Would you like to guess a letter or a whole word? Enter a word or a letter: ");

            //guessing game logic
            GuessingLetters(playerLifePoints, theCapital, misses, theCountry, lettersOfCapital, dashesOfCapital);
        }

        private static string[] LoadDataFile()
        {
            return File.ReadAllLines(@"C:\Users\Wiktor\RiderProjects\HangmanMotorola\HangmanMotorola\resources\countries_and_capitals.txt");
        }


        private static void FileNamesConversion(string stringCountry, char[] lettersOfCountries, char[] lettersOfCapital,
            char[] dashesOfCapital, StringBuilder theCountry, StringBuilder theCapital)
        {
            int count = 0;
            bool flag = true;

            foreach (char c in stringCountry)
            {
                if (c == ' ' || c == '|')
                {
                    flag = false;
                    count = 0;
                    continue;
                }

                if (flag)
                {
                    lettersOfCountries[count] = c;
                    theCountry.Append(lettersOfCountries[count]);
                    //Console.WriteLine(lettersOfCountries[count]);
                    count++;
                }
                else
                {
                    lettersOfCapital[count] = c;
                    //Console.WriteLine(lettersOfCapital[count]);
                    dashesOfCapital[count] = '_';
                    theCapital.Append(lettersOfCapital[count]);
                    count++;
                }
            }
        }

        private static void GuessingLetters(int playerLifePoints, StringBuilder theCapital, StringBuilder misses,
            StringBuilder theCountry, char[] lettersOfCapital, char[] dashesOfCapital)
        {
            while (playerLifePoints > 0)
            {
                var letterOrWord = Console.ReadLine();

                Console.WriteLine("Guess: " + letterOrWord);


                bool anotherFlag = false;
                foreach (char c in letterOrWord)
                {
                    for (int i = 0; i < theCapital.Length; i++)
                    {
                        if (lettersOfCapital[i] == c)
                        {
                            dashesOfCapital[i] = c;
                            anotherFlag = true;
                        }
                    }

                    if (letterOrWord.Length > 1 && (anotherFlag == false))
                    {
                        playerLifePoints -= 2;
                        if (playerLifePoints == 1)
                        {
                            Console.WriteLine("Be careful, that's your life point. A little Hint: The capital of " +
                                              theCountry);
                        }

                        break;
                    }
                    else if (anotherFlag == false)
                    {
                        playerLifePoints -= 1;
                        if (playerLifePoints > 0)
                        {
                            misses.Append(c + ", ");
                            Console.WriteLine("Misses: " + misses);
                        }

                        if (playerLifePoints == 1)
                        {
                            Console.WriteLine("Be careful, that's your life point. A little Hint: The capital of " +
                                              theCountry);
                        }
                    }
                }

                Dashes(dashesOfCapital);
                Console.WriteLine("After this round your life points = " + playerLifePoints);
                Console.WriteLine();
                DecideWinnerOrLoser(theCapital, dashesOfCapital, playerLifePoints);
            }
        }


        //nie dziala
        private static void DecideWinnerOrLoser(StringBuilder theCapital, char[] dashesOfCapital, int playerLifePoints)
        {
            if ((playerLifePoints > 0) && theCapital.Equals(dashesOfCapital))
            {
                Console.WriteLine("Congratulation, you're the winner. You guessed correctly.");
                Console.WriteLine();
                Console.WriteLine("Do you want to play again?");
            }
            else if (playerLifePoints <= 0)
            {
                Console.WriteLine("Unfortunately you haven't guess the capital");
                Console.WriteLine();
                Console.WriteLine("Do you want to play again?");
            }
        }

        //writing dashes
        private static void Dashes(char[] dashesOfCapital)
        {
            foreach (var t in dashesOfCapital)
            {
                Console.Write(t + " ");
            }

            Console.WriteLine();
            Console.WriteLine();
        }
    }
}