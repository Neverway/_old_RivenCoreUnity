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
    [Tooltip("")]
    public string onSwitchedSignal;
    public string resetSignal;
    public bool resetsAfterUse=true;
    
    public bool isPowered;
    public bool wasActivated;


    //=-----------------=
    // Private Variables
    //=-----------------=
    public bool previousIsPoweredState;


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

    private void Update()
    {
        // Check for any overrides that have modified the switch state
        if (previousIsPoweredState != isPowered) logicProcessor.UpdateState(onSwitchedSignal, isPowered);
        previousIsPoweredState = isPowered;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    private void Interact()
    {
        if (onSwitchedSignal == "" || wasActivated && !resetsAfterUse) return;
        
        // Flip the current activation state
        isPowered = !isPowered;
        previousIsPoweredState = isPowered;
        
        // Update connected devices
        logicProcessor.UpdateState(onSwitchedSignal, isPowered);
        wasActivated = true;
    }
}
