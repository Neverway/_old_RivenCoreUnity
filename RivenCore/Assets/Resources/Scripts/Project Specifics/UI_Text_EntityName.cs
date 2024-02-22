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

[RequireComponent(typeof(TMP_Text))]
public class UI_Text_EntityName : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public Entity targetEntity;
    public bool findPossessedEntity;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Update()
    {
        if (findPossessedEntity)
        {
            targetEntity = FindPossessedEntity();
        }
        if (targetEntity)
        {
            GetComponent<TMP_Text>().text = targetEntity.characterStats.stats.characterName;
        }
        else
        {
            GetComponent<TMP_Text>().text = "---";
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private Entity FindPossessedEntity()
    {
        foreach (var entity in FindObjectsByType<Entity>(FindObjectsSortMode.None))
        {
            if (entity.isPossessed) return entity;
        }
        return null;
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
