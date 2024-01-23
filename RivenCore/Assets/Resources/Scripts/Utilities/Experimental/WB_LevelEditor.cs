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
    private string currentTool = "paint";
    private int currentPaintMode = 3;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private System_LevelManager levelManager;
    private Camera viewCamera;
    [SerializeField] private Button[] topbarButtons;
    [SerializeField] private Button Bttn_Inspect, Bttn_Paint;
    [SerializeField] private Sprite[] paintModeSprites;
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
        UpdateToolImages();
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
        Bttn_Inspect.onClick.AddListener(() => { currentTool = "inspect"; });
        Bttn_Paint.onClick.AddListener(() => { currentTool = "paint"; });
        hotbarButtons[0].onClick.AddListener(() => { currentHotbarIndex = 0; CheckPaintMode(); });
        hotbarButtons[1].onClick.AddListener(() => { currentHotbarIndex = 1; CheckPaintMode(); });
        hotbarButtons[2].onClick.AddListener(() => { currentHotbarIndex = 2; CheckPaintMode(); });
        hotbarButtons[3].onClick.AddListener(() => { currentHotbarIndex = 3; CheckPaintMode(); });
        hotbarButtons[4].onClick.AddListener(() => { currentHotbarIndex = 4; CheckPaintMode(); });
        hotbarButtons[5].onClick.AddListener(() => { currentHotbarIndex = 5; CheckPaintMode(); });
        hotbarButtons[6].onClick.AddListener(() => { currentHotbarIndex = 6; CheckPaintMode(); });
        hotbarButtons[7].onClick.AddListener(() => { currentHotbarIndex = 7; CheckPaintMode(); });
        hotbarButtons[8].onClick.AddListener(() => { currentHotbarIndex = 8; CheckPaintMode(); });
        hotbarButtons[9].onClick.AddListener(() => { currentHotbarIndex = 9; CheckPaintMode(); });
        hotbarButtons[10].onClick.AddListener(() => { ToggleInventoryOpen(); });
    }
    
    private void UserInput()
    {
        // Place/Erase
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (currentTool=="paint") Place();
        }

        if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (currentTool=="paint") Erase();
        }
        
        // Save/Open
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S)) levelManager.LevelFile("Save");
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.O)) levelManager.LevelFile("Load");
        
        // Undo/Redo
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Z)) Redo();
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z)) Undo();
        
        // Tab toggle inventory
        if (Input.GetKeyDown(KeyCode.Tab)) ToggleInventoryOpen();
        
        // Scroll wheel hotbar
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            currentHotbarIndex = (currentHotbarIndex + hotbarTileID.Length - 1) % hotbarTileID.Length;
            CheckPaintMode();
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            currentHotbarIndex = (currentHotbarIndex + hotbarTileID.Length - -1) % hotbarTileID.Length;
            CheckPaintMode();
        }
    }
    
    private void Place()
    {
        switch (currentPaintMode)
        {
            // Are we placing a tile?
            case 0:
            case 2:
            {
                TileBase tile = levelManager.GetTileFromMemory(hotbarTileID[currentHotbarIndex]);
                Vector3Int position = new Vector3Int((int)MathF.Floor(cursorPos.x), (int)MathF.Floor(cursorPos.y), 0);
                currentTilemap.SetTile(position, tile);
                break;
            }
            // Are we placing an asset?
            case 1:
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
                break;
            }
        }
    }
    
    private void Erase()
    {
        switch (currentPaintMode)
        {
            // Are we placing a tile?
            case 0:
            case 2:
            {
                Vector3Int position = new Vector3Int((int)MathF.Floor(cursorPos.x), (int)MathF.Floor(cursorPos.y), 0);
                currentTilemap.SetTile(position, null);
                break;
            }
            // Are we placing an asset?
            case 1:
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

                break;
            }
        }
    }
    
    private void Undo()
    {
    }

    private void Redo()
    {
    }

    private void CheckPaintMode()
    {
        if (levelManager.GetTileFromMemory(hotbarTileID[currentHotbarIndex]) && !hotbarTileID[currentHotbarIndex].Contains("Collision")) currentPaintMode = 0;
        else if (levelManager.GetAssetFromMemory(hotbarTileID[currentHotbarIndex])) currentPaintMode = 1;
        else if (levelManager.GetTileFromMemory(hotbarTileID[currentHotbarIndex]) && hotbarTileID[currentHotbarIndex].Contains("Collision")) currentPaintMode = 2;
        else currentPaintMode = 3; // This is the ID for a missing/null paint mode
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

    private void UpdateToolImages()
    {
        var paintImage = Bttn_Paint.transform.GetChild(0).GetComponent<Image>();
        var inspectImage = Bttn_Inspect.transform.GetChild(0).GetComponent<Image>();
        switch (currentTool)
        {
            case "paint":
                paintImage.color = new Color(1, 1, 1, 1);
                inspectImage.color = new Color(1, 1, 1, 0.25f);
                break;
            case "inspect":
                paintImage.color = new Color(1, 1, 1, 0.25f);
                inspectImage.color = new Color(1, 1, 1, 1);
                break;
        }
        paintImage.sprite = paintModeSprites[currentPaintMode];
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
