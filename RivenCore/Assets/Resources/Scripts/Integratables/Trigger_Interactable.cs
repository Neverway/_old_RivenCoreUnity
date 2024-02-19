//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[RequireComponent(typeof(Logic_Processor))]
public class Trigger_Interactable : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public string targetSignalChannel; // Channel to send an activation or deactivation signal on
    
    public bool isPowered; // Flag indicating if the object is currently active


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private Logic_Processor logicProcessor;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        logicProcessor = GetComponent<Logic_Processor>();
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        var interaction = _other.GetComponent<Trigger_Interaction>();
        if (!interaction) return;
        Interact();
    }
    
    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    private void Interact()
    {
        if (targetSignalChannel == "") return;
        
        // Flip the current activation state
        isPowered = !isPowered;
        
        // Update connected devices
        logicProcessor.UpdateState(targetSignalChannel, isPowered);
    }
}
