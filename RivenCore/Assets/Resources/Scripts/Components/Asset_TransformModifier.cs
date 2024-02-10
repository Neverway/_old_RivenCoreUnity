//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asset_TransformExposer : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public float scaleX = 1, scaleY = 1, positionX, positionY;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        scaleX = transform.localScale.x;
        scaleY = transform.localScale.y;
        positionX = transform.position.x;
        positionY = transform.position.y;
    }

    private void Update()
    {
        transform.localScale = new Vector3(scaleX, scaleY, 1);
        transform.position = new Vector2(positionX, positionY);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
