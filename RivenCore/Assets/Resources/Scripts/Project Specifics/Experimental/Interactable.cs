//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Defines an interactable object in the game.
// Notes: This script should be attached to any object that can be interacted with by the player.
//
//=============================================================================

using System;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    // Channels for interacting with the object
    public string onInteractChannel; // Channel for when the object is interacted with
    public string onActivatedChannel; // Channel for when the object is activated
    public string onDeactivatedChannel; // Channel for when the object is deactivated
    
    public bool isActive; // Flag indicating if the object is currently active

    public UnityEvent OnInteract, OnActivated, OnDeactivated;

    private void Start()
    {
        // Initialize current active state
        if (isActive) OnActivated.Invoke();
        else OnDeactivated.Invoke();
    }
}