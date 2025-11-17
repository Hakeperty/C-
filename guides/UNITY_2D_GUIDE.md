# Unity 2D Game Development Guide

Complete guide for creating 2D games in Unity with C#.

## Table of Contents

- [Getting Started with Unity 2D](#getting-started-with-unity-2d)
- [Core Concepts](#core-concepts)
- [Player Movement](#player-movement)
- [Animation](#animation)
- [Physics and Collisions](#physics-and-collisions)
- [Camera Control](#camera-control)
- [User Interface](#user-interface)
- [Audio](#audio)
- [Particle Systems](#particle-systems)
- [Complete Game Example](#complete-game-example)

---

## Getting Started with Unity 2D

### Setting Up a 2D Project

1. Open Unity Hub
2. Click "New Project"
3. Select "2D" template
4. Name your project (e.g., "My2DGame")
5. Choose a location and click "Create"

### Project Structure

```
Assets/
├── Scenes/          # Your game scenes
├── Scripts/         # C# scripts
├── Sprites/         # 2D images and textures
├── Animations/      # Animation files
├── Prefabs/         # Reusable game objects
├── Audio/           # Sound effects and music
└── Materials/       # 2D materials and shaders
```

### Essential Unity 2D Components

- **SpriteRenderer**: Displays 2D sprites
- **Rigidbody2D**: Adds physics to objects
- **Collider2D**: Enables collision detection
  - BoxCollider2D
  - CircleCollider2D
  - PolygonCollider2D
- **Animator**: Controls animations
- **AudioSource**: Plays sounds

---

## Core Concepts

### Sprites and Sprite Sheets

#### Importing Sprites

1. Drag image files into Assets/Sprites folder
2. Select the sprite in Project window
3. In Inspector, set:
   - Texture Type: Sprite (2D and UI)
   - Sprite Mode: Single or Multiple
   - Pixels Per Unit: 100 (default)
   - Filter Mode: Point for pixel art, Bilinear for smooth graphics
4. Click "Apply"

#### Using Sprite Sheets

```csharp
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    public Sprite[] sprites;
    public float frameRate = 10f;
    
    private SpriteRenderer spriteRenderer;
    private int currentFrame = 0;
    private float timer = 0f;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= 1f / frameRate)
        {
            timer = 0f;
            currentFrame = (currentFrame + 1) % sprites.Length;
            spriteRenderer.sprite = sprites[currentFrame];
        }
    }
}
```

### Layers and Sorting

```csharp
using UnityEngine;

public class LayerManager : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        
        // Set sorting layer
        sr.sortingLayerName = "Foreground";
        
        // Set order in layer (higher values appear in front)
        sr.sortingOrder = 10;
    }
}
```

---

## Player Movement

### Basic 2D Movement

```csharp
using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float acceleration = 50f;
    public float deceleration = 50f;
    
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 currentVelocity;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        // Get input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();
    }
    
    void FixedUpdate()
    {
        if (movement != Vector2.zero)
        {
            // Accelerate
            currentVelocity = Vector2.MoveTowards(
                currentVelocity,
                movement * moveSpeed,
                acceleration * Time.fixedDeltaTime
            );
        }
        else
        {
            // Decelerate
            currentVelocity = Vector2.MoveTowards(
                currentVelocity,
                Vector2.zero,
                deceleration * Time.fixedDeltaTime
            );
        }
        
        rb.velocity = currentVelocity;
    }
}
```

### Platformer Movement with Jump

```csharp
using UnityEngine;

public class PlatformerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float acceleration = 20f;
    
    [Header("Jump")]
    public float jumpForce = 12f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    
    [Header("Ground Check")]
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(0.9f, 0.1f);
    public LayerMask groundLayer;
    
    [Header("Wall Jump")]
    public bool enableWallJump = false;
    public float wallSlideSpeed = 2f;
    public float wallJumpForce = 10f;
    public Transform wallCheck;
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;
    private float horizontalInput;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        
        // Ground check
        isGrounded = Physics2D.OverlapBox(
            groundCheck.position,
            groundCheckSize,
            0f,
            groundLayer
        );
        
        // Wall check (if wall jump enabled)
        if (enableWallJump)
        {
            isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, 0.2f, groundLayer);
            isWallSliding = isTouchingWall && !isGrounded && rb.velocity.y < 0;
        }
        
        // Jump
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            else if (isWallSliding && enableWallJump)
            {
                // Wall jump
                rb.velocity = new Vector2(-Mathf.Sign(horizontalInput) * wallJumpForce, jumpForce);
            }
        }
        
        // Better jump physics
        if (rb.velocity.y < 0)
        {
            // Falling faster
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            // Quick jump release
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        
        // Flip sprite
        if (horizontalInput > 0)
            spriteRenderer.flipX = false;
        else if (horizontalInput < 0)
            spriteRenderer.flipX = true;
    }
    
    void FixedUpdate()
    {
        // Horizontal movement
        float targetVelocity = horizontalInput * moveSpeed;
        rb.velocity = new Vector2(
            Mathf.MoveTowards(rb.velocity.x, targetVelocity, acceleration * Time.fixedDeltaTime),
            rb.velocity.y
        );
        
        // Wall sliding
        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
    }
    
    void OnDrawGizmosSelected()
    {
        // Visualize ground check
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }
        
        // Visualize wall check
        if (wallCheck != null && enableWallJump)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(wallCheck.position, 0.2f);
        }
    }
}
```

### Top-Down Movement

```csharp
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool use8DirectionalMovement = true;
    
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        
        if (use8DirectionalMovement)
        {
            moveInput.Normalize();
        }
        
        // Update animator
        if (animator != null)
        {
            animator.SetFloat("Horizontal", moveInput.x);
            animator.SetFloat("Vertical", moveInput.y);
            animator.SetFloat("Speed", moveInput.magnitude);
        }
    }
    
    void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed;
    }
}
```

---

## Animation

### Setting Up Animations

1. **Create Animation Controller**
   - Right-click in Project → Create → Animator Controller
   - Name it (e.g., "PlayerAnimator")

2. **Create Animation Clips**
   - Select GameObject in Hierarchy
   - Open Animation window (Window → Animation → Animation)
   - Click "Create" and name animation (e.g., "Idle")
   - Add property (e.g., Sprite)
   - Set keyframes with different sprites

3. **Configure Animator**
   - Open Animator window (Window → Animation → Animator)
   - Create states and transitions
   - Add parameters (Float, Int, Bool, Trigger)

### Animation Controller Script

```csharp
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    
    // Animation parameter names
    private readonly string ANIM_SPEED = "Speed";
    private readonly string ANIM_GROUNDED = "IsGrounded";
    private readonly string ANIM_VELOCITY_Y = "VelocityY";
    private readonly string ANIM_JUMP = "Jump";
    private readonly string ANIM_ATTACK = "Attack";
    
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        // Update animation parameters
        animator.SetFloat(ANIM_SPEED, Mathf.Abs(rb.velocity.x));
        animator.SetBool(ANIM_GROUNDED, IsGrounded());
        animator.SetFloat(ANIM_VELOCITY_Y, rb.velocity.y);
        
        // Trigger jump animation
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            animator.SetTrigger(ANIM_JUMP);
        }
        
        // Trigger attack animation
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger(ANIM_ATTACK);
        }
    }
    
    private bool IsGrounded()
    {
        // Implement ground check
        return Physics2D.Raycast(transform.position, Vector2.down, 1.1f);
    }
}
```

### Sprite Flip and Direction

```csharp
using UnityEngine;

public class SpriteDirection : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Vector2 lastPosition;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPosition = transform.position;
    }
    
    void Update()
    {
        Vector2 currentPosition = transform.position;
        Vector2 direction = currentPosition - lastPosition;
        
        if (direction.x > 0.01f)
        {
            spriteRenderer.flipX = false;
        }
        else if (direction.x < -0.01f)
        {
            spriteRenderer.flipX = true;
        }
        
        lastPosition = currentPosition;
    }
}
```

---

## Physics and Collisions

### Collision Detection

```csharp
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [Header("Tags")]
    public string enemyTag = "Enemy";
    public string collectibleTag = "Collectible";
    public string hazardTag = "Hazard";
    
    [Header("Settings")]
    public int health = 100;
    public int damage = 10;
    
    // Triggered when entering a trigger collider
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(collectibleTag))
        {
            CollectItem(other.gameObject);
        }
        else if (other.CompareTag(hazardTag))
        {
            TakeDamage(damage);
        }
    }
    
    // Triggered when staying in a trigger collider
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            // Apply swimming physics
            GetComponent<Rigidbody2D>().gravityScale = 0.5f;
        }
    }
    
    // Triggered when exiting a trigger collider
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            // Restore normal gravity
            GetComponent<Rigidbody2D>().gravityScale = 1f;
        }
    }
    
    // Called on collision with non-trigger collider
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(enemyTag))
        {
            // Calculate bounce direction
            Vector2 bounceDirection = (transform.position - collision.transform.position).normalized;
            GetComponent<Rigidbody2D>().AddForce(bounceDirection * 500f);
            
            TakeDamage(damage);
        }
    }
    
    void CollectItem(GameObject item)
    {
        Debug.Log($"Collected {item.name}");
        Destroy(item);
    }
    
    void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        Debug.Log($"Health: {health}");
        
        if (health <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        Debug.Log("Player died!");
        // Implement death logic
    }
}
```

### Raycasting

```csharp
using UnityEngine;

public class RaycastDetection : MonoBehaviour
{
    public float rayDistance = 5f;
    public LayerMask obstacleLayer;
    
    void Update()
    {
        // Cast ray forward
        Vector2 rayOrigin = transform.position;
        Vector2 rayDirection = transform.right;
        
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, obstacleLayer);
        
        if (hit.collider != null)
        {
            Debug.Log($"Hit {hit.collider.name} at distance {hit.distance}");
            
            // Visualize ray
            Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.red);
        }
        else
        {
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.green);
        }
    }
    
    // Check multiple rays (useful for ground detection)
    public bool CheckGround()
    {
        Vector2 leftRay = transform.position + Vector3.left * 0.4f;
        Vector2 centerRay = transform.position;
        Vector2 rightRay = transform.position + Vector3.right * 0.4f;
        
        bool leftGrounded = Physics2D.Raycast(leftRay, Vector2.down, 0.6f, obstacleLayer);
        bool centerGrounded = Physics2D.Raycast(centerRay, Vector2.down, 0.6f, obstacleLayer);
        bool rightGrounded = Physics2D.Raycast(rightRay, Vector2.down, 0.6f, obstacleLayer);
        
        return leftGrounded || centerGrounded || rightGrounded;
    }
}
```

---

## Camera Control

### Following Camera

```csharp
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    
    [Header("Follow Settings")]
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 0, -10);
    
    [Header("Bounds")]
    public bool useBounds = false;
    public float minX, maxX, minY, maxY;
    
    void LateUpdate()
    {
        if (target == null) return;
        
        Vector3 desiredPosition = target.position + offset;
        
        // Apply bounds if enabled
        if (useBounds)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        }
        
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
```

### Cinemachine Camera (Advanced)

```csharp
using UnityEngine;
using Cinemachine;

public class CinemachineController : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    
    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    
    public void ShakeCamera(float intensity, float duration)
    {
        StartCoroutine(ShakeCoroutine(intensity, duration));
    }
    
    System.Collections.IEnumerator ShakeCoroutine(float intensity, float duration)
    {
        CinemachineBasicMultiChannelPerlin noise = 
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        
        noise.m_AmplitudeGain = intensity;
        yield return new WaitForSeconds(duration);
        noise.m_AmplitudeGain = 0f;
    }
}
```

---

## User Interface

### Health Bar

```csharp
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("References")]
    public Image healthFill;
    public Text healthText;
    
    [Header("Settings")]
    public int maxHealth = 100;
    private int currentHealth;
    
    [Header("Animation")]
    public bool smoothTransition = true;
    public float transitionSpeed = 5f;
    
    private float targetFillAmount;
    
    void Start()
    {
        currentHealth = maxHealth;
        targetFillAmount = 1f;
        UpdateHealthBar();
    }
    
    void Update()
    {
        if (smoothTransition)
        {
            healthFill.fillAmount = Mathf.Lerp(
                healthFill.fillAmount,
                targetFillAmount,
                Time.deltaTime * transitionSpeed
            );
        }
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        UpdateHealthBar();
    }
    
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        UpdateHealthBar();
    }
    
    void UpdateHealthBar()
    {
        targetFillAmount = (float)currentHealth / maxHealth;
        
        if (!smoothTransition)
        {
            healthFill.fillAmount = targetFillAmount;
        }
        
        if (healthText != null)
        {
            healthText.text = $"{currentHealth} / {maxHealth}";
        }
    }
}
```

### Score Manager

```csharp
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    
    public Text scoreText;
    public int score = 0;
    
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
    
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }
    
    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }
    
    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }
}
```

---

## Audio

### Audio Manager

```csharp
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    
    [Header("Music")]
    public AudioClip mainMenuMusic;
    public AudioClip gameplayMusic;
    
    [Header("Sound Effects")]
    public AudioClip jumpSound;
    public AudioClip collectSound;
    public AudioClip damageSound;
    
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
    
    public void PlayMusic(AudioClip music)
    {
        if (musicSource.clip != music)
        {
            musicSource.clip = music;
            musicSource.Play();
        }
    }
    
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        sfxSource.PlayOneShot(clip, volume);
    }
    
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}

// Usage:
// AudioManager.Instance.PlaySFX(AudioManager.Instance.jumpSound);
```

---

## Particle Systems

### Particle Controller

```csharp
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public ParticleSystem dustParticles;
    public ParticleSystem explosionParticles;
    
    public void PlayDustEffect(Vector3 position)
    {
        if (dustParticles != null)
        {
            dustParticles.transform.position = position;
            dustParticles.Play();
        }
    }
    
    public void PlayExplosion(Vector3 position)
    {
        if (explosionParticles != null)
        {
            ParticleSystem explosion = Instantiate(explosionParticles, position, Quaternion.identity);
            Destroy(explosion.gameObject, explosion.main.duration);
        }
    }
}
```

---

## Complete Game Example

### Simple Platformer Game Structure

```csharp
// GameManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public int lives = 3;
    public int score = 0;
    
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
    
    public void LoseLife()
    {
        lives--;
        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            RestartLevel();
        }
    }
    
    public void AddScore(int points)
    {
        score += points;
    }
    
    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    void GameOver()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene("GameOver");
    }
    
    public void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("You Win!");
            SceneManager.LoadScene("Victory");
        }
    }
}
```

---

## Best Practices

1. **Use Object Pooling** for frequently spawned objects (bullets, particles)
2. **Optimize Physics** by using appropriate layer collision matrices
3. **Use Tags and Layers** wisely for organization
4. **Profile Your Game** regularly using Unity Profiler
5. **Keep Update Methods Light** - move heavy calculations to coroutines
6. **Use Events** for loose coupling between systems
7. **Cache Component References** in Start/Awake, not in Update
8. **Use Sprite Atlases** to reduce draw calls

---

## Common Pitfalls

1. Not using FixedUpdate for physics
2. Forgetting to normalize diagonal movement
3. Not caching GetComponent calls
4. Using Find methods in Update
5. Not properly handling player death/respawn
6. Ignoring layer collision matrix settings

---

## Next Steps

1. Complete a simple platformer project
2. Add more mechanics (double jump, dash, wall slide)
3. Implement enemy AI
4. Add power-ups and collectibles
5. Create multiple levels
6. Add a menu system
7. Polish with particles, sounds, and animations

**Happy game developing!**
