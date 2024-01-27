//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_InactiveViewDisabler : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private Entity entityComponent;
    private Camera cameraComponent;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        entityComponent = GetComponent<Entity>();
        cameraComponent = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        cameraComponent.enabled = entityComponent.isPossessed;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
