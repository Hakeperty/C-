using System;
using System.Collections.Generic;

class TextAdventure
{
    static int playerHealth = 100;
    static List<string> inventory = new List<string>();
    static bool hasKey = false;
    static bool hasSword = false;
    
    static void Main()
    {
        StartGame();
    }
    
    static void StartGame()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔═══════════════════════════════════════════════╗");
        Console.WriteLine("║        THE DUNGEON ADVENTURE                  ║");
        Console.WriteLine("╚═══════════════════════════════════════════════╝");
        Console.ResetColor();
        
        Console.WriteLine("\nYou wake up in a dark dungeon...");
        Console.WriteLine("Your goal is to escape!");
        Console.WriteLine("\nPress any key to begin your adventure...");
        Console.ReadKey();
        
        Room1_Cell();
    }
    
    static void Room1_Cell()
    {
        Console.Clear();
        ShowStatus();
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("=== THE CELL ===");
        Console.ResetColor();
        Console.WriteLine("\nYou are in a cold, damp cell.");
        Console.WriteLine("There's a rusty door to the NORTH.");
        Console.WriteLine("A small window to the EAST shows moonlight.");
        
        if (!hasKey)
            Console.WriteLine("You notice something shiny in the corner...");
        
        Console.WriteLine("\nWhat do you do?");
        Console.WriteLine("1. Go NORTH (try the door)");
        Console.WriteLine("2. Check the CORNER");
        Console.WriteLine("3. Look through WINDOW");
        Console.WriteLine("4. Check INVENTORY");
        
        string choice = Console.ReadLine();
        
        switch (choice)
        {
            case "1":
                if (hasKey)
                {
                    Console.WriteLine("\nYou unlock the door with the key!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Room2_Hallway();
                }
                else
                {
                    Console.WriteLine("\nThe door is locked. You need a key!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Room1_Cell();
                }
                break;
            case "2":
                if (!hasKey)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n✨ You found a KEY! ✨");
                    Console.ResetColor();
                    hasKey = true;
                    inventory.Add("Rusty Key");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("\nNothing else here...");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
                Room1_Cell();
                break;
            case "3":
                Console.WriteLine("\nYou see a full moon outside. So beautiful yet so far...");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Room1_Cell();
                break;
            case "4":
                ShowInventory();
                Room1_Cell();
                break;
            default:
                Console.WriteLine("\nInvalid choice!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Room1_Cell();
                break;
        }
    }
    
    static void Room2_Hallway()
    {
        Console.Clear();
        ShowStatus();
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("=== THE HALLWAY ===");
        Console.ResetColor();
        Console.WriteLine("\nA long, dimly lit hallway stretches before you.");
        Console.WriteLine("You can go WEST to a storage room.");
        Console.WriteLine("Or continue NORTH toward strange sounds...");
        Console.WriteLine("You can also go BACK to your cell.");
        
        Console.WriteLine("\nWhat do you do?");
        Console.WriteLine("1. Go WEST (storage room)");
        Console.WriteLine("2. Go NORTH (toward sounds)");
        Console.WriteLine("3. Go BACK (to cell)");
        Console.WriteLine("4. Check INVENTORY");
        
        string choice = Console.ReadLine();
        
        switch (choice)
        {
            case "1":
                Room3_Storage();
                break;
            case "2":
                Room4_GuardRoom();
                break;
            case "3":
                Room1_Cell();
                break;
            case "4":
                ShowInventory();
                Room2_Hallway();
                break;
            default:
                Console.WriteLine("\nInvalid choice!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Room2_Hallway();
                break;
        }
    }
    
    static void Room3_Storage()
    {
        Console.Clear();
        ShowStatus();
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("=== THE STORAGE ROOM ===");
        Console.ResetColor();
        Console.WriteLine("\nOld barrels and crates fill this dusty room.");
        
        if (!hasSword)
            Console.WriteLine("Something gleams behind a crate...");
        
        Console.WriteLine("\nWhat do you do?");
        Console.WriteLine("1. Search behind CRATES");
        Console.WriteLine("2. Go BACK");
        Console.WriteLine("3. Check INVENTORY");
        
        string choice = Console.ReadLine();
        
        switch (choice)
        {
            case "1":
                if (!hasSword)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n⚔️ You found a SWORD! ⚔️");
                    Console.ResetColor();
                    hasSword = true;
                    inventory.Add("Iron Sword");
                    Console.WriteLine("This might help against enemies!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("\nNothing else useful here...");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
                Room3_Storage();
                break;
            case "2":
                Room2_Hallway();
                break;
            case "3":
                ShowInventory();
                Room3_Storage();
                break;
            default:
                Console.WriteLine("\nInvalid choice!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Room3_Storage();
                break;
        }
    }
    
    static void Room4_GuardRoom()
    {
        Console.Clear();
        ShowStatus();
        
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("=== THE GUARD ROOM ===");
        Console.ResetColor();
        Console.WriteLine("\n💀 A SKELETON GUARD blocks your path! 💀");
        Console.WriteLine("Behind him, you see the EXIT!");
        
        Console.WriteLine("\nWhat do you do?");
        Console.WriteLine("1. FIGHT the skeleton");
        Console.WriteLine("2. Try to RUN past him");
        Console.WriteLine("3. Go BACK");
        Console.WriteLine("4. Check INVENTORY");
        
        string choice = Console.ReadLine();
        
        switch (choice)
        {
            case "1":
                if (hasSword)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n⚔️ You attack with your sword!");
                    Console.WriteLine("The skeleton crumbles to dust!");
                    Console.WriteLine("You won the battle! ⚔️");
                    Console.ResetColor();
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    Victory();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n💀 You have no weapon!");
                    Console.WriteLine("The skeleton strikes you!");
                    Console.ResetColor();
                    playerHealth -= 50;
                    
                    if (playerHealth <= 0)
                    {
                        GameOver();
                    }
                    else
                    {
                        Console.WriteLine($"You took damage! Health: {playerHealth}");
                        Console.WriteLine("You managed to escape back to the hallway!");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Room2_Hallway();
                    }
                }
                break;
            case "2":
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nYou try to run, but the skeleton catches you!");
                Console.ResetColor();
                playerHealth -= 30;
                
                if (playerHealth <= 0)
                {
                    GameOver();
                }
                else
                {
                    Console.WriteLine($"You took damage! Health: {playerHealth}");
                    Console.WriteLine("You escaped back to the hallway!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Room2_Hallway();
                }
                break;
            case "3":
                Room2_Hallway();
                break;
            case "4":
                ShowInventory();
                Room4_GuardRoom();
                break;
            default:
                Console.WriteLine("\nInvalid choice!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Room4_GuardRoom();
                break;
        }
    }
    
    static void Victory()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("╔═══════════════════════════════════════════════╗");
        Console.WriteLine("║             🎉 VICTORY! 🎉                    ║");
        Console.WriteLine("╚═══════════════════════════════════════════════╝");
        Console.ResetColor();
        
        Console.WriteLine("\nYou defeated the skeleton guard!");
        Console.WriteLine("The exit door swings open before you.");
        Console.WriteLine("Fresh air fills your lungs as you step outside.");
        Console.WriteLine("\n✨ YOU ESCAPED THE DUNGEON! ✨");
        Console.WriteLine($"\nFinal Health: {playerHealth}/100");
        
        PlayAgain();
    }
    
    static void GameOver()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("╔═══════════════════════════════════════════════╗");
        Console.WriteLine("║             💀 GAME OVER 💀                   ║");
        Console.WriteLine("╚═══════════════════════════════════════════════╝");
        Console.ResetColor();
        
        Console.WriteLine("\nYour health reached 0...");
        Console.WriteLine("You have fallen in the dungeon.");
        Console.WriteLine("\nBetter luck next time, adventurer!");
        
        PlayAgain();
    }
    
    static void PlayAgain()
    {
        Console.Write("\nPlay again? (y/n): ");
        string choice = Console.ReadLine()?.ToLower() ?? "n";
        
        if (choice == "y" || choice == "yes")
        {
            // Reset game
            playerHealth = 100;
            inventory.Clear();
            hasKey = false;
            hasSword = false;
            StartGame();
        }
        else
        {
            Console.WriteLine("\nThanks for playing! Goodbye! 👋");
        }
    }
    
    static void ShowStatus()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"❤️  Health: {playerHealth}/100");
        Console.Write($"  |  🎒 Items: {inventory.Count}");
        Console.WriteLine();
        Console.ResetColor();
        Console.WriteLine();
    }
    
    static void ShowInventory()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("=== INVENTORY ===");
        Console.ResetColor();
        
        if (inventory.Count == 0)
        {
            Console.WriteLine("\nYour inventory is empty.");
        }
        else
        {
            Console.WriteLine();
            foreach (string item in inventory)
            {
                Console.WriteLine($"• {item}");
            }
        }
        
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}
