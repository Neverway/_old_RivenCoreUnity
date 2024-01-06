//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: The stats are the data that are individual to an entity, for example
//  a characters name, walk speed, and special abilities would be stored here
// Notes:
//
//=============================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
    
[CreateAssetMenu(fileName="Entity_Stats", menuName="Neverway/ScriptableObjects/Entity/Stats")]
public class Entity_Stats : ScriptableObject
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Header("Basic Entity Values")]
    public string characterName="???";
    public RuntimeAnimatorController animationController;
    public float walkSpeed=3;
    public float runSpeed=7;
    public float maxHealth=100;
    public float invulnerabilityDuration=1;
    public List<string> teamGroups; // This probably shouldn't be part of a ScriptableObject since we may want to change the values
    public int partyPriority=0; // The higher the priority the further back they will follow
    [Header("3D Entity Values")] 
    public float groundDrag=6;
    public float airDrag=2;
    public float aerialControlMultiplier=0.6f;
    public float entityHeight=1;
    public float jumpForce=15;
    public float jumpCooldown = 0.25f;
    public float fallRate=32;
    public float doubleJumpForce=25;
    public int doubleJumps=0;
    [Header("Sounds")] 
    public Sounds sounds;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=

    //=-----------------=
    // Internal Functions
    //=-----------------=
    [Serializable] public class Sounds
    {
        public AudioClip heal;
        public AudioClip hurt;
        public AudioClip step;
        public AudioClip die;
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
