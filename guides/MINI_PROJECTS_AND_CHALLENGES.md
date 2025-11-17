# Mini-Projects and Coding Challenges

Learn by doing! Complete these fun projects and challenges to practice your C# and game development skills.

## Table of Contents

- [Beginner Projects](#beginner-projects)
- [Intermediate Projects](#intermediate-projects)
- [Advanced Projects](#advanced-projects)
- [Daily Coding Challenges](#daily-coding-challenges)
- [Game Mechanic Challenges](#game-mechanic-challenges)
- [Algorithm Challenges](#algorithm-challenges)

---

## Beginner Projects

### Project 1: Number Guessing Game (Console)

**Goal**: Create a game where the computer picks a random number and the player tries to guess it.

**Requirements**:
- Generate a random number between 1 and 100
- Give the player hints ("higher" or "lower")
- Count the number of attempts
- Allow the player to play again

**Starter Code**:
```csharp
using System;

class NumberGuessingGame
{
    static void Main()
    {
        Random random = new Random();
        bool playAgain = true;
        
        while (playAgain)
        {
            // TODO: Your code here!
            int secretNumber = random.Next(1, 101);
            int attempts = 0;
            bool hasWon = false;
            
            Console.WriteLine("I'm thinking of a number between 1 and 100!");
            
            while (!hasWon)
            {
                Console.Write("Enter your guess: ");
                // TODO: Read input, check guess, give hints
            }
            
            Console.WriteLine($"You won in {attempts} attempts!");
            Console.Write("Play again? (yes/no): ");
            // TODO: Handle play again logic
        }
    }
}
```

**Extra Challenges**:
- Add difficulty levels (easy: 1-50, hard: 1-1000)
- Limit the number of attempts
- Keep a high score (fewest attempts)

---

### Project 2: Simple Calculator (Console)

**Goal**: Create a calculator that can add, subtract, multiply, and divide.

**Requirements**:
- Support basic operations (+, -, *, /)
- Handle division by zero
- Allow continuous calculations
- Show calculation history

**Example**:
```csharp
using System;
using System.Collections.Generic;

class Calculator
{
    static List<string> history = new List<string>();
    
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n=== Calculator ===");
            Console.WriteLine("1. Add");
            Console.WriteLine("2. Subtract");
            Console.WriteLine("3. Multiply");
            Console.WriteLine("4. Divide");
            Console.WriteLine("5. View History");
            Console.WriteLine("6. Exit");
            Console.Write("Choose an operation: ");
            
            // TODO: Implement calculator logic
        }
    }
    
    static double Add(double a, double b) => a + b;
    static double Subtract(double a, double b) => a - b;
    static double Multiply(double a, double b) => a * b;
    static double Divide(double a, double b)
    {
        // TODO: Handle division by zero
        return a / b;
    }
}
```

---

### Project 3: Rock Paper Scissors (Console)

**Goal**: Play Rock Paper Scissors against the computer.

**Requirements**:
- Player chooses rock, paper, or scissors
- Computer makes a random choice
- Determine the winner
- Track wins, losses, and ties

**Challenge Yourself**:
```csharp
// Add these features:
// - Best of 3 or best of 5 modes
// - Computer AI that learns from your patterns
// - Lizard and Spock variations (Rock Paper Scissors Lizard Spock)
```

---

### Project 4: To-Do List Manager (Console)

**Goal**: Create a simple task management system.

**Requirements**:
- Add tasks
- Mark tasks as complete
- Delete tasks
- View all tasks
- Save/load from file

**Hint**: Use a `List<Task>` where `Task` is a class with properties like `Description`, `IsComplete`, `Priority`.

---

### Project 5: Simple Text Adventure (Console)

**Goal**: Create an interactive story game.

**Requirements**:
- Multiple rooms/locations
- Player can move between rooms
- Items to collect
- Simple inventory system
- Win/lose conditions

**Example Structure**:
```csharp
class Room
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Dictionary<string, Room> Exits { get; set; }
    public List<Item> Items { get; set; }
}

class Player
{
    public Room CurrentRoom { get; set; }
    public List<Item> Inventory { get; set; }
    
    public void Move(string direction)
    {
        // TODO: Move to connected room
    }
    
    public void TakeItem(string itemName)
    {
        // TODO: Add item to inventory
    }
}
```

---

## Intermediate Projects

### Project 6: Platformer Character Controller (Unity/MonoGame)

**Goal**: Create a smooth 2D platformer controller.

**Requirements**:
- Walk left/right
- Jump with variable height
- Double jump
- Wall jump
- Ground detection
- Smooth animations

**Features to Add**:
- Coyote time (grace period after leaving platform)
- Jump buffering (press jump slightly before landing)
- Acceleration and deceleration
- Dash ability

---

### Project 7: Tower Defense Wave System

**Goal**: Create a wave-based enemy spawning system.

**Requirements**:
- Spawn enemies in waves
- Increase difficulty each wave
- Different enemy types
- Wave countdown timer
- Score system

**Example**:
```csharp
[Serializable]
public class Wave
{
    public List<EnemySpawn> Enemies;
    public float TimeBetweenSpawns = 1f;
}

[Serializable]
public class EnemySpawn
{
    public GameObject EnemyPrefab;
    public int Count;
}

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<Wave> waves;
    private int currentWave = 0;
    
    public IEnumerator StartWave()
    {
        // TODO: Spawn enemies from current wave
        yield return null;
    }
}
```

---

### Project 8: Card Game System

**Goal**: Create a card game framework (like Hearthstone or Slay the Spire).

**Requirements**:
- Deck system
- Hand management
- Mana/energy system
- Card effects
- Draw and discard piles

**Challenge**:
```csharp
public class Card
{
    public string Name;
    public int ManaCost;
    public string Description;
    public CardType Type;  // Attack, Skill, Power
    
    public virtual void Play(Character target)
    {
        // Override in specific card classes
    }
}

public class DeckManager
{
    private Stack<Card> drawPile;
    private List<Card> hand;
    private List<Card> discardPile;
    
    public Card DrawCard()
    {
        // TODO: Draw card, reshuffle if needed
        return null;
    }
    
    public void DiscardCard(Card card)
    {
        // TODO: Move card from hand to discard
    }
}
```

---

### Project 9: Pathfinding System

**Goal**: Implement A* pathfinding for AI movement.

**Requirements**:
- Grid-based navigation
- Obstacle avoidance
- Find shortest path
- Visualize the path

**Starter**:
```csharp
public class Node
{
    public Vector2Int Position;
    public bool IsWalkable;
    public int GCost;  // Distance from start
    public int HCost;  // Distance to end
    public int FCost => GCost + HCost;
    public Node Parent;
}

public class Pathfinding
{
    public List<Node> FindPath(Vector2Int start, Vector2Int end)
    {
        // TODO: Implement A* algorithm
        return new List<Node>();
    }
}
```

---

### Project 10: Procedural Dungeon Generator

**Goal**: Generate random dungeon layouts.

**Requirements**:
- Create random rooms
- Connect rooms with corridors
- Place entrance and exit
- Spawn enemies and items

**Approaches**:
1. **Binary Space Partitioning (BSP)**
2. **Random Walk Algorithm**
3. **Cellular Automata**

---

## Advanced Projects

### Project 11: Complete RPG Battle System

**Goal**: Create a full turn-based combat system.

**Requirements**:
- Multiple characters and enemies
- Skills and abilities
- Status effects (poison, stun, etc.)
- Equipment system
- Battle UI
- AI for enemies

**Components**:
```csharp
public class BattleSystem
{
    private List<Character> playerTeam;
    private List<Character> enemyTeam;
    private Queue<Character> turnOrder;
    
    public void StartBattle() { }
    public void NextTurn() { }
    public void ExecuteAction(BattleAction action) { }
    public void CheckBattleEnd() { }
}

public class StatusEffect
{
    public string Name;
    public int Duration;
    public virtual void Apply(Character target) { }
    public virtual void OnTurnStart(Character target) { }
    public virtual void OnTurnEnd(Character target) { }
}
```

---

### Project 12: Multiplayer Networking

**Goal**: Create a simple multiplayer game.

**Requirements**:
- Host and join games
- Sync player positions
- Shared game state
- Latency compensation

**Technologies**: Unity Netcode, Mirror, or Photon

---

### Project 13: Physics-Based Puzzle Game

**Goal**: Create a puzzle game like Cut the Rope or Angry Birds.

**Requirements**:
- Physics-based gameplay
- Level editor
- Multiple solutions per puzzle
- 3-star rating system

---

### Project 14: Shader Effects System

**Goal**: Learn shader programming with visual effects.

**Create These Effects**:
- Water ripples
- Dissolve effect
- Outline shader
- Hit flash effect
- Screen transitions

---

### Project 15: Complete Game with All Systems

**Goal**: Build a full game combining everything you've learned!

**Requirements**:
- Main menu
- Multiple levels
- Save/load system
- Settings menu
- Audio management
- Particle effects
- Enemy AI
- Boss fights
- Achievements
- Leaderboards

---

## Daily Coding Challenges

### Week 1: Basics

**Day 1**: Write a function that reverses a string
```csharp
// Input: "Hello"
// Output: "olleH"
```

**Day 2**: Find the largest number in an array
```csharp
int[] numbers = { 3, 7, 2, 9, 1 };
// Output: 9
```

**Day 3**: Check if a word is a palindrome
```csharp
// "racecar" -> true
// "hello" -> false
```

**Day 4**: Count vowels in a string
```csharp
// "hello world" -> 3
```

**Day 5**: FizzBuzz challenge
```csharp
// Print numbers 1-100
// If divisible by 3: print "Fizz"
// If divisible by 5: print "Buzz"
// If divisible by both: print "FizzBuzz"
```

**Day 6**: Sum of all even numbers in an array
```csharp
int[] numbers = { 1, 2, 3, 4, 5, 6 };
// Output: 12 (2+4+6)
```

**Day 7**: Find duplicate items in a list
```csharp
List<int> numbers = new List<int> { 1, 2, 3, 2, 4, 3 };
// Output: [2, 3]
```

---

### Week 2: Data Structures

**Day 8**: Implement a Stack class from scratch

**Day 9**: Implement a Queue class from scratch

**Day 10**: Create a linked list

**Day 11**: Implement a binary search algorithm

**Day 12**: Sort an array (bubble sort, then optimize)

**Day 13**: Implement a simple hash table

**Day 14**: Create a circular buffer

---

### Week 3: Game Logic

**Day 15**: Write a dice rolling function with different dice types (d4, d6, d8, d20)

**Day 16**: Create a loot table system with rarity

**Day 17**: Implement a combo system (track sequential hits)

**Day 18**: Create a level-up system with exponential XP requirements

**Day 19**: Write a damage calculator with armor and resistance

**Day 20**: Implement a crafting recipe system

**Day 21**: Create a skill tree with prerequisites

---

## Game Mechanic Challenges

### Challenge 1: Double Jump
Implement a character controller that allows jumping twice in mid-air.

**Hints**:
- Track jump count
- Reset on ground contact
- Allow second jump only if first jump was used

---

### Challenge 2: Dash Ability
Create a quick dash movement ability with cooldown.

**Requirements**:
- Instant speed boost
- Short duration
- Cooldown period
- Visual trail effect

---

### Challenge 3: Grappling Hook
Implement a grappling hook mechanic.

**Physics Required**:
- Projectile launch
- Rope physics
- Swing momentum
- Release mechanics

---

### Challenge 4: Time Slow Effect
Create a bullet-time slow motion effect.

**Implementation**:
- Reduce Time.timeScale
- Smooth transition in/out
- Limited duration
- Visual effects (desaturation, motion blur)

---

### Challenge 5: Stealth System
Create a stealth detection system.

**Components**:
- Line of sight
- Noise detection
- Alert states
- Investigation behavior

---

## Algorithm Challenges

### Challenge 1: Maze Solver
Write an algorithm to find a path through a maze.

**Approaches**:
- Depth-First Search (DFS)
- Breadth-First Search (BFS)
- A* (for optimal path)

---

### Challenge 2: Enemy Formation
Create a formation system where enemies maintain positions.

**Requirements**:
- Leader-follower pattern
- Maintain spacing
- Avoid obstacles
- Smoothly adjust positions

---

### Challenge 3: Spawn Distribution
Spawn enemies evenly across a map avoiding clustersing.

**Algorithm**: Poisson Disk Sampling or Grid-based distribution

---

### Challenge 4: Difficulty Scaling
Create a dynamic difficulty system that adjusts to player skill.

**Factors to Track**:
- Deaths count
- Time taken
- Accuracy
- Resources remaining

---

### Challenge 5: Loot Probability
Implement a weighted random loot system.

```csharp
public class LootTable
{
    private List<LootEntry> entries;
    
    public class LootEntry
    {
        public GameObject Item;
        public float Weight;  // Higher = more common
    }
    
    public GameObject GetRandomLoot()
    {
        // TODO: Implement weighted random selection
        return null;
    }
}
```

---

## Mini-Game Ideas to Build

### 1. Snake Game
Classic snake game with growing tail

### 2. Breakout/Arkanoid
Paddle and ball brick breaker

### 3. Space Invaders
Classic shoot-em-up

### 4. Flappy Bird Clone
Obstacle avoidance with simple controls

### 5. Tetris
Falling block puzzle game

### 6. Pac-Man
Maze navigation with AI ghosts

### 7. Memory Match
Card matching game

### 8. Minesweeper
Logic-based puzzle

### 9. Connect Four
Two-player strategy game

### 10. Tic-Tac-Toe with AI
Simple AI opponent

---

## Challenge Mode: Code Golf

Solve these problems in the fewest lines of code!

### Golf 1: Fibonacci Sequence
Generate first 10 Fibonacci numbers

### Golf 2: Prime Numbers
Find all prime numbers up to 100

### Golf 3: Array Shuffle
Shuffle an array in place

### Golf 4: String Compression
"aaabbc" -> "a3b2c1"

### Golf 5: Anagram Checker
Check if two words are anagrams

---

## Progressive Learning Path

### Beginner Path (Weeks 1-4)
1. Complete Projects 1-5
2. Daily challenges Week 1-2
3. Build Snake or Tic-Tac-Toe

### Intermediate Path (Weeks 5-12)
1. Complete Projects 6-10
2. Daily challenges Week 3
3. Game mechanic challenges 1-3
4. Build Breakout or Space Invaders

### Advanced Path (Weeks 13+)
1. Complete Projects 11-15
2. All game mechanic challenges
3. All algorithm challenges
4. Build a complete original game

---

## Tips for Success

1. **Start Small**: Begin with console projects before graphics
2. **Code Daily**: Even 30 minutes helps
3. **Break Down Problems**: Divide large tasks into small steps
4. **Debug Systematically**: Use `Debug.Log()` liberally
5. **Read Others' Code**: Learn from open-source projects
6. **Refactor**: Revisit old projects and improve them
7. **Don't Give Up**: Stuck? Take a break, Google it, ask for help
8. **Build What Excites You**: Passion drives learning

---

## Next Steps

1. Pick a project from the beginner section
2. Set a deadline (1 week for beginners)
3. Code a little each day
4. Test frequently
5. Show your work to others
6. Move to the next challenge!

Remember: **The best way to learn programming is by programming!** 🚀

---

## Share Your Solutions!

When you complete a challenge:
- Create a GitHub repository
- Write a README explaining your approach
- Share on game dev communities
- Get feedback and iterate

Happy coding! 🎮✨
