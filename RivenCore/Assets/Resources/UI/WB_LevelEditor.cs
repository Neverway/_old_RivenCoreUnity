//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class WB_LevelEditor : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    public int selectedHotbarTile;
    public bool inventoryOpen;
    public int[] hotbarTiles;
    private List<Tile> MasterTileIndex { get { return LevelManager.instance.masterTileIndex; } }


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private Button Bttn_Tile1, Bttn_Tile2, Bttn_Tile3, Bttn_Tile4, Bttn_Tile5, Bttn_Tile6, Bttn_Tile7, Bttn_Tile8, Bttn_Tile9, Bttn_Tile10, Bttn_TileInventory;
    [SerializeField] private Button Bttn_Layer1Ground, Bttn_Layer1Wall, Bttn_Layer2Ground, Bttn_Layer2Wall;
    [SerializeField] private Image[] Img_Tile;
    [SerializeField] private GameObject inventory;
    [SerializeField] private TMP_Text debugText;
    [SerializeField] private GameObject inventoryRoot, inventoryTile, inventorySpacer;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        InitializeButton(Bttn_Tile1, "Tile1");
        InitializeButton(Bttn_Tile2, "Tile2");
        InitializeButton(Bttn_Tile3, "Tile3");
        InitializeButton(Bttn_Tile4, "Tile4");
        InitializeButton(Bttn_Tile5, "Tile5");
        InitializeButton(Bttn_Tile6, "Tile6");
        InitializeButton(Bttn_Tile7, "Tile7");
        InitializeButton(Bttn_Tile8, "Tile8");
        InitializeButton(Bttn_Tile9, "Tile9");
        InitializeButton(Bttn_Tile10, "Tile10");
        InitializeButton(Bttn_TileInventory, "TileInventory");
        Bttn_Layer2Wall.onClick.AddListener(delegate { OnClick("Layer2Wall"); });
        Bttn_Layer2Ground.onClick.AddListener(delegate { OnClick("Layer2Ground"); });
        Bttn_Layer1Wall.onClick.AddListener(delegate { OnClick("Layer1Wall"); });
        Bttn_Layer1Ground.onClick.AddListener(delegate { OnClick("Layer1Ground"); });

    }

    private void InitializeButton(Button button, string buttonName)
    {
        button.onClick.AddListener(delegate { OnClick(buttonName); });
    }

    private void Update()
    {
        UpdateHotbarTileImages();
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=
    private Dictionary<string, int> buttonToTileIndex = new Dictionary<string, int>
    {
        { "Tile1", 0 },
        { "Tile2", 1 },
        { "Tile3", 2 },
        { "Tile4", 3 },
        { "Tile5", 4 },
        { "Tile6", 5 },
        { "Tile7", 6 },
        { "Tile8", 7 },
        { "Tile9", 8 },
        { "Tile10", 9 },
        { "TileInventory", -1 } // Let's use -1 for the inventory button
    };

    private void OnClick(string button)
    {
        var lastSelectedHotbarTile = selectedHotbarTile;
        if (buttonToTileIndex.ContainsKey(button))
        {
            selectedHotbarTile = buttonToTileIndex[button];
            if (selectedHotbarTile == -1)
            {
                InitializeTileInventory();
                ToggleInventoryOpen();
                selectedHotbarTile = lastSelectedHotbarTile;
            }
        }

        if (button == "Layer2Wall") LevelManager.instance.GetComponent<LevelEditor>().currentTilemap = LevelManager.instance.tilemaps[3];
        if (button == "Layer2Ground") LevelManager.instance.GetComponent<LevelEditor>().currentTilemap = LevelManager.instance.tilemaps[2];
        if (button == "Layer1Wall") LevelManager.instance.GetComponent<LevelEditor>().currentTilemap = LevelManager.instance.tilemaps[1];
        if (button == "Layer1Ground") LevelManager.instance.GetComponent<LevelEditor>().currentTilemap = LevelManager.instance.tilemaps[0];
        LevelManager.instance.GetComponent<LevelEditor>().selectedTileIndex = hotbarTiles[selectedHotbarTile];
        debugText.text = $"Hotbar: {selectedHotbarTile},\n Layer: {LevelManager.instance.GetComponent<LevelEditor>().currentTilemap.name}";
    }
    
    private void ToggleInventoryOpen()
    {
        inventoryOpen = !inventoryOpen;
        if (inventoryOpen) inventory.SetActive(true);
        else inventory.SetActive(false);
    }

    
    private void UpdateHotbarTileImages()
    {
        for (int i = 0; i < Img_Tile.Length; i++)
        {
            if (MasterTileIndex[hotbarTiles[i]])
            {
                Img_Tile[i].GetComponent<Image>().enabled = true;
                Img_Tile[i].sprite = MasterTileIndex[hotbarTiles[i]].sprite;
            }
            else
            {
                Img_Tile[i].GetComponent<Image>().enabled = false;
            }
        }

        for (int i = 0; i < Img_Tile.Length; i++)
        {
            Img_Tile[i].transform.parent.GetChild(1).gameObject.SetActive(false);
            Img_Tile[selectedHotbarTile].transform.parent.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void InitializeTileInventory()
    {
        // Clear inventory
        for (int i = 0; i < inventoryRoot.transform.childCount; i++)
        {
            Destroy(inventoryRoot.transform.GetChild(i).gameObject);
        }
        
        // Create and assign inventory tiles
        for (int i = 0; i < MasterTileIndex.Count; i++)
        {
            var asset = Instantiate(inventoryTile, inventoryRoot.transform);
            asset.GetComponent<WB_LevelEditor_InventoryTile>().tileIndex = i;
            asset.GetComponent<WB_LevelEditor_InventoryTile>().tileSprite = MasterTileIndex[i].sprite;
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
