//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: 
// Notes: 
//
//=============================================================================


using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class Entity : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public Entity_Controller currentController;
    public Entity_Controller fallbackController;
    public Entity_Stats stats;
    public float movementMultiplier=1;
    [SerializeField] private LayerMask groundMask;
    public event Action EntityDeath;


    //=-----------------=
    // Private Variables
    //=-----------------=
    public bool paused;
    public float currentMovementSpeed;
    public float currentHealth;
    public int currentGold;
    public Vector2 movementDirection;
    public Vector2 facingDirection;
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");
    private static readonly int LastX = Animator.StringToHash("LastX");
    private static readonly int LastY = Animator.StringToHash("LastY");
    public bool invulnerable;
    private Vector2 storedMoveDirection; // used to restore momentum when un-pausing the entity
    private float storedAnimationSpeed; // used to restore animation when un-pausing the entity
    private bool animatorWasEnabled;
    [SerializeField] private bool isGrounded;
    [SerializeField] private int currentDoubleJumps;
    private bool jumpCooldown;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private Rigidbody2D entityRigidbody2D;
    private Rigidbody entityRigidbody;
    private Animator animator;
    private AudioVarienceModulator audioSource;

    public bool isPossessed;
    //[SerializeField] private Entity_Controller_Player playerController; // This is used to test to see if the current entity is controlled by a player


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        GetReferences();
        VerifyCurrentController();
        if (currentController) currentController.EntityAwake(this);
        InitializeStats();
    }
    
    private void Update()
    {
        VerifyCurrentController();
        if (paused) return;
        if (currentController) currentController.Think(this);
    }
    
    private void FixedUpdate()
    {
        UpdateMovement();
        UpdateAnimator();
    }
    

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void VerifyCurrentController()
    {
        if (currentController) return;
        if (!fallbackController) Debug.LogError(gameObject.name + "'s 'Entity' script requires a 'fallbackController' but none was specified!");
        currentController = fallbackController;
    }
    
    private void GetReferences()
    {
        entityRigidbody = GetComponent<Rigidbody>();
        entityRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioVarienceModulator>();
    }
    
    private void InitializeStats()
    {
        if (!stats) Debug.LogError(gameObject.name + "'s 'Entity' script requires a 'stats' object but none was specified!");
        // Set the entity animation controller (if one is specified)
        if (stats.animationController) animator.runtimeAnimatorController = stats.animationController;
        // Set initial values (I'm not sure this should stay here)
        currentMovementSpeed = stats.walkSpeed;
        currentHealth = stats.maxHealth;
    }
    
    private void UpdateMovement()
    {
        // Update object position
        var position = transform.position;
        if (entityRigidbody2D)
        {
            entityRigidbody2D.MovePosition(position + new Vector3(
                movementDirection.x, 
                movementDirection.y, 
                position.z) * (currentMovementSpeed * movementMultiplier * Time.fixedDeltaTime));
        }
        
        else if (entityRigidbody)
        {
            // Check grounded
            isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, 0, 0), 0.4f, groundMask);
            // Reset double jumps
            if (isGrounded) currentDoubleJumps = stats.doubleJumps;
            
            ControlDrag3D();
            // Apply Movement
            var movementDirection3D = transform.forward * movementDirection.y + transform.right * movementDirection.x;
            if (isGrounded)
            {
                entityRigidbody.AddForce(
                    movementDirection3D.normalized *
                    currentMovementSpeed * 
                    movementMultiplier,
                    ForceMode.Acceleration);
            }
            else
            {
                entityRigidbody.AddForce(
                    movementDirection3D.normalized * 
                    currentMovementSpeed * 
                    movementMultiplier *
                    stats.aerialControlMultiplier, 
                    ForceMode.Acceleration);
                entityRigidbody.AddForce(Vector3.up * -stats.fallRate, ForceMode.Acceleration);
            }
        }
    }
    
    private void UpdateAnimator()
    {
        if (!animator || !animator.enabled) return;
        // Set Animator Moving Direction
        animator.SetFloat(MoveX, movementDirection.x);
        animator.SetFloat(MoveY, movementDirection.y);
        // Set Animator Idling Direction
        if (movementDirection.x != 0 || movementDirection.y != 0) { facingDirection = movementDirection; }
        animator.SetFloat(LastX, facingDirection.x);
        animator.SetFloat(LastY, facingDirection.y);
    }
    
    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(stats.invulnerabilityDuration);
        invulnerable = false;
    }
    
    private IEnumerator JumpCooldown()
    {
        jumpCooldown = true;
        yield return new WaitForSeconds(stats.jumpCooldown);
        jumpCooldown = false;
    }

    private void ControlDrag3D()
    {
        if (isGrounded) entityRigidbody.drag = stats.groundDrag;
        else if (!isGrounded) entityRigidbody.drag = stats.airDrag;
    }

    public void Jump3D()
    {
        if (isGrounded && !jumpCooldown)
        {
            entityRigidbody.AddForce(transform.up * stats.jumpForce, ForceMode.Impulse);
            StartCoroutine(JumpCooldown());
        }

        else if (!isGrounded && currentDoubleJumps >= 1 && !jumpCooldown)
        {
            entityRigidbody.AddForce(transform.up * stats.doubleJumpForce, ForceMode.Impulse);
            --currentDoubleJumps;
            StartCoroutine(JumpCooldown());
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    public void SetMovement(Vector2 _movement)
    {
        movementDirection = _movement;
    }
    
    public void SetSprinting(bool _isSprinting)
    {
        currentMovementSpeed = _isSprinting ? stats.runSpeed : stats.walkSpeed;
    }

    public void SetCurrentController(Entity_Controller _entityController)
    {
        currentController = _entityController;
    }
    
    public void AddHealth(float _value)
    {
        if (invulnerable) return;
        invulnerable = true;
        switch (_value)
        {
            case < 0 when animator:
                // Double-Check if entity is dead
                if (currentHealth <= 0)
                {
                    if (EntityDeath != null) EntityDeath.Invoke();
                    if (isPossessed)
                    {
                        animator.Play("Knockout");
                        FindObjectOfType<System_MusicManager>().SetSecondaryChannel(stats.sounds.die);
                        FindObjectOfType<System_MusicManager>().CrossFadeTracks();
                        FindObjectOfType<System_SceneLoader>().ForceLoadScene("Game Over", 1);
                    }
                    else
                    {
                        animator.Play("Knockout");
                        audioSource.PlaySound(stats.sounds.die);
                        Destroy(gameObject, 1);
                    }
                }
                else
                {
                    animator.Play("Hurt");
                    audioSource.PlaySound(stats.sounds.hurt);
                }
                break;
            case > 0 when animator:
                animator.Play("Heal");
                audioSource.PlaySound(stats.sounds.heal);
                break;
        }
        // Modify the health value, then clamp the result to the health range
        currentHealth += _value;
        currentHealth = Mathf.Clamp(currentHealth, 0f, stats.maxHealth);
        // Kill entity if their health reaches 0
        if (currentHealth <= 0)
        {
            if (EntityDeath != null) EntityDeath.Invoke();
            if (isPossessed)
            {
                animator.Play("Knockout");
                FindObjectOfType<System_MusicManager>().SetSecondaryChannel(stats.sounds.die);
                FindObjectOfType<System_MusicManager>().CrossFadeTracks();
                FindObjectOfType<System_SceneLoader>().ForceLoadScene("Game Over", 1);
            }
            else
            {
                animator.Play("Knockout");
                audioSource.PlaySound(stats.sounds.die);
                Destroy(gameObject, 1);
            }
        }
        StartCoroutine(nameof(DamageCooldown));
    }
    
    public void Pause()
    {
        // Store and freeze entity movement
        storedMoveDirection = movementDirection;
        paused = true;
        SetMovement(new Vector2(0, 0));
        // Store and freeze entity animation
        if (!animator) return;
        storedAnimationSpeed = animator.speed;
        animatorWasEnabled = animator.enabled;
        animator.enabled = false;
    }
    
    public void Unpause()
    {
        // Restore entity movement
        SetMovement(storedMoveDirection);
        paused = false;
        // Restore entity animation
        if (!animator) return;
        animator.speed = storedAnimationSpeed;
        animator.enabled = animatorWasEnabled;
    }

    public Quaternion GetFaceDirection()
    {
        // Calculate the angle based on the facingDirection vector
        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
    
        // Create a Quaternion representing the rotation
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        return rotation;
    }

    public Quaternion GetFaceDirection(int zRotationOffset)
    {
        // Calculate the angle based on the facingDirection vector
        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
    
        // Create a Quaternion representing the rotation
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle+zRotationOffset));

        return rotation;
    }
}
