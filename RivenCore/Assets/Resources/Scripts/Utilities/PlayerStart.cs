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
using Unity.Mathematics;
using UnityEngine;

public class PlayerStart : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public string playerStartFilter;
    public Color debugPlayerStartColor = new Color(0, 0.5f, 1, 0.5f);


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
        Gizmos.color = debugPlayerStartColor;
        var fixedGizmoRotation = transform.rotation*Quaternion.AngleAxis(180, Vector3.up) * Quaternion.AngleAxis(-90, Vector3.right);
        Gizmos.DrawMesh(Resources.Load<Mesh>("Models/dev_char"), transform.position+(transform.up*-0.1f), fixedGizmoRotation, transform.localScale);
        Gizmos.DrawIcon(gameObject.transform.position,"player_start");
    }
    

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
