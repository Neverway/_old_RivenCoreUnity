//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: basically just player spawn points. If you only want to spawn certain
// players at that point, you can use the filter tag parameter. For example if
// the player’s spawn filter tag is set to “Blue Team“ they can only spawn at
// spawn points with the filter tag of “Blue Team“. If no valid player starts are
// found, the player will spawn at the world origin (x0, y0, z0)
// Notes:
//
//=============================================================================

using System;
using System.Collections.Generic;
using UnityEngine;

public class Player_Start : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public string spawnTagFilter;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0.5f, 1, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(1, 1.8f, 1));
        Gizmos.DrawIcon(gameObject.transform.position,"player_start");
    }
    

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
