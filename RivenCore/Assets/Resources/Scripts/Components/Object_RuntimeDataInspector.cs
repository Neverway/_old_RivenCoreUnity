//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Inspects and manipulates runtime data of a MonoBehaviour script.
// Notes: This script allows dynamically setting and getting variables of
// a specified MonoBehaviour. The VariableData class is located in 
//
//=============================================================================

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Object_RuntimeDataInspector : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public MonoBehaviour targetScript;
    public List<string> exposedVariables;
    public List<VariableData> variableData;


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
    /// <summary>
    /// Inspects the exposed variables of the target script and populates the variable data list.
    /// </summary>
    public void Inspect()
    {
        if (targetScript == null) return;
        
        variableData = new List<VariableData>();
        for (int i = 0; i < exposedVariables.Count; i++)
        {
            var data = new VariableData();
            Type scriptType = targetScript.GetType();
            FieldInfo field = scriptType.GetField(exposedVariables[i]);

            if (field == null) continue;

            data.name = exposedVariables[i];
            data.type = field.FieldType.ToString();
            if (field.GetValue(targetScript) != null) data.value = field.GetValue(targetScript).ToString();
            variableData.Add(data);
        }
    }

    /// <summary>
    /// Sends the variable data back to the target script, updating its variables based on the stored values.
    /// </summary>
    public void SendVariableDataToScript()
    {
        for (int i = 0; i < variableData.Count; i++)
        {
            SetData(variableData[i].name, variableData[i].value);
        }
    }
    
    /// <summary>
    /// Sets the value of a specific variable in the target script.
    /// </summary>
    /// <param name="_variableName">The name of the variable to set.</param>
    /// <param name="_value">The value to assign to the variable.</param>
    public void SetData(string _variableName, string _value)
    {
        for (int i = 0; i < exposedVariables.Count; i++)
        {
            if (exposedVariables[i] != _variableName) continue;
            Type scriptType = targetScript.GetType();
            FieldInfo field = scriptType.GetField(exposedVariables[i]); // Replace with the actual variable name

            if (field == null) return;
            // Convert the string back to the appropriate type
            if (variableData[i].type != "System.String" && _value == "") return;
            object convertedValue = Convert.ChangeType(_value, field.FieldType);
                
            // Set the value of the field in the targetScript
            field.SetValue(targetScript, convertedValue);
            return;
        }
    }
}
