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
using UnityEngine.Events;

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
    public bool useTalkIndicator;
    public UnityEvent OnInteract;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private bool previousIsPoweredState;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private Logic_Processor logicProcessor;
    [SerializeField] private GameObject interactionIndicator;


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
        if (interaction) { Interact(); return; }
        
        // Show indicator for player
        if (!_other.CompareTag("Entity")) return;
        var entity = _other.transform.parent.GetComponent<Entity>();
        if (entity.isPossessed)
        {
            entity.isNearInteractable = true;
            interactionIndicator.SetActive(true);
            interactionIndicator.GetComponent<Animator>().Play(useTalkIndicator ? "talk" : "use");
        }
    }

    private void OnTriggerExit2D(Collider2D _other)
    {
        // Hide indicator for player
        if (!_other.CompareTag("Entity")) return;
        var entity = _other.transform.parent.GetComponent<Entity>();
        if (entity.isPossessed)
        {
            entity.isNearInteractable = false;
            interactionIndicator.SetActive(false);
        }
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
        OnInteract.Invoke();
        
        // Flip the current activation state
        isPowered = !isPowered;
        previousIsPoweredState = isPowered;
        
        // Update connected devices
        logicProcessor.UpdateState(onSwitchedSignal, isPowered);
        wasActivated = true;
    }
}
