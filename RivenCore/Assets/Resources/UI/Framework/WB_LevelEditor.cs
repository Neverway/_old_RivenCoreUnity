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
    // Start/Stop test
    private Entity editorPlayer;
    private bool isTesting;
    
    // Tool mode
    private string currentTool = "paint";
    private int currentPaintMode = 3;
    
    // Tool cursor
    private Vector3 cursorPos;
    private float cursorOffset;
    private Vector2 viewZoomRange = new Vector2(2, 16);
    
    // Hotbar
    [SerializeField] private string[] hotBarTileID = { "", "", "", "", "", "", "", "", "", "" };
    private int currentHotBarIndex;
    
    // Layers
    private Tilemap currentTilemap;
    private int currentLayer;
    private int currentSublayer;
    private bool[] IsLayerVisible = new[] { true, true, true };
    private int sublayerCount;
    
    // Inventory
    private bool inventoryOpen;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private System_LevelManager levelManager;
    private Camera viewCamera;
    private WB_LevelEditor_Inspector inspector;
    [SerializeField] private Gamemode testingGamemode;
    [Header("Read-Only (Don't touch!)")]
    [SerializeField] private TMP_Text topbarTitle;
    [SerializeField] private Button[] fileButtons, viewButtons, helpButtons, filebarButtons, toolbarButtons, hotbarButtons, layerButtons, sublayerButtons, layerVisibilityButtons;
    [SerializeField] private Button clearHotbarButton, toggleInventoryButton;
    [SerializeField] private GameObject tileCursor, inspectionIndicator;
    [SerializeField] private Sprite[] paintToolSprites, visibilitySprites;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject[] layerGroups;
    


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        levelManager = FindObjectOfType<System_LevelManager>();
        viewCamera = FindObjectOfType<Camera>(false);
        inspector = FindObjectOfType<WB_LevelEditor_Inspector>(true);

        sublayerCount = sublayerButtons.Length;
        currentTilemap = levelManager.tilemaps[0];
        
        fileButtons[0].onClick.AddListener(NewMap);
        fileButtons[1].onClick.AddListener(() => { levelManager.ModifyLevelFile("Load"); });
        fileButtons[2].onClick.AddListener(SaveCurrentMap);
        fileButtons[3].onClick.AddListener(() => { levelManager.ModifyLevelFile("Save"); });
        fileButtons[4].onClick.AddListener(Application.Quit);
        
        helpButtons[1].onClick.AddListener(() => { Application.OpenURL("https://neverway.github.io/404.html"); });
        helpButtons[2].onClick.AddListener(() => { Application.OpenURL("https://neverway.github.io/404.html"); });
        
        filebarButtons[0].onClick.AddListener(() => { levelManager.ModifyLevelFile("Load"); });
        filebarButtons[1].onClick.AddListener(SaveCurrentMap);
        filebarButtons[2].onClick.AddListener(StartTest);
        filebarButtons[3].onClick.AddListener(StopTest);
        
        toolbarButtons[0].onClick.AddListener(() => { currentTool = "paint"; });
        toolbarButtons[1].onClick.AddListener(() => { currentTool = "inspect"; });
        
        for (var i = 0; i < hotbarButtons.Length; i++)
        {
            var layerIndex = i;
            hotbarButtons[i].onClick.AddListener(() => { currentHotBarIndex = layerIndex; });
        }
        clearHotbarButton.onClick.AddListener(() => { for (int i = 0; i < hotBarTileID.Length; i++) { hotBarTileID[i] = ""; } }); 
        toggleInventoryButton.onClick.AddListener(ToggleInventory); 
        
        for (var i = 0; i < layerButtons.Length; i++)
        {
            var index = i;
            layerButtons[i].onClick.AddListener(() => { currentLayer = index; });
        }
        for (var i = 0; i < sublayerButtons.Length; i++)
        {
            var index = i;
            sublayerButtons[i].onClick.AddListener(() => { currentSublayer = index; });
        }
        for (var i = 0; i < layerVisibilityButtons.Length; i++)
        {
            var index = i;
            layerVisibilityButtons[i].onClick.AddListener(() => { IsLayerVisible[index] = !IsLayerVisible[index]; });
        }
    }

    private void Update()
    {
        UpdateTopbarTitle();
        UpdateToolImages();
        UpdateHotBarImages();
        UpdateLayerSelection();
        UpdateLayerVisibility();
        UserInput();
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void NewMap()
    {
        // Clear the file path
        levelManager.filePath = "";
        // Clear all the tiles and assets
        foreach (var tilemap in levelManager.tilemaps) tilemap.ClearAllTiles();
        for (var i = 0; i < levelManager.assetsRoot.transform.childCount; i++)
            Destroy(levelManager.assetsRoot.transform.GetChild(i).gameObject);
    }

    private void SaveCurrentMap()
    {
        if (levelManager.filePath != "") levelManager.SaveLevel(levelManager.filePath);
        else levelManager.ModifyLevelFile("Save");
    }
    
    private void StartTest()
    {
        if (!editorPlayer) editorPlayer = FindObjectOfType<GameInstance>().localPlayerCharacter;
        isTesting = true;
        editorPlayer.gameObject.SetActive(false);
        filebarButtons[2].gameObject.SetActive(false);
        filebarButtons[3].gameObject.SetActive(true);
        FindObjectOfType<GameInstance>().CreateNewPlayerCharacter(testingGamemode, true, true);
    }
    
    private void StopTest()
    {
        isTesting = false;
        Destroy(FindObjectOfType<GameInstance>().localPlayerCharacter.gameObject);
        FindObjectOfType<GameInstance>().localPlayerCharacter = editorPlayer;
        editorPlayer.gameObject.SetActive(true);
        filebarButtons[2].gameObject.SetActive(true);
        filebarButtons[3].gameObject.SetActive(false);
    }

    private void UpdateTopbarTitle()
    {
        // Set the top-bar title
        if (levelManager.filePath != "")
        {
            var lastIndex = levelManager.filePath.LastIndexOfAny(new char[] { '/', '\\' });
            var fileName = levelManager.filePath.Substring(lastIndex + 1);
            topbarTitle.text = $"{fileName} - {Application.version}";
        }
        else
        {
            topbarTitle.text = $"*New Map - {Application.version}";
        }
    }

    private void UpdateToolImages()
    {
        var paintImage = toolbarButtons[0].transform.GetChild(0).GetComponent<Image>();
        var inspectImage = toolbarButtons[1].transform.GetChild(0).GetComponent<Image>();
        switch (currentTool)
        {
            case "paint":
                // Set tool icon colors
                paintImage.color = new Color(1, 1, 1, 1);
                inspectImage.color = new Color(1, 1, 1, 0.25f);
                
                // Set paint mode, cursor color, and offset
                if (levelManager.GetTileFromMemory(hotBarTileID[currentHotBarIndex]) && !hotBarTileID[currentHotBarIndex].Contains("Collision"))
                {
                    currentPaintMode = 0;
                    tileCursor.GetComponent<SpriteRenderer>().color = new Color(0.17f, 0.65f ,0.27f, 1f);
                    tileCursor.transform.position = new Vector3((float)MathF.Floor(cursorPos.x/1)*1+cursorOffset, (float)MathF.Floor(cursorPos.y/1)*1+cursorOffset, 0);
                    cursorOffset = 0.5f;
                }
                else if (levelManager.GetAssetFromMemory(hotBarTileID[currentHotBarIndex]))
                {
                    currentPaintMode = 1;
                    tileCursor.GetComponent<SpriteRenderer>().color = new Color(0.25f, 0.41f ,0.72f, 1f);
                    tileCursor.transform.position = new Vector3((float)MathF.Round(cursorPos.x/1)*1+cursorOffset, (float)MathF.Round(cursorPos.y/1)*1+cursorOffset, 0);
                    cursorOffset = 0f;
                }
                else if (levelManager.GetTileFromMemory(hotBarTileID[currentHotBarIndex]) && hotBarTileID[currentHotBarIndex].Contains("Collision"))
                {
                    currentPaintMode = 2;
                    tileCursor.GetComponent<SpriteRenderer>().color = new Color(0.55f, 0.22f ,0.22f, 1f);
                    tileCursor.transform.position = new Vector3((float)MathF.Floor(cursorPos.x/1)*1+cursorOffset, (float)MathF.Floor(cursorPos.y/1)*1+cursorOffset, 0);
                    cursorOffset = 0.5f;
                }
                else 
                {
                    currentPaintMode = 3; // This is the ID for a missing/null paint mode
                    tileCursor.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                    tileCursor.transform.position = new Vector3((float)MathF.Floor(cursorPos.x/1)*1+cursorOffset, (float)MathF.Floor(cursorPos.y/1)*1+cursorOffset, 0);
                    cursorOffset = 0.5f;
                }
                
                break;
            case "inspect":
                // Set tool icon colors
                paintImage.color = new Color(1, 1, 1, 0.25f);
                inspectImage.color = new Color(1, 1, 1, 1);
                
                // Set cursor color and offset
                tileCursor.GetComponent<SpriteRenderer>().color = new Color(0.25f, 0.41f ,0.72f, 1f);
                tileCursor.transform.position = new Vector3((float)MathF.Round(cursorPos.x/1)*1+cursorOffset, (float)MathF.Round(cursorPos.y/1)*1+cursorOffset, 0);
                cursorOffset = 0f;
                
                break;
        }

        paintImage.sprite = paintToolSprites[currentPaintMode];
    }

    private void UpdateHotBarImages()
    {
        for (var i = 0; i < hotbarButtons.Length; i++)
        {
            // Set sprite for each hotBar tile
            Image hotBarPreview = hotbarButtons[i].transform.GetChild(0).GetComponent<Image>();
            if (levelManager.GetTileFromMemory(hotBarTileID[i]))
            {
                hotBarPreview.enabled = true;
                hotBarPreview.sprite = levelManager.GetTileFromMemory(hotBarTileID[i]).sprite;
            }
            else if (levelManager.GetAssetFromMemory(hotBarTileID[i]))
            {
                hotBarPreview.enabled = true;
                hotBarPreview.sprite = levelManager.GetAssetFromMemory(hotBarTileID[i]).GetComponent<SpriteRenderer>().sprite;
            }
            else
            {
                hotBarPreview.enabled = false;
            }

            // Show hotBar selection indicator
            var hotBarSelection = hotbarButtons[i].transform.GetChild(1).gameObject;
            hotBarSelection.SetActive(i == currentHotBarIndex);
        }
    }

    private void UpdateLayerSelection()
    {
        if (currentTool == "inspect") { inspector.gameObject.SetActive(true); }
        else { inspector.gameObject.SetActive(false); }
        
        // Hide the menu if not in the painting tool mode
        if (currentTool != "paint") { layerButtons[0].transform.parent.parent.gameObject.SetActive(false); return; }
        
        // Show the menu if in the painting tool mode
        layerButtons[0].transform.parent.parent.gameObject.SetActive(true);


        // Set the layer text colors
        foreach (var button in layerButtons)
        {
            button.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(1, 1, 1, 0.15f);
        }
        layerButtons[currentLayer].transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(1, 1, 1, 1);
        
        // Set the sublayer text and active state
        //sublayerToggleButton.transform.parent.gameObject.SetActive(currentPaintMode == 0);
        foreach (var button in sublayerButtons)
        {
            button.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(1, 1, 1, 0.15f);
        }
        sublayerButtons[currentSublayer].transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(1, 1, 1, 1);
        
        // Set the paint mode
        switch (currentPaintMode)
        {
            case 0:
                // Select the ground layer for each tile map
                currentTilemap = levelManager.tilemaps[currentLayer*sublayerCount+currentSublayer];
                break;
            case 2:
                // Select the collision layer for each tile map
                currentTilemap = levelManager.tilemaps[currentLayer*sublayerCount+(sublayerCount-1)];
                break;
        }
    }

    private void UpdateLayerVisibility()
    {
        for (int i = 0; i < IsLayerVisible.Length; i++)
        {
            switch (IsLayerVisible[i])
            {
                case true:
                    layerVisibilityButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = visibilitySprites[0];
                    layerGroups[i].gameObject.SetActive(true);
                    break;
                case false:
                    layerVisibilityButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = visibilitySprites[1];
                    layerGroups[i].gameObject.SetActive(false);
                    break;
            }
        }
    }

    private void UserInput()
    {
        if (!viewCamera)
        {
            Debug.Log("The view camera could not be found! Attempting to re-assign view camera...");
            viewCamera = FindObjectOfType<Camera>();
        }
        
        cursorPos = viewCamera.ScreenToWorldPoint(Input.mousePosition);
        if (isTesting) return;
        
        // Place/Inspect/Erase/Pick
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (currentTool == "paint" && IsLayerVisible[currentLayer]) Place();
        }
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (currentTool == "inspect") Inspect();
        }

        if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject())
            if (currentTool == "paint" && IsLayerVisible[currentLayer])
                Erase();
        
        if (Input.GetMouseButton(2) && !EventSystem.current.IsPointerOverGameObject())
            if (currentTool == "paint" && IsLayerVisible[currentLayer])
                Pick();

        // Save/Open
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S)) SaveCurrentMap();
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.O)) levelManager.ModifyLevelFile("Load");

        // Undo/Redo
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Z)) Redo();
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z)) Undo();

        // Tab toggle inventory
        if (Input.GetKeyDown(KeyCode.Tab)) ToggleInventory();

        // Scroll wheel hotBar and zoom
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && !inventoryOpen)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (!(viewCamera.orthographicSize > viewZoomRange.x)) return;
                viewCamera.orthographicSize--;
            }
            else
            {
                currentHotBarIndex = (currentHotBarIndex + hotBarTileID.Length - 1) % hotBarTileID.Length;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && !inventoryOpen)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (!(viewCamera.orthographicSize < viewZoomRange.y)) return;
                viewCamera.orthographicSize++;
            }
            else
            {
                currentHotBarIndex = (currentHotBarIndex + hotBarTileID.Length - -1) % hotBarTileID.Length;
            }
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
                TileBase tile = levelManager.GetTileFromMemory(hotBarTileID[currentHotBarIndex]);
                var position = new Vector3Int((int)MathF.Floor(cursorPos.x), (int)MathF.Floor(cursorPos.y), 0);
                currentTilemap.SetTile(position, tile);
                break;
            }
            // Are we placing an asset?
            case 1:
            {
                // Destroy any asset already in the selected position
                for (var i = 0; i < levelManager.assetsRoot.transform.childCount; i++)
                    if (levelManager.assetsRoot.transform.GetChild(i).transform.position == new Vector3(
                            MathF.Round(cursorPos.x),
                            MathF.Round(cursorPos.y),
                            levelManager.assetsRoot.transform.GetChild(i).gameObject.transform.position.z))
                        Destroy(levelManager.assetsRoot.transform.GetChild(i).gameObject);

                // Place the current asset at the selected position
                var asset = levelManager.GetAssetFromMemory(hotBarTileID[currentHotBarIndex]);
                // We are using the current depth + the asset prefabs depth so that certain assets (like lights and entities) can use the z position as an offset
                var assetRef = Instantiate(asset,
                    new Vector3(MathF.Round(cursorPos.x), MathF.Round(cursorPos.y),
                        asset.transform.position.z + -currentLayer),
                    new Quaternion(0, 0, 0, 0), levelManager.assetsRoot.transform);
                assetRef.name = assetRef.name.Replace("(Clone)", "").Trim();
                break;
            }
        }
    }

    private void Inspect()
    {
        inspector.Clear();
        
        // Check for asset at cursor position
        for (var i = 0; i < levelManager.assetsRoot.transform.childCount; i++)
        {
            var currentAsset = levelManager.assetsRoot.transform.GetChild(i);
            if (currentAsset.transform.position == new Vector3(MathF.Round(cursorPos.x), MathF.Round(cursorPos.y), currentAsset.transform.position.z))
            {
                print($"Selected {currentAsset.name}");
                inspectionIndicator.SetActive(true);
                inspectionIndicator.transform.position = new Vector3(currentAsset.transform.position.x, currentAsset.transform.position.y);
                var assetData = currentAsset.gameObject.GetComponent<RuntimeDataInspector>();
                if (assetData)
                {
                    inspector.InitializeInspector(assetData);
                    print($"{currentAsset.name} has inspection data");
                }
                return;
            }
        }
        
        inspectionIndicator.SetActive(false);
    }

    private void Pick()
    {
        // Check for asset at cursor position
        for (var i = 0; i < levelManager.assetsRoot.transform.childCount; i++)
        {
            var currentAsset = levelManager.assetsRoot.transform.GetChild(i);
            if (currentAsset.transform.position == new Vector3(
                    MathF.Round(cursorPos.x), 
                    MathF.Round(cursorPos.y), 
                    currentAsset.transform.position.z))
            {
                SetCurrentHotBarTile(currentAsset.name);
                return;
            }
        }
        
        // Asset wasn't found so check for a tile at cursor position
        var position = new Vector3Int((int)MathF.Floor(cursorPos.x), (int)MathF.Floor(cursorPos.y), 0);
        if (!currentTilemap.GetTile(position)) return; // Exit if no tile was found
        SetCurrentHotBarTile(currentTilemap.GetTile(position).name);
    }

    private void Erase()
    {
        switch (currentPaintMode)
        {
            // Are we placing a tile?
            case 0:
            case 2:
            {
                var position = new Vector3Int((int)MathF.Floor(cursorPos.x), (int)MathF.Floor(cursorPos.y), 0);
                currentTilemap.SetTile(position, null);
                break;
            }
            // Are we placing an asset?
            case 1:
            {
                // Destroy any asset already in the selected position
                for (var i = 0; i < levelManager.assetsRoot.transform.childCount; i++)
                    if (levelManager.assetsRoot.transform.GetChild(i).transform.position == new Vector3(
                            MathF.Round(cursorPos.x),
                            MathF.Round(cursorPos.y),
                            levelManager.assetsRoot.transform.GetChild(i).gameObject.transform.position.z))
                        Destroy(levelManager.assetsRoot.transform.GetChild(i).gameObject);

                break;
            }
        }
    }

    private void Redo()
    {
        
    }

    private void Undo()
    {
        
    }

    private void ToggleInventory()
    {
        inventoryOpen = !inventoryOpen;
        inventory.SetActive(inventoryOpen);
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    public void SetCurrentHotBarTile(string _tileID)
    {
        hotBarTileID[currentHotBarIndex] = _tileID;
    }
}
