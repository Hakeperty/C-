using System;
using System.Collections.Generic;
using System.Linq;

class Hangman
{
    static string[] words = { "PROGRAMMING", "COMPUTER", "DEVELOPER", "ALGORITHM", "FUNCTION", 
                              "VARIABLE", "CONSOLE", "TERMINAL", "KEYBOARD", "MONITOR" };
    static string wordToGuess;
    static char[] guessedWord;
    static List<char> guessedLetters = new List<char>();
    static int wrongGuesses = 0;
    static int maxWrongGuesses = 6;
    
    static void Main()
    {
        PlayGame();
    }
    
    static void PlayGame()
    {
        InitializeGame();
        
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔═══════════════════════════════════╗");
        Console.WriteLine("║        HANGMAN GAME               ║");
        Console.WriteLine("╚═══════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine("\nGuess the word letter by letter!");
        Console.WriteLine("Press any key to start...");
        Console.ReadKey();
        
        while (wrongGuesses < maxWrongGuesses && !IsWordGuessed())
        {
            DisplayGameState();
            
            Console.Write("\nEnter a letter: ");
            string input = Console.ReadLine()?.ToUpper() ?? "";
            
            if (string.IsNullOrEmpty(input) || input.Length != 1 || !char.IsLetter(input[0]))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please enter a single letter!");
                Console.ResetColor();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                continue;
            }
            
            char guess = input[0];
            
            if (guessedLetters.Contains(guess))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("You already guessed that letter!");
                Console.ResetColor();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                continue;
            }
            
            guessedLetters.Add(guess);
            
            if (wordToGuess.Contains(guess))
            {
                // Correct guess
                for (int i = 0; i < wordToGuess.Length; i++)
                {
                    if (wordToGuess[i] == guess)
                    {
                        guessedWord[i] = guess;
                    }
                }
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✓ Correct!");
                Console.ResetColor();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
                // Wrong guess
                wrongGuesses++;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("✗ Wrong!");
                Console.ResetColor();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
        
        DisplayGameState();
        
        if (IsWordGuessed())
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n🎉 CONGRATULATIONS! YOU WON! 🎉");
            Console.WriteLine($"The word was: {wordToGuess}");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n💀 GAME OVER! 💀");
            Console.WriteLine($"The word was: {wordToGuess}");
            Console.ResetColor();
        }
        
        Console.Write("\nPlay again? (y/n): ");
        string playAgain = Console.ReadLine()?.ToLower() ?? "n";
        
        if (playAgain == "y" || playAgain == "yes")
        {
            PlayGame();
        }
        else
        {
            Console.WriteLine("\nThanks for playing! Goodbye! 👋");
        }
    }
    
    static void InitializeGame()
    {
        Random random = new Random();
        wordToGuess = words[random.Next(words.Length)];
        guessedWord = new char[wordToGuess.Length];
        
        for (int i = 0; i < guessedWord.Length; i++)
        {
            guessedWord[i] = '_';
        }
        
        guessedLetters.Clear();
        wrongGuesses = 0;
    }
    
    static void DisplayGameState()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔═══════════════════════════════════╗");
        Console.WriteLine("║        HANGMAN GAME               ║");
        Console.WriteLine("╚═══════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();
        
        DrawHangman();
        
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Word: ");
        Console.ResetColor();
        
        foreach (char c in guessedWord)
        {
            Console.Write(c + " ");
        }
        
        Console.WriteLine("\n");
        Console.WriteLine($"Wrong guesses: {wrongGuesses}/{maxWrongGuesses}");
        
        if (guessedLetters.Count > 0)
        {
            Console.Write("Guessed letters: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (char letter in guessedLetters.OrderBy(c => c))
            {
                Console.Write(letter + " ");
            }
            Console.ResetColor();
            Console.WriteLine();
        }
    }
    
    static void DrawHangman()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        
        switch (wrongGuesses)
        {
            case 0:
                Console.WriteLine("  +---+");
                Console.WriteLine("  |   |");
                Console.WriteLine("      |");
                Console.WriteLine("      |");
                Console.WriteLine("      |");
                Console.WriteLine("      |");
                Console.WriteLine("=========");
                break;
            case 1:
                Console.WriteLine("  +---+");
                Console.WriteLine("  |   |");
                Console.WriteLine("  O   |");
                Console.WriteLine("      |");
                Console.WriteLine("      |");
                Console.WriteLine("      |");
                Console.WriteLine("=========");
                break;
            case 2:
                Console.WriteLine("  +---+");
                Console.WriteLine("  |   |");
                Console.WriteLine("  O   |");
                Console.WriteLine("  |   |");
                Console.WriteLine("      |");
                Console.WriteLine("      |");
                Console.WriteLine("=========");
                break;
            case 3:
                Console.WriteLine("  +---+");
                Console.WriteLine("  |   |");
                Console.WriteLine("  O   |");
                Console.WriteLine(" /|   |");
                Console.WriteLine("      |");
                Console.WriteLine("      |");
                Console.WriteLine("=========");
                break;
            case 4:
                Console.WriteLine("  +---+");
                Console.WriteLine("  |   |");
                Console.WriteLine("  O   |");
                Console.WriteLine(" /|\\  |");
                Console.WriteLine("      |");
                Console.WriteLine("      |");
                Console.WriteLine("=========");
                break;
            case 5:
                Console.WriteLine("  +---+");
                Console.WriteLine("  |   |");
                Console.WriteLine("  O   |");
                Console.WriteLine(" /|\\  |");
                Console.WriteLine(" /    |");
                Console.WriteLine("      |");
                Console.WriteLine("=========");
                break;
            case 6:
                Console.WriteLine("  +---+");
                Console.WriteLine("  |   |");
                Console.WriteLine("  O   |");
                Console.WriteLine(" /|\\  |");
                Console.WriteLine(" / \\  |");
                Console.WriteLine("      |");
                Console.WriteLine("=========");
                break;
        }
        
        Console.ResetColor();
    }
    
    static bool IsWordGuessed()
    {
        return !guessedWord.Contains('_');
    }
}
