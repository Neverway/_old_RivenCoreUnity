//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class VariableExposer_Light : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public float intensity;
    public float range;
    public float colorRed, colorGreen, colorBlue;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private Light targetLight;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        targetLight = GetComponent<Light>();
        intensity = targetLight.intensity;
        range = targetLight.range;
        colorRed = targetLight.color.r;
        colorGreen = targetLight.color.g;
        colorBlue = targetLight.color.b;
    }

    private void Update()
    {
        targetLight.intensity = intensity;
        targetLight.range = range;
        targetLight.color = new Color(colorRed, colorGreen, colorBlue);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
