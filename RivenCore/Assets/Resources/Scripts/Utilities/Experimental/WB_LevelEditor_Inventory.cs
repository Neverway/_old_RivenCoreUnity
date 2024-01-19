//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WB_LevelEditor_Inventory : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private System_LevelManager levelManager;
    [SerializeField] private GameObject inventoryBrowserRoot, inventoryTile, inventorySpacer;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        levelManager = FindObjectOfType<System_LevelManager>();
    }

    private void Update()
    {
    
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    /*
    private void InitializeTileInventory()
    {
        // Clear inventory
        for (var i = 0; i < inventoryBrowserRoot.transform.childCount; i++)
        {
            Destroy(inventoryBrowserRoot.transform.GetChild(i).gameObject);
        }

        foreach (var group in levelManager.tileMemory)
        {
            for (int i = 0; i < group.tiles; i++)
            {
                foreach (var tile in group.tiles)
                {
                }
                var asset = Instantiate(inventoryTile, inventoryBrowserRoot.transform);
                asset.GetComponent<WB_LevelEditor_InventoryTile>().tileID = tile.name;
                asset.GetComponent<WB_LevelEditor_InventoryTile>().tileSprite = tile.sprite;
            }
        }

        // Create and assign inventory tiles
        for (var i = 0; i < MasterTileIndex.Count; i++)
        {
            if (MasterTileIndex[i] != null)
            {
                var asset = Instantiate(inventoryTile, inventoryRoot.transform);
                asset.GetComponent<WB_LevelEditor_InventoryTile>().tileIndex = i;
                asset.GetComponent<WB_LevelEditor_InventoryTile>().tileSprite = MasterTileIndex[i].sprite;
            }
            else
            {
                var asset = Instantiate(inventorySpacer, inventoryRoot.transform);
            }
        }
    }*/


    //=-----------------=
    // External Functions
    //=-----------------=
}
