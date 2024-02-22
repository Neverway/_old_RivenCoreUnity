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
public class UI_Image_EntityPortrait : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public Entity targetEntity;
    public bool findPossessedEntity;
    public Sprite fallbackImage;


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
        if (targetEntity && targetEntity.GetComponent<SpriteRenderer>())
        {
            GetComponent<Image>().sprite = targetEntity.GetComponent<SpriteRenderer>().sprite;
        }
        else if (fallbackImage)
        {
            GetComponent<Image>().sprite = fallbackImage;
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