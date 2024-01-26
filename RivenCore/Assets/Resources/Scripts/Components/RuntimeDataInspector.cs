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
    public void Inspect()
    {
        if (targetScript == null) return;
        
        variableData = new List<VariableData>();
        for (int i = 0; i < exposedVariables.Count; i++)
        {
            var data = new VariableData();
            Type scriptType = targetScript.GetType();
            FieldInfo field = scriptType.GetField(exposedVariables[i]); // Replace with the actual variable name

            if (field == null) continue;
            //print($"{exposedVariables[i]} : {field.FieldType} : {field.GetValue(targetScript)}");
            data.name = exposedVariables[i];
            data.type = field.FieldType.ToString();
            if (field.GetValue(targetScript) != null) data.value = field.GetValue(targetScript).ToString();
            variableData.Add(data);
        }
    }
    
    public void SetData(string _variableName, string _value)
    {
        for (int i = 0; i < exposedVariables.Count; i++)
        {
            if (exposedVariables[i] == _variableName)
            {
                Type scriptType = targetScript.GetType();
                FieldInfo field = scriptType.GetField(exposedVariables[i]); // Replace with the actual variable name

                if (field != null)
                {
                    // Convert the string back to the appropriate type
                    print($"{field.FieldType} {_value}");
                    if (variableData[i].type != "System.String" && _value == "") return;
                    object convertedValue = Convert.ChangeType(_value, field.FieldType);
                    
                    // Set the value of the field in the targetScript
                    field.SetValue(targetScript, convertedValue);
                }
                return;
            }
        }
    }
}

[SerializeField]
public class VariableData
{
    public string name;
    public string type;
    public string value;
}
