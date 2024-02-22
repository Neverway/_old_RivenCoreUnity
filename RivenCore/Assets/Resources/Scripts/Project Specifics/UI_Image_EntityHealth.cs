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
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UI_Image_EntityHealth : MonoBehaviour
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
            GetComponent<Image>().fillAmount = (targetEntity.currentStats.health/targetEntity.characterStats.stats.health)*100*0.01f;
        }
        else
        {
            GetComponent<Image>().fillAmount = 0;
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