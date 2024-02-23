//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Logic_Processor))]
public class Trigger_Event : Trigger
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public string onOccupiedSignal;
    public string onUnoccupiedSignal;
    public string resetSignal;
    public bool resetsAutomatically = true;
    public bool checksOnlyForPlayer = true;
    public UnityEvent onOccupied;
    public UnityEvent onUnoccupied;


    //=-----------------=
    // Private Variables
    //=-----------------=
    [HideInInspector] public bool hasBeenTriggered;


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
        base.OnTriggerEnter2D(_other); // Call the base class method
        if (!resetsAutomatically && hasBeenTriggered) return;
        if (checksOnlyForPlayer && targetEnt)
        {
            if (!targetEnt.isPossessed) return;
            logicProcessor.UpdateState(onOccupiedSignal, true);
            logicProcessor.UpdateState(onUnoccupiedSignal, false);
            onOccupied.Invoke();
        }
        else if (targetEnt || targetProp)
        {
            logicProcessor.UpdateState(onOccupiedSignal, true);
            logicProcessor.UpdateState(onUnoccupiedSignal, false);
            onOccupied.Invoke();
        }
        hasBeenTriggered = true;
    }

    private void OnTriggerExit2D(Collider2D _other)
    {
        base.OnTriggerExit2D(_other); // Call the base class method
        if (checksOnlyForPlayer && targetEnt)
        {
            if (!targetEnt.isPossessed) return;
            logicProcessor.UpdateState(onOccupiedSignal, false);
            logicProcessor.UpdateState(onUnoccupiedSignal, true);
            onUnoccupied.Invoke();
        }
        else if (entitiesInTrigger.Count == 0 && propsInTrigger.Count == 0)
        {
            logicProcessor.UpdateState(onOccupiedSignal, false);
            logicProcessor.UpdateState(onUnoccupiedSignal, true);
            onUnoccupied.Invoke();
        }
        if (resetsAutomatically) hasBeenTriggered = false;
    }
    
    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
