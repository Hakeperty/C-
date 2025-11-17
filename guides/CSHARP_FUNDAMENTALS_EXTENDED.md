# C# Fundamentals - Extended Guide

This guide goes beyond the basics to give you practical C# knowledge that you'll use every day in game development!

## Table of Contents

- [Collections and Data Structures](#collections-and-data-structures)
- [LINQ - Making Data Queries Easy](#linq---making-data-queries-easy)
- [Properties and Auto-Properties](#properties-and-auto-properties)
- [Nullable Types](#nullable-types)
- [String Manipulation](#string-manipulation)
- [Exception Handling](#exception-handling)
- [File I/O](#file-io)
- [Async/Await for Game Development](#asyncawait-for-game-development)
- [Generics](#generics)
- [Extension Methods](#extension-methods)

---

## Collections and Data Structures

### Lists - Your Best Friend

```csharp
using System.Collections.Generic;

// Creating and using Lists
List<string> playerNames = new List<string>();

// Adding items
playerNames.Add("Alice");
playerNames.Add("Bob");
playerNames.Add("Charlie");

// Adding multiple items at once
playerNames.AddRange(new[] { "David", "Eve" });

// Accessing items
string firstPlayer = playerNames[0];  // "Alice"
string lastPlayer = playerNames[playerNames.Count - 1];  // "Eve"

// Removing items
playerNames.Remove("Bob");  // Remove by value
playerNames.RemoveAt(0);    // Remove by index

// Checking if item exists
if (playerNames.Contains("Charlie"))
{
    Console.WriteLine("Charlie is playing!");
}

// Iterating through list
foreach (string name in playerNames)
{
    Console.WriteLine(name);
}

// Finding items
string player = playerNames.Find(p => p.StartsWith("D"));  // "David"
List<string> longNames = playerNames.FindAll(p => p.Length > 5);

// Sorting
playerNames.Sort();  // Alphabetical order
```

### Dictionaries - Fast Lookups

```csharp
using System.Collections.Generic;

// Create a dictionary (key-value pairs)
Dictionary<string, int> playerScores = new Dictionary<string, int>();

// Adding items
playerScores["Alice"] = 100;
playerScores["Bob"] = 250;
playerScores.Add("Charlie", 150);

// Accessing items
int aliceScore = playerScores["Alice"];  // 100

// Safe access (check if key exists)
if (playerScores.ContainsKey("David"))
{
    int davidScore = playerScores["David"];
}

// TryGetValue - safer way
if (playerScores.TryGetValue("Eve", out int eveScore))
{
    Console.WriteLine($"Eve's score: {eveScore}");
}
else
{
    Console.WriteLine("Eve is not in the scoreboard");
}

// Updating values
playerScores["Alice"] += 50;  // Alice now has 150

// Removing items
playerScores.Remove("Bob");

// Iterating through dictionary
foreach (KeyValuePair<string, int> entry in playerScores)
{
    Console.WriteLine($"{entry.Key}: {entry.Value}");
}

// Or using var
foreach (var entry in playerScores)
{
    Console.WriteLine($"{entry.Key}: {entry.Value}");
}

// Just keys or just values
foreach (string name in playerScores.Keys)
{
    Console.WriteLine(name);
}

foreach (int score in playerScores.Values)
{
    Console.WriteLine(score);
}
```

### Queue - First In, First Out (FIFO)

```csharp
using System.Collections.Generic;

// Perfect for processing items in order
Queue<string> commandQueue = new Queue<string>();

// Add items to the end
commandQueue.Enqueue("Move Forward");
commandQueue.Enqueue("Turn Left");
commandQueue.Enqueue("Attack");

// Remove and get the first item
string nextCommand = commandQueue.Dequeue();  // "Move Forward"

// Look at first item without removing
string peek = commandQueue.Peek();  // "Turn Left"

// Process all commands
while (commandQueue.Count > 0)
{
    string command = commandQueue.Dequeue();
    ExecuteCommand(command);
}
```

### Stack - Last In, First Out (LIFO)

```csharp
using System.Collections.Generic;

// Great for undo systems!
Stack<GameState> undoStack = new Stack<GameState>();

// Push items on top
undoStack.Push(currentGameState);
undoStack.Push(newGameState);

// Pop items from top
GameState previousState = undoStack.Pop();

// Look at top item without removing
GameState topState = undoStack.Peek();

// Example: Undo system
public class UndoManager
{
    private Stack<GameAction> undoStack = new Stack<GameAction>();
    private Stack<GameAction> redoStack = new Stack<GameAction>();
    
    public void ExecuteAction(GameAction action)
    {
        action.Execute();
        undoStack.Push(action);
        redoStack.Clear();  // Clear redo when new action is performed
    }
    
    public void Undo()
    {
        if (undoStack.Count > 0)
        {
            GameAction action = undoStack.Pop();
            action.Undo();
            redoStack.Push(action);
        }
    }
    
    public void Redo()
    {
        if (redoStack.Count > 0)
        {
            GameAction action = redoStack.Pop();
            action.Execute();
            undoStack.Push(action);
        }
    }
}
```

### HashSet - Unique Items Only

```csharp
using System.Collections.Generic;

// No duplicates allowed!
HashSet<string> uniqueItems = new HashSet<string>();

uniqueItems.Add("Sword");
uniqueItems.Add("Shield");
uniqueItems.Add("Sword");  // Won't be added - already exists!

Console.WriteLine(uniqueItems.Count);  // 2

// Very fast Contains check
if (uniqueItems.Contains("Shield"))
{
    Console.WriteLine("Player has a shield!");
}

// Set operations
HashSet<string> playerInventory = new HashSet<string> { "Sword", "Potion", "Key" };
HashSet<string> requiredItems = new HashSet<string> { "Key", "Map" };

// Check if player has all required items
bool hasAll = requiredItems.IsSubsetOf(playerInventory);  // false (no Map)

// Get items player needs
HashSet<string> missing = new HashSet<string>(requiredItems);
missing.ExceptWith(playerInventory);  // { "Map" }
```

---

## LINQ - Making Data Queries Easy

LINQ (Language Integrated Query) makes working with collections super easy!

```csharp
using System.Linq;
using System.Collections.Generic;

// Sample data
List<Enemy> enemies = new List<Enemy>
{
    new Enemy { Name = "Goblin", Health = 50, Damage = 10, Level = 1 },
    new Enemy { Name = "Orc", Health = 100, Damage = 20, Level = 2 },
    new Enemy { Name = "Dragon", Health = 500, Damage = 50, Level = 5 },
    new Enemy { Name = "Skeleton", Health = 30, Damage = 8, Level = 1 },
};

// Filter - Find all weak enemies
var weakEnemies = enemies.Where(e => e.Health < 100).ToList();

// Find - Get first match
Enemy firstGoblin = enemies.FirstOrDefault(e => e.Name == "Goblin");

// Sort - Order by health
var sortedByHealth = enemies.OrderBy(e => e.Health).ToList();
var sortedDescending = enemies.OrderByDescending(e => e.Level).ToList();

// Select - Transform data
List<string> enemyNames = enemies.Select(e => e.Name).ToList();
List<int> healthValues = enemies.Select(e => e.Health).ToList();

// Count with condition
int strongEnemies = enemies.Count(e => e.Level > 2);

// Sum, Average, Max, Min
int totalHealth = enemies.Sum(e => e.Health);
double avgDamage = enemies.Average(e => e.Damage);
int maxLevel = enemies.Max(e => e.Level);
int minHealth = enemies.Min(e => e.Health);

// Any - Check if at least one matches
bool hasDragon = enemies.Any(e => e.Name == "Dragon");
bool hasWeakEnemy = enemies.Any(e => e.Health < 50);

// All - Check if all match
bool allAreDangerous = enemies.All(e => e.Damage > 5);

// GroupBy - Group enemies by level
var groupedByLevel = enemies.GroupBy(e => e.Level);
foreach (var group in groupedByLevel)
{
    Console.WriteLine($"Level {group.Key}:");
    foreach (var enemy in group)
    {
        Console.WriteLine($"  - {enemy.Name}");
    }
}

// Chaining operations
var result = enemies
    .Where(e => e.Level > 1)          // Filter
    .OrderBy(e => e.Health)            // Sort
    .Select(e => e.Name)               // Get names
    .Take(5)                           // Take first 5
    .ToList();                         // Convert to list

// Complex example: Find the 3 strongest enemies by total power
var topEnemies = enemies
    .Select(e => new { 
        Enemy = e, 
        TotalPower = e.Health + (e.Damage * 10) 
    })
    .OrderByDescending(x => x.TotalPower)
    .Take(3)
    .Select(x => x.Enemy)
    .ToList();
```

### Real Game Example: Inventory System with LINQ

```csharp
public class InventoryManager
{
    private List<Item> items = new List<Item>();
    
    // Find all weapons
    public List<Item> GetWeapons()
    {
        return items.Where(i => i.Type == ItemType.Weapon).ToList();
    }
    
    // Get total weight
    public float GetTotalWeight()
    {
        return items.Sum(i => i.Weight);
    }
    
    // Find the most expensive item
    public Item GetMostExpensiveItem()
    {
        return items.OrderByDescending(i => i.Value).FirstOrDefault();
    }
    
    // Check if we have an item with specific property
    public bool HasItem(string itemName)
    {
        return items.Any(i => i.Name == itemName);
    }
    
    // Get items we can afford
    public List<Item> GetAffordableItems(int playerGold)
    {
        return items.Where(i => i.Value <= playerGold).ToList();
    }
    
    // Group items by rarity
    public Dictionary<Rarity, List<Item>> GetItemsByRarity()
    {
        return items.GroupBy(i => i.Rarity)
                   .ToDictionary(g => g.Key, g => g.ToList());
    }
}
```

---

## Properties and Auto-Properties

Properties provide a way to read and write private fields with validation!

```csharp
// Traditional property with backing field
public class Player
{
    private int health;
    
    public int Health
    {
        get { return health; }
        set 
        { 
            if (value < 0)
                health = 0;
            else if (value > 100)
                health = 100;
            else
                health = value;
        }
    }
}

// Auto-property (C# does the backing field for you!)
public class Enemy
{
    public string Name { get; set; }
    public int Level { get; set; }
    
    // Read-only auto-property
    public string ID { get; }
    
    // Auto-property with default value
    public int Health { get; set; } = 100;
    
    // Private setter (can only be set from inside the class)
    public float Speed { get; private set; }
    
    public Enemy(string id)
    {
        ID = id;  // Can set in constructor
        Speed = 5.0f;
    }
    
    public void IncreaseSpeed(float amount)
    {
        Speed += amount;  // Can modify inside class
    }
}

// Property with custom logic
public class Character
{
    private int experience;
    
    public int Experience
    {
        get { return experience; }
        set
        {
            experience = value;
            // Update level when experience changes
            Level = experience / 100;
            OnExperienceChanged?.Invoke(experience);
        }
    }
    
    public int Level { get; private set; }
    public event Action<int> OnExperienceChanged;
}

// Expression-bodied properties (C# 6+)
public class Weapon
{
    public int BaseDamage { get; set; }
    public float DamageMultiplier { get; set; } = 1.0f;
    
    // Calculated property
    public int TotalDamage => (int)(BaseDamage * DamageMultiplier);
    
    // Another example
    public bool IsPowerful => TotalDamage > 50;
}

// Using properties
var player = new Player();
player.Health = 150;  // Will be clamped to 100
player.Health = -10;  // Will be clamped to 0

var weapon = new Weapon { BaseDamage = 30, DamageMultiplier = 1.5f };
int damage = weapon.TotalDamage;  // 45
```

---

## Nullable Types

Nullable types let you represent "no value" for value types!

```csharp
// Regular int cannot be null
int score = null;  // ERROR!

// Nullable int can be null
int? optionalScore = null;  // OK!
optionalScore = 100;         // Also OK!

// Checking for null
int? health = GetPlayerHealth();

if (health.HasValue)
{
    Console.WriteLine($"Health: {health.Value}");
}
else
{
    Console.WriteLine("Health is unknown");
}

// Shorter syntax
if (health != null)
{
    Console.WriteLine($"Health: {health}");
}

// Null coalescing operator - provide default value
int actualHealth = health ?? 100;  // Use 100 if health is null

// Null conditional operator
int? length = playerName?.Length;  // Returns null if playerName is null

// Real game example
public class SaveData
{
    public int? LastCheckpoint { get; set; }
    public DateTime? LastPlayed { get; set; }
    
    public void LoadGame()
    {
        // Use last checkpoint if available, otherwise start from beginning
        int startPoint = LastCheckpoint ?? 0;
        
        // Check if we have a last played date
        if (LastPlayed.HasValue)
        {
            TimeSpan timeSince = DateTime.Now - LastPlayed.Value;
            if (timeSince.TotalDays > 30)
            {
                Console.WriteLine("Welcome back! It's been a while!");
            }
        }
    }
}

// Nullable reference types (C# 8+)
// Enable in .csproj: <Nullable>enable</Nullable>
#nullable enable

public class GameManager
{
    // Can be null
    public Player? CurrentPlayer { get; set; }
    
    // Cannot be null (compiler will warn)
    public string GameTitle { get; set; } = "My Game";
    
    public void ProcessPlayer()
    {
        // Compiler warns: CurrentPlayer might be null!
        // Console.WriteLine(CurrentPlayer.Name);
        
        // Correct way:
        if (CurrentPlayer != null)
        {
            Console.WriteLine(CurrentPlayer.Name);
        }
        
        // Or using null conditional:
        Console.WriteLine(CurrentPlayer?.Name ?? "No player");
    }
}
```

---

## String Manipulation

Strings are everywhere in games - player names, dialogue, messages, etc.

```csharp
// String concatenation
string firstName = "John";
string lastName = "Doe";
string fullName = firstName + " " + lastName;

// String interpolation (better!)
string greeting = $"Hello, {firstName} {lastName}!";
int score = 1000;
string message = $"Your score is: {score:N0}";  // Formatted: 1,000

// Multiline strings
string dialogue = @"This is a long dialogue
that spans multiple lines.
It preserves formatting!";

// String methods
string text = "  Hello World  ";
string upper = text.ToUpper();              // "  HELLO WORLD  "
string lower = text.ToLower();              // "  hello world  "
string trimmed = text.Trim();               // "Hello World"
bool contains = text.Contains("World");     // true
bool startsWith = text.StartsWith("  H");  // true
bool endsWith = text.EndsWith("d  ");      // true

// Substring
string hello = "Hello World".Substring(0, 5);  // "Hello"
string world = "Hello World".Substring(6);     // "World"

// Replace
string fixed = "Hello World".Replace("World", "Game");  // "Hello Game"

// Split
string csv = "Apple,Orange,Banana";
string[] fruits = csv.Split(',');  // ["Apple", "Orange", "Banana"]

// Join
string joined = string.Join(", ", fruits);  // "Apple, Orange, Banana"

// String formatting
int health = 75;
int maxHealth = 100;
string healthBar = string.Format("Health: {0}/{1}", health, maxHealth);

// Padding
string id = "42";
string paddedId = id.PadLeft(5, '0');  // "00042"

// String Builder for performance (when building large strings)
using System.Text;

StringBuilder sb = new StringBuilder();
for (int i = 0; i < 1000; i++)
{
    sb.AppendLine($"Line {i}");
}
string result = sb.ToString();

// Practical game example: Chat filter
public class ChatFilter
{
    private HashSet<string> bannedWords = new HashSet<string>
    {
        "badword1", "badword2"
    };
    
    public string FilterMessage(string message)
    {
        string filtered = message.ToLower();
        
        foreach (string word in bannedWords)
        {
            filtered = filtered.Replace(word, new string('*', word.Length));
        }
        
        return filtered;
    }
    
    public bool IsValidUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return false;
            
        if (username.Length < 3 || username.Length > 20)
            return false;
            
        // Only letters, numbers, and underscores
        return username.All(c => char.IsLetterOrDigit(c) || c == '_');
    }
}
```

---

## Exception Handling

Handle errors gracefully to prevent game crashes!

```csharp
// Basic try-catch
try
{
    int result = 10 / 0;  // This will throw an exception!
}
catch (DivideByZeroException ex)
{
    Console.WriteLine("Cannot divide by zero!");
    Console.WriteLine($"Error: {ex.Message}");
}

// Multiple catch blocks
try
{
    string text = File.ReadAllText("save.txt");
    int value = int.Parse(text);
}
catch (FileNotFoundException ex)
{
    Console.WriteLine("Save file not found!");
}
catch (FormatException ex)
{
    Console.WriteLine("Save file is corrupted!");
}
catch (Exception ex)
{
    Console.WriteLine($"Unknown error: {ex.Message}");
}
finally
{
    // Always executes, even if exception occurs
    Console.WriteLine("Cleanup code here");
}

// Throwing exceptions
public class Player
{
    private int health;
    
    public void TakeDamage(int damage)
    {
        if (damage < 0)
            throw new ArgumentException("Damage cannot be negative", nameof(damage));
            
        health -= damage;
    }
}

// Custom exceptions
public class GameException : Exception
{
    public GameException(string message) : base(message) { }
}

public class InvalidMoveException : GameException
{
    public Vector2 Position { get; }
    
    public InvalidMoveException(Vector2 position) 
        : base($"Invalid move to position {position}")
    {
        Position = position;
    }
}

// Using custom exceptions
try
{
    MoveTo(new Vector2(100, 100));
}
catch (InvalidMoveException ex)
{
    Console.WriteLine(ex.Message);
    Console.WriteLine($"Tried to move to: {ex.Position}");
}

// Real game example: Safe save/load
public class SaveManager
{
    public bool TrySaveGame(GameData data, string filename)
    {
        try
        {
            string json = JsonSerializer.Serialize(data);
            File.WriteAllText(filename, json);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save game: {ex.Message}");
            return false;
        }
    }
    
    public GameData LoadGame(string filename)
    {
        try
        {
            string json = File.ReadAllText(filename);
            return JsonSerializer.Deserialize<GameData>(json);
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("No save file found, starting new game");
            return new GameData();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load game: {ex.Message}");
            return new GameData();
        }
    }
}
```

---

## File I/O

Save games, load levels, read configuration files!

```csharp
using System.IO;
using System.Text.Json;

// Reading a text file
string text = File.ReadAllText("config.txt");

// Reading all lines
string[] lines = File.ReadAllLines("highscores.txt");
foreach (string line in lines)
{
    Console.WriteLine(line);
}

// Writing a text file
File.WriteAllText("log.txt", "Game started\n");

// Appending to a file
File.AppendAllText("log.txt", "Level loaded\n");

// Writing multiple lines
string[] highScores = { "Alice: 1000", "Bob: 950", "Charlie: 900" };
File.WriteAllLines("highscores.txt", highScores);

// Checking if file exists
if (File.Exists("save.dat"))
{
    Console.WriteLine("Save file found!");
}

// Checking if directory exists
if (!Directory.Exists("saves"))
{
    Directory.CreateDirectory("saves");
}

// Getting all files in directory
string[] saveFiles = Directory.GetFiles("saves", "*.sav");

// JSON serialization (modern way to save data)
public class GameData
{
    public string PlayerName { get; set; }
    public int Level { get; set; }
    public int Score { get; set; }
    public List<string> Inventory { get; set; }
}

// Saving to JSON
public void SaveGame(GameData data)
{
    string json = JsonSerializer.Serialize(data, new JsonSerializerOptions 
    { 
        WriteIndented = true 
    });
    File.WriteAllText("savegame.json", json);
}

// Loading from JSON
public GameData LoadGame()
{
    if (File.Exists("savegame.json"))
    {
        string json = File.ReadAllText("savegame.json");
        return JsonSerializer.Deserialize<GameData>(json);
    }
    return new GameData();
}

// Complete save system example
public class SaveSystem
{
    private const string SAVE_DIRECTORY = "saves";
    private const string SAVE_EXTENSION = ".sav";
    
    public void SaveGame(string slotName, GameData data)
    {
        try
        {
            // Ensure directory exists
            if (!Directory.Exists(SAVE_DIRECTORY))
            {
                Directory.CreateDirectory(SAVE_DIRECTORY);
            }
            
            // Create file path
            string filePath = Path.Combine(SAVE_DIRECTORY, slotName + SAVE_EXTENSION);
            
            // Serialize and save
            string json = JsonSerializer.Serialize(data);
            File.WriteAllText(filePath, json);
            
            Console.WriteLine($"Game saved to {slotName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Save failed: {ex.Message}");
        }
    }
    
    public GameData LoadGame(string slotName)
    {
        try
        {
            string filePath = Path.Combine(SAVE_DIRECTORY, slotName + SAVE_EXTENSION);
            
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<GameData>(json);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Load failed: {ex.Message}");
        }
        
        return null;
    }
    
    public List<string> GetSaveSlots()
    {
        if (!Directory.Exists(SAVE_DIRECTORY))
            return new List<string>();
            
        return Directory.GetFiles(SAVE_DIRECTORY, "*" + SAVE_EXTENSION)
            .Select(Path.GetFileNameWithoutExtension)
            .ToList();
    }
    
    public void DeleteSave(string slotName)
    {
        string filePath = Path.Combine(SAVE_DIRECTORY, slotName + SAVE_EXTENSION);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Console.WriteLine($"Deleted save: {slotName}");
        }
    }
}
```

---

## Async/Await for Game Development

Async/await lets you do things without freezing the game!

```csharp
using System.Threading.Tasks;

// Basic async method
public async Task LoadLevelAsync()
{
    Console.WriteLine("Loading level...");
    
    // Simulate time-consuming operation
    await Task.Delay(2000);  // Wait 2 seconds
    
    Console.WriteLine("Level loaded!");
}

// Async method with return value
public async Task<GameData> LoadGameDataAsync()
{
    await Task.Delay(1000);
    return new GameData { PlayerName = "Hero", Level = 5 };
}

// Using async methods
public async void StartGame()
{
    // Method 1: await (wait for completion)
    await LoadLevelAsync();
    Console.WriteLine("This runs after level is loaded");
    
    // Method 2: Start and continue
    Task loadTask = LoadLevelAsync();
    Console.WriteLine("This runs immediately");
    await loadTask;  // Wait later if needed
}

// Loading multiple things at once
public async Task LoadAllAssetsAsync()
{
    // Start all tasks
    Task texturesTask = LoadTexturesAsync();
    Task soundsTask = LoadSoundsAsync();
    Task modelsTask = LoadModelsAsync();
    
    // Wait for all to complete
    await Task.WhenAll(texturesTask, soundsTask, modelsTask);
    
    Console.WriteLine("All assets loaded!");
}

// Real example: Loading with progress
public class AssetLoader
{
    public async Task<bool> LoadGameAssetsAsync(IProgress<float> progress)
    {
        try
        {
            progress?.Report(0.0f);
            
            await LoadTexturesAsync();
            progress?.Report(0.33f);
            
            await LoadSoundsAsync();
            progress?.Report(0.66f);
            
            await LoadModelsAsync();
            progress?.Report(1.0f);
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Loading failed: {ex.Message}");
            return false;
        }
    }
    
    private async Task LoadTexturesAsync()
    {
        await Task.Delay(1000);
        // Load textures here
    }
    
    private async Task LoadSoundsAsync()
    {
        await Task.Delay(1000);
        // Load sounds here
    }
    
    private async Task LoadModelsAsync()
    {
        await Task.Delay(1000);
        // Load models here
    }
}

// Usage in Unity
public class LoadingScreen : MonoBehaviour
{
    private AssetLoader loader = new AssetLoader();
    
    async void Start()
    {
        var progress = new Progress<float>(value => 
        {
            // Update loading bar
            loadingSlider.value = value;
            percentText.text = $"{value * 100:F0}%";
        });
        
        bool success = await loader.LoadGameAssetsAsync(progress);
        
        if (success)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}

// Async file operations
public async Task<string> LoadConfigAsync()
{
    return await Task.Run(() => 
    {
        return File.ReadAllText("config.txt");
    });
}

// Downloading from web
using System.Net.Http;

public async Task<string> FetchLeaderboardAsync()
{
    using (HttpClient client = new HttpClient())
    {
        string url = "https://api.example.com/leaderboard";
        string json = await client.GetStringAsync(url);
        return json;
    }
}
```

---

## Generics

Write code that works with any type!

```csharp
// Generic method
public T GetRandomItem<T>(List<T> items)
{
    int index = Random.Range(0, items.Count);
    return items[index];
}

// Usage:
string randomName = GetRandomItem(new List<string> { "Alice", "Bob", "Charlie" });
int randomNumber = GetRandomItem(new List<int> { 1, 2, 3, 4, 5 });

// Generic class
public class ObjectPool<T> where T : new()
{
    private Stack<T> pool = new Stack<T>();
    
    public T Get()
    {
        if (pool.Count > 0)
            return pool.Pop();
        return new T();
    }
    
    public void Return(T obj)
    {
        pool.Push(obj);
    }
}

// Usage:
ObjectPool<Enemy> enemyPool = new ObjectPool<Enemy>();
Enemy enemy = enemyPool.Get();
enemyPool.Return(enemy);

// Generic inventory system
public class Inventory<T> where T : IItem
{
    private List<T> items = new List<T>();
    
    public void AddItem(T item)
    {
        items.Add(item);
    }
    
    public T GetItem(int index)
    {
        return items[index];
    }
    
    public List<T> GetAllItems()
    {
        return new List<T>(items);
    }
    
    public T FindItem(Predicate<T> condition)
    {
        return items.Find(condition);
    }
}

// Multiple type parameters
public class Pair<TKey, TValue>
{
    public TKey Key { get; set; }
    public TValue Value { get; set; }
    
    public Pair(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }
}

// Usage:
var pair = new Pair<string, int>("Score", 100);

// Real game example: Generic component system
public class ComponentManager<T> where T : Component
{
    private Dictionary<string, T> components = new Dictionary<string, T>();
    
    public void Register(string id, T component)
    {
        components[id] = component;
    }
    
    public T Get(string id)
    {
        return components.TryGetValue(id, out T component) ? component : null;
    }
    
    public List<T> GetAll()
    {
        return components.Values.ToList();
    }
    
    public void Remove(string id)
    {
        components.Remove(id);
    }
}
```

---

## Extension Methods

Add new methods to existing types!

```csharp
// Extension methods must be in a static class
public static class Extensions
{
    // Extension method for Vector2
    public static float DistanceTo(this Vector2 from, Vector2 to)
    {
        return Vector2.Distance(from, to);
    }
    
    // Extension method for strings
    public static bool IsValidEmail(this string email)
    {
        return email.Contains("@") && email.Contains(".");
    }
    
    // Extension method for lists
    public static T GetRandom<T>(this List<T> list)
    {
        if (list.Count == 0)
            return default(T);
        return list[Random.Range(0, list.Count)];
    }
    
    // Extension method for transforms (Unity)
    public static void ResetTransform(this Transform transform)
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }
}

// Using extension methods
Vector2 playerPos = new Vector2(10, 5);
Vector2 enemyPos = new Vector2(20, 15);
float distance = playerPos.DistanceTo(enemyPos);

string email = "test@example.com";
if (email.IsValidEmail())
{
    Console.WriteLine("Valid email!");
}

List<string> names = new List<string> { "Alice", "Bob", "Charlie" };
string randomName = names.GetRandom();

// More useful extension methods
public static class GameExtensions
{
    // Check if number is between two values
    public static bool IsBetween(this int value, int min, int max)
    {
        return value >= min && value <= max;
    }
    
    // Clamp value
    public static float Clamped(this float value, float min, float max)
    {
        return Mathf.Clamp(value, min, max);
    }
    
    // Convert to percentage string
    public static string ToPercentage(this float value)
    {
        return $"{value * 100:F1}%";
    }
    
    // Shuffle a list
    public static void Shuffle<T>(this List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}

// Usage examples
int health = 75;
if (health.IsBetween(50, 100))
{
    Console.WriteLine("Health is moderate to high");
}

float speed = 150f;
float clampedSpeed = speed.Clamped(0f, 100f);  // 100

float accuracy = 0.875f;
string display = accuracy.ToPercentage();  // "87.5%"

List<int> deck = new List<int> { 1, 2, 3, 4, 5 };
deck.Shuffle();
```

---

## Practice Challenges

Try these to test your knowledge!

### Challenge 1: Score Manager
Create a class that manages player scores with the following features:
- Add score
- Get top 10 scores
- Save/load from file
- Find player rank

### Challenge 2: Inventory System
Build an inventory that:
- Holds different item types
- Has weight limit
- Can stack items
- Can search for items

### Challenge 3: Dialogue System
Create a system that:
- Reads dialogue from JSON
- Supports branching conversations
- Tracks dialogue history
- Handles player choices

---

## Summary

You've learned:
- ✅ Collections (Lists, Dictionaries, Queues, Stacks, HashSets)
- ✅ LINQ for easy data queries
- ✅ Properties and auto-properties
- ✅ Nullable types
- ✅ String manipulation
- ✅ Exception handling
- ✅ File I/O and saving data
- ✅ Async/await for non-blocking operations
- ✅ Generics for reusable code
- ✅ Extension methods to enhance existing types

Keep practicing and building projects to master these concepts! 🚀
