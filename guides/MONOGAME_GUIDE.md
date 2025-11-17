# MonoGame Development Guide

Complete guide for creating 2D and 3D games with MonoGame - a free, open-source C# framework for making games without an editor.

## Table of Contents

- [Introduction to MonoGame](#introduction-to-monogame)
- [Setup and Installation](#setup-and-installation)
- [MonoGame Architecture](#monogame-architecture)
- [2D Game Development](#2d-game-development)
- [3D Game Development](#3d-game-development)
- [Input Handling](#input-handling)
- [Audio](#audio)
- [Content Pipeline](#content-pipeline)
- [Complete Game Examples](#complete-game-examples)

---

## Introduction to MonoGame

MonoGame is an open-source implementation of the Microsoft XNA 4 Framework. It allows you to create games that run on:
- Windows, macOS, Linux
- iOS, Android
- Xbox, PlayStation, Nintendo Switch
- Web (via WebAssembly)

### Why MonoGame?

- **Code-centric**: Everything is code, no visual editor
- **Cross-platform**: Write once, deploy anywhere
- **Performance**: Direct control over the game loop
- **Learning**: Great for understanding game engine internals
- **Free**: Completely open-source and free

---

## Setup and Installation

### Prerequisites

```bash
# Install .NET SDK (version 6.0 or higher)
# Download from: https://dotnet.microsoft.com/download

# Verify installation
dotnet --version
```

### Installing MonoGame Templates

```bash
# Install MonoGame templates
dotnet new install MonoGame.Templates.CSharp

# Verify installation
dotnet new list | grep MonoGame
```

### Creating a New Project

```bash
# Create a new DesktopGL project (cross-platform)
dotnet new mgdesktopgl -o MyGame
cd MyGame

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the game
dotnet run
```

### Project Templates

- **mgdesktopgl**: Desktop (Windows, macOS, Linux)
- **mgwindowsdx**: Windows DirectX
- **mgandroid**: Android
- **mgios**: iOS
- **mguap**: Universal Windows Platform

---

## MonoGame Architecture

### Basic Game Structure

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Initialize game logic here
            // This is called once before LoadContent
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // Load textures, sounds, fonts, etc.
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed 
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Update game logic here
            // gameTime.ElapsedGameTime - time since last update
            // gameTime.TotalGameTime - total time since game start
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw game objects here
            _spriteBatch.Begin();
            // ... drawing code ...
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
```

### Game Loop Lifecycle

1. **Constructor**: Initialize graphics manager
2. **Initialize()**: Set up non-graphical components
3. **LoadContent()**: Load textures, sounds, etc.
4. **Update()**: Called 60 times per second (default)
5. **Draw()**: Render to screen
6. **UnloadContent()**: Clean up resources (called on exit)

---

## 2D Game Development

### Drawing Sprites

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class SpriteExample : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _playerTexture;
    private Vector2 _playerPosition;
    
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        // Load texture from Content folder
        // Make sure to add the image through Content.mgcb
        _playerTexture = Content.Load<Texture2D>("player");
        
        _playerPosition = new Vector2(100, 100);
    }
    
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        
        _spriteBatch.Begin();
        
        // Basic draw
        _spriteBatch.Draw(_playerTexture, _playerPosition, Color.White);
        
        // Draw with rotation, scale, and origin
        _spriteBatch.Draw(
            texture: _playerTexture,
            position: _playerPosition,
            sourceRectangle: null,  // Use entire texture
            color: Color.White,
            rotation: 0f,           // Rotation in radians
            origin: new Vector2(_playerTexture.Width / 2, _playerTexture.Height / 2),
            scale: 1.5f,            // Scale factor
            effects: SpriteEffects.None,
            layerDepth: 0f          // 0 = back, 1 = front
        );
        
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}
```

### Creating Sprites from Code

```csharp
public Texture2D CreateColoredTexture(int width, int height, Color color)
{
    Texture2D texture = new Texture2D(GraphicsDevice, width, height);
    Color[] data = new Color[width * height];
    
    for (int i = 0; i < data.Length; i++)
    {
        data[i] = color;
    }
    
    texture.SetData(data);
    return texture;
}

// Usage in LoadContent:
_playerTexture = CreateColoredTexture(64, 64, Color.Red);
```

### Sprite Animation

```csharp
public class AnimatedSprite
{
    private Texture2D _spriteSheet;
    private Rectangle[] _sourceRectangles;
    private int _currentFrame;
    private float _frameTimer;
    private float _frameTime; // Time per frame in seconds
    
    public AnimatedSprite(Texture2D spriteSheet, int frameCount, int frameWidth, int frameHeight, float frameRate)
    {
        _spriteSheet = spriteSheet;
        _frameTime = 1f / frameRate;
        _sourceRectangles = new Rectangle[frameCount];
        
        for (int i = 0; i < frameCount; i++)
        {
            _sourceRectangles[i] = new Rectangle(
                i * frameWidth,
                0,
                frameWidth,
                frameHeight
            );
        }
    }
    
    public void Update(GameTime gameTime)
    {
        _frameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        if (_frameTimer >= _frameTime)
        {
            _frameTimer = 0f;
            _currentFrame = (_currentFrame + 1) % _sourceRectangles.Length;
        }
    }
    
    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        spriteBatch.Draw(
            _spriteSheet,
            position,
            _sourceRectangles[_currentFrame],
            Color.White
        );
    }
}
```

### 2D Movement and Physics

```csharp
public class Player
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public float Speed { get; set; } = 200f;
    public Rectangle Bounds { get; set; }
    
    private Texture2D _texture;
    
    public Player(Texture2D texture, Vector2 startPosition)
    {
        _texture = texture;
        Position = startPosition;
        Bounds = new Rectangle(
            (int)Position.X,
            (int)Position.Y,
            texture.Width,
            texture.Height
        );
    }
    
    public void Update(GameTime gameTime, KeyboardState keyboardState)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        // Input handling
        Velocity = Vector2.Zero;
        
        if (keyboardState.IsKeyDown(Keys.W))
            Velocity.Y = -1;
        if (keyboardState.IsKeyDown(Keys.S))
            Velocity.Y = 1;
        if (keyboardState.IsKeyDown(Keys.A))
            Velocity.X = -1;
        if (keyboardState.IsKeyDown(Keys.D))
            Velocity.X = 1;
        
        // Normalize diagonal movement
        if (Velocity != Vector2.Zero)
        {
            Velocity.Normalize();
        }
        
        // Apply movement
        Position += Velocity * Speed * deltaTime;
        
        // Update bounds
        Bounds.X = (int)Position.X;
        Bounds.Y = (int)Position.Y;
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, Position, Color.White);
    }
}
```

### Collision Detection

```csharp
public class CollisionHelper
{
    // Rectangle collision (AABB - Axis-Aligned Bounding Box)
    public static bool CheckCollision(Rectangle rect1, Rectangle rect2)
    {
        return rect1.Intersects(rect2);
    }
    
    // Circle collision
    public static bool CircleCollision(Vector2 pos1, float radius1, Vector2 pos2, float radius2)
    {
        float distance = Vector2.Distance(pos1, pos2);
        return distance < radius1 + radius2;
    }
    
    // Point in rectangle
    public static bool PointInRectangle(Vector2 point, Rectangle rectangle)
    {
        return rectangle.Contains((int)point.X, (int)point.Y);
    }
    
    // SAT (Separating Axis Theorem) for rotated rectangles
    public static bool CheckCollisionSAT(Rectangle rect1, float rotation1, Rectangle rect2, float rotation2)
    {
        // Implementation of SAT algorithm
        // More complex but handles rotated rectangles
        return false; // Simplified for example
    }
}
```

### Camera System

```csharp
public class Camera2D
{
    public Vector2 Position { get; set; }
    public float Rotation { get; set; }
    public float Zoom { get; set; }
    public Vector2 Origin { get; set; }
    
    private Viewport _viewport;
    
    public Camera2D(Viewport viewport)
    {
        _viewport = viewport;
        Zoom = 1f;
        Rotation = 0f;
        Position = Vector2.Zero;
        Origin = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
    }
    
    public Matrix GetTransformMatrix()
    {
        return
            Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
            Matrix.CreateRotationZ(Rotation) *
            Matrix.CreateScale(Zoom, Zoom, 1) *
            Matrix.CreateTranslation(new Vector3(Origin.X, Origin.Y, 0));
    }
    
    public void Follow(Vector2 targetPosition, float smoothSpeed = 1f)
    {
        Position = Vector2.Lerp(Position, targetPosition, smoothSpeed);
    }
    
    public Vector2 ScreenToWorld(Vector2 screenPosition)
    {
        Matrix inverseTransform = Matrix.Invert(GetTransformMatrix());
        return Vector2.Transform(screenPosition, inverseTransform);
    }
}

// Usage in Draw:
_spriteBatch.Begin(transformMatrix: _camera.GetTransformMatrix());
// ... draw sprites ...
_spriteBatch.End();
```

### Text Rendering

```csharp
public class TextRenderer
{
    private SpriteFont _font;
    
    public void LoadContent(ContentManager content)
    {
        // Load font (created with Content Pipeline)
        _font = content.Load<SpriteFont>("Fonts/Arial");
    }
    
    public void DrawText(SpriteBatch spriteBatch, string text, Vector2 position, Color color)
    {
        spriteBatch.DrawString(_font, text, position, color);
    }
    
    public void DrawCenteredText(SpriteBatch spriteBatch, string text, Vector2 position, Color color)
    {
        Vector2 textSize = _font.MeasureString(text);
        Vector2 centeredPosition = position - (textSize / 2);
        spriteBatch.DrawString(_font, text, centeredPosition, color);
    }
    
    public void DrawTextWithOutline(SpriteBatch spriteBatch, string text, Vector2 position, Color color, Color outlineColor)
    {
        // Draw outline
        spriteBatch.DrawString(_font, text, position + new Vector2(-1, -1), outlineColor);
        spriteBatch.DrawString(_font, text, position + new Vector2(1, -1), outlineColor);
        spriteBatch.DrawString(_font, text, position + new Vector2(-1, 1), outlineColor);
        spriteBatch.DrawString(_font, text, position + new Vector2(1, 1), outlineColor);
        
        // Draw main text
        spriteBatch.DrawString(_font, text, position, color);
    }
}
```

---

## 3D Game Development

### Basic 3D Rendering

```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Basic3DExample : Game
{
    private GraphicsDeviceManager _graphics;
    private BasicEffect _basicEffect;
    private VertexPositionColor[] _vertices;
    
    protected override void Initialize()
    {
        // Set up 3D rendering
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();
        
        // Create triangle vertices
        _vertices = new VertexPositionColor[3];
        _vertices[0] = new VertexPositionColor(new Vector3(-1, -1, 0), Color.Red);
        _vertices[1] = new VertexPositionColor(new Vector3(0, 1, 0), Color.Green);
        _vertices[2] = new VertexPositionColor(new Vector3(1, -1, 0), Color.Blue);
        
        base.Initialize();
    }
    
    protected override void LoadContent()
    {
        _basicEffect = new BasicEffect(GraphicsDevice);
        _basicEffect.VertexColorEnabled = true;
        
        // Set up camera
        _basicEffect.View = Matrix.CreateLookAt(
            new Vector3(0, 0, 5),  // Camera position
            Vector3.Zero,          // Look at
            Vector3.Up             // Up direction
        );
        
        _basicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.PiOver4,                              // Field of view
            GraphicsDevice.Viewport.AspectRatio,             // Aspect ratio
            0.1f,                                            // Near plane
            100f                                             // Far plane
        );
    }
    
    protected override void Update(GameTime gameTime)
    {
        // Rotate the triangle
        float rotation = (float)gameTime.TotalGameTime.TotalSeconds;
        _basicEffect.World = Matrix.CreateRotationY(rotation);
        
        base.Update(gameTime);
    }
    
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        
        foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();
            
            GraphicsDevice.DrawUserPrimitives(
                PrimitiveType.TriangleList,
                _vertices,
                0,
                1  // Number of triangles
            );
        }
        
        base.Draw(gameTime);
    }
}
```

### Loading 3D Models

```csharp
public class ModelRenderer
{
    private Model _model;
    private Matrix _worldMatrix;
    private Matrix _viewMatrix;
    private Matrix _projectionMatrix;
    
    public void LoadContent(ContentManager content)
    {
        // Load model (must be processed by Content Pipeline)
        _model = content.Load<Model>("Models/Spaceship");
        
        _worldMatrix = Matrix.Identity;
        _viewMatrix = Matrix.CreateLookAt(
            new Vector3(0, 10, 20),
            Vector3.Zero,
            Vector3.Up
        );
        _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.PiOver4,
            16f / 9f,  // Aspect ratio
            0.1f,
            1000f
        );
    }
    
    public void Update(GameTime gameTime)
    {
        // Rotate model
        float rotation = (float)gameTime.TotalGameTime.TotalSeconds;
        _worldMatrix = Matrix.CreateRotationY(rotation);
    }
    
    public void Draw()
    {
        foreach (ModelMesh mesh in _model.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.World = _worldMatrix;
                effect.View = _viewMatrix;
                effect.Projection = _projectionMatrix;
                effect.EnableDefaultLighting();
            }
            
            mesh.Draw();
        }
    }
}
```

### First-Person Camera

```csharp
public class FirstPersonCamera
{
    public Vector3 Position { get; set; }
    public Vector3 Target { get; set; }
    public float Yaw { get; set; }    // Left/right rotation
    public float Pitch { get; set; }  // Up/down rotation
    public float MoveSpeed { get; set; } = 10f;
    public float MouseSensitivity { get; set; } = 0.002f;
    
    private GraphicsDevice _graphicsDevice;
    
    public FirstPersonCamera(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;
        Position = new Vector3(0, 0, 5);
        Yaw = 0;
        Pitch = 0;
    }
    
    public void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        // Mouse look
        int centerX = _graphicsDevice.Viewport.Width / 2;
        int centerY = _graphicsDevice.Viewport.Height / 2;
        
        float deltaX = mouseState.X - centerX;
        float deltaY = mouseState.Y - centerY;
        
        Yaw += deltaX * MouseSensitivity;
        Pitch -= deltaY * MouseSensitivity;
        Pitch = MathHelper.Clamp(Pitch, -MathHelper.PiOver2 + 0.01f, MathHelper.PiOver2 - 0.01f);
        
        // Reset mouse to center
        Mouse.SetPosition(centerX, centerY);
        
        // Calculate direction vectors
        Vector3 forward = new Vector3(
            (float)(Math.Cos(Pitch) * Math.Sin(Yaw)),
            (float)Math.Sin(Pitch),
            (float)(Math.Cos(Pitch) * Math.Cos(Yaw))
        );
        Vector3 right = Vector3.Cross(forward, Vector3.Up);
        right.Normalize();
        
        // Movement
        if (keyboardState.IsKeyDown(Keys.W))
            Position += forward * MoveSpeed * deltaTime;
        if (keyboardState.IsKeyDown(Keys.S))
            Position -= forward * MoveSpeed * deltaTime;
        if (keyboardState.IsKeyDown(Keys.A))
            Position -= right * MoveSpeed * deltaTime;
        if (keyboardState.IsKeyDown(Keys.D))
            Position += right * MoveSpeed * deltaTime;
        
        Target = Position + forward;
    }
    
    public Matrix GetViewMatrix()
    {
        return Matrix.CreateLookAt(Position, Target, Vector3.Up);
    }
    
    public Matrix GetProjectionMatrix()
    {
        return Matrix.CreatePerspectiveFieldOfView(
            MathHelper.PiOver4,
            _graphicsDevice.Viewport.AspectRatio,
            0.1f,
            1000f
        );
    }
}
```

---

## Input Handling

### Keyboard Input

```csharp
public class InputManager
{
    private KeyboardState _currentKeyboardState;
    private KeyboardState _previousKeyboardState;
    
    public void Update()
    {
        _previousKeyboardState = _currentKeyboardState;
        _currentKeyboardState = Keyboard.GetState();
    }
    
    public bool IsKeyDown(Keys key)
    {
        return _currentKeyboardState.IsKeyDown(key);
    }
    
    public bool IsKeyUp(Keys key)
    {
        return _currentKeyboardState.IsKeyUp(key);
    }
    
    public bool IsKeyPressed(Keys key)
    {
        return _currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key);
    }
    
    public bool IsKeyReleased(Keys key)
    {
        return _currentKeyboardState.IsKeyUp(key) && _previousKeyboardState.IsKeyDown(key);
    }
}
```

### Mouse Input

```csharp
public class MouseManager
{
    private MouseState _currentMouseState;
    private MouseState _previousMouseState;
    
    public Vector2 Position => new Vector2(_currentMouseState.X, _currentMouseState.Y);
    public Vector2 Delta => Position - new Vector2(_previousMouseState.X, _previousMouseState.Y);
    
    public void Update()
    {
        _previousMouseState = _currentMouseState;
        _currentMouseState = Mouse.GetState();
    }
    
    public bool IsLeftButtonDown() => _currentMouseState.LeftButton == ButtonState.Pressed;
    public bool IsRightButtonDown() => _currentMouseState.RightButton == ButtonState.Pressed;
    
    public bool IsLeftButtonPressed()
    {
        return _currentMouseState.LeftButton == ButtonState.Pressed
            && _previousMouseState.LeftButton == ButtonState.Released;
    }
    
    public bool IsRightButtonPressed()
    {
        return _currentMouseState.RightButton == ButtonState.Pressed
            && _previousMouseState.RightButton == ButtonState.Released;
    }
    
    public int ScrollWheelDelta()
    {
        return _currentMouseState.ScrollWheelValue - _previousMouseState.ScrollWheelValue;
    }
}
```

### Gamepad Input

```csharp
public class GamePadManager
{
    private GamePadState _currentState;
    private GamePadState _previousState;
    private PlayerIndex _playerIndex;
    
    public GamePadManager(PlayerIndex playerIndex = PlayerIndex.One)
    {
        _playerIndex = playerIndex;
    }
    
    public void Update()
    {
        _previousState = _currentState;
        _currentState = GamePad.GetState(_playerIndex);
    }
    
    public bool IsConnected() => _currentState.IsConnected;
    
    public Vector2 LeftStick() => _currentState.ThumbSticks.Left;
    public Vector2 RightStick() => _currentState.ThumbSticks.Right;
    
    public bool IsButtonPressed(Buttons button)
    {
        return _currentState.IsButtonDown(button) && _previousState.IsButtonUp(button);
    }
    
    public float LeftTrigger() => _currentState.Triggers.Left;
    public float RightTrigger() => _currentState.Triggers.Right;
}
```

---

## Audio

### Sound Effects

```csharp
using Microsoft.Xna.Framework.Audio;

public class AudioManager
{
    private Dictionary<string, SoundEffect> _soundEffects;
    private Dictionary<string, SoundEffectInstance> _soundInstances;
    
    public float MasterVolume { get; set; } = 1.0f;
    public float SFXVolume { get; set; } = 1.0f;
    public float MusicVolume { get; set; } = 1.0f;
    
    public AudioManager()
    {
        _soundEffects = new Dictionary<string, SoundEffect>();
        _soundInstances = new Dictionary<string, SoundEffectInstance>();
    }
    
    public void LoadSound(ContentManager content, string name, string assetPath)
    {
        SoundEffect sound = content.Load<SoundEffect>(assetPath);
        _soundEffects[name] = sound;
    }
    
    public void PlaySound(string name, float volume = 1.0f, float pitch = 0f, float pan = 0f)
    {
        if (_soundEffects.TryGetValue(name, out SoundEffect sound))
        {
            sound.Play(volume * SFXVolume * MasterVolume, pitch, pan);
        }
    }
    
    public void CreateSoundInstance(string name)
    {
        if (_soundEffects.TryGetValue(name, out SoundEffect sound))
        {
            _soundInstances[name] = sound.CreateInstance();
        }
    }
    
    public void PlayLoopingSound(string name)
    {
        if (_soundInstances.TryGetValue(name, out SoundEffectInstance instance))
        {
            instance.IsLooped = true;
            instance.Volume = SFXVolume * MasterVolume;
            instance.Play();
        }
    }
    
    public void StopSound(string name)
    {
        if (_soundInstances.TryGetValue(name, out SoundEffectInstance instance))
        {
            instance.Stop();
        }
    }
}
```

### Music Playback

```csharp
using Microsoft.Xna.Framework.Media;

public class MusicPlayer
{
    private Dictionary<string, Song> _songs;
    private string _currentSong;
    
    public MusicPlayer()
    {
        _songs = new Dictionary<string, Song>();
    }
    
    public void LoadSong(ContentManager content, string name, string assetPath)
    {
        Song song = content.Load<Song>(assetPath);
        _songs[name] = song;
    }
    
    public void PlaySong(string name, bool repeat = true)
    {
        if (_songs.TryGetValue(name, out Song song))
        {
            MediaPlayer.IsRepeating = repeat;
            MediaPlayer.Play(song);
            _currentSong = name;
        }
    }
    
    public void Stop()
    {
        MediaPlayer.Stop();
        _currentSong = null;
    }
    
    public void Pause()
    {
        MediaPlayer.Pause();
    }
    
    public void Resume()
    {
        MediaPlayer.Resume();
    }
    
    public void SetVolume(float volume)
    {
        MediaPlayer.Volume = MathHelper.Clamp(volume, 0f, 1f);
    }
}
```

---

## Content Pipeline

### Creating Content

1. **Install MGCB Editor**:
```bash
dotnet tool install -g dotnet-mgcb-editor
mgcb-editor --register
```

2. **Open Content.mgcb**:
   - Double-click Content.mgcb in your project
   - Or run: `mgcb-editor Content/Content.mgcb`

3. **Add Content**:
   - Right-click → Add → Existing Item
   - Select your images, sounds, fonts, models
   - Build the content

### Supported File Types

- **Images**: PNG, JPG, BMP, DDS
- **Audio**: WAV, MP3, WMA, OGG
- **Fonts**: SpriteFont (.spritefont)
- **3D Models**: FBX, X
- **Effects**: .fx shaders

---

## Complete Game Examples

### Simple Platformer

```csharp
public class PlatformerGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private Texture2D _playerTexture;
    private Texture2D _platformTexture;
    
    private Vector2 _playerPosition;
    private Vector2 _playerVelocity;
    private List<Rectangle> _platforms;
    
    private const float GRAVITY = 800f;
    private const float JUMP_FORCE = -400f;
    private const float MOVE_SPEED = 200f;
    
    private bool _isGrounded;
    
    public PlatformerGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
    }
    
    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = 800;
        _graphics.PreferredBackBufferHeight = 600;
        _graphics.ApplyChanges();
        
        _playerPosition = new Vector2(100, 100);
        _playerVelocity = Vector2.Zero;
        
        // Create platforms
        _platforms = new List<Rectangle>
        {
            new Rectangle(0, 550, 800, 50),        // Ground
            new Rectangle(200, 400, 200, 20),      // Platform 1
            new Rectangle(500, 300, 200, 20),      // Platform 2
        };
        
        base.Initialize();
    }
    
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        // Create simple colored textures
        _playerTexture = CreateTexture(32, 32, Color.Red);
        _platformTexture = CreateTexture(1, 1, Color.Green);
    }
    
    protected override void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        KeyboardState keyboard = Keyboard.GetState();
        
        // Horizontal movement
        float moveInput = 0;
        if (keyboard.IsKeyDown(Keys.Left)) moveInput = -1;
        if (keyboard.IsKeyDown(Keys.Right)) moveInput = 1;
        
        _playerVelocity.X = moveInput * MOVE_SPEED;
        
        // Jump
        if (keyboard.IsKeyDown(Keys.Space) && _isGrounded)
        {
            _playerVelocity.Y = JUMP_FORCE;
        }
        
        // Apply gravity
        _playerVelocity.Y += GRAVITY * deltaTime;
        
        // Update position
        _playerPosition += _playerVelocity * deltaTime;
        
        // Collision detection
        Rectangle playerRect = new Rectangle(
            (int)_playerPosition.X,
            (int)_playerPosition.Y,
            32, 32
        );
        
        _isGrounded = false;
        
        foreach (Rectangle platform in _platforms)
        {
            if (playerRect.Intersects(platform))
            {
                // Simple collision resolution
                if (_playerVelocity.Y > 0) // Falling
                {
                    _playerPosition.Y = platform.Top - 32;
                    _playerVelocity.Y = 0;
                    _isGrounded = true;
                }
            }
        }
        
        base.Update(gameTime);
    }
    
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        
        _spriteBatch.Begin();
        
        // Draw platforms
        foreach (Rectangle platform in _platforms)
        {
            _spriteBatch.Draw(_platformTexture, platform, Color.Green);
        }
        
        // Draw player
        _spriteBatch.Draw(_playerTexture, _playerPosition, Color.White);
        
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
    
    private Texture2D CreateTexture(int width, int height, Color color)
    {
        Texture2D texture = new Texture2D(GraphicsDevice, width, height);
        Color[] data = new Color[width * height];
        for (int i = 0; i < data.Length; i++) data[i] = color;
        texture.SetData(data);
        return texture;
    }
}
```

---

## Best Practices

1. **Use Object Pooling** for frequently created objects
2. **Cache Content** - don't reload in Update/Draw
3. **Use SpriteBatch Efficiently** - minimize Begin/End calls
4. **Profile Your Game** - use built-in diagnostics
5. **Handle Different Resolutions** properly
6. **Use Fixed Time Step** for consistent physics
7. **Dispose Resources** properly in UnloadContent

---

## Resources

- **Official Site**: [monogame.net](https://monogame.net)
- **Documentation**: [docs.monogame.net](http://docs.monogame.net/)
- **Community**: [MonoGame Forums](https://community.monogame.net/)
- **GitHub**: [MonoGame Repository](https://github.com/MonoGame/MonoGame)

---

## Next Steps

1. Complete a simple 2D game (Pong, Breakout, or Platformer)
2. Experiment with particle systems
3. Implement advanced collision detection
4. Add menus and game states
5. Try 3D rendering
6. Deploy to multiple platforms

**Happy MonoGame development!**
