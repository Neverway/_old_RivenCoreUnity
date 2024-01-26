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
public class Entity_Stats
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
}

