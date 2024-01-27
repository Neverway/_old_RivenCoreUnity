//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Fake parallax scrolling on a tilemap for use with orthographic cameras
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Paralax : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [SerializeField] private Vector2 parallaxAmount;
    [SerializeField] private Vector2 parallaxLimit;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private Camera currentCamera;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        
    }

    private void Update()
    {
        currentCamera = FindObjectOfType<Camera>(false);
        Vector3 position = currentCamera.transform.position;
        Vector2 distance = new Vector2(position.x * parallaxAmount.x, position.y * parallaxAmount.y);

        // Clamp distance.x and distance.y within parallaxLimit
        distance.x = Mathf.Clamp(distance.x, -parallaxLimit.x, parallaxLimit.x);
        distance.y = Mathf.Clamp(distance.y, -parallaxLimit.y, parallaxLimit.y);

        Vector3 newPosition = new Vector3(distance.x, distance.y, transform.position.z);

        transform.position = newPosition;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
