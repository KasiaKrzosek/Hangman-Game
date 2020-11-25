using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
//using static System.Windows.Forms.Application;


namespace Hangman
{    
    class Program
    {
        static void Main()
        {
            Console.Title = ("HangMan Game");

            // randomowe pobieranie linijek 
            Random rand = new Random();
            DateTime thisDay = DateTime.Now;
            // Console.WriteLine(thisDay.ToString("g"));

            //variables
            int randNum = rand.Next(1, 46);
            int counter, count, i;
            counter = count = i = 0;            
            int live = 5;
            string capitals = @"EU country, capitals.txt";
            string lines, line, s;
            string capital = "1";
            string country = "1";
            string path = @"Results.txt";
            //listy zgadywane litery,zle,zle slowa
            List<string> letterGuessed = new List<string>();
            List<string> letterWrongs = new List<string>();
            List<string> WordWrongs = new List<string>();

            // Read the file and display it line by line.  
            System.IO.StreamReader files =
                new System.IO.StreamReader(capitals);
            while ((lines = files.ReadLine()) != null)
            {
                {
                    i++; // For demonstration.
                    string[] parts = lines.Split('|');
                    foreach (string part in parts)
                    {
                        // Console.WriteLine("{0}:{1}", i, part);
                        counter++;
                        if (randNum == i)
                        {
                            if (counter % 2 == 1)
                            {
                                country = (SpacesRemover.NoSpace(part)); //usunac " ", 
                                //Console.WriteLine(SpacesRemover.NoSpace(country));
                            }
                            else
                            {
                                string parte = Convert.ToString(part);
                                string partes = parte.ToLowerInvariant();
                                capital = (SpacesRemover.NoSpace(partes)); //usunac " ", 
                               // Console.WriteLine(SpacesRemover.NoSpace(capital));
                            }
                        }
                    }
                }
            }

            //instruction
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Welcome To HangMan Game ;)\n");          
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Guess for a {0} letter long name of European capitals. \n", capital.Length);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("You Have {0} Live. \n\nPlease, write: \nletter, the risk is 1 live or \nword, the risk is 2 live.\nIf in the Capital are same ' ' 'space' there is as '-'.\n", live);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Let's start ;)");

            //licznik czasu start
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            // checks the letter
            Isletter(capital, letterGuessed);
            while (live > 0)
            {
                while ((live == 2) || (live == 1))
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("Capital of {0} is: ", country);
                    break;
                }
                //enter a letter
                Console.ForegroundColor = ConsoleColor.Yellow;
                string input = Console.ReadLine();

                if (letterGuessed.Contains(input))
                {
                    //ta litera juz byla
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You Entered Letter '{0}' already \n", input);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Try a Different Word \n");
                    //pokazuje alfabet, a jakby pokazac liste za kazdym razem tych zlych liter
                    //GetAlphabet(input);
                    count++;
                    continue;
                }
                //counting 
                count++;
                letterGuessed.Add(input);
                //writining letterGuessed
                foreach (string letterGuesseda in letterGuessed)
                {
                    Console.Write(letterGuesseda + " ");
                }
                //Congratulations
                if (IsWord(capital, letterGuessed) || (input == capital))
                {
                    //stop czasu
                    stopWatch.Stop();
                    // Get the elapsed time as a TimeSpan value.
                    TimeSpan ts = stopWatch.Elapsed;

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n \n" + capital);
                    Console.WriteLine("\n \nCongratulations! You have right!");

                    // Format and display the TimeSpan value.
                    string elapsedTime = String.Format("{0:00}.{1:00}", ts.Seconds,ts.Milliseconds / 10);                  
                    Console.WriteLine("Your time is " + elapsedTime);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nWhat is Your name?");
                    string userName = Console.ReadLine();
                  
                    //ma sprawdzic plik czy jest, jesli tak -zapisac wartosc na pustym polu, jesli nie utworzyc plik i zapisac wartosc
                    if (!File.Exists(path))
                    {
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path))
                        {
                            Console.WriteLine("{0} | {1} | {2} | {3} | {4} | {5} \n", userName, thisDay.ToString("g"), elapsedTime, count, capital, country);
                            sw.WriteLine("{0} | {1} | {2} | {3} | {4} | {5} ", userName, thisDay.ToString("g"), elapsedTime, count, capital, country);
                            //name | date | guessing_time | guessing_tries | guessed_word | country
                        }
                        // Open the file to read from.
                        using (StreamReader sr = File.OpenText(path))
                        {
                            while ((s = sr.ReadLine()) != null)
                            {
                                Console.WriteLine(s);
                            }
                        }
                    }
                    else
                    {
                        //Append new text to an existing file.
                        using (System.IO.StreamWriter fil =
                            new System.IO.StreamWriter(path, true))
                        {
                            Console.WriteLine("{0} | {1} | {2} | {3} | {4} | {5}\n", userName, thisDay.ToString("g"), elapsedTime, count, capital, country);
                            fil.WriteLine("{0} | {1} | {2} | {3} | {4} | {5} ", userName, thisDay.ToString("g"), elapsedTime, count, capital, country);
                            //name | date | guessing_time | guessing_tries | guessed_word | country
                        }
                        Console.WriteLine("Previous results:");
                        // Read the file and display it line by line.  
                        System.IO.StreamReader file = new System.IO.StreamReader(path);
                        while ((line = file.ReadLine()) != null)
                        {
                            System.Console.WriteLine(line);
                        }
                    }

                    //jesli Yes or yes jesli No or no zakoncz
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("{0} do You want to restart game?\n plise enter Yes or No", userName);
                   // string restart = Console.ReadLine();
                   /* if (restart == "yes")
                    {
                        Program.Restart();
                    }
                   else
                   Program.Exit();
                   */
                    break;
                }
                //gdy powtorka
                else if (capital.Contains(input))
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("\n \n Nice Entry \n");
                    Console.ForegroundColor = ConsoleColor.White;
                    string letters = Isletter(capital, letterGuessed);
                    Console.Write(letters);
                }
                else if (!capital.Contains(input))
                //letterWrong
                {
                    if (input.Length == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n'{0}' is a wrong letter \n", input);
                        letterWrongs.Add(input);

                        foreach (string letterWrong in letterWrongs)
                        {
                            Console.Write(letterWrong + " ");
                        }
                        live -= 1;
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("\n You Have {0} Live", live);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Wrong word \n");
                        WordWrongs.Add(input);

                        foreach (string WordWrong in WordWrongs)
                        {
                            Console.Write(WordWrong + " ");
                        }
                        live -= 2;
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("\n You Have {0} Live", live);
                    }
                }
                Console.WriteLine();
                if (live == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Game Over");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("The '{0}'", capital);

                    //jesli Yes or yes jesli No or no zakoncz
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Do You want to restart game?\n plise enter Yes or No");
                    // string restart = Console.ReadLine();
                    /* if (restart == "yes")
                     {
                         Program.Restart();
                     }
                    else
                    Program.Exit();
                    */
                    break;
                }
            }
            Console.ReadKey();
        }


        //odwolanie czy slowo sie zgadza
        static bool IsWord(string capital, List<string> letterGuessed)
        {
            bool word = false;
            for (int i = 0; i < capital.Length; i++)
            {
                string c = Convert.ToString(capital[i]);
                if (letterGuessed.Contains(c))
                {
                    word = true;
                }
                else
                {
                    return word = false;
                }
            }
            return word;
        }

        static string Isletter(string capital, List<string> letterGuessed)
        {
            string correctletters = "";
            for (int i = 0; i < capital.Length; i++)
            {
                string c = Convert.ToString(capital[i]);
                if (letterGuessed.Contains(c))
                {
                    correctletters += c;
                }
                else
                {
                    correctletters += "_ ";
                }
            }
            return correctletters;
        }

        public static class SpacesRemover
        {
            public static string NoSpace(string input)
            {
                return input.Replace(" ", "");
            }
        }       
        //public void Restart();
    }
}
