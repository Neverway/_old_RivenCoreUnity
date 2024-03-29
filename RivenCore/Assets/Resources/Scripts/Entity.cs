//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: 
// Notes: The way the entity system works is like this:
// The main entity script is added as a component to the root of the character.
// The entity script can take in an entity controller, which is essentially just
// a child of the entity script (which is the parent class.) It also takes a
// ‘stats' object that holds the default values for whatever entity it is (like
// it’s base health, team, speed, etc.)
//
// The entity class has some pre-built variables and functions such as handling
// entity death, health, speed, movement directions, and functions that handle
// simple 2D and 3D movement (which I need to expand upon in the future. (2D
// side-scroller, 2D top-down, 3D first person, 3D third person))
//
// The entity controller (the child class) is something you create for your
// project. The entity controller can use the exiting functions to create a
// basic character very quickly and easily. You can then expand upon the base
// class to create something that fits your project better.
//
// You should create separate entity controllers for AI, local player,
// and network player driven entities.
//
// Entity - Base class (locked)
//    |> Controller - Movement handler for entity (scriptable)
//    |> Stats_Character - Collection of base stats for a character (scriptable)
//    |> Stats - The current values of the character identity (modifiable)
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
    [Tooltip(
        "The current controller that's driving this entity, can be swapped during runtime for things like cutscenes or a possession-like game mechanic")]
    public Entity_Controller currentController;

    [Tooltip("The default controller for this entities gamemode")]
    public Entity_Controller fallbackController;

    [Tooltip("The default stats for this entity")]
    public Entity_CharacterStats characterStats;

    [Tooltip("The current stats for this entity (Initialized at awake, modified during runtime)")]
    public Entity_Stats currentStats;

    public bool isPossessed;
    public bool isPaused;

    public event Action EntityDeath;
    public Vector2 movementDirection;
    public Vector2 facingDirection;
    public bool isGrounded;
    [Tooltip("The collision layers that will be checked when testing if the entity is grounded")]
    [SerializeField] private LayerMask groundMask;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        VerifyCurrentController();
        currentController.EntityAwake(this);
        currentStats = characterStats.stats;
    }

    private void Update()
    {
        VerifyCurrentController();
        currentController.Think(this);
    }

    private void FixedUpdate()
    {
        currentController.FixedRateThink(this);
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void VerifyCurrentController()
    {
        if (currentController) return;
        if (!fallbackController)
            Debug.LogError(gameObject.name +
                           "'s 'Entity' script requires a 'fallbackController' but none was specified!");
        currentController = fallbackController;
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    public void ModifyHealth(float _value)
    {
        switch (_value)
        {
            case > 0 when currentStats.sounds.heal:
                GetComponent<AudioVarienceModulator>().PlaySound(currentStats.sounds.heal);
                break;
            case < 0 when currentStats.sounds.heal:
                GetComponent<AudioVarienceModulator>().PlaySound(currentStats.sounds.hurt);
                break;
        }

        if (currentStats.health + _value <= 0)
        {
            GetComponent<AudioVarienceModulator>().PlaySound(currentStats.sounds.death);
            if (EntityDeath != null) EntityDeath.Invoke();
        }

        currentStats.health += _value;
    }

    public bool IsGrounded3D()
    {
        return Physics.CheckSphere(transform.position - new Vector3(0, 0, 0), 0.4f, groundMask);
    }

    public void Move(Vector3 movement, string mode)
    {
        if (mode == "translate") transform.Translate(movement * (currentStats.movementSpeed * Time.deltaTime));
    }
    public void Move(Vector3 movement, string mode, float movementSpeed)
    {
        if (mode == "translate") transform.Translate(movement * (movementSpeed * Time.deltaTime));
    }

}
