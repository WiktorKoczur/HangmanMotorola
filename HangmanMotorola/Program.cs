using System.Collections.Generic;
using System;
using System.Text;

namespace HangmanMotorola
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> countryAndCapital = new List<string>();
            countryAndCapital.Add("Afghanistan | Kabul");
            countryAndCapital.Add("Albania | Tirana");

            var rand = new Random();
            int chosenCountryNumber = rand.Next(2);
            int playerLifePoints = 5;

            Console.WriteLine("Hangman game! Guess a capital of a random country. Each dash represents a letter of random capital city.  You have for a start " + playerLifePoints + " life points.");


            string stringCountry = countryAndCapital[chosenCountryNumber];

            //litery stolic i krajow
            char[] lettersOfCountries = new char[stringCountry.Length];
            char[] lettersOfCapital = new char[stringCountry.Length];
            char[] dashesOfCapital = new char[stringCountry.Length];

            StringBuilder theCountry = new StringBuilder();
            StringBuilder theCapital = new StringBuilder();
            StringBuilder misses = new StringBuilder();

            //taking names of countries and capitals from file to variables
            fileNamesConversion(stringCountry, lettersOfCountries, lettersOfCapital, dashesOfCapital, theCountry, theCapital);

            //showing dashes of a capital
            dashes(dashesOfCapital);

            Console.WriteLine("Would you like to guess a letter or a whole word? Enter a word or a letter: ");

            //guessing game logic
            guessingLetters(playerLifePoints, theCapital, misses, theCountry, lettersOfCapital, dashesOfCapital);
        }


        public static void fileNamesConversion(string stringCountry, char[] lettersOfCountries, char[] lettersOfCapital, char[]
            dashesOfCapital, StringBuilder theCountry, StringBuilder theCapital){

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

        public static void guessingLetters(int playerLifePoints, StringBuilder theCapital, StringBuilder misses, 
            StringBuilder theCountry, char[] lettersOfCapital, char[] dashesOfCapital){
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
                            Console.WriteLine("Be careful, that's your life point. A little Hint: The capital of " + theCountry);
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
                            Console.WriteLine("Be careful, that's your life point. A little Hint: The capital of " + theCountry);
                        }
                    }
                }

                dashes(dashesOfCapital);
                Console.WriteLine("After this round your life points = " + playerLifePoints);
                Console.WriteLine();
                decideWinnerOrLoser(theCapital, dashesOfCapital, playerLifePoints);

            }

        }


        //nie dziala
        public static void decideWinnerOrLoser(StringBuilder theCapital, char[] dashesOfCapital, int playerLifePoints){
            
            if ((playerLifePoints > 0) && theCapital.Equals(dashesOfCapital)){ 
                Console.WriteLine("Congratulation, you're the winner. You guessed correctly.");
                Console.WriteLine();
                Console.WriteLine("Do you want to play again?");
            }
            else if (playerLifePoints <= 0){
                Console.WriteLine("Unfortunately you haven't guess the capital");
                Console.WriteLine();
                Console.WriteLine("Do you want to play again?");
            }
        }

        //writing dashes
        public static void dashes(char[] dashesOfCapital)
        {
                for (int i = 0; i < dashesOfCapital.Length; i++)
                {
                    Console.Write(dashesOfCapital[i]+ " ");
                }
                Console.WriteLine();
                Console.WriteLine();
        }
    }
}
