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
    
[Serializable]
public struct Entity_Stats
{ 
    //=-----------------=
    // Public Variables
    //=-----------------=
    public string characterName;
    public string team;
    public float health;
    public float movementSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public RuntimeAnimatorController animator;
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
    [Serializable]
    public class Sounds
    {
        public AudioClip hurt;
        public AudioClip heal;
        public AudioClip death;
        public AudioClip alerted;
    }


    //=-----------------=
    // External Functions
    //=-----------------=

    // Constructor with parameters to copy another Entity_Stats
    public Entity_Stats(Entity_Stats other)
    {
        // Copy values from 'other' to the new instance
        characterName = other.characterName;
        team = other.team;
        health = other.health;
        movementSpeed = other.movementSpeed;
        walkSpeed = other.walkSpeed;
        sprintSpeed = other.sprintSpeed;
        animator = other.animator; // Assuming RuntimeAnimatorController is a value type
        sounds = new Sounds(); // Copying Sounds separately
        sounds.hurt = other.sounds.hurt;
        sounds.heal = other.sounds.heal;
        sounds.death = other.sounds.death;
        sounds.alerted = other.sounds.alerted;
    }
}

