//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableExposer_Transform : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public float scaleX, scaleY, positionOffsetX, positionOffsetY;


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

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
