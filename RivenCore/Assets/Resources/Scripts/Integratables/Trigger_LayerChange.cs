//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes: Layer mask ids: 1-6, 2-7, 3-8
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_LayerChange : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Tooltip("Which layer should the entity switch to when entering the trigger. P.S DON'T FORGET TO SWITCH THE TRIGGERS LAYER!!")]
    [Range(0, 2)] public int exitLayer;

    public float fallTime;


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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Entity") && !other.CompareTag("PhysProp")) return;
        var entity = other.transform.parent.GetComponent<Object_DepthAssigner>();
        entity.depthLayer = exitLayer;
        if (fallTime <= 0) return;
        entity.fallTime = fallTime;
        entity.Fall();
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
