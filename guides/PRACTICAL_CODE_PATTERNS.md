# Practical C# Examples for Game Development

Real-world code patterns you'll use in every game! Copy, paste, and customize these for your projects.

## Table of Contents

- [Design Patterns](#design-patterns)
- [Inventory Systems](#inventory-systems)
- [Health and Damage Systems](#health-and-damage-systems)
- [State Machines](#state-machines)
- [Timer and Cooldown Systems](#timer-and-cooldown-systems)
- [Dialogue Systems](#dialogue-systems)
- [Quest Systems](#quest-systems)
- [Save/Load Systems](#saveload-systems)
- [Audio Managers](#audio-managers)
- [Particle and VFX Managers](#particle-and-vfx-managers)

---

## Design Patterns

### Singleton Pattern

Perfect for managers that should only exist once (GameManager, AudioManager, etc.)

```csharp
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    instance = go.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    public int Score { get; set; }
    public int Lives { get; set; } = 3;
    
    public void AddScore(int points)
    {
        Score += points;
        OnScoreChanged?.Invoke(Score);
    }
    
    public event Action<int> OnScoreChanged;
}

// Usage from anywhere:
// GameManager.Instance.AddScore(100);
```

### Observer Pattern (Events)

Let objects communicate without tight coupling!

```csharp
// Event definitions
public class GameEvents
{
    // Simple events
    public static event Action OnGameStart;
    public static event Action OnGameEnd;
    public static event Action OnPlayerDeath;
    
    // Events with parameters
    public static event Action<int> OnScoreChanged;
    public static event Action<float> OnHealthChanged;
    public static event Action<string> OnItemCollected;
    
    // Trigger events
    public static void TriggerGameStart() => OnGameStart?.Invoke();
    public static void TriggerScoreChanged(int newScore) => OnScoreChanged?.Invoke(newScore);
}

// Subscriber example
public class UIManager : MonoBehaviour
{
    void OnEnable()
    {
        GameEvents.OnScoreChanged += UpdateScoreDisplay;
        GameEvents.OnHealthChanged += UpdateHealthBar;
    }
    
    void OnDisable()
    {
        GameEvents.OnScoreChanged -= UpdateScoreDisplay;
        GameEvents.OnHealthChanged -= UpdateHealthBar;
    }
    
    void UpdateScoreDisplay(int score)
    {
        scoreText.text = $"Score: {score}";
    }
    
    void UpdateHealthBar(float health)
    {
        healthSlider.value = health / 100f;
    }
}

// Publisher example
public class Player : MonoBehaviour
{
    private float health = 100f;
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        GameEvents.TriggerHealthChanged(health);
        
        if (health <= 0)
        {
            GameEvents.TriggerPlayerDeath();
        }
    }
}
```

### Object Pool Pattern

Reuse objects instead of constantly creating and destroying them!

```csharp
public class ObjectPool<T> where T : Component
{
    private T prefab;
    private Queue<T> objects = new Queue<T>();
    private Transform parent;
    
    public ObjectPool(T prefab, int initialSize = 10, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;
        
        // Pre-populate pool
        for (int i = 0; i < initialSize; i++)
        {
            T obj = GameObject.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            objects.Enqueue(obj);
        }
    }
    
    public T Get()
    {
        if (objects.Count > 0)
        {
            T obj = objects.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            return GameObject.Instantiate(prefab, parent);
        }
    }
    
    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        objects.Enqueue(obj);
    }
    
    public void Clear()
    {
        foreach (T obj in objects)
        {
            if (obj != null)
                GameObject.Destroy(obj.gameObject);
        }
        objects.Clear();
    }
}

// Usage example
public class BulletManager : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    private ObjectPool<Bullet> bulletPool;
    
    void Start()
    {
        bulletPool = new ObjectPool<Bullet>(bulletPrefab, 20, transform);
    }
    
    public void SpawnBullet(Vector3 position, Vector3 direction)
    {
        Bullet bullet = bulletPool.Get();
        bullet.transform.position = position;
        bullet.Initialize(direction, () => bulletPool.Return(bullet));
    }
}

public class Bullet : MonoBehaviour
{
    private Vector3 direction;
    private Action onDestroy;
    
    public void Initialize(Vector3 dir, Action onDestroyCallback)
    {
        direction = dir;
        onDestroy = onDestroyCallback;
    }
    
    void Update()
    {
        transform.position += direction * 10f * Time.deltaTime;
    }
    
    void OnCollisionEnter(Collision collision)
    {
        onDestroy?.Invoke();  // Return to pool
    }
}
```

### Command Pattern

Great for undo/redo systems and input handling!

```csharp
public interface ICommand
{
    void Execute();
    void Undo();
}

public class MoveCommand : ICommand
{
    private Transform transform;
    private Vector3 movement;
    
    public MoveCommand(Transform transform, Vector3 movement)
    {
        this.transform = transform;
        this.movement = movement;
    }
    
    public void Execute()
    {
        transform.position += movement;
    }
    
    public void Undo()
    {
        transform.position -= movement;
    }
}

public class CommandManager
{
    private Stack<ICommand> undoStack = new Stack<ICommand>();
    private Stack<ICommand> redoStack = new Stack<ICommand>();
    
    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        undoStack.Push(command);
        redoStack.Clear();
    }
    
    public void Undo()
    {
        if (undoStack.Count > 0)
        {
            ICommand command = undoStack.Pop();
            command.Undo();
            redoStack.Push(command);
        }
    }
    
    public void Redo()
    {
        if (redoStack.Count > 0)
        {
            ICommand command = redoStack.Pop();
            command.Execute();
            undoStack.Push(command);
        }
    }
}
```

---

## Inventory Systems

### Basic Inventory

```csharp
[Serializable]
public class Item
{
    public string Name;
    public string Description;
    public Sprite Icon;
    public int MaxStack = 1;
    public float Weight = 0f;
}

[Serializable]
public class InventorySlot
{
    public Item Item;
    public int Quantity;
    
    public bool IsEmpty => Item == null || Quantity == 0;
    
    public bool CanAddItem(Item item)
    {
        if (IsEmpty)
            return true;
        if (Item == item && Quantity < item.MaxStack)
            return true;
        return false;
    }
    
    public void AddItem(Item item, int quantity = 1)
    {
        if (IsEmpty)
        {
            Item = item;
            Quantity = quantity;
        }
        else if (Item == item)
        {
            Quantity += quantity;
        }
    }
    
    public void RemoveItem(int quantity = 1)
    {
        Quantity -= quantity;
        if (Quantity <= 0)
        {
            Item = null;
            Quantity = 0;
        }
    }
}

public class Inventory
{
    private List<InventorySlot> slots;
    public int Size => slots.Count;
    
    public event Action OnInventoryChanged;
    
    public Inventory(int size)
    {
        slots = new List<InventorySlot>();
        for (int i = 0; i < size; i++)
        {
            slots.Add(new InventorySlot());
        }
    }
    
    public bool AddItem(Item item, int quantity = 1)
    {
        // Try to stack with existing items
        foreach (var slot in slots)
        {
            if (slot.CanAddItem(item))
            {
                int spaceLeft = item.MaxStack - slot.Quantity;
                int toAdd = Mathf.Min(quantity, spaceLeft);
                slot.AddItem(item, toAdd);
                quantity -= toAdd;
                
                if (quantity == 0)
                {
                    OnInventoryChanged?.Invoke();
                    return true;
                }
            }
        }
        
        // Find empty slot
        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                int toAdd = Mathf.Min(quantity, item.MaxStack);
                slot.AddItem(item, toAdd);
                quantity -= toAdd;
                
                if (quantity == 0)
                {
                    OnInventoryChanged?.Invoke();
                    return true;
                }
            }
        }
        
        return false; // Inventory full
    }
    
    public void RemoveItem(Item item, int quantity = 1)
    {
        foreach (var slot in slots)
        {
            if (slot.Item == item && slot.Quantity > 0)
            {
                int toRemove = Mathf.Min(quantity, slot.Quantity);
                slot.RemoveItem(toRemove);
                quantity -= toRemove;
                
                if (quantity == 0)
                {
                    OnInventoryChanged?.Invoke();
                    return;
                }
            }
        }
    }
    
    public int GetItemCount(Item item)
    {
        return slots.Where(s => s.Item == item).Sum(s => s.Quantity);
    }
    
    public bool HasItem(Item item, int quantity = 1)
    {
        return GetItemCount(item) >= quantity;
    }
    
    public InventorySlot GetSlot(int index)
    {
        if (index >= 0 && index < slots.Count)
            return slots[index];
        return null;
    }
    
    public float GetTotalWeight()
    {
        return slots.Where(s => !s.IsEmpty)
                   .Sum(s => s.Item.Weight * s.Quantity);
    }
}
```

---

## Health and Damage Systems

### Advanced Health System

```csharp
public class HealthSystem
{
    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }
    public bool IsAlive => CurrentHealth > 0;
    public float HealthPercentage => CurrentHealth / MaxHealth;
    
    public event Action<float> OnHealthChanged;
    public event Action<float> OnDamageTaken;
    public event Action<float> OnHealed;
    public event Action OnDeath;
    
    private bool isInvulnerable = false;
    
    public HealthSystem(float maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }
    
    public void TakeDamage(float damage)
    {
        if (isInvulnerable || !IsAlive)
            return;
        
        CurrentHealth -= damage;
        CurrentHealth = Mathf.Max(0, CurrentHealth);
        
        OnDamageTaken?.Invoke(damage);
        OnHealthChanged?.Invoke(CurrentHealth);
        
        if (!IsAlive)
        {
            OnDeath?.Invoke();
        }
    }
    
    public void Heal(float amount)
    {
        if (!IsAlive)
            return;
        
        float healedAmount = Mathf.Min(amount, MaxHealth - CurrentHealth);
        CurrentHealth += healedAmount;
        CurrentHealth = Mathf.Min(CurrentHealth, MaxHealth);
        
        OnHealed?.Invoke(healedAmount);
        OnHealthChanged?.Invoke(CurrentHealth);
    }
    
    public void SetInvulnerable(bool invulnerable)
    {
        isInvulnerable = invulnerable;
    }
    
    public void Revive(float healthAmount)
    {
        CurrentHealth = Mathf.Min(healthAmount, MaxHealth);
        OnHealthChanged?.Invoke(CurrentHealth);
    }
    
    public void IncreaseMaxHealth(float amount)
    {
        MaxHealth += amount;
        OnHealthChanged?.Invoke(CurrentHealth);
    }
}

// Usage in a MonoBehaviour
public class Character : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private HealthSystem healthSystem;
    
    void Start()
    {
        healthSystem = new HealthSystem(maxHealth);
        healthSystem.OnHealthChanged += OnHealthChanged;
        healthSystem.OnDeath += OnDeath;
    }
    
    void OnHealthChanged(float newHealth)
    {
        Debug.Log($"Health: {newHealth}/{healthSystem.MaxHealth}");
    }
    
    void OnDeath()
    {
        Debug.Log("Character died!");
        // Play death animation
        // Disable controls
        // Show death screen
    }
    
    public void TakeDamage(float damage)
    {
        healthSystem.TakeDamage(damage);
    }
}
```

### Damage Types and Resistance

```csharp
public enum DamageType
{
    Physical,
    Fire,
    Ice,
    Lightning,
    Poison
}

[Serializable]
public class DamageInfo
{
    public float Amount;
    public DamageType Type;
    public GameObject Source;
    
    public DamageInfo(float amount, DamageType type, GameObject source)
    {
        Amount = amount;
        Type = type;
        Source = source;
    }
}

public class DamageResistance
{
    private Dictionary<DamageType, float> resistances = new Dictionary<DamageType, float>();
    
    public void SetResistance(DamageType type, float percentage)
    {
        resistances[type] = Mathf.Clamp01(percentage);
    }
    
    public float GetResistance(DamageType type)
    {
        return resistances.TryGetValue(type, out float value) ? value : 0f;
    }
    
    public float CalculateDamage(DamageInfo damageInfo)
    {
        float resistance = GetResistance(damageInfo.Type);
        return damageInfo.Amount * (1f - resistance);
    }
}

public class AdvancedHealthSystem : HealthSystem
{
    private DamageResistance resistance;
    
    public AdvancedHealthSystem(float maxHealth) : base(maxHealth)
    {
        resistance = new DamageResistance();
    }
    
    public void SetResistance(DamageType type, float percentage)
    {
        resistance.SetResistance(type, percentage);
    }
    
    public void TakeDamage(DamageInfo damageInfo)
    {
        float actualDamage = resistance.CalculateDamage(damageInfo);
        base.TakeDamage(actualDamage);
    }
}
```

---

## State Machines

### Simple State Machine

```csharp
public enum PlayerState
{
    Idle,
    Walking,
    Running,
    Jumping,
    Attacking,
    Dead
}

public class PlayerController : MonoBehaviour
{
    private PlayerState currentState = PlayerState.Idle;
    
    void Update()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                HandleIdleState();
                break;
            case PlayerState.Walking:
                HandleWalkingState();
                break;
            case PlayerState.Running:
                HandleRunningState();
                break;
            case PlayerState.Jumping:
                HandleJumpingState();
                break;
            case PlayerState.Attacking:
                HandleAttackingState();
                break;
            case PlayerState.Dead:
                HandleDeadState();
                break;
        }
    }
    
    void ChangeState(PlayerState newState)
    {
        if (currentState == newState)
            return;
        
        // Exit current state
        OnStateExit(currentState);
        
        // Change state
        currentState = newState;
        
        // Enter new state
        OnStateEnter(currentState);
    }
    
    void OnStateEnter(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Idle:
                animator.Play("Idle");
                break;
            case PlayerState.Walking:
                animator.Play("Walk");
                break;
            case PlayerState.Attacking:
                animator.Play("Attack");
                StartCoroutine(AttackDuration());
                break;
        }
    }
    
    void OnStateExit(PlayerState state)
    {
        // Cleanup when leaving a state
    }
    
    void HandleIdleState()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                ChangeState(PlayerState.Running);
            else
                ChangeState(PlayerState.Walking);
        }
        
        if (Input.GetButtonDown("Jump"))
            ChangeState(PlayerState.Jumping);
            
        if (Input.GetButtonDown("Fire1"))
            ChangeState(PlayerState.Attacking);
    }
    
    void HandleWalkingState()
    {
        // Movement logic
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            ChangeState(PlayerState.Idle);
    }
    
    void HandleRunningState() { /* ... */ }
    void HandleJumpingState() { /* ... */ }
    void HandleAttackingState() { /* ... */ }
    void HandleDeadState() { /* ... */ }
    
    IEnumerator AttackDuration()
    {
        yield return new WaitForSeconds(0.5f);
        if (currentState == PlayerState.Attacking)
            ChangeState(PlayerState.Idle);
    }
}
```

### Advanced State Machine

```csharp
public interface IState
{
    void Enter();
    void Update();
    void Exit();
}

public class StateMachine
{
    private IState currentState;
    
    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
    
    public void Update()
    {
        currentState?.Update();
    }
}

// Example states
public class IdleState : IState
{
    private PlayerController player;
    
    public IdleState(PlayerController player)
    {
        this.player = player;
    }
    
    public void Enter()
    {
        player.Animator.Play("Idle");
    }
    
    public void Update()
    {
        if (player.IsMoving)
            player.StateMachine.ChangeState(new WalkingState(player));
    }
    
    public void Exit()
    {
        // Cleanup
    }
}

public class WalkingState : IState
{
    private PlayerController player;
    
    public WalkingState(PlayerController player)
    {
        this.player = player;
    }
    
    public void Enter()
    {
        player.Animator.Play("Walk");
    }
    
    public void Update()
    {
        player.Move();
        
        if (!player.IsMoving)
            player.StateMachine.ChangeState(new IdleState(player));
    }
    
    public void Exit() { }
}
```

---

## Timer and Cooldown Systems

### Simple Timer

```csharp
public class Timer
{
    private float duration;
    private float currentTime;
    private bool isRunning;
    
    public bool IsFinished => currentTime >= duration;
    public float RemainingTime => Mathf.Max(0, duration - currentTime);
    public float Progress => currentTime / duration;
    
    public event Action OnTimerComplete;
    
    public Timer(float duration)
    {
        this.duration = duration;
    }
    
    public void Start()
    {
        currentTime = 0;
        isRunning = true;
    }
    
    public void Stop()
    {
        isRunning = false;
    }
    
    public void Reset()
    {
        currentTime = 0;
    }
    
    public void Update(float deltaTime)
    {
        if (!isRunning)
            return;
        
        currentTime += deltaTime;
        
        if (IsFinished && isRunning)
        {
            isRunning = false;
            OnTimerComplete?.Invoke();
        }
    }
}

// Usage
public class GameController : MonoBehaviour
{
    private Timer gameTimer;
    
    void Start()
    {
        gameTimer = new Timer(60f);  // 60 second timer
        gameTimer.OnTimerComplete += OnGameTimeUp;
        gameTimer.Start();
    }
    
    void Update()
    {
        gameTimer.Update(Time.deltaTime);
        timerText.text = $"Time: {gameTimer.RemainingTime:F1}";
    }
    
    void OnGameTimeUp()
    {
        Debug.Log("Time's up!");
    }
}
```

### Cooldown System

```csharp
public class Cooldown
{
    private float duration;
    private float lastUsedTime;
    
    public bool IsReady => Time.time >= lastUsedTime + duration;
    public float RemainingTime => Mathf.Max(0, (lastUsedTime + duration) - Time.time);
    public float Progress => 1f - (RemainingTime / duration);
    
    public Cooldown(float duration)
    {
        this.duration = duration;
        lastUsedTime = -duration;  // Ready immediately
    }
    
    public bool TryUse()
    {
        if (IsReady)
        {
            Use();
            return true;
        }
        return false;
    }
    
    public void Use()
    {
        lastUsedTime = Time.time;
    }
    
    public void Reset()
    {
        lastUsedTime = -duration;
    }
}

// Usage example
public class Weapon : MonoBehaviour
{
    [SerializeField] private float fireRate = 0.5f;
    private Cooldown shootCooldown;
    
    void Start()
    {
        shootCooldown = new Cooldown(fireRate);
    }
    
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (shootCooldown.TryUse())
            {
                Shoot();
            }
        }
        
        // Update UI
        cooldownImage.fillAmount = shootCooldown.Progress;
    }
    
    void Shoot()
    {
        Debug.Log("Bang!");
        // Shoot logic here
    }
}

// Multiple cooldowns
public class Ability
{
    public string Name;
    public float CooldownDuration;
    private Cooldown cooldown;
    
    public bool IsReady => cooldown.IsReady;
    public float RemainingCooldown => cooldown.RemainingTime;
    
    public Ability(string name, float cooldownDuration)
    {
        Name = name;
        CooldownDuration = cooldownDuration;
        cooldown = new Cooldown(cooldownDuration);
    }
    
    public bool TryUse()
    {
        return cooldown.TryUse();
    }
}

public class AbilityManager
{
    private Dictionary<string, Ability> abilities = new Dictionary<string, Ability>();
    
    public void RegisterAbility(Ability ability)
    {
        abilities[ability.Name] = ability;
    }
    
    public bool UseAbility(string abilityName)
    {
        if (abilities.TryGetValue(abilityName, out Ability ability))
        {
            return ability.TryUse();
        }
        return false;
    }
    
    public float GetCooldown(string abilityName)
    {
        if (abilities.TryGetValue(abilityName, out Ability ability))
        {
            return ability.RemainingCooldown;
        }
        return 0f;
    }
}
```

---

## Dialogue Systems

### Simple Dialogue System

```csharp
[Serializable]
public class DialogueLine
{
    public string Speaker;
    public string Text;
    public float Duration = 2f;
}

[Serializable]
public class Dialogue
{
    public string ID;
    public List<DialogueLine> Lines;
}

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    
    [SerializeField] private Text speakerText;
    [SerializeField] private Text dialogueText;
    [SerializeField] private GameObject dialoguePanel;
    
    private Queue<DialogueLine> lineQueue;
    private bool isPlaying;
    
    void Awake()
    {
        Instance = this;
        lineQueue = new Queue<DialogueLine>();
    }
    
    public void StartDialogue(Dialogue dialogue)
    {
        if (isPlaying)
            return;
        
        isPlaying = true;
        dialoguePanel.SetActive(true);
        
        lineQueue.Clear();
        foreach (var line in dialogue.Lines)
        {
            lineQueue.Enqueue(line);
        }
        
        DisplayNextLine();
    }
    
    public void DisplayNextLine()
    {
        if (lineQueue.Count == 0)
        {
            EndDialogue();
            return;
        }
        
        DialogueLine line = lineQueue.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeLine(line));
    }
    
    IEnumerator TypeLine(DialogueLine line)
    {
        speakerText.text = line.Speaker;
        dialogueText.text = "";
        
        foreach (char c in line.Text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.02f);
        }
        
        yield return new WaitForSeconds(line.Duration);
        DisplayNextLine();
    }
    
    void EndDialogue()
    {
        isPlaying = false;
        dialoguePanel.SetActive(false);
    }
}

// Usage
public class NPC : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;
    
    void OnInteract()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
    }
}
```

---

## Quest Systems

```csharp
public enum QuestStatus
{
    NotStarted,
    InProgress,
    Completed,
    Failed
}

[Serializable]
public class Quest
{
    public string ID;
    public string Title;
    public string Description;
    public int RequiredAmount;
    public int CurrentAmount;
    public QuestStatus Status;
    public int RewardGold;
    public int RewardXP;
    
    public float Progress => (float)CurrentAmount / RequiredAmount;
    public bool IsComplete => CurrentAmount >= RequiredAmount;
    
    public void UpdateProgress(int amount)
    {
        if (Status != QuestStatus.InProgress)
            return;
        
        CurrentAmount += amount;
        CurrentAmount = Mathf.Min(CurrentAmount, RequiredAmount);
        
        if (IsComplete)
        {
            Status = QuestStatus.Completed;
        }
    }
}

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    
    private Dictionary<string, Quest> activeQuests = new Dictionary<string, Quest>();
    private List<Quest> completedQuests = new List<Quest>();
    
    public event Action<Quest> OnQuestStarted;
    public event Action<Quest> OnQuestCompleted;
    public event Action<Quest> OnQuestProgress;
    
    void Awake()
    {
        Instance = this;
    }
    
    public void StartQuest(Quest quest)
    {
        if (activeQuests.ContainsKey(quest.ID))
            return;
        
        quest.Status = QuestStatus.InProgress;
        activeQuests[quest.ID] = quest;
        OnQuestStarted?.Invoke(quest);
    }
    
    public void UpdateQuestProgress(string questID, int amount)
    {
        if (activeQuests.TryGetValue(questID, out Quest quest))
        {
            quest.UpdateProgress(amount);
            OnQuestProgress?.Invoke(quest);
            
            if (quest.IsComplete)
            {
                CompleteQuest(quest);
            }
        }
    }
    
    void CompleteQuest(Quest quest)
    {
        quest.Status = QuestStatus.Completed;
        activeQuests.Remove(quest.ID);
        completedQuests.Add(quest);
        
        // Give rewards
        PlayerStats.Instance.AddGold(quest.RewardGold);
        PlayerStats.Instance.AddXP(quest.RewardXP);
        
        OnQuestCompleted?.Invoke(quest);
    }
    
    public List<Quest> GetActiveQuests()
    {
        return activeQuests.Values.ToList();
    }
}
```

---

## Save/Load Systems

```csharp
using System.IO;
using System.Text.Json;

[Serializable]
public class SaveData
{
    public string PlayerName;
    public int Level;
    public float Health;
    public Vector3 Position;
    public List<string> Inventory;
    public Dictionary<string, int> Currencies;
    public DateTime LastSaved;
}

public class SaveManager
{
    private static readonly string SAVE_FOLDER = Path.Combine(Application.persistentDataPath, "Saves");
    private static readonly string SAVE_EXTENSION = ".json";
    
    public static void Save(string fileName, SaveData data)
    {
        try
        {
            // Ensure directory exists
            if (!Directory.Exists(SAVE_FOLDER))
            {
                Directory.CreateDirectory(SAVE_FOLDER);
            }
            
            string filePath = Path.Combine(SAVE_FOLDER, fileName + SAVE_EXTENSION);
            data.LastSaved = DateTime.Now;
            
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
            
            File.WriteAllText(filePath, json);
            Debug.Log($"Game saved to: {filePath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Save failed: {ex.Message}");
        }
    }
    
    public static SaveData Load(string fileName)
    {
        try
        {
            string filePath = Path.Combine(SAVE_FOLDER, fileName + SAVE_EXTENSION);
            
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                SaveData data = JsonSerializer.Deserialize<SaveData>(json);
                Debug.Log($"Game loaded from: {filePath}");
                return data;
            }
            else
            {
                Debug.LogWarning("Save file not found");
                return null;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Load failed: {ex.Message}");
            return null;
        }
    }
    
    public static List<string> GetSaveFiles()
    {
        if (!Directory.Exists(SAVE_FOLDER))
            return new List<string>();
        
        return Directory.GetFiles(SAVE_FOLDER, "*" + SAVE_EXTENSION)
            .Select(Path.GetFileNameWithoutExtension)
            .ToList();
    }
    
    public static void DeleteSave(string fileName)
    {
        string filePath = Path.Combine(SAVE_FOLDER, fileName + SAVE_EXTENSION);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log($"Deleted save: {fileName}");
        }
    }
}

// Usage example
public class GameController : MonoBehaviour
{
    public void SaveGame()
    {
        SaveData data = new SaveData
        {
            PlayerName = player.Name,
            Level = player.Level,
            Health = player.Health,
            Position = player.transform.position,
            Inventory = inventory.GetItemNames(),
            Currencies = new Dictionary<string, int> 
            { 
                { "Gold", player.Gold },
                { "Gems", player.Gems }
            }
        };
        
        SaveManager.Save("quicksave", data);
    }
    
    public void LoadGame()
    {
        SaveData data = SaveManager.Load("quicksave");
        
        if (data != null)
        {
            player.Name = data.PlayerName;
            player.Level = data.Level;
            player.Health = data.Health;
            player.transform.position = data.Position;
            // ... restore other data
        }
    }
}
```

---

## Audio Managers

```csharp
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private AudioClip[] sfxClips;
    
    private Dictionary<string, AudioClip> musicLibrary;
    private Dictionary<string, AudioClip> sfxLibrary;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeLibraries();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void InitializeLibraries()
    {
        musicLibrary = new Dictionary<string, AudioClip>();
        sfxLibrary = new Dictionary<string, AudioClip>();
        
        foreach (var clip in musicClips)
            musicLibrary[clip.name] = clip;
            
        foreach (var clip in sfxClips)
            sfxLibrary[clip.name] = clip;
    }
    
    public void PlayMusic(string clipName, bool loop = true)
    {
        if (musicLibrary.TryGetValue(clipName, out AudioClip clip))
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }
    }
    
    public void PlaySFX(string clipName, float volume = 1f)
    {
        if (sfxLibrary.TryGetValue(clipName, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }
    
    public void PlaySFXAtPoint(string clipName, Vector3 position, float volume = 1f)
    {
        if (sfxLibrary.TryGetValue(clipName, out AudioClip clip))
        {
            AudioSource.PlayClipAtPoint(clip, position, volume);
        }
    }
    
    public void StopMusic()
    {
        musicSource.Stop();
    }
    
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = Mathf.Clamp01(volume);
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp01(volume);
    }
    
    public void FadeOutMusic(float duration)
    {
        StartCoroutine(FadeOutCoroutine(duration));
    }
    
    IEnumerator FadeOutCoroutine(float duration)
    {
        float startVolume = musicSource.volume;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            yield return null;
        }
        
        musicSource.Stop();
        musicSource.volume = startVolume;
    }
}

// Usage
AudioManager.Instance.PlayMusic("MenuTheme");
AudioManager.Instance.PlaySFX("ButtonClick");
AudioManager.Instance.PlaySFXAtPoint("Explosion", transform.position);
```

---

## Particle and VFX Managers

```csharp
public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance { get; private set; }
    
    [SerializeField] private ParticleSystem explosionPrefab;
    [SerializeField] private ParticleSystem hitEffectPrefab;
    [SerializeField] private ParticleSystem healEffectPrefab;
    
    private Dictionary<string, ObjectPool<ParticleSystem>> effectPools;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePools();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void InitializePools()
    {
        effectPools = new Dictionary<string, ObjectPool<ParticleSystem>>();
        effectPools["Explosion"] = new ObjectPool<ParticleSystem>(explosionPrefab, 10, transform);
        effectPools["Hit"] = new ObjectPool<ParticleSystem>(hitEffectPrefab, 20, transform);
        effectPools["Heal"] = new ObjectPool<ParticleSystem>(healEffectPrefab, 10, transform);
    }
    
    public void PlayEffect(string effectName, Vector3 position, Quaternion rotation)
    {
        if (effectPools.TryGetValue(effectName, out var pool))
        {
            ParticleSystem effect = pool.Get();
            effect.transform.position = position;
            effect.transform.rotation = rotation;
            effect.Play();
            
            StartCoroutine(ReturnToPool(effect, pool));
        }
    }
    
    IEnumerator ReturnToPool(ParticleSystem effect, ObjectPool<ParticleSystem> pool)
    {
        yield return new WaitForSeconds(effect.main.duration);
        pool.Return(effect);
    }
}

// Usage
VFXManager.Instance.PlayEffect("Explosion", enemy.position, Quaternion.identity);
VFXManager.Instance.PlayEffect("Hit", hitPoint, Quaternion.identity);
```

---

## Summary

You now have a library of practical, ready-to-use code patterns for:
- ✅ Design Patterns (Singleton, Observer, Object Pool, Command)
- ✅ Inventory Systems
- ✅ Health and Damage Systems
- ✅ State Machines
- ✅ Timers and Cooldowns
- ✅ Dialogue Systems
- ✅ Quest Systems
- ✅ Save/Load Systems
- ✅ Audio Managers
- ✅ VFX Managers

Copy these patterns and modify them for your games! 🎮✨
