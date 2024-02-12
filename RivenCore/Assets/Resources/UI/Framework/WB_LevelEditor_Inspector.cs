//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WB_LevelEditor_Inspector : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    private Object_RuntimeDataInspector target;
    private List<GameObject> fields;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private GameObject stringField, intField, floatField, boolField, fieldListRoot;
    

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Update()
    {
        if (!target) return;
        for (int i = 0; i < fields.Count; i++)
        {
            switch (target.variableData[i].type)
            {
                case "System.String":
                    target.SetData(target.variableData[i].name, fields[i].transform.GetChild(1).GetComponent<TMP_InputField>().text);
                    break;
                case "System.Int32":
                    target.SetData(target.variableData[i].name, fields[i].transform.GetChild(1).GetComponent<TMP_InputField>().text);
                    break;
                case "System.Single":
                    target.SetData(target.variableData[i].name, fields[i].transform.GetChild(1).GetComponent<TMP_InputField>().text);
                    break;
                case "System.Boolean":
                    target.SetData(target.variableData[i].name, fields[i].transform.GetChild(1).GetComponent<Toggle>().isOn.ToString());
                    break;
            }
        }
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void InitializeInspector(Object_RuntimeDataInspector _data)
    {
        Clear();
        target = _data;
        target.Inspect();
        
        // Loop through variable data and create fields accordingly
        for (int i = 0; i < target.variableData.Count; i++)
        {
            
            GameObject field = null;
            
            switch (target.variableData[i].type)
            {
                case "System.String":
                    field = Instantiate(stringField, fieldListRoot.transform);
                    field.transform.GetChild(1).GetComponent<TMP_InputField>().text = target.variableData[i].value;
                    break;
                case "System.Int32":
                    field = Instantiate(intField, fieldListRoot.transform);
                    field.transform.GetChild(1).GetComponent<TMP_InputField>().text = target.variableData[i].value;
                    break;
                case "System.Single":
                    field = Instantiate(floatField, fieldListRoot.transform);
                    field.transform.GetChild(1).GetComponent<TMP_InputField>().text = target.variableData[i].value;
                    break;
                case "System.Boolean":
                    field = Instantiate(boolField, fieldListRoot.transform);
                    field.transform.GetChild(1).GetComponent<Toggle>().isOn = target.variableData[i].value == "True";
                    break;
            }
            
            // Set the field name and make it visible
            field.transform.GetChild(0).GetComponent<TMP_Text>().text = target.variableData[i].name;
            field.SetActive(true);
            fields.Add(field);
        }
    }

    public void Clear()
    {
        target = null;
        fields = new List<GameObject>();
        for (int i = 0; i < fieldListRoot.transform.childCount; i++)
        {
            Destroy(fieldListRoot.transform.GetChild(i).gameObject);
        }
    }
}
