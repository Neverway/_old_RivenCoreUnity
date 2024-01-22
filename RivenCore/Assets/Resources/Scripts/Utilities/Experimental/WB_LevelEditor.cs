//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Handles input and UI events for controlling level editor
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    private Tilemap currentTilemap;
    private int currentHotbarIndex;
    [SerializeField] private string[] hotbarTileID = new []{"","","","","","","","","",""};
    private bool inventoryOpen;
    private bool isTesting;
    private Entity editorPlayer;
    private Vector3 cursorPos;
    private int currentDepth;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private System_LevelManager levelManager;
    private Camera viewCamera;
    [SerializeField] private Button[] topbarButtons;
    [SerializeField] private Button[] sidebarButtons;
    [SerializeField] private Button[] hotbarButtons;
    [SerializeField] private GameObject inventory;
    [SerializeField] private Gamemode testingGamemode;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        levelManager = FindObjectOfType<System_LevelManager>();
        currentTilemap = levelManager.tilemaps[0];
        InitializeButtons();
    }

    private void Update()
    {
        viewCamera = FindObjectOfType<Camera>();
        //cursorPos = currentTilemap.WorldToCell(viewCamera.ScreenToWorldPoint(Input.mousePosition));
        cursorPos = viewCamera.ScreenToWorldPoint(Input.mousePosition);

        UserInput();
        UpdateHotbarImages();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void InitializeButtons()
    {
        topbarButtons[0].onClick.AddListener(() => { foreach (var tilemap in levelManager.tilemaps) tilemap.ClearAllTiles();
            for (int i = 0; i < levelManager.assetsRoot.transform.childCount; i++) { Destroy(levelManager.assetsRoot.transform.GetChild(i).gameObject); } });
        topbarButtons[1].onClick.AddListener(() => { levelManager.LevelFile("Load"); });
        topbarButtons[2].onClick.AddListener(() => { levelManager.LevelFile("Save"); });
        topbarButtons[3].onClick.AddListener(() => { StartTest(); });
        topbarButtons[4].onClick.AddListener(() => { StopTest(); });
        hotbarButtons[0].onClick.AddListener(() => { currentHotbarIndex = 0; });
        hotbarButtons[1].onClick.AddListener(() => { currentHotbarIndex = 1; });
        hotbarButtons[2].onClick.AddListener(() => { currentHotbarIndex = 2; });
        hotbarButtons[3].onClick.AddListener(() => { currentHotbarIndex = 3; });
        hotbarButtons[4].onClick.AddListener(() => { currentHotbarIndex = 4; });
        hotbarButtons[5].onClick.AddListener(() => { currentHotbarIndex = 5; });
        hotbarButtons[6].onClick.AddListener(() => { currentHotbarIndex = 6; });
        hotbarButtons[7].onClick.AddListener(() => { currentHotbarIndex = 7; });
        hotbarButtons[8].onClick.AddListener(() => { currentHotbarIndex = 8; });
        hotbarButtons[9].onClick.AddListener(() => { currentHotbarIndex = 9; });
        hotbarButtons[10].onClick.AddListener(() => { ToggleInventoryOpen(); });
    }
    
    private void UserInput()
    {
        // Place/Erase
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) Place();
        if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject()) { /*EraseTile(pos);*/ }
        
        // Save/Open
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S)) levelManager.LevelFile("Save");
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.O)) levelManager.LevelFile("Load");
        
        // Undo/Redo
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Z)) Redo();
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z)) Undo();
        
        // Tab toggle inventory
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //InitializeTileInventory();
            ToggleInventoryOpen();
        }
        
        // Scroll wheel hotbar
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            currentHotbarIndex = (currentHotbarIndex + hotbarTileID.Length - 1) % hotbarTileID.Length;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            currentHotbarIndex = (currentHotbarIndex + hotbarTileID.Length - -1) % hotbarTileID.Length;
        }
    }
    
    private void Place()
    {
        // Are we placing a tile?
        if (levelManager.GetTileFromMemory(hotbarTileID[currentHotbarIndex]))
        {
            TileBase tile = levelManager.GetTileFromMemory(hotbarTileID[currentHotbarIndex]);
            Vector3Int position = new Vector3Int((int)MathF.Floor(cursorPos.x), (int)MathF.Floor(cursorPos.y), 0);
            currentTilemap.SetTile(position, tile);
        }

        // Are we placing an asset?
        else if (levelManager.GetAssetFromMemory(hotbarTileID[currentHotbarIndex]))
        {
            
            // Destroy any asset already in the selected position
            for (int i = 0; i < levelManager.assetsRoot.transform.childCount; i++)
            {
                if (levelManager.assetsRoot.transform.GetChild(i).transform.position == new Vector3(MathF.Round(cursorPos.x),
                        MathF.Round(cursorPos.y), levelManager.assetsRoot.transform.GetChild(i).gameObject.transform.position.z+currentDepth))
                {
                    Destroy(levelManager.assetsRoot.transform.GetChild(i).gameObject);
                }
            }

            // Place the current asset at the selected position
            var asset = levelManager.GetAssetFromMemory(hotbarTileID[currentHotbarIndex]);
            // We are using the current depth + the asset prefabs depth so that certain assets (like lights and entities) can use the z position as an offset
            var assetRef = Instantiate(asset,
                new Vector3(MathF.Round(cursorPos.x), MathF.Round(cursorPos.y), asset.transform.position.z+currentDepth), 
                new Quaternion(0, 0, 0, 0), levelManager.assetsRoot.transform);
            assetRef.name = assetRef.name.Replace("(Clone)","").Trim();
        }
    }
    
    private void Undo()
    {
    }

    private void Redo()
    {
    }

    private void UpdateHotbarImages()
    {
        for (var i = 0; i < hotbarButtons.Length-1; i++)
        {
            // Set sprite for each hotbar tile
            var hotbarPreview = hotbarButtons[i].transform.GetChild(0).GetComponent<Image>();
            if (levelManager.GetTileFromMemory(hotbarTileID[i]))
            {
                hotbarPreview.enabled = true;
                hotbarPreview.sprite = levelManager.GetTileFromMemory(hotbarTileID[i]).sprite;
            }
            else if (levelManager.GetAssetFromMemory(hotbarTileID[i]))
            {
                hotbarPreview.enabled = true;
                hotbarPreview.sprite = levelManager.GetAssetFromMemory(hotbarTileID[i]).GetComponent<SpriteRenderer>().sprite;
            }
            else hotbarPreview.enabled = false;
            
            // Show hotbar selection indicator
            var hotbarSelection = hotbarButtons[i].transform.GetChild(1).gameObject;
            if (i != currentHotbarIndex) hotbarSelection.SetActive(false);
            else hotbarSelection.SetActive(true);
        }
    }
    
    private void ToggleInventoryOpen()
    {
        inventoryOpen = !inventoryOpen;
        inventory.SetActive(inventoryOpen);
    }
    
    private void StartTest()
    {
        if (!editorPlayer) editorPlayer = FindObjectOfType<GameInstance>().localPlayerCharacter;
        editorPlayer.gameObject.SetActive(false);
        topbarButtons[3].gameObject.SetActive(false);
        topbarButtons[4].gameObject.SetActive(true);
        FindObjectOfType<GameInstance>().CreateNewPlayerCharacter(testingGamemode, true, false);
    }

    private void StopTest()
    {
        Destroy(FindObjectOfType<GameInstance>().localPlayerCharacter.gameObject);
        FindObjectOfType<GameInstance>().localPlayerCharacter = editorPlayer;
        editorPlayer.gameObject.SetActive(true);
        topbarButtons[3].gameObject.SetActive(true);
        topbarButtons[4].gameObject.SetActive(false);
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    public void SetCurrentHotbarTile(string tileID)
    {
        hotbarTileID[currentHotbarIndex] = tileID;
    }
}
