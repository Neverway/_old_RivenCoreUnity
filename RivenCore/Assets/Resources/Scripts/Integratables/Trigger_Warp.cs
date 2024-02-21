//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Warp : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public string warpID;
    public string exitWarpID;
    public float exitOffsetX, exitOffsetY;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private Transform exitTransform;
    private Vector3 exitOffset;


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=

    /*
    private void OnDrawGizmos()
    {
        if (!exitPosition) return;
        Gizmos.color = new Color(0.4f,0.4f,0.4f, 0.2f);
        Gizmos.DrawLine(transform.position, exitPosition.position+exitOffset);
        Gizmos.DrawIcon(exitPosition.position+exitOffset, "trigger_warp_exit.png",false);
    }*/
    private Transform GetExitWarp()
    {
        exitOffset = new Vector3(exitOffsetX, exitOffsetY);
        
        if (exitWarpID == "") return null;
        
        foreach (var warp in FindObjectsOfType<Trigger_Warp>())
        {
            if (warp.warpID == exitWarpID)
            {
                return warp.transform;
            }
        }

        return null;
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (!_other.CompareTag("Entity") && !_other.CompareTag("PhysProp") ) return;
        var targetEnt = _other.gameObject.transform.parent;
        if (GetExitWarp()) targetEnt.transform.position = GetExitWarp().position+exitOffset;
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
