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

public class Trigger_Interactable : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public float scaleX = 1, scaleY = 1, positionOffsetX, positionOffsetY;
    public string onToggled, onActivated, onDeactivated;
    private bool isActive;


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
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        var interaction = other.GetComponent<Trigger_Interaction>();
        if (!interaction) return;
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
        //OnInteract.Invoke();
        isActive = !isActive;
        switch (isActive)
        {
            case true:
                //OnToggled.Invoke();
                break;
            case false:
                //OnUntoggled.Invoke();
                break;
        }
    }
}
