# Unity 3D Game Development Guide

Complete guide for creating 3D games in Unity with C#.

## Table of Contents

- [Getting Started with Unity 3D](#getting-started-with-unity-3d)
- [3D Fundamentals](#3d-fundamentals)
- [Character Controllers](#character-controllers)
- [Camera Systems](#camera-systems)
- [Physics and Collisions](#physics-and-collisions)
- [AI and Navigation](#ai-and-navigation)
- [Lighting and Materials](#lighting-and-materials)
- [Terrain and Environments](#terrain-and-environments)
- [Advanced Topics](#advanced-topics)
- [Complete Game Examples](#complete-game-examples)

---

## Getting Started with Unity 3D

### Setting Up a 3D Project

1. Open Unity Hub
2. Click "New Project"
3. Select "3D" or "3D (URP)" template
4. Name your project (e.g., "My3DGame")
5. Choose location and click "Create"

### Understanding 3D Space

```csharp
// 3D Coordinate System in Unity
// X-axis: Left (-) to Right (+)
// Y-axis: Down (-) to Up (+)
// Z-axis: Back (-) to Forward (+)

Vector3 position = new Vector3(10, 5, 20);
Vector3 forward = Vector3.forward;   // (0, 0, 1)
Vector3 up = Vector3.up;            // (0, 1, 0)
Vector3 right = Vector3.right;      // (1, 0, 0)
```

### Essential 3D Components

- **Transform**: Position, Rotation, Scale in 3D space
- **Rigidbody**: Physics simulation
- **Collider**: Collision detection (BoxCollider, SphereCollider, CapsuleCollider, MeshCollider)
- **MeshFilter**: Holds 3D mesh data
- **MeshRenderer**: Renders the 3D mesh
- **Light**: Illuminates the scene
- **Camera**: Renders the 3D view

---

## 3D Fundamentals

### Working with Vectors

```csharp
using UnityEngine;

public class VectorMath : MonoBehaviour
{
    void Start()
    {
        // Vector operations
        Vector3 a = new Vector3(1, 2, 3);
        Vector3 b = new Vector3(4, 5, 6);
        
        // Addition
        Vector3 sum = a + b;
        
        // Subtraction (direction from a to b)
        Vector3 direction = b - a;
        
        // Distance
        float distance = Vector3.Distance(a, b);
        
        // Magnitude (length)
        float length = direction.magnitude;
        
        // Normalize (make length = 1)
        Vector3 normalized = direction.normalized;
        
        // Dot product (useful for angles)
        float dot = Vector3.Dot(a, b);
        
        // Cross product (perpendicular vector)
        Vector3 cross = Vector3.Cross(a, b);
        
        // Lerp (linear interpolation)
        Vector3 lerped = Vector3.Lerp(a, b, 0.5f);
    }
}
```

### Quaternions and Rotation

```csharp
using UnityEngine;

public class RotationExamples : MonoBehaviour
{
    void Start()
    {
        // Euler angles (degrees)
        Vector3 eulerRotation = new Vector3(45, 90, 0);
        transform.rotation = Quaternion.Euler(eulerRotation);
        
        // Look at target
        Transform target = GameObject.Find("Target").transform;
        transform.LookAt(target);
        
        // Rotate towards
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            90f * Time.deltaTime
        );
        
        // Slerp (smooth rotation)
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime
        );
    }
    
    void Update()
    {
        // Continuous rotation
        transform.Rotate(Vector3.up, 90f * Time.deltaTime);
        
        // Rotate around point
        transform.RotateAround(Vector3.zero, Vector3.up, 90f * Time.deltaTime);
    }
}
```

---

## Character Controllers

### First-Person Controller

```csharp
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 6f;
    public float sprintSpeed = 12f;
    public float crouchSpeed = 3f;
    public float jumpHeight = 2f;
    public float gravity = -20f;
    
    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    
    [Header("Camera")]
    public Camera playerCamera;
    public float mouseSensitivity = 100f;
    public float cameraSmoothing = 0.1f;
    
    [Header("Head Bob")]
    public bool enableHeadBob = true;
    public float bobFrequency = 10f;
    public float bobAmplitude = 0.05f;
    
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;
    
    // Head bob variables
    private float bobTimer = 0f;
    private Vector3 cameraStartPosition;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        
        if (playerCamera != null)
        {
            cameraStartPosition = playerCamera.transform.localPosition;
        }
    }
    
    void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        // Movement input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        Vector3 move = transform.right * x + transform.forward * z;
        
        // Speed selection
        float speed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
            speed = sprintSpeed;
        else if (Input.GetKey(KeyCode.LeftControl))
            speed = crouchSpeed;
        
        controller.Move(move * speed * Time.deltaTime);
        
        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        
        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        
        // Mouse look
        HandleMouseLook();
        
        // Head bob
        if (enableHeadBob && isGrounded)
        {
            HandleHeadBob(move.magnitude);
        }
    }
    
    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
    
    void HandleHeadBob(float movementSpeed)
    {
        if (movementSpeed > 0.1f)
        {
            bobTimer += Time.deltaTime * bobFrequency;
            
            float bobOffsetY = Mathf.Sin(bobTimer) * bobAmplitude;
            float bobOffsetX = Mathf.Cos(bobTimer * 0.5f) * bobAmplitude * 0.5f;
            
            playerCamera.transform.localPosition = new Vector3(
                cameraStartPosition.x + bobOffsetX,
                cameraStartPosition.y + bobOffsetY,
                cameraStartPosition.z
            );
        }
        else
        {
            bobTimer = 0f;
            playerCamera.transform.localPosition = Vector3.Lerp(
                playerCamera.transform.localPosition,
                cameraStartPosition,
                Time.deltaTime * 10f
            );
        }
    }
}
```

### Third-Person Controller

```csharp
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float rotationSpeed = 10f;
    public float jumpForce = 5f;
    
    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;
    
    [Header("Camera")]
    public Transform cameraTarget;
    
    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;
    private bool isGrounded;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        // Get input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;
        
        if (inputDirection.magnitude >= 0.1f)
        {
            // Calculate movement direction relative to camera
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg 
                              + Camera.main.transform.eulerAngles.y;
            
            // Smooth rotation
            float angle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetAngle,
                ref rotationSpeed,
                0.1f
            );
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            
            // Move
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;
            controller.Move(moveDirection.normalized * speed * Time.deltaTime);
            
            // Update animator
            if (animator != null)
            {
                animator.SetFloat("Speed", speed);
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetFloat("Speed", 0f);
            }
        }
        
        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
            if (animator != null)
            {
                animator.SetTrigger("Jump");
            }
        }
        
        // Gravity
        velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        
        // Update animator
        if (animator != null)
        {
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("VelocityY", velocity.y);
        }
    }
}
```

### Rigidbody Character Controller

```csharp
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float jumpForce = 10f;
    public float airControl = 0.5f;
    
    [Header("Ground Check")]
    public LayerMask groundMask;
    public float groundCheckDistance = 1.1f;
    
    private Rigidbody rb;
    private bool isGrounded;
    private Vector3 moveInput;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent rigidbody from rotating
    }
    
    void Update()
    {
        // Get input
        moveInput = new Vector3(
            Input.GetAxis("Horizontal"),
            0f,
            Input.GetAxis("Vertical")
        );
        
        // Ground check
        isGrounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            groundCheckDistance,
            groundMask
        );
        
        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    
    void FixedUpdate()
    {
        // Calculate movement
        Vector3 targetVelocity = moveInput.normalized * moveSpeed;
        
        // Apply air control
        float control = isGrounded ? 1f : airControl;
        
        // Apply force
        Vector3 velocityChange = (targetVelocity - rb.velocity);
        velocityChange.y = 0; // Don't affect vertical velocity
        
        rb.AddForce(velocityChange * control, ForceMode.VelocityChange);
    }
}
```

---

## Camera Systems

### Third-Person Camera

```csharp
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -5);
    
    [Header("Rotation")]
    public float rotationSpeed = 5f;
    public float minVerticalAngle = -30f;
    public float maxVerticalAngle = 70f;
    
    [Header("Zoom")]
    public float zoomSpeed = 2f;
    public float minZoom = 2f;
    public float maxZoom = 10f;
    
    [Header("Collision")]
    public bool enableCollision = true;
    public LayerMask collisionLayers;
    public float collisionOffset = 0.3f;
    
    private float currentRotationX = 0f;
    private float currentRotationY = 0f;
    private float currentZoom = 5f;
    
    void Start()
    {
        currentZoom = offset.magnitude;
        
        // Initialize rotation from current offset
        Vector3 angles = Quaternion.LookRotation(offset).eulerAngles;
        currentRotationX = angles.y;
        currentRotationY = angles.x;
    }
    
    void LateUpdate()
    {
        if (target == null) return;
        
        // Handle rotation input
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;
        
        currentRotationX += mouseX;
        currentRotationY -= mouseY;
        currentRotationY = Mathf.Clamp(currentRotationY, minVerticalAngle, maxVerticalAngle);
        
        // Handle zoom input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        currentZoom -= scrollInput * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        
        // Calculate desired position
        Quaternion rotation = Quaternion.Euler(currentRotationY, currentRotationX, 0);
        Vector3 desiredPosition = target.position - (rotation * Vector3.forward * currentZoom);
        desiredPosition.y = target.position.y + offset.y;
        
        // Handle collision
        if (enableCollision)
        {
            RaycastHit hit;
            Vector3 direction = desiredPosition - target.position;
            
            if (Physics.Raycast(target.position, direction.normalized, out hit, direction.magnitude, collisionLayers))
            {
                desiredPosition = hit.point + hit.normal * collisionOffset;
            }
        }
        
        transform.position = desiredPosition;
        transform.LookAt(target.position + Vector3.up * offset.y);
    }
}
```

### Cinemachine Camera Setup

```csharp
using UnityEngine;
using Cinemachine;

public class CinemachineManager : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineFreeLook freeLookCamera;
    
    [Header("Shake")]
    public float shakeDuration = 0.3f;
    public float shakeAmplitude = 1.5f;
    public float shakeFrequency = 2f;
    
    private CinemachineBasicMultiChannelPerlin noise;
    
    void Start()
    {
        if (virtualCamera != null)
        {
            noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }
    
    public void ShakeCamera()
    {
        StartCoroutine(ShakeCameraCoroutine());
    }
    
    System.Collections.IEnumerator ShakeCameraCoroutine()
    {
        if (noise != null)
        {
            noise.m_AmplitudeGain = shakeAmplitude;
            noise.m_FrequencyGain = shakeFrequency;
            
            yield return new WaitForSeconds(shakeDuration);
            
            noise.m_AmplitudeGain = 0f;
        }
    }
    
    public void SwitchToFreeLook()
    {
        if (virtualCamera != null) virtualCamera.Priority = 0;
        if (freeLookCamera != null) freeLookCamera.Priority = 10;
    }
    
    public void SwitchToVirtualCamera()
    {
        if (freeLookCamera != null) freeLookCamera.Priority = 0;
        if (virtualCamera != null) virtualCamera.Priority = 10;
    }
}
```

---

## Physics and Collisions

### Rigidbody Physics

```csharp
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    private Rigidbody rb;
    
    [Header("Settings")]
    public float explosionForce = 500f;
    public float torqueAmount = 10f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Rigidbody settings
        rb.mass = 1f;
        rb.drag = 0f;               // Air resistance
        rb.angularDrag = 0.05f;     // Rotational resistance
        rb.useGravity = true;
        rb.isKinematic = false;     // false = affected by physics
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Smooth movement
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // Better collision
    }
    
    void Update()
    {
        // Apply force
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * explosionForce);
        }
        
        // Apply torque (rotation force)
        if (Input.GetKey(KeyCode.T))
        {
            rb.AddTorque(Vector3.up * torqueAmount);
        }
    }
    
    // Add explosion force from point
    public void Explode(Vector3 explosionPosition, float radius)
    {
        rb.AddExplosionForce(explosionForce, explosionPosition, radius, 3f, ForceMode.Impulse);
    }
}
```

### Collision Detection

```csharp
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    [Header("Collision Events")]
    public UnityEngine.Events.UnityEvent onCollisionEnter;
    public UnityEngine.Events.UnityEvent onTriggerEnter;
    
    // Called when collision starts
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collided with {collision.gameObject.name}");
        
        // Get collision info
        ContactPoint contact = collision.GetContact(0);
        Vector3 contactPoint = contact.point;
        Vector3 contactNormal = contact.normal;
        
        // Get impact force
        float impactForce = collision.relativeVelocity.magnitude;
        
        if (impactForce > 5f)
        {
            Debug.Log("Heavy impact!");
        }
        
        onCollisionEnter?.Invoke();
    }
    
    // Called while objects are touching
    void OnCollisionStay(Collision collision)
    {
        // Continuous collision
    }
    
    // Called when collision ends
    void OnCollisionExit(Collision collision)
    {
        Debug.Log($"Stopped colliding with {collision.gameObject.name}");
    }
    
    // Trigger events (collider must have "Is Trigger" checked)
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger entered by {other.gameObject.name}");
        onTriggerEnter?.Invoke();
    }
    
    void OnTriggerStay(Collider other)
    {
        // Continuous trigger
    }
    
    void OnTriggerExit(Collider other)
    {
        Debug.Log($"Trigger exited by {other.gameObject.name}");
    }
}
```

### Raycasting in 3D

```csharp
using UnityEngine;

public class Raycasting3D : MonoBehaviour
{
    [Header("Settings")]
    public float rayDistance = 100f;
    public LayerMask hitLayers;
    
    void Update()
    {
        // Simple raycast from camera
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, rayDistance, hitLayers))
        {
            Debug.Log($"Hit {hit.collider.name} at {hit.point}");
            
            // Visualize hit point
            Debug.DrawLine(ray.origin, hit.point, Color.green);
            
            // Get hit information
            Vector3 hitPoint = hit.point;
            Vector3 hitNormal = hit.normal;
            float distance = hit.distance;
            Transform hitTransform = hit.transform;
            
            // Interact with hit object
            if (Input.GetMouseButtonDown(0))
            {
                Rigidbody rb = hit.rigidbody;
                if (rb != null)
                {
                    rb.AddForceAtPosition(ray.direction * 100f, hit.point);
                }
            }
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);
        }
    }
    
    // Raycast from object position
    public bool CheckForward(out RaycastHit hit)
    {
        return Physics.Raycast(
            transform.position,
            transform.forward,
            out hit,
            rayDistance,
            hitLayers
        );
    }
    
    // Sphere cast (thicker ray)
    public bool SphereCast(float radius, out RaycastHit hit)
    {
        return Physics.SphereCast(
            transform.position,
            radius,
            transform.forward,
            out hit,
            rayDistance,
            hitLayers
        );
    }
    
    // Check all objects in ray path
    public RaycastHit[] RaycastAll()
    {
        return Physics.RaycastAll(
            transform.position,
            transform.forward,
            rayDistance,
            hitLayers
        );
    }
}
```

---

## AI and Navigation

### NavMesh Agent Setup

```csharp
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIController : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    
    [Header("Patrol")]
    public Transform[] patrolPoints;
    public float waitTime = 2f;
    
    [Header("Chase")]
    public float chaseRange = 10f;
    public float attackRange = 2f;
    
    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    private float waitTimer = 0f;
    private bool isWaiting = false;
    
    public enum AIState
    {
        Idle,
        Patrol,
        Chase,
        Attack
    }
    
    public AIState currentState = AIState.Patrol;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    
    void Update()
    {
        float distanceToTarget = target != null ? Vector3.Distance(transform.position, target.position) : Mathf.Infinity;
        
        switch (currentState)
        {
            case AIState.Idle:
                Idle();
                break;
            
            case AIState.Patrol:
                Patrol();
                if (distanceToTarget < chaseRange)
                {
                    currentState = AIState.Chase;
                }
                break;
            
            case AIState.Chase:
                Chase();
                if (distanceToTarget > chaseRange)
                {
                    currentState = AIState.Patrol;
                }
                else if (distanceToTarget < attackRange)
                {
                    currentState = AIState.Attack;
                }
                break;
            
            case AIState.Attack:
                Attack();
                if (distanceToTarget > attackRange)
                {
                    currentState = AIState.Chase;
                }
                break;
        }
    }
    
    void Idle()
    {
        agent.isStopped = true;
    }
    
    void Patrol()
    {
        if (patrolPoints.Length == 0) return;
        
        if (!isWaiting)
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                isWaiting = true;
                waitTimer = waitTime;
            }
        }
        else
        {
            waitTimer -= Time.deltaTime;
            
            if (waitTimer <= 0)
            {
                isWaiting = false;
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            }
        }
    }
    
    void Chase()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }
    
    void Attack()
    {
        agent.isStopped = true;
        
        // Look at target
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        
        // Implement attack logic here
        Debug.Log("Attacking!");
    }
}
```

### Simple Enemy AI

```csharp
using UnityEngine;

public class SimpleEnemyAI : MonoBehaviour
{
    [Header("Target")]
    public Transform player;
    
    [Header("Settings")]
    public float detectionRange = 15f;
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    
    [Header("Field of View")]
    public float viewAngle = 90f;
    
    private float lastAttackTime;
    private bool playerDetected;
    
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        // Check if player is in range and field of view
        if (distanceToPlayer < detectionRange)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToPlayer);
            
            if (angle < viewAngle / 2f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRange))
                {
                    if (hit.transform == player)
                    {
                        playerDetected = true;
                    }
                }
            }
        }
        else
        {
            playerDetected = false;
        }
        
        if (playerDetected)
        {
            if (distanceToPlayer > attackRange)
            {
                // Move towards player
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
                
                // Rotate towards player
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                // Attack
                if (Time.time - lastAttackTime > attackCooldown)
                {
                    AttackPlayer();
                    lastAttackTime = Time.time;
                }
            }
        }
    }
    
    void AttackPlayer()
    {
        Debug.Log("Enemy attacks player!");
        // Implement attack logic
    }
    
    void OnDrawGizmosSelected()
    {
        // Draw detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // Draw field of view
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward * detectionRange;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward * detectionRange;
        
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        
        // Draw attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
```

---

## Lighting and Materials

### Dynamic Lighting

```csharp
using UnityEngine;

public class DynamicLight : MonoBehaviour
{
    private Light lightComponent;
    
    [Header("Flicker")]
    public bool enableFlicker = false;
    public float flickerSpeed = 10f;
    public float flickerAmount = 0.5f;
    
    [Header("Pulse")]
    public bool enablePulse = false;
    public float pulseSpeed = 2f;
    public float minIntensity = 0.5f;
    public float maxIntensity = 2f;
    
    private float baseIntensity;
    
    void Start()
    {
        lightComponent = GetComponent<Light>();
        baseIntensity = lightComponent.intensity;
    }
    
    void Update()
    {
        if (enableFlicker)
        {
            float flicker = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
            lightComponent.intensity = baseIntensity + (flicker - 0.5f) * flickerAmount;
        }
        
        if (enablePulse)
        {
            float pulse = Mathf.PingPong(Time.time * pulseSpeed, 1f);
            lightComponent.intensity = Mathf.Lerp(minIntensity, maxIntensity, pulse);
        }
    }
}
```

### Material Property Modifier

```csharp
using UnityEngine;

public class MaterialModifier : MonoBehaviour
{
    private Renderer rend;
    private MaterialPropertyBlock propBlock;
    
    void Start()
    {
        rend = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();
    }
    
    public void SetColor(Color color)
    {
        rend.GetPropertyBlock(propBlock);
        propBlock.SetColor("_Color", color);
        rend.SetPropertyBlock(propBlock);
    }
    
    public void SetEmission(Color emissionColor)
    {
        rend.GetPropertyBlock(propBlock);
        propBlock.SetColor("_EmissionColor", emissionColor);
        rend.SetPropertyBlock(propBlock);
    }
    
    public void FadeOut(float duration)
    {
        StartCoroutine(FadeCoroutine(1f, 0f, duration));
    }
    
    System.Collections.IEnumerator FadeCoroutine(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color color = rend.material.color;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            color.a = alpha;
            SetColor(color);
            yield return null;
        }
    }
}
```

---

## Terrain and Environments

### Terrain Height Sampling

```csharp
using UnityEngine;

public class TerrainInteraction : MonoBehaviour
{
    public Terrain terrain;
    
    void Start()
    {
        if (terrain == null)
        {
            terrain = Terrain.activeTerrain;
        }
    }
    
    public float GetTerrainHeight(Vector3 worldPosition)
    {
        return terrain.SampleHeight(worldPosition);
    }
    
    public void PlaceOnTerrain()
    {
        Vector3 pos = transform.position;
        pos.y = GetTerrainHeight(pos);
        transform.position = pos;
    }
    
    public Vector3 GetTerrainNormal(Vector3 worldPosition)
    {
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainPosition = worldPosition - terrain.transform.position;
        
        Vector3 normalizedPos = new Vector3(
            terrainPosition.x / terrainData.size.x,
            0,
            terrainPosition.z / terrainData.size.z
        );
        
        return terrainData.GetInterpolatedNormal(normalizedPos.x, normalizedPos.z);
    }
}
```

---

## Advanced Topics

### Object Pooling

```csharp
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    
    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;
    
    public static ObjectPool Instance;
    
    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            
            poolDictionary.Add(pool.tag, objectPool);
        }
    }
    
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return null;
        }
        
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        
        poolDictionary[tag].Enqueue(objectToSpawn);
        
        return objectToSpawn;
    }
}

// Usage:
// ObjectPool.Instance.SpawnFromPool("Bullet", transform.position, Quaternion.identity);
```

### Save System

```csharp
using UnityEngine;
using System.IO;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public int playerHealth;
    public int score;
    public float[] additionalData;
}

public class SaveSystem : MonoBehaviour
{
    private string savePath;
    
    void Start()
    {
        savePath = Application.persistentDataPath + "/savegame.json";
    }
    
    public void SaveGame(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log($"Game saved to {savePath}");
    }
    
    public GameData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            GameData data = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game loaded");
            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found");
            return null;
        }
    }
    
    public void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Save file deleted");
        }
    }
}
```

---

## Complete Game Examples

### FPS Shooting Mechanics

```csharp
using UnityEngine;

public class FPSWeapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    public int damage = 25;
    public float range = 100f;
    public float fireRate = 15f;
    public int maxAmmo = 30;
    public float reloadTime = 1.5f;
    
    [Header("Effects")]
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    
    [Header("Camera")]
    public Camera fpsCam;
    
    private int currentAmmo;
    private float nextTimeToFire = 0f;
    private bool isReloading = false;
    
    void Start()
    {
        currentAmmo = maxAmmo;
    }
    
    void Update()
    {
        if (isReloading) return;
        
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }
    
    void Shoot()
    {
        currentAmmo--;
        
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
        
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log($"Hit {hit.transform.name}");
            
            // Apply damage
            Health targetHealth = hit.transform.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damage);
            }
            
            // Impact effect
            if (impactEffect != null)
            {
                GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 2f);
            }
        }
    }
    
    System.Collections.IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        
        yield return new WaitForSeconds(reloadTime);
        
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    
    void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} health: {currentHealth}");
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        Destroy(gameObject);
    }
}
```

---

## Best Practices

1. **Use LOD (Level of Detail)** for complex models
2. **Occlusion Culling** to improve performance
3. **Bake Lighting** for static objects
4. **Use Object Pooling** for frequently spawned objects
5. **Optimize Physics** with layer collision matrix
6. **Profile Regularly** using Unity Profiler
7. **Use NavMesh** for AI pathfinding
8. **Implement LOD Groups** for distant objects

---

## Next Steps

1. Build a complete FPS or third-person game
2. Implement advanced AI behaviors
3. Add multiplayer functionality with Netcode/Mirror
4. Create procedural content generation
5. Optimize for mobile or VR platforms
6. Add post-processing effects
7. Implement advanced physics simulations

**Happy 3D game development!**
