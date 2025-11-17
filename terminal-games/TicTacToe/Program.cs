using System;

class TicTacToe
{
    static char[] board = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
    static char currentPlayer = 'X';
    static int moveCount = 0;
    
    static void Main()
    {
        PlayGame();
    }
    
    static void PlayGame()
    {
        ResetGame();
        
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔═══════════════════════════════════╗");
        Console.WriteLine("║        TIC-TAC-TOE GAME           ║");
        Console.WriteLine("╚═══════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine("\nPlayer 1: X  |  Player 2: O\n");
        
        bool gameWon = false;
        bool gameDraw = false;
        
        while (!gameWon && !gameDraw)
        {
            DrawBoard();
            
            Console.ForegroundColor = currentPlayer == 'X' ? ConsoleColor.Green : ConsoleColor.Yellow;
            Console.WriteLine($"\nPlayer {currentPlayer}'s turn");
            Console.ResetColor();
            Console.Write("Enter position (1-9): ");
            
            if (!int.TryParse(Console.ReadLine(), out int position) || position < 1 || position > 9)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input! Please enter a number between 1 and 9.");
                Console.ResetColor();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                continue;
            }
            
            if (!MakeMove(position))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("That position is already taken!");
                Console.ResetColor();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                continue;
            }
            
            moveCount++;
            
            if (CheckWin())
            {
                gameWon = true;
                Console.Clear();
                DrawBoard();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n🎉 Player {currentPlayer} WINS! 🎉");
                Console.ResetColor();
            }
            else if (moveCount == 9)
            {
                gameDraw = true;
                Console.Clear();
                DrawBoard();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n🤝 It's a DRAW! 🤝");
                Console.ResetColor();
            }
            else
            {
                SwitchPlayer();
            }
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
    
    static void ResetGame()
    {
        board = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        currentPlayer = 'X';
        moveCount = 0;
    }
    
    static void DrawBoard()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔═══════════════════════════════════╗");
        Console.WriteLine("║        TIC-TAC-TOE GAME           ║");
        Console.WriteLine("╚═══════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();
        
        for (int i = 0; i < 9; i += 3)
        {
            Console.Write("     ");
            for (int j = 0; j < 3; j++)
            {
                char cell = board[i + j];
                
                if (cell == 'X')
                    Console.ForegroundColor = ConsoleColor.Green;
                else if (cell == 'O')
                    Console.ForegroundColor = ConsoleColor.Yellow;
                else
                    Console.ForegroundColor = ConsoleColor.Gray;
                
                Console.Write($" {cell} ");
                Console.ResetColor();
                
                if (j < 2)
                    Console.Write("│");
            }
            Console.WriteLine();
            
            if (i < 6)
                Console.WriteLine("    ────┼────┼────");
        }
        
        Console.WriteLine();
    }
    
    static bool MakeMove(int position)
    {
        int index = position - 1;
        
        if (board[index] != 'X' && board[index] != 'O')
        {
            board[index] = currentPlayer;
            return true;
        }
        
        return false;
    }
    
    static void SwitchPlayer()
    {
        currentPlayer = (currentPlayer == 'X') ? 'O' : 'X';
    }
    
    static bool CheckWin()
    {
        // Check rows
        for (int i = 0; i < 9; i += 3)
        {
            if (board[i] == board[i + 1] && board[i + 1] == board[i + 2])
                return true;
        }
        
        // Check columns
        for (int i = 0; i < 3; i++)
        {
            if (board[i] == board[i + 3] && board[i + 3] == board[i + 6])
                return true;
        }
        
        // Check diagonals
        if (board[0] == board[4] && board[4] == board[8])
            return true;
        
        if (board[2] == board[4] && board[4] == board[6])
            return true;
        
        return false;
    }
}
