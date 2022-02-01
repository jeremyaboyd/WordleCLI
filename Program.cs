using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace WordleCLI
{
    class Program
    {
        static string[] words = null;
        static void Main(string[] args)
        {
            words = File.ReadAllLines("words.txt");

            Dictionary<int, int> stats = new Dictionary<int, int>();
            while (Prompt("Do you want to play Wordle? (y/n)").ToUpper().Equals("Y"))
            {
                string randomWord = words[new Random().Next(0, words.Length)];

                Console.Clear();
                Console.WriteLine("[ ] [ ] [ ] [ ] [ ]");
                Console.WriteLine("Discarded Letters: ");
                HashSet<char> discards = new();
                bool won = false;
                int tries = 6;
                while (tries > 0)
                {
                    tries--;
                    string currentGuess = String.Empty;
                    while (currentGuess.Length != 5)
                        currentGuess = Prompt($"Guess ({tries + 1} Left)");

                    var cursorPos = Console.GetCursorPosition();

                    int right = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        Console.SetCursorPosition(1 + (i * 4), 0);
                        if (currentGuess[i] == randomWord[i])
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            right++;
                        }
                        else if (randomWord.Contains(currentGuess[i]))
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            discards.Add(currentGuess[i]);
                        }

                        Console.Write(currentGuess[i]);
                    }

                    Console.ResetColor();
                    Console.SetCursorPosition(19, 1);
                    Console.Write(string.Join(' ', discards.OrderBy(a => a)));
                    Console.SetCursorPosition(cursorPos.Left, cursorPos.Top);

                    if (right == 5)
                    {
                        Console.WriteLine($"YOU DID IT IN ONLY {6 - tries} ATTEMPT{(tries < 5 ? "S" : "")}!");
                        stats.TryAdd(tries + 1, 0);
                        stats[tries + 1]++;
                        won = true;
                        break;
                    }
                }

                if (!won)
                {
                    Console.WriteLine($"Sorry, you didn't quite get it. The word was \"{randomWord}\". Better luck next time.");
                    stats.TryAdd(tries, 0);
                    stats[tries]++;
                }

                DisplayStats(stats);
            }
        }

        static string Prompt(string prompt)
        {
            Console.Write(prompt + " > ");
            return Console.ReadLine();
        }

        static void DisplayStats(Dictionary<int, int> stats)
        {
            for (int i = 0; i < 7; i++)
            {
                Console.WriteLine($"{i} PTS: {(stats.ContainsKey(i) ? stats[i] : 0)}");
            }
        }
    }
}
