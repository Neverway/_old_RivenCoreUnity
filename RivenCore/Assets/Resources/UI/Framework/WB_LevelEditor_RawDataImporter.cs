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

public class WB_LevelEditor_RawDataImporter : MonoBehaviour
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
    public void LoadData()
    {
        if(inputField.text == "" || !FindObjectOfType<System_LevelManager>()) return;
        FindObjectOfType<System_LevelManager>().LoadRawLevel(inputField.text);
    }
}
