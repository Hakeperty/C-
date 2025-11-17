# Raylib-cs Development Guide

Complete guide for creating 2D and 3D games with Raylib-cs - a simple and easy-to-use C# binding for Raylib game library.

## Table of Contents

- [Introduction to Raylib](#introduction-to-raylib)
- [Setup and Installation](#setup-and-installation)
- [Basic Structure](#basic-structure)
- [2D Graphics](#2d-graphics)
- [3D Graphics](#3d-graphics)
- [Input Handling](#input-handling)
- [Audio](#audio)
- [Complete Examples](#complete-examples)

---

## Introduction to Raylib

Raylib is a simple and easy-to-use library to enjoy videogames programming. Raylib-cs is the C# binding for Raylib.

### Why Raylib?

- **Simple**: Very easy API, perfect for beginners
- **Fast**: Written in C, highly optimized
- **Lightweight**: No external dependencies
- **Cross-platform**: Windows, Linux, macOS, Web, Mobile
- **Free**: zlib/libpng licensed
- **No Editor**: Pure code approach

---

## Setup and Installation

### Installing Raylib-cs via NuGet

```bash
# Create new console project
dotnet new console -o MyRaylibGame
cd MyRaylibGame

# Add Raylib-cs package
dotnet add package Raylib-cs

# Run the project
dotnet run
```

### Basic Project Setup

```csharp
using Raylib_cs;
using System.Numerics;

namespace MyRaylibGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialization
            const int screenWidth = 800;
            const int screenHeight = 450;
            
            Raylib.InitWindow(screenWidth, screenHeight, "My Raylib Game");
            Raylib.SetTargetFPS(60);
            
            // Main game loop
            while (!Raylib.WindowShouldClose())
            {
                // Update
                // ... game logic ...
                
                // Draw
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.RAYWHITE);
                
                Raylib.DrawText("Hello Raylib!", 190, 200, 20, Color.LIGHTGRAY);
                
                Raylib.EndDrawing();
            }
            
            // Cleanup
            Raylib.CloseWindow();
        }
    }
}
```

---

## Basic Structure

### Window Management

```csharp
using Raylib_cs;

// Initialize window
Raylib.InitWindow(800, 600, "Window Title");
Raylib.SetTargetFPS(60);

// Window state
bool isFullscreen = Raylib.IsWindowFullscreen();
bool isMinimized = Raylib.IsWindowMinimized();
bool isFocused = Raylib.IsWindowFocused();

// Toggle fullscreen
if (Raylib.IsKeyPressed(KeyboardKey.KEY_F11))
{
    Raylib.ToggleFullscreen();
}

// Get screen size
int width = Raylib.GetScreenWidth();
int height = Raylib.GetScreenHeight();

// Set window size
Raylib.SetWindowSize(1024, 768);
```

### Timing and FPS

```csharp
// Get delta time (time between frames)
float deltaTime = Raylib.GetFrameTime();

// Get FPS
int fps = Raylib.GetFPS();

// Show FPS
Raylib.DrawFPS(10, 10);

// Get elapsed time
float time = Raylib.GetTime();
```

---

## 2D Graphics

### Drawing Basic Shapes

```csharp
using Raylib_cs;
using System.Numerics;

Raylib.BeginDrawing();
Raylib.ClearBackground(Color.RAYWHITE);

// Draw circle
Raylib.DrawCircle(100, 100, 50, Color.RED);
Raylib.DrawCircleV(new Vector2(200, 100), 50, Color.GREEN);

// Draw rectangle
Raylib.DrawRectangle(300, 50, 100, 100, Color.BLUE);
Raylib.DrawRectangleRec(new Rectangle(450, 50, 100, 100), Color.ORANGE);

// Draw line
Raylib.DrawLine(50, 200, 750, 200, Color.BLACK);
Raylib.DrawLineV(new Vector2(50, 250), new Vector2(750, 250), Color.PURPLE);

// Draw triangle
Raylib.DrawTriangle(
    new Vector2(400, 300),
    new Vector2(350, 400),
    new Vector2(450, 400),
    Color.YELLOW
);

// Draw polygon
Raylib.DrawPoly(new Vector2(600, 350), 6, 50, 0, Color.DARKGREEN);

Raylib.EndDrawing();
```

### Loading and Drawing Textures

```csharp
using Raylib_cs;
using System.Numerics;

class TextureExample
{
    static void Main()
    {
        Raylib.InitWindow(800, 600, "Texture Example");
        Raylib.SetTargetFPS(60);
        
        // Load texture
        Texture2D texture = Raylib.LoadTexture("player.png");
        
        Vector2 position = new Vector2(100, 100);
        
        while (!Raylib.WindowShouldClose())
        {
            // Update
            if (Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT)) position.X += 2;
            if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT)) position.X -= 2;
            if (Raylib.IsKeyDown(KeyboardKey.KEY_DOWN)) position.Y += 2;
            if (Raylib.IsKeyDown(KeyboardKey.KEY_UP)) position.Y -= 2;
            
            // Draw
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.RAYWHITE);
            
            // Draw texture
            Raylib.DrawTextureV(texture, position, Color.WHITE);
            
            // Draw texture with rotation and scale
            Raylib.DrawTextureEx(
                texture,
                position,
                0f,              // rotation
                2.0f,            // scale
                Color.WHITE
            );
            
            // Draw part of texture (sprite sheet)
            Rectangle source = new Rectangle(0, 0, 64, 64);
            Rectangle dest = new Rectangle(position.X, position.Y, 128, 128);
            Vector2 origin = new Vector2(32, 32);
            
            Raylib.DrawTexturePro(
                texture,
                source,
                dest,
                origin,
                45f,             // rotation
                Color.WHITE
            );
            
            Raylib.EndDrawing();
        }
        
        // Unload texture
        Raylib.UnloadTexture(texture);
        Raylib.CloseWindow();
    }
}
```

### Text Rendering

```csharp
using Raylib_cs;
using System.Numerics;

// Draw basic text
Raylib.DrawText("Hello World!", 100, 100, 20, Color.BLACK);

// Draw text with different font sizes
Raylib.DrawText("Small", 100, 150, 10, Color.GRAY);
Raylib.DrawText("Medium", 100, 170, 20, Color.DARKGRAY);
Raylib.DrawText("Large", 100, 200, 40, Color.BLACK);

// Load custom font
Font customFont = Raylib.LoadFont("myfont.ttf");
Raylib.DrawTextEx(customFont, "Custom Font!", new Vector2(100, 300), 32, 2, Color.MAROON);

// Measure text size
int textWidth = Raylib.MeasureText("Hello", 20);
Vector2 textSize = Raylib.MeasureTextEx(customFont, "Hello", 32, 2);

// Unload font
Raylib.UnloadFont(customFont);
```

### Sprite Animation

```csharp
using Raylib_cs;
using System.Numerics;

class SpriteAnimation
{
    private Texture2D spriteSheet;
    private int frameWidth;
    private int frameHeight;
    private int currentFrame;
    private int totalFrames;
    private float frameTime;
    private float frameCounter;
    
    public SpriteAnimation(string texturePath, int frameWidth, int frameHeight, int totalFrames, float fps)
    {
        spriteSheet = Raylib.LoadTexture(texturePath);
        this.frameWidth = frameWidth;
        this.frameHeight = frameHeight;
        this.totalFrames = totalFrames;
        this.frameTime = 1.0f / fps;
        this.currentFrame = 0;
        this.frameCounter = 0;
    }
    
    public void Update(float deltaTime)
    {
        frameCounter += deltaTime;
        
        if (frameCounter >= frameTime)
        {
            frameCounter = 0;
            currentFrame = (currentFrame + 1) % totalFrames;
        }
    }
    
    public void Draw(Vector2 position)
    {
        Rectangle source = new Rectangle(
            currentFrame * frameWidth,
            0,
            frameWidth,
            frameHeight
        );
        
        Rectangle dest = new Rectangle(
            position.X,
            position.Y,
            frameWidth,
            frameHeight
        );
        
        Raylib.DrawTexturePro(
            spriteSheet,
            source,
            dest,
            Vector2.Zero,
            0f,
            Color.WHITE
        );
    }
    
    public void Unload()
    {
        Raylib.UnloadTexture(spriteSheet);
    }
}

// Usage:
SpriteAnimation playerAnim = new SpriteAnimation("player_walk.png", 64, 64, 8, 10f);

// In game loop:
playerAnim.Update(Raylib.GetFrameTime());
playerAnim.Draw(new Vector2(100, 100));
```

### Camera 2D

```csharp
using Raylib_cs;
using System.Numerics;

class Camera2DExample
{
    static void Main()
    {
        Raylib.InitWindow(800, 600, "Camera 2D");
        Raylib.SetTargetFPS(60);
        
        Vector2 playerPos = new Vector2(400, 300);
        
        Camera2D camera = new Camera2D();
        camera.target = playerPos;
        camera.offset = new Vector2(400, 300);  // Center of screen
        camera.rotation = 0f;
        camera.zoom = 1f;
        
        while (!Raylib.WindowShouldClose())
        {
            // Update player
            if (Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT)) playerPos.X += 2;
            if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT)) playerPos.X -= 2;
            if (Raylib.IsKeyDown(KeyboardKey.KEY_DOWN)) playerPos.Y += 2;
            if (Raylib.IsKeyDown(KeyboardKey.KEY_UP)) playerPos.Y -= 2;
            
            // Update camera
            camera.target = playerPos;
            
            // Camera zoom
            float wheel = Raylib.GetMouseWheelMove();
            if (wheel != 0)
            {
                camera.zoom += wheel * 0.1f;
                camera.zoom = Math.Clamp(camera.zoom, 0.1f, 3.0f);
            }
            
            // Draw
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.RAYWHITE);
            
            Raylib.BeginMode2D(camera);
            
            // Draw world
            Raylib.DrawRectangle(-6000, 320, 13000, 8000, Color.DARKGRAY);
            
            for (int i = 0; i < 100; i++)
            {
                Raylib.DrawRectangle(i * 100, 320, 100, 20, Color.GRAY);
            }
            
            // Draw player
            Raylib.DrawRectangleV(playerPos, new Vector2(40, 40), Color.RED);
            
            Raylib.EndMode2D();
            
            // Draw UI (not affected by camera)
            Raylib.DrawText("Use arrow keys to move", 10, 10, 20, Color.BLACK);
            Raylib.DrawText($"Zoom: {camera.zoom:F2}", 10, 40, 20, Color.BLACK);
            
            Raylib.EndDrawing();
        }
        
        Raylib.CloseWindow();
    }
}
```

---

## 3D Graphics

### Basic 3D Scene

```csharp
using Raylib_cs;
using System.Numerics;

class Basic3D
{
    static void Main()
    {
        Raylib.InitWindow(800, 600, "3D Example");
        Raylib.SetTargetFPS(60);
        
        // Define camera
        Camera3D camera = new Camera3D();
        camera.position = new Vector3(0f, 10f, 10f);
        camera.target = new Vector3(0f, 0f, 0f);
        camera.up = new Vector3(0f, 1f, 0f);
        camera.fovy = 45f;
        camera.projection = CameraProjection.CAMERA_PERSPECTIVE;
        
        Vector3 cubePosition = new Vector3(0f, 0f, 0f);
        
        while (!Raylib.WindowShouldClose())
        {
            // Update camera
            Raylib.UpdateCamera(ref camera, CameraMode.CAMERA_ORBITAL);
            
            // Draw
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.RAYWHITE);
            
            Raylib.BeginMode3D(camera);
            
            // Draw 3D objects
            Raylib.DrawCube(cubePosition, 2f, 2f, 2f, Color.RED);
            Raylib.DrawCubeWires(cubePosition, 2f, 2f, 2f, Color.MAROON);
            
            Raylib.DrawGrid(10, 1f);
            
            Raylib.EndMode3D();
            
            Raylib.DrawText("Use mouse to rotate camera", 10, 10, 20, Color.BLACK);
            Raylib.DrawFPS(10, 40);
            
            Raylib.EndDrawing();
        }
        
        Raylib.CloseWindow();
    }
}
```

### 3D Models

```csharp
using Raylib_cs;
using System.Numerics;

class ModelExample
{
    static void Main()
    {
        Raylib.InitWindow(800, 600, "3D Model");
        Raylib.SetTargetFPS(60);
        
        Camera3D camera = new Camera3D();
        camera.position = new Vector3(5f, 5f, 5f);
        camera.target = new Vector3(0f, 0f, 0f);
        camera.up = new Vector3(0f, 1f, 0f);
        camera.fovy = 45f;
        camera.projection = CameraProjection.CAMERA_PERSPECTIVE;
        
        // Load model
        Model model = Raylib.LoadModel("model.obj");
        Texture2D texture = Raylib.LoadTexture("texture.png");
        
        // Set model texture
        unsafe
        {
            model.materials[0].maps[(int)MaterialMapIndex.MATERIAL_MAP_DIFFUSE].texture = texture;
        }
        
        Vector3 position = Vector3.Zero;
        float rotation = 0f;
        
        while (!Raylib.WindowShouldClose())
        {
            // Update
            rotation += 1f * Raylib.GetFrameTime() * 20f;
            Raylib.UpdateCamera(ref camera, CameraMode.CAMERA_ORBITAL);
            
            // Draw
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.RAYWHITE);
            
            Raylib.BeginMode3D(camera);
            
            Raylib.DrawModel(model, position, 1f, Color.WHITE);
            Raylib.DrawModelEx(model, position, new Vector3(0, 1, 0), rotation, new Vector3(1, 1, 1), Color.WHITE);
            
            Raylib.DrawGrid(10, 1f);
            
            Raylib.EndMode3D();
            
            Raylib.EndDrawing();
        }
        
        // Cleanup
        Raylib.UnloadTexture(texture);
        Raylib.UnloadModel(model);
        Raylib.CloseWindow();
    }
}
```

### First-Person Camera

```csharp
using Raylib_cs;
using System.Numerics;

Camera3D camera = new Camera3D();
camera.position = new Vector3(0f, 2f, 4f);
camera.target = new Vector3(0f, 2f, 0f);
camera.up = new Vector3(0f, 1f, 0f);
camera.fovy = 60f;
camera.projection = CameraProjection.CAMERA_PERSPECTIVE;

Raylib.SetCameraMode(camera, CameraMode.CAMERA_FIRST_PERSON);

// In game loop:
Raylib.UpdateCamera(ref camera, CameraMode.CAMERA_FIRST_PERSON);

// Controls:
// Mouse - Look around
// W/S - Move forward/backward
// A/D - Strafe left/right
// Space - Move up
// Left Control - Move down
```

---

## Input Handling

### Keyboard

```csharp
using Raylib_cs;

// Check if key is currently pressed
if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE))
{
    // Key is held down
}

// Check if key was just pressed
if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
{
    // Key was just pressed this frame
}

// Check if key was just released
if (Raylib.IsKeyReleased(KeyboardKey.KEY_ESCAPE))
{
    // Key was just released
}

// Check if key is up (not pressed)
if (Raylib.IsKeyUp(KeyboardKey.KEY_W))
{
    // Key is not pressed
}

// Get last key pressed
KeyboardKey key = (KeyboardKey)Raylib.GetKeyPressed();
```

### Mouse

```csharp
using Raylib_cs;
using System.Numerics;

// Get mouse position
Vector2 mousePos = Raylib.GetMousePosition();
int mouseX = Raylib.GetMouseX();
int mouseY = Raylib.GetMouseY();

// Mouse buttons
if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
{
    // Left click
}

if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_RIGHT))
{
    // Right button held
}

// Mouse wheel
float wheelMove = Raylib.GetMouseWheelMove();

// Mouse delta (movement since last frame)
Vector2 mouseDelta = Raylib.GetMouseDelta();

// Show/hide cursor
Raylib.ShowCursor();
Raylib.HideCursor();

// Enable/disable cursor
Raylib.EnableCursor();
Raylib.DisableCursor();
```

### Gamepad

```csharp
using Raylib_cs;

// Check if gamepad is available
if (Raylib.IsGamepadAvailable(0))
{
    string name = Raylib.GetGamepadName(0);
    
    // Button input
    if (Raylib.IsGamepadButtonPressed(0, GamepadButton.GAMEPAD_BUTTON_RIGHT_FACE_DOWN))
    {
        // A button pressed
    }
    
    // Analog sticks
    float leftX = Raylib.GetGamepadAxisMovement(0, GamepadAxis.GAMEPAD_AXIS_LEFT_X);
    float leftY = Raylib.GetGamepadAxisMovement(0, GamepadAxis.GAMEPAD_AXIS_LEFT_Y);
    
    // Triggers
    float leftTrigger = Raylib.GetGamepadAxisMovement(0, GamepadAxis.GAMEPAD_AXIS_LEFT_TRIGGER);
}
```

### Touch Input (Mobile)

```csharp
using Raylib_cs;
using System.Numerics;

// Get touch position
Vector2 touchPos = Raylib.GetTouchPosition(0);

// Get touch count
int touchCount = Raylib.GetTouchPointCount();

// Check multiple touches
for (int i = 0; i < touchCount; i++)
{
    Vector2 pos = Raylib.GetTouchPosition(i);
    Raylib.DrawCircleV(pos, 40, Color.RED);
}
```

---

## Audio

### Sound Effects

```csharp
using Raylib_cs;

// Initialize audio device
Raylib.InitAudioDevice();

// Load sound
Sound sound = Raylib.LoadSound("jump.wav");

// Play sound
Raylib.PlaySound(sound);

// Sound control
Raylib.StopSound(sound);
Raylib.PauseSound(sound);
Raylib.ResumeSound(sound);

// Check if playing
if (Raylib.IsSoundPlaying(sound))
{
    // Sound is playing
}

// Set volume (0.0 to 1.0)
Raylib.SetSoundVolume(sound, 0.5f);

// Set pitch
Raylib.SetSoundPitch(sound, 1.2f);

// Unload sound
Raylib.UnloadSound(sound);

// Close audio device
Raylib.CloseAudioDevice();
```

### Music Streaming

```csharp
using Raylib_cs;

Raylib.InitAudioDevice();

// Load music
Music music = Raylib.LoadMusicStream("music.ogg");

// Play music
Raylib.PlayMusicStream(music);

// In game loop:
while (!Raylib.WindowShouldClose())
{
    // Update music buffer
    Raylib.UpdateMusicStream(music);
    
    // ... game code ...
}

// Music control
Raylib.StopMusicStream(music);
Raylib.PauseMusicStream(music);
Raylib.ResumeMusicStream(music);

// Check if playing
if (Raylib.IsMusicStreamPlaying(music))
{
    // Music is playing
}

// Set volume
Raylib.SetMusicVolume(music, 0.8f);

// Get time played
float timePlayed = Raylib.GetMusicTimePlayed(music);
float timeLength = Raylib.GetMusicTimeLength(music);

// Unload music
Raylib.UnloadMusicStream(music);

Raylib.CloseAudioDevice();
```

---

## Complete Examples

### Pong Game

```csharp
using Raylib_cs;
using System.Numerics;

class PongGame
{
    const int SCREEN_WIDTH = 800;
    const int SCREEN_HEIGHT = 600;
    const int PADDLE_WIDTH = 20;
    const int PADDLE_HEIGHT = 100;
    const int BALL_SIZE = 15;
    
    static void Main()
    {
        Raylib.InitWindow(SCREEN_WIDTH, SCREEN_HEIGHT, "Pong");
        Raylib.SetTargetFPS(60);
        Raylib.InitAudioDevice();
        
        // Game objects
        Rectangle leftPaddle = new Rectangle(50, SCREEN_HEIGHT / 2 - PADDLE_HEIGHT / 2, PADDLE_WIDTH, PADDLE_HEIGHT);
        Rectangle rightPaddle = new Rectangle(SCREEN_WIDTH - 50 - PADDLE_WIDTH, SCREEN_HEIGHT / 2 - PADDLE_HEIGHT / 2, PADDLE_WIDTH, PADDLE_HEIGHT);
        Vector2 ballPos = new Vector2(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2);
        Vector2 ballVel = new Vector2(5, 5);
        
        int leftScore = 0;
        int rightScore = 0;
        
        Sound hitSound = Raylib.LoadSound("hit.wav");
        Sound scoreSound = Raylib.LoadSound("score.wav");
        
        while (!Raylib.WindowShouldClose())
        {
            // Update
            float deltaTime = Raylib.GetFrameTime();
            
            // Left paddle (W/S keys)
            if (Raylib.IsKeyDown(KeyboardKey.KEY_W) && leftPaddle.y > 0)
                leftPaddle.y -= 5;
            if (Raylib.IsKeyDown(KeyboardKey.KEY_S) && leftPaddle.y < SCREEN_HEIGHT - PADDLE_HEIGHT)
                leftPaddle.y += 5;
            
            // Right paddle (Arrow keys)
            if (Raylib.IsKeyDown(KeyboardKey.KEY_UP) && rightPaddle.y > 0)
                rightPaddle.y -= 5;
            if (Raylib.IsKeyDown(KeyboardKey.KEY_DOWN) && rightPaddle.y < SCREEN_HEIGHT - PADDLE_HEIGHT)
                rightPaddle.y += 5;
            
            // Ball movement
            ballPos += ballVel;
            
            // Ball collision with top/bottom
            if (ballPos.Y <= 0 || ballPos.Y >= SCREEN_HEIGHT)
            {
                ballVel.Y *= -1;
                Raylib.PlaySound(hitSound);
            }
            
            // Ball collision with paddles
            Rectangle ballRect = new Rectangle(ballPos.X, ballPos.Y, BALL_SIZE, BALL_SIZE);
            
            if (Raylib.CheckCollisionRecs(ballRect, leftPaddle) || Raylib.CheckCollisionRecs(ballRect, rightPaddle))
            {
                ballVel.X *= -1;
                Raylib.PlaySound(hitSound);
            }
            
            // Scoring
            if (ballPos.X < 0)
            {
                rightScore++;
                ballPos = new Vector2(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2);
                ballVel = new Vector2(5, 5);
                Raylib.PlaySound(scoreSound);
            }
            if (ballPos.X > SCREEN_WIDTH)
            {
                leftScore++;
                ballPos = new Vector2(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2);
                ballVel = new Vector2(-5, 5);
                Raylib.PlaySound(scoreSound);
            }
            
            // Draw
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.BLACK);
            
            // Draw center line
            for (int i = 0; i < SCREEN_HEIGHT; i += 20)
            {
                Raylib.DrawRectangle(SCREEN_WIDTH / 2 - 2, i, 4, 10, Color.WHITE);
            }
            
            // Draw paddles
            Raylib.DrawRectangleRec(leftPaddle, Color.WHITE);
            Raylib.DrawRectangleRec(rightPaddle, Color.WHITE);
            
            // Draw ball
            Raylib.DrawRectangleV(ballPos, new Vector2(BALL_SIZE, BALL_SIZE), Color.WHITE);
            
            // Draw scores
            Raylib.DrawText(leftScore.ToString(), SCREEN_WIDTH / 4, 20, 40, Color.WHITE);
            Raylib.DrawText(rightScore.ToString(), 3 * SCREEN_WIDTH / 4, 20, 40, Color.WHITE);
            
            Raylib.EndDrawing();
        }
        
        Raylib.UnloadSound(hitSound);
        Raylib.UnloadSound(scoreSound);
        Raylib.CloseAudioDevice();
        Raylib.CloseWindow();
    }
}
```

---

## Collision Detection

```csharp
using Raylib_cs;
using System.Numerics;

// Rectangle collision
bool collision = Raylib.CheckCollisionRecs(rect1, rect2);

// Circle collision
bool circleCollision = Raylib.CheckCollisionCircles(center1, radius1, center2, radius2);

// Circle and rectangle collision
bool circleRectCollision = Raylib.CheckCollisionCircleRec(center, radius, rect);

// Point and rectangle
bool pointInRect = Raylib.CheckCollisionPointRec(point, rect);

// Point and circle
bool pointInCircle = Raylib.CheckCollisionPointCircle(point, center, radius);

// Line collision
bool lineCollision = Raylib.CheckCollisionLines(startPos1, endPos1, startPos2, endPos2, out Vector2 collisionPoint);
```

---

## Best Practices

1. **Always call InitWindow() and CloseWindow()**
2. **Use BeginDrawing() and EndDrawing() for all rendering**
3. **Unload resources** when done (UnloadTexture, UnloadSound, etc.)
4. **Use delta time** for frame-independent movement
5. **SetTargetFPS(60)** for consistent frame rate
6. **InitAudioDevice()** before using audio
7. **Check WindowShouldClose()** in main loop

---

## Resources

- **Official Site**: [raylib.com](https://www.raylib.com/)
- **Raylib-cs GitHub**: [github.com/ChrisDill/Raylib-cs](https://github.com/ChrisDill/Raylib-cs)
- **Cheatsheet**: [raylib.com/cheatsheet](https://www.raylib.com/cheatsheet/cheatsheet.html)
- **Examples**: [raylib.com/examples.html](https://www.raylib.com/examples.html)

---

## Next Steps

1. Create simple games (Snake, Tetris, Space Invaders)
2. Experiment with particle effects
3. Try procedural generation
4. Implement game physics
5. Create a complete game with menus and levels
6. Explore shaders and post-processing

**Happy Raylib development!**
