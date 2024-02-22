//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WB_LevelEditor_RawDataExporter : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public TMP_InputField inputField;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void PullData()
    {
        if(!FindObjectOfType<System_LevelManager>()) return;
        inputField.text = FindObjectOfType<System_LevelManager>().GetRawMapData();
    }
}
