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
    
    public string onInteractChannel; // Channel for when the object is interacted with
    public string onActivatedChannel; // Channel for when the object is activated
    public string onDeactivatedChannel; // Channel for when the object is deactivated
    
    public bool isActive; // Flag indicating if the object is currently active


    //=-----------------=
    // Private Variables
    //=-----------------=
    private Vector2 positionOrigin;


    //=-----------------=
    // Reference Variables
    //=-----------------=


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
    
    private void OnTriggerEnter2D(Collider2D _other)
    {
        var interaction = _other.GetComponent<Trigger_Interaction>();
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
        // On Toggle
        foreach (var interactable in FindObjectsOfType<Interactable>())
        {
            if (interactable.onInteractChannel == onInteractChannel) interactable.OnInteract.Invoke();
            if (interactable.onActivatedChannel == onActivatedChannel && isActive) interactable.OnActivated.Invoke();
            if (interactable.onDeactivatedChannel == onDeactivatedChannel && !isActive) interactable.OnDeactivated.Invoke();
        }
        isActive = !isActive;
    }
}
