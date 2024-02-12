//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Trigger_Damage : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public float scaleX = 1, scaleY = 1, positionOffsetX, positionOffsetY;
    [Tooltip("The amount of damage to deal to an entity within the trigger. (Value can be negative to heal)")]
    public float damageAmount;
    [Tooltip("The team that owns the trigger (for regular damage, they won't be hurt, for healing, other teams won't be healed. (Don't define a team for friendly fire))")]
    public string owningTeam;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private Vector2 positionOrigin;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [ReadOnly] public List<Entity> entitiesInTrigger = new List<Entity>();


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        positionOrigin = new Vector2(transform.position.x-positionOffsetX, transform.position.y-positionOffsetY);
    }
    private void Update()
    {
        transform.localScale = new Vector3(scaleX, scaleY, 1);
        transform.position = new Vector2(positionOrigin.x+positionOffsetX, positionOrigin.y+positionOffsetY);
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
