# C# Terminal Learning Guide

Welcome to the interactive C# learning guide! This guide focuses on learning C# through terminal-based applications and games. Perfect for beginners who want hands-on practice without the complexity of GUIs or game engines.

## Table of Contents

- [Why Learn with Terminal Applications?](#why-learn-with-terminal-applications)
- [Setting Up Your Environment](#setting-up-your-environment)
- [C# Basics Through Terminal](#c-basics-through-terminal)
- [Building Your First Terminal Game](#building-your-first-terminal-game)
- [Terminal Games Collection](#terminal-games-collection)
- [Next Steps](#next-steps)

---

## Why Learn with Terminal Applications?

Terminal applications are perfect for learning because they:

- **Focus on Logic**: No distractions from UI design - pure programming logic
- **Instant Feedback**: See results immediately in the console
- **Easy to Debug**: Simple input/output makes tracking bugs easier
- **Build Strong Foundations**: Core concepts apply to all programming
- **Fun and Interactive**: Games make learning engaging!

---

## Setting Up Your Environment

### Prerequisites

1. **Install .NET SDK** (Latest version recommended)
   ```bash
   # Check if installed
   dotnet --version
   ```
   
   If not installed, download from: https://dotnet.microsoft.com/download

2. **Choose a Text Editor**
   - Visual Studio Code (recommended for beginners)
   - Visual Studio
   - JetBrains Rider
   - Even a simple text editor works!

3. **Terminal/Command Prompt**
   - Windows: Command Prompt or PowerShell
   - macOS/Linux: Terminal

### Creating Your First Project

```bash
# Create a new console application
dotnet new console -n MyFirstGame

# Navigate to the project
cd MyFirstGame

# Run the application
dotnet run
```

---

## C# Basics Through Terminal

### 1. Hello World and Output

```csharp
using System;

class Program
{
    static void Main()
    {
        // Basic output
        Console.WriteLine("Hello, World!");
        
        // Output without newline
        Console.Write("Enter your name: ");
        
        // Colored output
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Success!");
        Console.ResetColor();
    }
}
```

**Try it yourself**: Create different greeting messages with different colors!

### 2. Variables and Input

```csharp
using System;

class Program
{
    static void Main()
    {
        // Getting user input
        Console.Write("What's your name? ");
        string name = Console.ReadLine();
        
        Console.Write("How old are you? ");
        int age = int.Parse(Console.ReadLine());
        
        // Using variables
        Console.WriteLine($"Hello {name}! You are {age} years old.");
        Console.WriteLine($"Next year you'll be {age + 1}!");
    }
}
```

**Learn by doing**: Modify this to ask for favorite color and hobby!

### 3. Conditionals (If/Else)

```csharp
using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter a number: ");
        int number = int.Parse(Console.ReadLine());
        
        if (number > 0)
        {
            Console.WriteLine("Positive number!");
        }
        else if (number < 0)
        {
            Console.WriteLine("Negative number!");
        }
        else
        {
            Console.WriteLine("It's zero!");
        }
        
        // Ternary operator
        string result = (number % 2 == 0) ? "even" : "odd";
        Console.WriteLine($"The number is {result}");
    }
}
```

**Challenge**: Create a simple age checker (child, teen, adult, senior).

### 4. Loops

```csharp
using System;

class Program
{
    static void Main()
    {
        // For loop - counting
        Console.WriteLine("Counting to 5:");
        for (int i = 1; i <= 5; i++)
        {
            Console.WriteLine(i);
        }
        
        // While loop - user controlled
        Console.WriteLine("\nGuess the magic number (1-10):");
        int magicNumber = 7;
        int guess = 0;
        
        while (guess != magicNumber)
        {
            Console.Write("Your guess: ");
            guess = int.Parse(Console.ReadLine());
            
            if (guess != magicNumber)
            {
                Console.WriteLine("Try again!");
            }
        }
        
        Console.WriteLine("Correct! 🎉");
    }
}
```

**Practice**: Add hints (higher/lower) to the guessing game.

### 5. Arrays and Lists

```csharp
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Array - fixed size
        string[] fruits = { "Apple", "Banana", "Cherry" };
        
        Console.WriteLine("Fruits:");
        for (int i = 0; i < fruits.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {fruits[i]}");
        }
        
        // List - dynamic size
        List<int> scores = new List<int>();
        scores.Add(100);
        scores.Add(85);
        scores.Add(92);
        
        Console.WriteLine($"\nAverage score: {CalculateAverage(scores)}");
    }
    
    static double CalculateAverage(List<int> numbers)
    {
        int sum = 0;
        foreach (int num in numbers)
        {
            sum += num;
        }
        return (double)sum / numbers.Count;
    }
}
```

**Try it**: Create a shopping list manager with add/remove/display functions.

### 6. Functions/Methods

```csharp
using System;

class Program
{
    static void Main()
    {
        ShowMenu();
        
        Console.Write("Choose an option: ");
        int choice = int.Parse(Console.ReadLine());
        
        ProcessChoice(choice);
    }
    
    static void ShowMenu()
    {
        Console.WriteLine("=== MENU ===");
        Console.WriteLine("1. Greet");
        Console.WriteLine("2. Calculate");
        Console.WriteLine("3. Exit");
        Console.WriteLine("============");
    }
    
    static void ProcessChoice(int choice)
    {
        switch (choice)
        {
            case 1:
                Greet();
                break;
            case 2:
                Calculate();
                break;
            case 3:
                Console.WriteLine("Goodbye!");
                break;
            default:
                Console.WriteLine("Invalid choice!");
                break;
        }
    }
    
    static void Greet()
    {
        Console.Write("Enter your name: ");
        string name = Console.ReadLine();
        Console.WriteLine($"Hello, {name}!");
    }
    
    static void Calculate()
    {
        Console.Write("Enter first number: ");
        int a = int.Parse(Console.ReadLine());
        
        Console.Write("Enter second number: ");
        int b = int.Parse(Console.ReadLine());
        
        Console.WriteLine($"Sum: {Add(a, b)}");
    }
    
    static int Add(int x, int y)
    {
        return x + y;
    }
}
```

**Challenge**: Add more menu options (subtract, multiply, divide).

### 7. Classes and Objects

```csharp
using System;

// Define a Player class
class Player
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int Score { get; set; }
    
    public Player(string name)
    {
        Name = name;
        Health = 100;
        Score = 0;
    }
    
    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health < 0) Health = 0;
        Console.WriteLine($"{Name} took {damage} damage! Health: {Health}");
    }
    
    public void Heal(int amount)
    {
        Health += amount;
        if (Health > 100) Health = 100;
        Console.WriteLine($"{Name} healed {amount}! Health: {Health}");
    }
    
    public void DisplayStatus()
    {
        Console.WriteLine($"\n=== {Name}'s Status ===");
        Console.WriteLine($"Health: {Health}/100");
        Console.WriteLine($"Score: {Score}");
        Console.WriteLine("===================\n");
    }
}

class Program
{
    static void Main()
    {
        Console.Write("Enter player name: ");
        string name = Console.ReadLine();
        
        Player player = new Player(name);
        player.DisplayStatus();
        
        // Simulate gameplay
        player.TakeDamage(20);
        player.Score += 10;
        player.Heal(15);
        player.DisplayStatus();
    }
}
```

**Exercise**: Add an Enemy class and create a simple battle system.

---

## Building Your First Terminal Game

Let's build a complete Number Guessing Game from scratch!

### Step 1: Plan the Game

**Goal**: Computer picks a random number, player guesses it.

**Features**:
- Random number generation
- User input
- Feedback (higher/lower)
- Attempt counter
- Win condition

### Step 2: Create the Project

```bash
dotnet new console -n NumberGuessingGame
cd NumberGuessingGame
```

### Step 3: Write the Code

```csharp
using System;

class NumberGuessingGame
{
    static void Main()
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
        string playAgain = Console.ReadLine().ToLower();
        
        if (playAgain == "y" || playAgain == "yes")
        {
            Main(); // Restart the game
        }
        else
        {
            Console.WriteLine("\nThanks for playing! Goodbye! 👋");
        }
    }
}
```

### Step 4: Run Your Game

```bash
dotnet run
```

### Step 5: Enhance It!

Ideas to make it better:
- Add difficulty levels (easy: 1-50, hard: 1-1000)
- Track high scores
- Add a hint system (show range narrowing)
- Timer for speed challenges
- Multiplayer mode

---

## Terminal Games Collection

This repository includes several complete terminal games in the `terminal-games/` directory:

### 1. **Number Guessing Game** (Beginner)
- Perfect first project
- Teaches: loops, conditionals, random numbers
- Location: `terminal-games/NumberGuessingGame/`

### 2. **Text Adventure Game** (Intermediate)
- Story-driven gameplay
- Teaches: strings, conditionals, classes
- Location: `terminal-games/TextAdventure/`

### 3. **Tic-Tac-Toe** (Intermediate)
- Two-player strategy game
- Teaches: arrays, game loops, win conditions
- Location: `terminal-games/TicTacToe/`

### 4. **Hangman** (Intermediate)
- Word guessing game
- Teaches: strings, lists, ASCII art
- Location: `terminal-games/Hangman/`

### 5. **Snake Game** (Advanced)
- Classic arcade game
- Teaches: coordinates, collision, timing
- Location: `terminal-games/SnakeGame/`

### 6. **RPG Battle System** (Advanced)
- Turn-based combat
- Teaches: OOP, inheritance, game systems
- Location: `terminal-games/RPGBattle/`

---

## Running the Games

Each game is a self-contained .NET console application:

```bash
# Navigate to any game directory
cd terminal-games/[GameName]

# Run the game
dotnet run

# Or build and run the executable
dotnet build
dotnet run --no-build
```

---

## Tips for Terminal Development

### 1. **Use Colors Effectively**

```csharp
Console.ForegroundColor = ConsoleColor.Green;  // Text color
Console.BackgroundColor = ConsoleColor.Black;   // Background color
Console.WriteLine("Colored text!");
Console.ResetColor();  // Always reset!
```

Available colors:
- Black, DarkBlue, DarkGreen, DarkCyan, DarkRed, DarkMagenta
- DarkYellow, Gray, DarkGray, Blue, Green, Cyan, Red, Magenta
- Yellow, White

### 2. **Clear and Position**

```csharp
Console.Clear();  // Clear the screen
Console.SetCursorPosition(10, 5);  // Set position (x, y)
Console.CursorVisible = false;  // Hide cursor
```

### 3. **Read Input Without Enter**

```csharp
// Read single key
ConsoleKeyInfo key = Console.ReadKey(true);  // true = don't display
if (key.Key == ConsoleKey.Escape)
{
    Console.WriteLine("ESC pressed!");
}
```

### 4. **Handle Errors Gracefully**

```csharp
Console.Write("Enter a number: ");
if (int.TryParse(Console.ReadLine(), out int number))
{
    Console.WriteLine($"You entered: {number}");
}
else
{
    Console.WriteLine("That's not a valid number!");
}
```

### 5. **Create ASCII Art**

```csharp
string[] art = new string[]
{
    "  /\\_/\\  ",
    " ( o.o ) ",
    "  > ^ <  "
};

foreach (string line in art)
{
    Console.WriteLine(line);
}
```

---

## Common Terminal Game Patterns

### Game Loop Pattern

```csharp
bool gameRunning = true;

while (gameRunning)
{
    // 1. Display game state
    DrawGame();
    
    // 2. Get player input
    var input = GetPlayerInput();
    
    // 3. Update game state
    UpdateGame(input);
    
    // 4. Check win/lose conditions
    if (CheckGameOver())
    {
        gameRunning = false;
    }
}

DisplayResults();
```

### Menu System Pattern

```csharp
void ShowMenu()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("1. Start Game");
        Console.WriteLine("2. Instructions");
        Console.WriteLine("3. Exit");
        
        switch (Console.ReadKey(true).Key)
        {
            case ConsoleKey.D1:
                StartGame();
                break;
            case ConsoleKey.D2:
                ShowInstructions();
                break;
            case ConsoleKey.D3:
                return;
        }
    }
}
```

### Score/Stats Tracking Pattern

```csharp
class GameStats
{
    public int Score { get; set; }
    public int HighScore { get; set; }
    public int GamesPlayed { get; set; }
    
    public void UpdateHighScore()
    {
        if (Score > HighScore)
        {
            HighScore = Score;
            Console.WriteLine("🏆 NEW HIGH SCORE!");
        }
    }
    
    public void Display()
    {
        Console.WriteLine($"Score: {Score}");
        Console.WriteLine($"High Score: {HighScore}");
        Console.WriteLine($"Games Played: {GamesPlayed}");
    }
}
```

---

## Debugging Tips

### 1. **Use Breakpoints**

In Visual Studio Code:
- Click left of line number to add breakpoint (red dot)
- Press F5 to start debugging
- Step through code with F10 (over) and F11 (into)

### 2. **Console Output for Debugging**

```csharp
// Temporary debug output
Console.WriteLine($"DEBUG: variable = {variable}");

// Only show in debug builds
#if DEBUG
Console.WriteLine("Debug information");
#endif
```

### 3. **Try-Catch for Errors**

```csharp
try
{
    int number = int.Parse(Console.ReadLine());
    Console.WriteLine($"Number: {number}");
}
catch (FormatException)
{
    Console.WriteLine("Error: Not a valid number!");
}
catch (Exception ex)
{
    Console.WriteLine($"Unexpected error: {ex.Message}");
}
```

---

## Next Steps

### After Mastering Terminal Games

1. **Add File I/O**: Save high scores, game progress
2. **Networking**: Create multiplayer games
3. **Advanced Graphics**: Try terminal graphics libraries
4. **Move to GUI**: Apply concepts to WPF, WinForms, or Unity

### Recommended Learning Path

1. ✅ Complete all games in this guide
2. ✅ Modify games with your own features
3. ✅ Create your own original game
4. ➡️ Learn about classes and OOP deeply
5. ➡️ Explore the [Game Development Guide](./GAME_DEVELOPMENT_GUIDE.md)
6. ➡️ Try Unity with the [Unity 2D Guide](./guides/UNITY_2D_GUIDE.md)

### Challenge Projects

Ready for more? Try building:

- **Calculator** with memory functions
- **Todo List Manager** with save/load
- **Quiz Game** with questions from a file
- **Blackjack** card game
- **Dungeon Crawler** with procedural generation
- **Type Speed Test** measuring WPM
- **Connect Four** with AI opponent

---

## Resources

### Official Documentation
- [Microsoft C# Documentation](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [.NET API Browser](https://docs.microsoft.com/en-us/dotnet/api/)

### Online Practice
- [LeetCode](https://leetcode.com/)
- [HackerRank](https://www.hackerrank.com/)
- [Codewars](https://www.codewars.com/)

### Communities
- [r/csharp](https://reddit.com/r/csharp)
- [C# Discord](https://discord.gg/csharp)
- [Stack Overflow](https://stackoverflow.com/questions/tagged/c%23)

---

## Contributing

Found a bug in a game? Have an idea for a new one? Feel free to:
- Report issues
- Suggest improvements
- Submit your own games!

---

## Conclusion

Terminal games are an excellent way to learn C# because they let you focus on programming logic without the complexity of graphics engines. Every concept you learn here applies to larger projects.

**Remember**: The best way to learn is by doing. Don't just read the code - type it, run it, break it, fix it, and make it your own!

Happy coding! 🚀🎮

---

**[⬅️ Back to Main README](./README.md)** | **[➡️ Explore Game Development Guide](./GAME_DEVELOPMENT_GUIDE.md)**
