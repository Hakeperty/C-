# Complete C# Game Development Guide

Welcome to the comprehensive guide for game development in C#! This guide covers both 2D and 3D game development with two approaches:

1. **[Part 1: Game Development with Editors (Unity)](#part-1-game-development-with-editors)**
2. **[Part 2: Game Development without Editors](#part-2-game-development-without-editors)**

## Table of Contents

- [Prerequisites](#prerequisites)
- [C# Programming Fundamentals](#c-programming-fundamentals)
- [Part 1: Game Development with Editors](#part-1-game-development-with-editors)
- [Part 2: Game Development without Editors](#part-2-game-development-without-editors)
- [Additional Resources](#additional-resources)

---

## Prerequisites

Before diving into game development, ensure you have:

- Basic understanding of programming concepts
- C# development environment (Visual Studio, Visual Studio Code, or JetBrains Rider)
- .NET SDK installed (latest version recommended)
- Passion for creating games!

---

## C# Programming Fundamentals

Before creating games, you need to understand core C# concepts that are essential for game development.

### Variables and Data Types

```csharp
// Basic data types used in games
int score = 0;
float speed = 5.5f;
bool isGameOver = false;
string playerName = "Hero";

// Constants
const int MAX_HEALTH = 100;
```

### Functions/Methods

Functions are reusable blocks of code that perform specific tasks.

```csharp
// Function without parameters
void StartGame()
{
    Console.WriteLine("Game Started!");
}

// Function with parameters
int CalculateDamage(int baseDamage, float multiplier)
{
    return (int)(baseDamage * multiplier);
}

// Function with return value
float GetPlayerHealth()
{
    return currentHealth;
}
```

### Loops

Loops allow you to repeat code multiple times.

#### For Loop
```csharp
// Spawn 10 enemies
for (int i = 0; i < 10; i++)
{
    SpawnEnemy(i);
}
```

#### While Loop
```csharp
// Game loop pattern
while (isGameRunning)
{
    Update();
    Render();
}
```

#### Foreach Loop
```csharp
// Iterate through all enemies
foreach (Enemy enemy in enemies)
{
    enemy.Update();
}
```

### Conditional Statements

```csharp
// If-else statements
if (health <= 0)
{
    GameOver();
}
else if (health < 30)
{
    PlayLowHealthWarning();
}
else
{
    // Continue playing
}

// Switch statement for game states
switch (currentState)
{
    case GameState.Menu:
        ShowMenu();
        break;
    case GameState.Playing:
        UpdateGame();
        break;
    case GameState.Paused:
        ShowPauseMenu();
        break;
    default:
        break;
}
```

### Classes and Objects

Classes are blueprints for creating objects in your game.

```csharp
public class Player
{
    // Properties
    public string Name { get; set; }
    public int Health { get; private set; }
    public float Speed { get; set; }
    
    // Constructor
    public Player(string name)
    {
        Name = name;
        Health = 100;
        Speed = 5.0f;
    }
    
    // Methods
    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        Console.WriteLine($"{Name} has died!");
    }
}

// Using the class
Player player = new Player("Hero");
player.TakeDamage(25);
```

### Arrays and Lists

```csharp
// Fixed-size array
int[] scores = new int[10];
scores[0] = 100;

// Dynamic list
List<Enemy> enemies = new List<Enemy>();
enemies.Add(new Enemy());
enemies.Remove(enemies[0]);

// 2D array for tile maps
int[,] tileMap = new int[10, 10];
tileMap[0, 0] = 1; // Grass tile
```

### Inheritance

```csharp
// Base class
public class GameObject
{
    public float X { get; set; }
    public float Y { get; set; }
    
    public virtual void Update()
    {
        // Base update logic
    }
}

// Derived classes
public class Enemy : GameObject
{
    public int Damage { get; set; }
    
    public override void Update()
    {
        base.Update();
        // Enemy-specific update logic
    }
}

public class PowerUp : GameObject
{
    public string PowerType { get; set; }
    
    public override void Update()
    {
        base.Update();
        // PowerUp-specific update logic
    }
}
```

### Events and Delegates

Events are crucial for game development, allowing objects to communicate.

```csharp
// Delegate definition
public delegate void GameEventHandler();

public class GameManager
{
    // Event declaration
    public event GameEventHandler OnGameOver;
    public event GameEventHandler OnLevelComplete;
    
    public void TriggerGameOver()
    {
        OnGameOver?.Invoke(); // Notify all subscribers
    }
}

// Usage
GameManager gameManager = new GameManager();
gameManager.OnGameOver += () => Console.WriteLine("Game Over!");
gameManager.OnGameOver += SaveHighScore;
```

---

## Part 1: Game Development with Editors

For detailed information about game development with Unity and other editors, see:
- **[Unity 2D Game Development Guide](./guides/UNITY_2D_GUIDE.md)**
- **[Unity 3D Game Development Guide](./guides/UNITY_3D_GUIDE.md)**

### Overview of Unity Game Engine

Unity is one of the most popular game engines, offering:
- Visual editor for scene creation
- Built-in physics engine (2D and 3D)
- Asset store with ready-made assets
- Cross-platform deployment
- Strong C# integration

### Quick Start with Unity

1. **Download and Install Unity Hub**
   - Visit [unity.com](https://unity.com)
   - Download Unity Hub
   - Install Unity Editor (LTS version recommended)

2. **Create a New Project**
   - Open Unity Hub
   - Click "New Project"
   - Choose 2D or 3D template
   - Name your project and select location

3. **Understanding the Unity Interface**
   - **Scene View**: Where you design your game levels
   - **Game View**: Where you test your game
   - **Hierarchy**: Lists all objects in your scene
   - **Inspector**: Shows properties of selected objects
   - **Project**: Your asset library

### Basic Unity C# Script Structure

```csharp
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables visible in Unity Inspector
    public float speed = 5f;
    public int health = 100;
    
    // Private variables
    private Rigidbody2D rb;
    
    // Called once when the script is first loaded
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Log("Player initialized!");
    }
    
    // Called once per frame
    void Update()
    {
        // Input handling
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        // Movement
        Vector2 movement = new Vector2(horizontal, vertical);
        rb.velocity = movement * speed;
    }
    
    // Called at fixed time intervals (for physics)
    void FixedUpdate()
    {
        // Physics calculations go here
    }
    
    // Called when another collider touches this object
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(10);
        }
    }
    
    void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
```

### Unity 2D Game Development

#### Setting Up a 2D Project

1. Create a new 2D project in Unity
2. Import or create 2D sprites
3. Add a player GameObject with a SpriteRenderer
4. Add Rigidbody2D for physics
5. Add Collider2D for collision detection

#### Example: Simple 2D Player Movement

```csharp
using UnityEngine;

public class Player2DMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    
    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    
    private Rigidbody2D rb;
    private bool isGrounded;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        // Check if player is on ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        
        // Horizontal movement
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        
        // Flip sprite based on direction
        if (moveInput < 0)
            spriteRenderer.flipX = true;
        else if (moveInput > 0)
            spriteRenderer.flipX = false;
        
        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}
```

#### 2D Camera Follow Script

```csharp
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 0, -10);
    
    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
```

### Unity 3D Game Development

#### Setting Up a 3D Project

1. Create a new 3D project in Unity
2. Add a player GameObject (capsule or custom model)
3. Add Rigidbody for physics
4. Add Collider for collision detection
5. Set up lighting and skybox

#### Example: First-Person Controller

```csharp
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;
    
    [Header("Mouse Settings")]
    public float mouseSensitivity = 2f;
    public Transform playerCamera;
    
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        // Ground check
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        // Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
        
        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
        
        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        
        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
```

#### 3D Third-Person Camera

```csharp
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 5f;
    public float height = 2f;
    public float rotationSpeed = 5f;
    public float smoothSpeed = 0.125f;
    
    private float currentRotation = 0f;
    
    void LateUpdate()
    {
        if (target == null) return;
        
        // Rotate camera with mouse
        float horizontalInput = Input.GetAxis("Mouse X");
        currentRotation += horizontalInput * rotationSpeed;
        
        // Calculate position
        Quaternion rotation = Quaternion.Euler(0, currentRotation, 0);
        Vector3 desiredPosition = target.position - (rotation * Vector3.forward * distance);
        desiredPosition.y = target.position.y + height;
        
        // Smooth camera movement
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.LookAt(target.position + Vector3.up * height);
    }
}
```

### Unity Common Patterns

#### Singleton Pattern

```csharp
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public int Score { get; set; }
    public int Lives { get; set; }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

// Usage from any script:
// GameManager.Instance.Score += 100;
```

#### Object Pooling

```csharp
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 10;
    
    private Queue<GameObject> pool = new Queue<GameObject>();
    
    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }
    
    public GameObject GetObject()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            return Instantiate(prefab);
        }
    }
    
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
```

---

## Part 2: Game Development without Editors

For detailed information about engine-free game development, see:
- **[MonoGame 2D/3D Development Guide](./guides/MONOGAME_GUIDE.md)**
- **[Raylib C# Development Guide](./guides/RAYLIB_GUIDE.md)**

### Overview

Developing games without an editor gives you:
- Complete control over every aspect
- Better understanding of game architecture
- Lighter weight projects
- More flexibility

Popular C# frameworks for editor-free development:
- **MonoGame**: Cross-platform, similar to XNA
- **Raylib-cs**: Simple and easy to learn
- **Silk.NET**: Modern, low-level graphics
- **OpenTK**: OpenGL bindings for C#

### MonoGame Introduction

MonoGame is an open-source framework for creating games without an editor.

#### Setting Up MonoGame

```bash
# Install MonoGame templates
dotnet new install MonoGame.Templates.CSharp

# Create a new 2D game project
dotnet new mgdesktopgl -o MyGame
cd MyGame

# Restore packages
dotnet restore

# Run the game
dotnet run
```

#### Basic MonoGame Structure

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        
        // Game variables
        private Texture2D playerTexture;
        private Vector2 playerPosition;
        private float playerSpeed = 200f;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        
        protected override void Initialize()
        {
            // Initialize game logic
            playerPosition = new Vector2(100, 100);
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // Load textures
            // playerTexture = Content.Load<Texture2D>("player");
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed 
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            // Get input
            KeyboardState keyboardState = Keyboard.GetState();
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // Movement
            if (keyboardState.IsKeyDown(Keys.W))
                playerPosition.Y -= playerSpeed * deltaTime;
            if (keyboardState.IsKeyDown(Keys.S))
                playerPosition.Y += playerSpeed * deltaTime;
            if (keyboardState.IsKeyDown(Keys.A))
                playerPosition.X -= playerSpeed * deltaTime;
            if (keyboardState.IsKeyDown(Keys.D))
                playerPosition.X += playerSpeed * deltaTime;
            
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            spriteBatch.Begin();
            
            // Draw game objects
            // spriteBatch.Draw(playerTexture, playerPosition, Color.White);
            
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
```

### Creating a Complete 2D Game Loop

```csharp
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyGame
{
    public class GameObject
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public bool IsActive { get; set; }
        
        public virtual void Update(GameTime gameTime)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // Override in derived classes
        }
    }
    
    public class Player : GameObject
    {
        private Texture2D texture;
        public float Speed { get; set; } = 300f;
        public int Health { get; set; } = 100;
        
        public Player(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            Position = position;
            IsActive = true;
        }
        
        public void HandleInput(KeyboardState keyboardState, GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 movement = Vector2.Zero;
            
            if (keyboardState.IsKeyDown(Keys.W)) movement.Y -= 1;
            if (keyboardState.IsKeyDown(Keys.S)) movement.Y += 1;
            if (keyboardState.IsKeyDown(Keys.A)) movement.X -= 1;
            if (keyboardState.IsKeyDown(Keys.D)) movement.X += 1;
            
            if (movement != Vector2.Zero)
            {
                movement.Normalize();
                Position += movement * Speed * deltaTime;
            }
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null)
                spriteBatch.Draw(texture, Position, Color.White);
        }
    }
    
    public class Enemy : GameObject
    {
        private Texture2D texture;
        public int Damage { get; set; } = 10;
        
        public Enemy(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            Position = position;
            IsActive = true;
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null)
                spriteBatch.Draw(texture, Position, Color.Red);
        }
    }
}
```

### Collision Detection

```csharp
public class CollisionHelper
{
    public static bool CheckCollision(Vector2 pos1, int width1, int height1,
                                     Vector2 pos2, int width2, int height2)
    {
        Rectangle rect1 = new Rectangle((int)pos1.X, (int)pos1.Y, width1, height1);
        Rectangle rect2 = new Rectangle((int)pos2.X, (int)pos2.Y, width2, height2);
        
        return rect1.Intersects(rect2);
    }
    
    public static bool CircleCollision(Vector2 pos1, float radius1,
                                      Vector2 pos2, float radius2)
    {
        float distance = Vector2.Distance(pos1, pos2);
        return distance < radius1 + radius2;
    }
}
```

### Simple 3D with MonoGame

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Simple3DGame : Game
{
    private GraphicsDeviceManager graphics;
    private BasicEffect basicEffect;
    
    private Matrix worldMatrix;
    private Matrix viewMatrix;
    private Matrix projectionMatrix;
    
    private VertexPositionColor[] vertices;
    
    public Simple3DGame()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
    }
    
    protected override void Initialize()
    {
        // Set up matrices
        worldMatrix = Matrix.Identity;
        viewMatrix = Matrix.CreateLookAt(
            new Vector3(0, 0, 5),  // Camera position
            new Vector3(0, 0, 0),  // Look at position
            Vector3.Up);           // Up direction
        
        projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.PiOver4,
            GraphicsDevice.Viewport.AspectRatio,
            0.1f,
            100f);
        
        base.Initialize();
    }
    
    protected override void LoadContent()
    {
        // Create a triangle
        vertices = new VertexPositionColor[3];
        vertices[0] = new VertexPositionColor(new Vector3(-1, -1, 0), Color.Red);
        vertices[1] = new VertexPositionColor(new Vector3(0, 1, 0), Color.Green);
        vertices[2] = new VertexPositionColor(new Vector3(1, -1, 0), Color.Blue);
        
        // Set up effect
        basicEffect = new BasicEffect(GraphicsDevice);
        basicEffect.VertexColorEnabled = true;
        basicEffect.World = worldMatrix;
        basicEffect.View = viewMatrix;
        basicEffect.Projection = projectionMatrix;
    }
    
    protected override void Update(GameTime gameTime)
    {
        // Rotate the triangle
        worldMatrix *= Matrix.CreateRotationY(0.01f);
        basicEffect.World = worldMatrix;
        
        base.Update(gameTime);
    }
    
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        
        foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();
            GraphicsDevice.DrawUserPrimitives(
                PrimitiveType.TriangleList,
                vertices,
                0,
                1);
        }
        
        base.Draw(gameTime);
    }
}
```

### Game Architecture Patterns

#### Game State Management

```csharp
public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}

public class StateManager
{
    private GameState currentState;
    private Dictionary<GameState, IGameState> states;
    
    public StateManager()
    {
        states = new Dictionary<GameState, IGameState>();
    }
    
    public void AddState(GameState state, IGameState stateObject)
    {
        states[state] = stateObject;
    }
    
    public void ChangeState(GameState newState)
    {
        if (states.ContainsKey(currentState))
            states[currentState].Exit();
        
        currentState = newState;
        
        if (states.ContainsKey(currentState))
            states[currentState].Enter();
    }
    
    public void Update(GameTime gameTime)
    {
        if (states.ContainsKey(currentState))
            states[currentState].Update(gameTime);
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        if (states.ContainsKey(currentState))
            states[currentState].Draw(spriteBatch);
    }
}

public interface IGameState
{
    void Enter();
    void Exit();
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
}
```

#### Entity Component System (ECS) Basics

```csharp
public interface IComponent { }

public class TransformComponent : IComponent
{
    public Vector2 Position { get; set; }
    public float Rotation { get; set; }
    public Vector2 Scale { get; set; }
}

public class SpriteComponent : IComponent
{
    public Texture2D Texture { get; set; }
    public Color Tint { get; set; }
}

public class Entity
{
    private Dictionary<Type, IComponent> components;
    
    public Entity()
    {
        components = new Dictionary<Type, IComponent>();
    }
    
    public void AddComponent<T>(T component) where T : IComponent
    {
        components[typeof(T)] = component;
    }
    
    public T GetComponent<T>() where T : IComponent
    {
        if (components.TryGetValue(typeof(T), out IComponent component))
            return (T)component;
        return default(T);
    }
    
    public bool HasComponent<T>() where T : IComponent
    {
        return components.ContainsKey(typeof(T));
    }
}
```

---

## Additional Resources

### Learning Resources

- **Unity Learn**: [learn.unity.com](https://learn.unity.com)
- **MonoGame Documentation**: [docs.monogame.net](http://docs.monogame.net/)
- **Microsoft C# Documentation**: [docs.microsoft.com/dotnet/csharp](https://docs.microsoft.com/dotnet/csharp/)
- **Game Programming Patterns**: [gameprogrammingpatterns.com](https://gameprogrammingpatterns.com/)

### Recommended Books

- "Unity in Action" by Joe Hocking
- "MonoGame Mastery" by Jarred Capellman
- "C# Game Programming Cookbook for Unity 3D"
- "Game Programming Patterns" by Robert Nystrom

### Communities

- Unity Forums
- r/Unity3D on Reddit
- r/gamedev on Reddit
- MonoGame Community Forums
- Game Dev Stack Exchange

### Tools and Assets

- **Art & Graphics**:
  - Aseprite (pixel art)
  - GIMP (free image editor)
  - Blender (3D modeling)
  
- **Audio**:
  - Audacity (audio editing)
  - BFXR (sound effects generator)
  - Bosca Ceoil (music creation)

- **Version Control**:
  - Git with GitHub/GitLab
  - Plastic SCM (Unity integration)

---

## Next Steps

1. Choose your path: With Editor (Unity) or Without Editor (MonoGame/Raylib)
2. Set up your development environment
3. Follow the detailed guides in the `/guides` folder
4. Start with small projects and gradually increase complexity
5. Join communities and share your progress
6. Practice regularly and build complete projects

**Remember**: Game development is a journey. Start small, learn continuously, and most importantly, have fun creating!

---

## Contributing

This guide is open for contributions. If you find errors or want to add more content, feel free to submit a pull request!

## License

This guide is provided as-is for educational purposes. Feel free to use and share!
