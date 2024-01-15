//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WB_LevelEditor_InventoryTile : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public int tileIndex;
    public Sprite tileSprite;


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
        transform.GetChild(0).GetComponent<Image>().sprite = tileSprite;
    }
    

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void SendTileToHotbar()
    {
        var LevelEditorWidget = FindObjectOfType<WB_LevelEditor>();
        LevelEditorWidget.hotbarTiles[LevelEditorWidget.selectedHotbarTile] = tileIndex;
    }
}
