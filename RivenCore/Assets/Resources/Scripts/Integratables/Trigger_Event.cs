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
    public List<Rigidbody2D> propsInTrigger = new List<Rigidbody2D>();


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
        if (!_other.CompareTag("Entity") && !_other.CompareTag("PhysProp") || wasActivated && !resetsAfterUse) return;
        // get the target entity/prop
        var targetEnt = _other.gameObject.transform.parent.GetComponent<Entity>();
        var targetProp = _other.gameObject.transform.parent.GetComponent<Rigidbody2D>();
        if (targetProp) print("Target Prop found");
        // Exit if not able to get entity/prop component
        if (!targetEnt && !targetProp) return;
        // Add the entity/prop to the list if it's not already
        if (targetEnt) if (!entitiesInTrigger.Contains(targetEnt)) entitiesInTrigger.Add(targetEnt);
        if (targetProp) if (!propsInTrigger.Contains(targetProp)) propsInTrigger.Add(targetProp);
        
        // Update the trigger state
        Interact();
    }
    
    private void OnTriggerExit2D(Collider2D _other)
    {
        // Exit if not a target, or activated & one time use
        if (!_other.CompareTag("Entity") && !_other.CompareTag("PhysProp") || wasActivated && !resetsAfterUse) return;
        // get the target entity
        var targetEnt = _other.gameObject.transform.parent.GetComponent<Entity>();
        var targetProp = _other.gameObject.transform.parent.GetComponent<Rigidbody2D>();
        // Exit if not able to get entity/prop component
        if (!targetEnt && !targetProp) return;
        // Remove the entity from the list if it's not already
        if (targetEnt) if (entitiesInTrigger.Contains(targetEnt)) entitiesInTrigger.Remove(targetEnt);
        if (targetProp) if (propsInTrigger.Contains(targetProp)) propsInTrigger.Remove(targetProp);
        
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
        
        print(entitiesInTrigger.Count);
        // Is an entity in the trigger?
        if (entitiesInTrigger.Count > 0 || propsInTrigger.Count > 0)
        {
            if (onOccupiedSignal != "") logicProcessor.UpdateState(onOccupiedSignal, true);
            if (onUnoccupiedSignal != "") logicProcessor.UpdateState(onUnoccupiedSignal, false);
        }
        // Have all entities left the trigger?
        if (entitiesInTrigger.Count == 0 && propsInTrigger.Count == 0)
        {
            if (onOccupiedSignal != "") logicProcessor.UpdateState(onOccupiedSignal, false);
            if (onUnoccupiedSignal != "") logicProcessor.UpdateState(onUnoccupiedSignal, true);
            if (onOccupiedSignal != "" || onUnoccupiedSignal != "") wasActivated = true;
        }
    }
}
