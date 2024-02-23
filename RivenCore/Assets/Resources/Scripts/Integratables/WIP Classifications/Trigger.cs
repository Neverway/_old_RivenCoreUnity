//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Trigger : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public string owningTeam; // Which team owns the trigger
    public bool affectsOwnTeam; // If true, the trigger will only affect objects that are a part of the owning team


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [HideInInspector] public List<Entity> entitiesInTrigger = new List<Entity>();
    [HideInInspector] public List<Prop> propsInTrigger = new List<Prop>();
    [HideInInspector] public Entity targetEnt;
    [HideInInspector] public Prop targetProp;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    protected void OnTriggerEnter2D(Collider2D _other)
    {
        print("Parent-Enter");
        // An Entity has entered the trigger
        if (_other.CompareTag("Entity"))
        {
            print("Parent-Entity");
            // Get a reference to the entity component
            targetEnt = _other.gameObject.transform.parent.GetComponent<Entity>();
            // Exit if they are not on the effected team
            print(IsOnAffectedTeam(targetEnt));
            if (!IsOnAffectedTeam(targetEnt)) return;
            // Add the entity to the list if they are not already present
            if(!entitiesInTrigger.Contains(targetEnt)) { entitiesInTrigger.Add(targetEnt); }
        }
        
        // A physics prop has entered the trigger
        if (_other.CompareTag("PhysProp"))
        {
            print("Parent-Prop");
            // Get a reference to the entity component
            targetProp = _other.gameObject.transform.parent.GetComponent<Prop>();
            // Add the entity to the list if they are not already present
            if(!propsInTrigger.Contains(targetProp)) { propsInTrigger.Add(targetProp); }
        }
    }

    protected void OnTriggerExit2D(Collider2D _other)
    {
        print("Parent-Exit");
        // An Entity has exited the trigger
        if (_other.CompareTag("Entity"))
        {
            // Get a reference to the entity component
            targetEnt = _other.gameObject.transform.parent.GetComponent<Entity>();
            // Remove the entity to the list if they are not already absent
            if(entitiesInTrigger.Contains(targetEnt)) { entitiesInTrigger.Remove(targetEnt); }
        }
        
        // A physics prop has entered the trigger
        if (_other.CompareTag("PhysProp"))
        {
            // Get a reference to the entity component
            targetProp = _other.gameObject.transform.parent.GetComponent<Prop>();
            // Add the entity to the list if they are not already present
            if(propsInTrigger.Contains(targetProp)) { propsInTrigger.Remove(targetProp); }
        }
    }
    

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private bool IsOnAffectedTeam(Entity _targetEntity)
    {
        // If an owning team is specified
        if (owningTeam != "")
        {
            // If targeting team
            if (affectsOwnTeam)
            {
                // Return if target is a part of team
                return _targetEntity.currentStats.team == owningTeam;
            }
            // If targeting non-team
            // Return if target is not a part of team
            return _targetEntity.currentStats.team != owningTeam;
        }
        
        // Owning team wasn't specified, so result is that all are affected
        return true;
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    protected Entity GetPlayerInTrigger()
    {
        foreach (var entity in entitiesInTrigger)
        {
            if (entity.isPossessed) return entity;
        }
        return null;
    }
}
