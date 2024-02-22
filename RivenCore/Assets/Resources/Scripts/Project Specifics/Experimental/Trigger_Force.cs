//======== Neverway 2023 Project Script | Written by Arthur Aka Liz ===========
// 
// Type: Utility
// Purpose: Remove health from an entity (or add health if the value on
//	the trigger is negative)
// Applied to: A 2D damage trigger
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Trigger_Force : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public Vector2 forceStrength;
    public float removeEffectDelay;
    public string owningTeam;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private bool inTrigger;
    
    
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
		    if (entity) entity.GetComponent<Rigidbody2D>().AddForce(transform.right*forceStrength.x, ForceMode2D.Force);
		    if (entity) entity.GetComponent<Rigidbody2D>().AddForce(transform.up*forceStrength.y, ForceMode2D.Force);
	    }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
	    if (!other.CompareTag("Entity")) return;
	    var targetEnt = other.gameObject.transform.parent.GetComponent<Entity>();
	    if(!entitiesInTrigger.Contains(targetEnt) && owningTeam != targetEnt.currentStats.team)
	    {
		    entitiesInTrigger.Add(targetEnt);
	    }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
	    if (!other.CompareTag("Entity")) return;
	    var targetEnt = other.gameObject.transform.parent.GetComponent<Entity>();
	    if (!entitiesInTrigger.Contains(targetEnt)) return;
	    if (gameObject.activeInHierarchy) StartCoroutine(RemoveEntity(targetEnt));
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private IEnumerator RemoveEntity(Entity targetEnt)
    {
	    yield return new WaitForSeconds(removeEffectDelay);
	    entitiesInTrigger.Remove(targetEnt);
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}

