//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Damage : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Tooltip("The amount of damage to deal to an entity within the trigger. (Value can be negative to heal)")]
    public float damageAmount;
    [Tooltip("The team that owns the trigger (for regular damage, they won't be hurt, for healing, other teams won't be healed. (Don't define a team for friendly fire))")]
    public string team;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    public List<Entity> entitiesInTrigger = new List<Entity>();


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Update()
    {
        foreach (var entity in entitiesInTrigger)
        {
            if (team == "")
            {
                entity.ModifyHealth(-damageAmount);
            }
            else
            {
                if (entity.currentStats.team == team && damageAmount < 0) entity.ModifyHealth(-damageAmount);
                else if (entity.currentStats.team != team && damageAmount > 0) entity.ModifyHealth(-damageAmount);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Entity")) return;
        var targetEnt = other.gameObject.transform.parent.GetComponent<Entity>();
        if(!entitiesInTrigger.Contains(targetEnt))
        {
            entitiesInTrigger.Add(targetEnt);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Entity")) return;
        var targetEnt = other.gameObject.transform.parent.GetComponent<Entity>();
        if(entitiesInTrigger.Contains(targetEnt))
        {
            entitiesInTrigger.Remove(targetEnt);
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
