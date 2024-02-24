//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using UnityEngine;

public class Trigger_Damage : Trigger
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Tooltip("The amount of damage to deal to an entity within the trigger. (Value can be negative to heal)")]
    public float damageAmount;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Update()
    {
        foreach (var entity in entitiesInTrigger)
        {
            if (owningTeam == "")
            {
                entity.ModifyHealth(-damageAmount);
            }
            else
            {
                if (entity.currentStats.team == owningTeam && damageAmount < 0) entity.ModifyHealth(-damageAmount);
                else if (entity.currentStats.team != owningTeam && damageAmount > 0) entity.ModifyHealth(-damageAmount);
            }
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
