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


    //=-----------------=
    // Private Variables
    //=-----------------=
    private Vector3 originalScale;
    private Vector3 parallaxScale;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private GameInstance gameInstance;
    private Camera currentCamera;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        gameInstance = FindObjectOfType<GameInstance>();
        // Calculate the new local scale based on the parallax amount
        originalScale = transform.localScale;
        parallaxScale = new Vector3(originalScale.x + (parallaxAmount.x/2), originalScale.y + (parallaxAmount.y/2), originalScale.z);
    }

    private void Update()
    {
        currentCamera = FindObjectOfType<Camera>(false);
        
        // Only set the scale to the parallax scale if the correct gamemode is active
        transform.localScale = gameInstance.GetCurrentGamemode().Contains("Topdown2D") ? parallaxScale : originalScale;
        if (!gameInstance.GetCurrentGamemode().Contains("Topdown2D"))
        {
            transform.position = new Vector3();
            return;
        }
        
        // Calculate the distance to move based on the parallax amount
        Vector3 position = currentCamera.transform.position;
        Vector2 distance = new Vector2(position.x * -parallaxAmount.x, position.y * -parallaxAmount.y);
        Vector3 newPosition = new Vector3(distance.x, distance.y, transform.position.z);

        // Set the new position
        transform.position = newPosition;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}