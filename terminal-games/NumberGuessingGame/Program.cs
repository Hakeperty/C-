using System;

class NumberGuessingGame
{
    static void Main()
    {
        PlayGame();
    }

    static void PlayGame()
    {
        // Game settings
        int minNumber = 1;
        int maxNumber = 100;
        int attempts = 0;
        
        // Generate random number
        Random random = new Random();
        int secretNumber = random.Next(minNumber, maxNumber + 1);
        
        // Display welcome message
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔═══════════════════════════════════╗");
        Console.WriteLine("║   NUMBER GUESSING GAME            ║");
        Console.WriteLine("╚═══════════════════════════════════╝");
        Console.ResetColor();
        
        Console.WriteLine($"\nI'm thinking of a number between {minNumber} and {maxNumber}.");
        Console.WriteLine("Can you guess it?\n");
        
        // Game loop
        bool hasWon = false;
        
        while (!hasWon)
        {
            Console.Write("Enter your guess: ");
            
            // Validate input
            if (!int.TryParse(Console.ReadLine(), out int guess))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please enter a valid number!");
                Console.ResetColor();
                continue;
            }
            
            attempts++;
            
            // Check guess
            if (guess < secretNumber)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("📈 Higher! Try again.\n");
                Console.ResetColor();
            }
            else if (guess > secretNumber)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("📉 Lower! Try again.\n");
                Console.ResetColor();
            }
            else
            {
                hasWon = true;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n🎉 Congratulations! You guessed it!");
                Console.WriteLine($"The number was {secretNumber}");
                Console.WriteLine($"It took you {attempts} attempts!");
                Console.ResetColor();
                
                // Rating based on attempts
                if (attempts <= 5)
                {
                    Console.WriteLine("⭐⭐⭐ Excellent!");
                }
                else if (attempts <= 10)
                {
                    Console.WriteLine("⭐⭐ Good job!");
                }
                else
                {
                    Console.WriteLine("⭐ You did it!");
                }
            }
        }
        
        // Play again option
        Console.Write("\nPlay again? (y/n): ");
        string playAgain = Console.ReadLine()?.ToLower() ?? "n";
        
        if (playAgain == "y" || playAgain == "yes")
        {
            PlayGame(); // Restart the game
        }
        else
        {
            Console.WriteLine("\nThanks for playing! Goodbye! 👋");
        }
    }
}
