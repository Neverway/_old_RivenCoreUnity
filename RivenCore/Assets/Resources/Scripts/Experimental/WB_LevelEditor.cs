//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Handles input and UI events for controlling level editor
// Notes:
//
//=============================================================================

using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class WB_LevelEditor : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    private Vector2 viewZoomRange = new Vector2(2, 16);
    // File-bar
    private bool isTesting;
    private Entity editorPlayer;
    // Toolbar
    private Vector3 cursorPos;
    private Tilemap currentTilemap;
    private string currentTool = "paint";
    private int currentPaintMode = 3;
    // Layers
    private int currentLayer;
    private int currentSublayer;
    private bool[] IsLayerVisible = new[] { true, true, true };

    // Hot-bar
    private string[] hotBarTileID = { "", "", "", "", "", "", "", "", "", "" };
    private int currentHotBarIndex;
    // Inventory
    private bool inventoryOpen;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private System_LevelManager levelManager;
    private Camera viewCamera;
    // File-bar
    [SerializeField] private Gamemode testingGamemode;
    [SerializeField] private Button[] topBarButtons;
    // Toolbar
    [SerializeField] private Button Bttn_Inspect, Bttn_Paint;
    [SerializeField] private Sprite[] paintModeSprites;
    // Layers
    [SerializeField] private GameObject[] layerGroups;
    [SerializeField] private Button[] layerButtons;
    [SerializeField] private Button[] sublayerButtons;
    [SerializeField] private Button[] layerVisibilityButtons;
    [SerializeField] private Button[] sublayerVisibilityButtons;
    [SerializeField] private Button sublayerToggleButton;
    [SerializeField] private Sprite[] visibilitySprites;
    // Hot-bar
    [SerializeField] private Button[] hotBarButtons;
    [SerializeField] private Button clearHotBarButton;
    // Inventory
    [SerializeField] private Button inventoryToggleButton;
    [SerializeField] private GameObject inventory;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        levelManager = FindObjectOfType<System_LevelManager>();
        currentTilemap = levelManager.tilemaps[0];
        viewCamera = FindObjectOfType<Camera>();
        InitializeButtons();
    }

    private void Update()
    {
        UserInput();
        UpdateHotBarImages();
        UpdateToolImages();
        UpdateLayerSelection();
        UpdateLayerVisibility();
        CheckPaintMode();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void InitializeButtons()
    {
        // File-bar
        topBarButtons[0].onClick.AddListener(() =>
        {
            foreach (var tilemap in levelManager.tilemaps) tilemap.ClearAllTiles();
            for (var i = 0; i < levelManager.assetsRoot.transform.childCount; i++)
                Destroy(levelManager.assetsRoot.transform.GetChild(i).gameObject);
        });
        topBarButtons[1].onClick.AddListener(() => { levelManager.LevelFile("Load"); });
        topBarButtons[2].onClick.AddListener(() => { levelManager.LevelFile("Save"); });
        topBarButtons[3].onClick.AddListener(StartTest);
        topBarButtons[4].onClick.AddListener(StopTest);
        
        // Toolbar
        Bttn_Inspect.onClick.AddListener(() => { currentTool = "inspect"; });
        Bttn_Paint.onClick.AddListener(() => { currentTool = "paint"; });
        
        // Layers
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
        sublayerToggleButton.onClick.AddListener(ToggleSublayerMenu);
        for (var i = 0; i < layerVisibilityButtons.Length; i++)
        {
            var index = i;
            layerVisibilityButtons[i].onClick.AddListener(() => { IsLayerVisible[index] = !IsLayerVisible[index]; });
        }

        // Hot-bar
        for (var i = 0; i < hotBarButtons.Length; i++)
        {
            var layerIndex = i;
            hotBarButtons[i].onClick.AddListener(() => { currentHotBarIndex = layerIndex; });
        }
        clearHotBarButton.onClick.AddListener(() => { for (int i = 0; i < hotBarTileID.Length; i++) { hotBarTileID[i] = ""; } }); 
        
        // Inventory
        inventoryToggleButton.onClick.AddListener(ToggleInventoryOpen); 
    }

    private void UserInput()
    {
        if (!viewCamera)
        {
            Debug.LogError("The view camera could not be found! Attempting to re-assign view camera...");
            viewCamera = FindObjectOfType<Camera>();
        }
        
        cursorPos = viewCamera.ScreenToWorldPoint(Input.mousePosition);
        
        // Place/Erase
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            if (currentTool == "paint" && IsLayerVisible[currentLayer])
                Place();

        if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject())
            if (currentTool == "paint" && IsLayerVisible[currentLayer])
                Erase();
        
        if (Input.GetMouseButton(2) && !EventSystem.current.IsPointerOverGameObject())
            if (currentTool == "paint" && IsLayerVisible[currentLayer])
                Pick();

        // Save/Open
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S)) levelManager.LevelFile("Save");
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.O)) levelManager.LevelFile("Load");

        // Undo/Redo
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Z)) Redo();
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z)) Undo();

        // Tab toggle inventory
        if (Input.GetKeyDown(KeyCode.Tab)) ToggleInventoryOpen();

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

    private void UpdateHotBarImages()
    {
        for (var i = 0; i < hotBarButtons.Length; i++)
        {
            // Set sprite for each hotBar tile
            Image hotBarPreview = hotBarButtons[i].transform.GetChild(0).GetComponent<Image>();
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
            var hotBarSelection = hotBarButtons[i].transform.GetChild(1).gameObject;
            hotBarSelection.SetActive(i == currentHotBarIndex);
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

    private void UpdateLayerSelection()
    {
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
        sublayerToggleButton.transform.parent.gameObject.SetActive(currentPaintMode == 0);
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
                print(currentTilemap.name);
                currentTilemap = levelManager.tilemaps[currentLayer*5+currentSublayer];
                break;
            case 2:
                // Select the collision layer for each tile map
                print(currentTilemap.name);
                currentTilemap = levelManager.tilemaps[currentLayer*5+4];
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

    private void CheckPaintMode()
    {
        if (levelManager.GetTileFromMemory(hotBarTileID[currentHotBarIndex]) &&
            !hotBarTileID[currentHotBarIndex].Contains("Collision")) currentPaintMode = 0;
        else if (levelManager.GetAssetFromMemory(hotBarTileID[currentHotBarIndex])) currentPaintMode = 1;
        else if (levelManager.GetTileFromMemory(hotBarTileID[currentHotBarIndex]) &&
                 hotBarTileID[currentHotBarIndex].Contains("Collision")) currentPaintMode = 2;
        else currentPaintMode = 3; // This is the ID for a missing/null paint mode
    }

    private void ToggleInventoryOpen()
    {
        inventoryOpen = !inventoryOpen;
        inventory.SetActive(inventoryOpen);
    }
    private void ToggleSublayerMenu()
    {
        var sublayerMenu = sublayerToggleButton.transform.parent.GetChild(1).gameObject;
        sublayerMenu.SetActive(!sublayerMenu.activeInHierarchy);
    }
    private void StartTest()
    {
        if (!editorPlayer) editorPlayer = FindObjectOfType<GameInstance>().localPlayerCharacter;
        editorPlayer.gameObject.SetActive(false);
        topBarButtons[3].gameObject.SetActive(false);
        topBarButtons[4].gameObject.SetActive(true);
        FindObjectOfType<GameInstance>().CreateNewPlayerCharacter(testingGamemode, true, true);
    }
    private void StopTest()
    {
        Destroy(FindObjectOfType<GameInstance>().localPlayerCharacter.gameObject);
        FindObjectOfType<GameInstance>().localPlayerCharacter = editorPlayer;
        editorPlayer.gameObject.SetActive(true);
        topBarButtons[3].gameObject.SetActive(true);
        topBarButtons[4].gameObject.SetActive(false);
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
    private static void Undo()
    {
    }
    private static void Redo()
    {
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    public void SetCurrentHotBarTile(string _tileID)
    {
        hotBarTileID[currentHotBarIndex] = _tileID;
    }
}