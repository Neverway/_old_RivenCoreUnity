//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Logic_Processor))]
public class Trigger_Interactable : Trigger
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public string onInteractSignal;
    public bool isPowered;
    public string resetSignal;
    public bool resetsAutomatically = true;
    public bool hideIndicator;
    public bool useTalkIndicator;
    public UnityEvent onInteract;


    //=-----------------=
    // Private Variables
    //=-----------------=
    [HideInInspector] public bool hasBeenTriggered;
    private bool previousIsPoweredState; // Used to check for any overrides to the initial isPowered state in the level editor
    
    
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
        // Check for interaction
        var interaction = _other.GetComponent<Trigger_Interaction>();
        if (interaction) Interact();
    }

    private void Update()
    {
        CheckForPoweredStateOverrides();
        SetIndicatorVisibility();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void CheckForPoweredStateOverrides()
    {
        // Check for any overrides that have modified the switch state
        if (previousIsPoweredState != isPowered) logicProcessor.UpdateState(onInteractSignal, isPowered);
        previousIsPoweredState = isPowered;
    }

    private void SetIndicatorVisibility()
    {
        if (!interactionIndicator || hideIndicator) return;
        
        interactionIndicator.GetComponent<Animator>().Play(useTalkIndicator ? "talk" : "use");
        
        if (GetPlayerInTrigger())
        {
            GetPlayerInTrigger().isNearInteractable = true;
            interactionIndicator.SetActive(true);
        }
        else
        {
            GetPlayerInTrigger().isNearInteractable = false;
            interactionIndicator.SetActive(false);
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    private void Interact()
    {
        if (onInteractSignal == "" || hasBeenTriggered && !resetsAutomatically) return;
        onInteract.Invoke();
        
        // Flip the current activation state
        isPowered = !isPowered;
        previousIsPoweredState = isPowered;
        
        // Update connected devices
        logicProcessor.UpdateState(onInteractSignal, isPowered);
        hasBeenTriggered = true;
    }
}
