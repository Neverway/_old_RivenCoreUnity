//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class RuntimeDataInspector : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public MonoBehaviour targetScript;
    public List<string> exposedVariables;
    public List<string> assetData;


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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            var data = new List<string>();
            data.Add("200");
            data.Add("Howelfen");
            Write(data);
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void Inspect()
    {
        if (targetScript != null)
        {
            assetData = new List<string>();
            for (int i = 0; i < exposedVariables.Count; i++)
            {
                Type scriptType = targetScript.GetType();
                FieldInfo field = scriptType.GetField(exposedVariables[i]); // Replace with the actual variable name

                if (field != null)
                {
                    print($"{exposedVariables[i]} : {field.FieldType} : {field.GetValue(targetScript)}");
                    if (field.GetValue(targetScript) != null)
                    {
                        assetData.Add(field.GetValue(targetScript).ToString());
                    }
                    else assetData.Add("");
                }
            }
        }
    }

    public void Write(List<string> _assetData)
    {
        if (targetScript != null)
        {
            for (int i = 0; i < exposedVariables.Count; i++)
            {
                Type scriptType = targetScript.GetType();
                FieldInfo field = scriptType.GetField(exposedVariables[i]); // Replace with the actual variable name

                if (field != null)
                {
                    // Convert the string back to the appropriate type
                    object convertedValue = Convert.ChangeType(_assetData[i], field.FieldType);

                    // Set the value of the field in the targetScript
                    field.SetValue(targetScript, convertedValue);
                    print($"{exposedVariables[i]} : {field.FieldType} : {field.GetValue(targetScript)}");
                }
            }
        }
    }
    
}
