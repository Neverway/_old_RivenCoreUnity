//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Entity_Controller_Topdown2D", menuName="Neverway/ScriptableObjects/Entity/Controller/Topdown2D")]
public class Entity_Controller_Topdown2D : Entity_Controller
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    public Vector2 movementDirection;
    public float movementMultiplier=1;
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");
    private static readonly int LastX = Animator.StringToHash("LastX");
    private static readonly int LastY = Animator.StringToHash("LastY");
    public Vector2 facingDirection;
    public bool invulnerable;
    
    
    //=-----------------=
    // Reference Variables
    //=-----------------=
    private GameInstance gameInstance;
    public InputActions.TopDown2DActions topdown2DActions;
    private Rigidbody2D entityRigidbody2D;
    private Animator animator;
    private Entity entity;
    [SerializeField] private GameObject interactionPrefab;
    

    //=-----------------=
    // Internal Functions
    //=-----------------=
    public override void EntityAwake(Entity _entity)
    {
        gameInstance = FindObjectOfType<GameInstance>();
        topdown2DActions = new InputActions().TopDown2D;
        topdown2DActions.Enable();
        GetReferences(_entity);
    }
    
    public override void EntityUpdate(Entity _entity)
    {
        if (!_entity.isPossessed) return;
        if (topdown2DActions.Pause.WasPressedThisFrame()) gameInstance.UI_ShowPause();
        if (topdown2DActions.Interact.WasPressedThisFrame()) Interact();
        if (_entity.isPaused) return;
        _entity.currentStats.movementSpeed = topdown2DActions.Action.IsPressed() ? _entity.currentStats.sprintSpeed : _entity.currentStats.walkSpeed; 
        movementDirection = topdown2DActions.Move.ReadValue<Vector2>();
    }
    
    public override void EntityFixedUpdate(Entity _entity)
    {
        if (!_entity.isPossessed) return;
        if (_entity.isPaused) return;
        UpdateMovement();
        UpdateAnimator();
    }
    
    private void GetReferences(Entity _entity)
    {
        entityRigidbody2D = _entity.GetComponent<Rigidbody2D>();
        animator = _entity.GetComponent<Animator>();
        entity = _entity;
        _entity.currentStats = _entity.characterStats.stats;
        _entity.OnEntityHeal += OnEntityHeal;
        _entity.OnEntityHurt += OnEntityHurt;
        _entity.OnEntityDeath += OnEntityDeath;
    }
    
    private void UpdateMovement()
    {
        // Update object position
        var position = entity.transform.position;
        if (entityRigidbody2D)
        {
            entityRigidbody2D.MovePosition(position + new Vector3(
                movementDirection.x, 
                movementDirection.y, 
                position.z) * (entity.currentStats.movementSpeed * movementMultiplier * Time.fixedDeltaTime));
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

    private void OnEntityHeal()
    {
        animator.Play("Heal");
    }

    private void OnEntityHurt()
    {
        animator.Play("Hurt");
    }

    private void OnEntityDeath()
    {
        animator.Play("Knockout");
    }

    private void Interact()
    {
        // Get interaction direction
        var directionalRotation = facingDirection.y switch
        {
            1 => 0,
            -1 => 180,
            _ => facingDirection.x switch
            {
                1 => -90,
                -1 => 90,
                _ => 0
            }
        };
    
        // Create interaction trigger
        var interactionTrigger = Instantiate(interactionPrefab, entity.transform.position, Quaternion.Euler(0, 0, directionalRotation));
        interactionTrigger.GetComponent<Trigger_Interaction>().targetEntity = entity;
        interactionTrigger.GetComponent<Object_DepthAssigner>().depthLayer =
            entity.GetComponent<Object_DepthAssigner>().depthLayer;
        Destroy(interactionTrigger, 0.15f);
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
