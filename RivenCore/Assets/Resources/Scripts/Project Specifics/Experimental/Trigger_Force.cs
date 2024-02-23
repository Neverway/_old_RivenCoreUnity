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
    public float forceStrengthX;
    public float forceStrengthY;
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
    public List<Rigidbody2D> propsInTrigger = new List<Rigidbody2D>();


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
	    foreach (var prop in propsInTrigger)
	    {
		    if (prop) prop.AddForce(transform.right*forceStrength.x, ForceMode2D.Force);
		    if (prop) prop.AddForce(transform.up*forceStrength.y, ForceMode2D.Force);
	    }
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
	    // Exit if not a target, or activated & one time use
	    if (!_other.CompareTag("Entity") && !_other.CompareTag("PhysProp")) return;
	    // get the target entity
	    var targetEnt = _other.gameObject.transform.parent.GetComponent<Entity>();
	    var targetProp = _other.gameObject.transform.parent.GetComponent<Rigidbody2D>();

	    if (targetEnt)
	    {
		    if (!entitiesInTrigger.Contains(targetEnt) && owningTeam != targetEnt.currentStats.team)
		    {
			    entitiesInTrigger.Add(targetEnt);
		    }
	    }

	    if(!propsInTrigger.Contains(targetProp))
	    {
		    propsInTrigger.Add(targetProp);
	    }
    }

    private void OnTriggerExit2D(Collider2D _other)
    {
	    // Exit if not a target, or activated & one time use
	    if (!_other.CompareTag("Entity") && !_other.CompareTag("PhysProp")) return;
	    // get the target entity
	    var targetEnt = _other.gameObject.transform.parent.GetComponent<Entity>();
	    var targetProp = _other.gameObject.transform.parent.GetComponent<Rigidbody2D>();
	    
	    // Exit if not able to get entity/prop component
	    if (!targetEnt && !targetProp) return;
	    // Remove the entity from the list if it's not already
	    if (targetEnt) if (entitiesInTrigger.Contains(targetEnt)) if (gameObject.activeInHierarchy) StartCoroutine(RemoveEntity(targetEnt));
	    if (targetProp) if (propsInTrigger.Contains(targetProp)) if (gameObject.activeInHierarchy) StartCoroutine(RemoveProp(targetProp));
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private IEnumerator RemoveEntity(Entity _targetEnt)
    {
	    yield return new WaitForSeconds(removeEffectDelay);
	    entitiesInTrigger.Remove(_targetEnt);
    }
    private IEnumerator RemoveProp(Rigidbody2D _targetProp)
    {
	    yield return new WaitForSeconds(removeEffectDelay);
	    propsInTrigger.Remove(_targetProp);
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}

