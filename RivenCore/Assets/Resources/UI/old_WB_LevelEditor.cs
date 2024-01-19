//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================
/*
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class old_WB_LevelEditor : MonoBehaviour
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

    private List<Tile> MasterTileIndex => old_LevelManager.instance.masterTileIndex;

    
    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private Button[] hotbarButtons;
    [SerializeField] private Button[] layerButtons;
    [SerializeField] private Image[] Img_Tile;
    [SerializeField] private GameObject inventory;
    [SerializeField] private TMP_Text debugText;
    [SerializeField] private GameObject inventoryRoot, inventoryTile, inventorySpacer;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        InitializeButtons();
    }

    private void Update()
    {
        UpdateHotbarTileImages();
        PlayerInput();
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=
    private Dictionary<string, int> buttonToTileIndex = new Dictionary<string, int>
    {
        { "Tile1", 0 }, { "Tile2", 1 }, { "Tile3", 2 }, { "Tile4", 3 },
        { "Tile5", 4 }, { "Tile6", 5 }, { "Tile7", 6 }, { "Tile8", 7 },
        { "Tile9", 8 }, { "Tile10", 9 }, { "TileInventory", -1 }
    };

    private void InitializeButtons()
    {
        for (int i = 0; i < hotbarButtons.Length; i++)
        {
            int index = i; // Capture the variable for the delegate
            InitializeButton(hotbarButtons[i], $"Tile{index + 1}", () => OnClick($"Tile{index + 1}"));
        }

        InitializeButton(hotbarButtons[hotbarButtons.Length - 1], "TileInventory", () => OnClick("TileInventory"));

        for (int i = 0; i < layerButtons.Length; i++)
        {
            InitializeButton(layerButtons[i], $"Layer{i + 1}{(i % 2 == 0 ? "Ground" : "Wall")}", () => OnClick($"Layer{i + 1}{(i % 2 == 0 ? "Ground" : "Wall")}"));
        }
    }

    private void InitializeButton(Button button, string buttonName, UnityEngine.Events.UnityAction action)
    {
        button.onClick.AddListener(action);
    }

    private void OnClick(string button)
    {
        HandleHotbarClick(button);
        HandleLayerClick(button);
    }

    private void HandleHotbarClick(string button)
    {
        print("Clicked");
        var lastSelectedHotbarTile = selectedHotbarTile;
        if (buttonToTileIndex.ContainsKey(button))
        {
            selectedHotbarTile = buttonToTileIndex[button];
            debugText.text = $"Hotbar: {selectedHotbarTile},\n Layer: {old_LevelManager.instance.GetComponent<old_LevelEditor>().currentTilemap.name}";
            if (selectedHotbarTile == -1)
            {
                InitializeTileInventory();
                ToggleInventoryOpen();
                selectedHotbarTile = lastSelectedHotbarTile;
            }
            old_LevelManager.instance.GetComponent<old_LevelEditor>().selectedTileIndex = hotbarTiles[selectedHotbarTile];
        }
    }   
    
    private void HandleLayerClick(string button)
    {
        var layerIndex = button.StartsWith("Layer") ? int.Parse(button.Substring(5, 1)) : -1;
        if (layerIndex <= 0) return;
        old_LevelManager.instance.GetComponent<old_LevelEditor>().currentTilemap = old_LevelManager.instance.tilemaps[layerIndex + 1];
        debugText.text = $"Hotbar: {selectedHotbarTile},\n Layer: {old_LevelManager.instance.GetComponent<old_LevelEditor>().currentTilemap.name}";
    }
    
    private void ToggleInventoryOpen()
    {
        inventoryOpen = !inventoryOpen;
        inventory.SetActive(inventoryOpen);
    }

    
    private void UpdateHotbarTileImages()
    {
        for (var i = 0; i < Img_Tile.Length; i++)
        {
            var isMasterTileIndexValid = MasterTileIndex[hotbarTiles[i]] != null;
            Img_Tile[i].GetComponent<Image>().enabled = isMasterTileIndexValid;
            Img_Tile[i].sprite = isMasterTileIndexValid ? MasterTileIndex[hotbarTiles[i]].sprite : null;
        }

        for (var i = 0; i < Img_Tile.Length; i++)
        {
            Img_Tile[i].transform.parent.GetChild(1).gameObject.SetActive(i == selectedHotbarTile);
        }
    }

    private void InitializeTileInventory()
    {
        // Clear inventory
        for (var i = 0; i < inventoryRoot.transform.childCount; i++)
        {
            Destroy(inventoryRoot.transform.GetChild(i).gameObject);
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
    }

    private void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InitializeTileInventory();
            ToggleInventoryOpen();
        }

        var scrollDirection = Mathf.RoundToInt(Input.GetAxis("Mouse ScrollWheel"));
        if (scrollDirection != 0)
        {
            selectedHotbarTile = (selectedHotbarTile + hotbarTiles.Length - scrollDirection) % hotbarTiles.Length;
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}*/
