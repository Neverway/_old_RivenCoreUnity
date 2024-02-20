//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Logic_Processor))]
public class Trigger_Event : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public string onOccupiedSignal;
    public string onUnoccupiedSignal;
    public string resetSignal;
    public bool resetsAfterUse=true;
    //public bool detectsPlayersOnly;
    
    //public bool isPowered;
    public bool wasActivated;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private Logic_Processor logicProcessor;
    public List<Entity> entitiesInTrigger = new List<Entity>();


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        logicProcessor = GetComponent<Logic_Processor>();
    }
    private void OnTriggerEnter2D(Collider2D _other)
    {
        // Exit if not a target, or activated & one time use
        if (!_other.CompareTag("Entity") || wasActivated && !resetsAfterUse) return;
        // get the target entity
        var targetEnt = _other.gameObject.transform.parent.GetComponent<Entity>();
        // Exit if not able to get entity component
        if (!targetEnt) return;
        // Add the entity to the list if it's not already
        if (!entitiesInTrigger.Contains(targetEnt)) entitiesInTrigger.Add(targetEnt);
        
        // Update the trigger state
        Interact();
    }
    
    private void OnTriggerExit2D(Collider2D _other)
    {
        // Exit if not a target, or activated & one time use
        if (!_other.CompareTag("Entity") || wasActivated && !resetsAfterUse) return;
        // get the target entity
        var targetEnt = _other.gameObject.transform.parent.GetComponent<Entity>();
        // Exit if not able to get entity component
        if (!targetEnt) return;
        // Remove the entity from the list if it's not already
        if (entitiesInTrigger.Contains(targetEnt)) entitiesInTrigger.Remove(targetEnt);
        
        // Update the trigger state
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
        // Is an entity in the trigger?
        if (entitiesInTrigger.Capacity > 0)
        {
            logicProcessor.UpdateState(onOccupiedSignal, true);
            logicProcessor.UpdateState(onUnoccupiedSignal, false);
        }
        // Have all entities left the trigger?
        if (entitiesInTrigger.Capacity == 0)
        {
            logicProcessor.UpdateState(onOccupiedSignal, false);
            logicProcessor.UpdateState(onUnoccupiedSignal, true);
            wasActivated = true;
        }
    }
}
