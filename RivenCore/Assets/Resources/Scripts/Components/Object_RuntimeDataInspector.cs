using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Object_RuntimeDataInspector : MonoBehaviour
{
    public List<MonoData> monoDataList;

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
    /// Inspects the exposed variables of the target scripts and populates the variable data list.
    /// </summary>
    public void Inspect()
    {
        if (monoDataList == null) return;
        
        foreach (var monoData in monoDataList)
        {
            if (monoData.targetScript == null || monoData.exposedVariables == null) continue;

            monoData.variableData = new List<VariableData>(); // Initialize variableData here

            foreach (var variableName in monoData.exposedVariables)
            {
                var data = new VariableData();
                Type scriptType = monoData.targetScript.GetType();
                FieldInfo field = scriptType.GetField(variableName);

                if (field == null) continue;

                data.name = variableName;
                data.type = field.FieldType.ToString();
                if (field.GetValue(monoData.targetScript) != null) data.value = field.GetValue(monoData.targetScript).ToString();
                monoData.variableData.Add(data);
            }
        }
    }

    /// <summary>
    /// Sends the variable data back to the target scripts, updating their variables based on the stored values.
    /// </summary>
    public void SendVariableDataToScript()
    {
        if (monoDataList == null) return;

        foreach (var monoData in monoDataList)
        {
            if (monoData.targetScript == null || monoData.variableData == null) continue;

            foreach (var variableData in monoData.variableData)
            {
                SetData(monoData.targetScript, variableData.name, variableData.value);
            }
        }
    }

    /// <summary>
    /// Sets the value of a specific variable in the target script.
    /// </summary>
    /// <param name="targetScript">The MonoBehaviour instance containing the variable to set.</param>
    /// <param name="variableName">The name of the variable to set.</param>
    /// <param name="value">The value to assign to the variable.</param>
    public void SetData(MonoBehaviour targetScript, string variableName, string value)
    {
        if (targetScript == null) return;

        Type scriptType = targetScript.GetType();
        FieldInfo field = scriptType.GetField(variableName);

        if (field == null) return;
        if (value == "" && field.FieldType != typeof(string)) return;

        object convertedValue = Convert.ChangeType(value, field.FieldType);
                
        field.SetValue(targetScript, convertedValue);
    }
}

public class MonoData
{
    public MonoBehaviour targetScript;
    public List<string> exposedVariables;
    public List<VariableData> variableData;
}
