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

public class Trigger_Force : Trigger
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public Vector2 forceStrength;
    public float forceStrengthX;
    public float forceStrengthY;
    public float removeEffectDelay;


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
		    if (entity) entity.GetComponent<Rigidbody2D>().AddForce(transform.right*forceStrengthX, ForceMode2D.Force);
		    if (entity) entity.GetComponent<Rigidbody2D>().AddForce(transform.up*forceStrengthY, ForceMode2D.Force);
	    }
	    foreach (var prop in propsInTrigger)
	    {
		    if (prop) prop.GetComponent<Rigidbody2D>().AddForce(transform.right*forceStrengthX, ForceMode2D.Force);
		    if (prop) prop.GetComponent<Rigidbody2D>().AddForce(transform.up*forceStrengthY, ForceMode2D.Force);
	    }
    }
    private void OnTriggerExit2D(Collider2D _other)
    {
	    if (targetEnt)
	    {
		    if (gameObject.activeInHierarchy) StartCoroutine(RemoveEntity(targetEnt));
	    }
	    if (targetProp)
	    {
		    if (gameObject.activeInHierarchy) StartCoroutine(RemoveProp(targetProp));
	    }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private IEnumerator RemoveEntity(Entity _targetEnt)
    {
	    yield return new WaitForSeconds(removeEffectDelay);
	    entitiesInTrigger.Remove(_targetEnt);
    }
    private IEnumerator RemoveProp(Prop _targetProp)
    {
	    yield return new WaitForSeconds(removeEffectDelay);
	    propsInTrigger.Remove(_targetProp);
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}

