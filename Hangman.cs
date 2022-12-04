using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace HangmanGame {
    public class Hangman{
        private static ArrayList STAGES = new ArrayList() {
            "  +---+\n  |   |\n      |\n      |\n      |\n      |\n=========\n",
            "  +---+\n  |   |\n  O   |\n      |\n      |\n      |\n=========\n",
            "  +---+\n  |   |\n  O   |\n  |   |\n      |\n      |\n=========\n",
            "  +---+\n  |   |\n  O   |\n /|   |\n      |\n      |\n=========\n",
            "  +---+\n  |   |\n  O   |\n /|\\  |\n      |\n      |\n=========\n",
            "  +---+\n  |   |\n  O   |\n /|\\  |\n /    |\n      |\n=========\n",
            "  +---+\n  |   |\n  O   |\n /|\\  |\n / \\  |\n      |\n=========\n"

        };
        private const int LIVES = 6;

        private static int replace(char[] letters, string word, char guess) {
            int count = 0;
            for (int i = 0; i < word.Length; i++) {
                if (word[i] == guess) {
                    letters[i] = guess;
                    count++;
                }
            }

            return count;
        }

        private static void printLetters(char[] letters) {
            for (int i = 0; i < letters.Length; i++) {
                Console.Write(letters[i]);
                Console.Write(' ');
            }
            Console.Write('\n');
        }

        public static void play() {
            // read the list of words
            StreamReader reader = new StreamReader("words.txt");
            string content = reader.ReadToEnd();
            string[] words = content.Split(",");
            
            // pick a random word
            Random random = new Random();
            string word = words[random.Next(0, words.Length)];
            word = word.Trim('"').ToUpper();

            // create a representation of the word
            int toGuess = word.Length;
            char[] letters = new char[word.Length];
            for (int i = 0; i < word.Length; i++) {
                letters[i] = '_';
            }
            toGuess -= replace(letters, word, word[0]);
            if (word[0] != word[word.Length - 1])
                toGuess -= replace(letters, word, word[word.Length - 1]);

            // the set of characters in the word
            HashSet<char> chars = new HashSet<char>();
            for (int i = 0; i < word.Length; i++) {
                chars.Add(Char.ToUpper(word[i]));
            }

            // game loop
            int mistakes = 0;
            HashSet<char> guesses = new HashSet<char>();

            while (mistakes < LIVES && toGuess != 0) {
                // print the stage
                Console.WriteLine(STAGES[mistakes]);
                printLetters(letters);

                // read a character from the user
                char guess = Console.ReadLine()[0];
                guess = Char.ToUpper(guess);
                if (!chars.Contains(guess) || guesses.Contains(guess)) {
                    mistakes++;
                } else {
                    toGuess -= replace(letters, word, guess);
                }
                guesses.Add(guess);
            }

            Console.WriteLine(STAGES[mistakes]);
            if (mistakes == LIVES) {
                Console.WriteLine("You lost!");
                Console.WriteLine("The word was " + word + "!");
            } else {
                printLetters(letters);
                Console.WriteLine("You won!");
            }
        }

        public static void Main(string[] args) {
            string answer = "YES";
            while (answer == "YES") {
                play();
                Console.WriteLine("Do you want to play again? YES/NO");
                answer = Console.ReadLine();
            }
            Console.WriteLine("Thanks for playing!");
        }
    }
}