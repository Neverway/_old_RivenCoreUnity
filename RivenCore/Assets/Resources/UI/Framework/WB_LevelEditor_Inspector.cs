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
    private RuntimeDataInspector targetAsset;
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
        if (!targetAsset) return;
        for (int i = 0; i < fields.Count; i++)
        {
            switch (targetAsset.variableData[i].type)
            {
                case "System.String":
                    targetAsset.SetData(targetAsset.variableData[i].name, fields[i].transform.GetChild(1).GetComponent<TMP_InputField>().text);
                    break;
                case "System.Int32":
                    targetAsset.SetData(targetAsset.variableData[i].name, fields[i].transform.GetChild(1).GetComponent<TMP_InputField>().text);
                    break;
                case "System.Single":
                    targetAsset.SetData(targetAsset.variableData[i].name, fields[i].transform.GetChild(1).GetComponent<TMP_InputField>().text);
                    break;
                case "System.Boolean":
                    targetAsset.SetData(targetAsset.variableData[i].name, fields[i].transform.GetChild(1).GetComponent<Toggle>().isOn.ToString());
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
    public void InitializeInspector(RuntimeDataInspector _assetData)
    {
        targetAsset = _assetData;
        targetAsset.Inspect();
        fields = new List<GameObject>();
        for (int i = 0; i < fieldListRoot.transform.childCount; i++)
        {
            Destroy(fieldListRoot.transform.GetChild(i).gameObject);
        }
        
        // Loop through variable data and create fields accordingly
        for (int i = 0; i < targetAsset.variableData.Count; i++)
        {
            
            GameObject field = null;
            
            switch (targetAsset.variableData[i].type)
            {
                case "System.String":
                    field = Instantiate(stringField, fieldListRoot.transform);
                    field.transform.GetChild(1).GetComponent<TMP_InputField>().text = targetAsset.variableData[i].value;
                    break;
                case "System.Int32":
                    field = Instantiate(intField, fieldListRoot.transform);
                    field.transform.GetChild(1).GetComponent<TMP_InputField>().text = targetAsset.variableData[i].value;
                    break;
                case "System.Single":
                    field = Instantiate(floatField, fieldListRoot.transform);
                    field.transform.GetChild(1).GetComponent<TMP_InputField>().text = targetAsset.variableData[i].value;
                    break;
                case "System.Boolean":
                    field = Instantiate(boolField, fieldListRoot.transform);
                    field.transform.GetChild(1).GetComponent<Toggle>().isOn = targetAsset.variableData[i].value == "True";
                    break;
            }
            
            // Set the field name and make it visible
            field.transform.GetChild(0).GetComponent<TMP_Text>().text = targetAsset.variableData[i].name;
            field.SetActive(true);
            fields.Add(field);
        }
    }
}
