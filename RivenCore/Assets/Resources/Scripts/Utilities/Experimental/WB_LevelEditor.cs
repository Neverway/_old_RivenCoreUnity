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
    private int currentHotBarIndex;
    [SerializeField] private string[] hotBarTileID = { "", "", "", "", "", "", "", "", "", "" };
    private bool inventoryOpen;
    private bool isTesting;
    private Entity editorPlayer;
    private Vector3 cursorPos;
    private int currentDepth; // NOT IN USE, POTENTIALLY FOR Z OFFSET
    private string currentTool = "paint";
    private int currentPaintMode = 3;
    private int currentLayer;
    private int currentSublayer;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private System_LevelManager levelManager;
    private Camera viewCamera;
    [SerializeField] private Button[] topBarButtons;
    [SerializeField] private Button Bttn_Inspect, Bttn_Paint;
    [SerializeField] private Sprite[] paintModeSprites;
    [SerializeField] private Button[] layerButtons;
    [SerializeField] private Button[] sublayerButtons;
    [SerializeField] private Button sublayerToggleButton;
    [SerializeField] private Button[] hotBarButtons;
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
        cursorPos = viewCamera.ScreenToWorldPoint(Input.mousePosition);

        UserInput();
        UpdateHotBarImages();
        UpdateToolImages();
        UpdateLayerSelection();
        CheckPaintMode();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void InitializeButtons()
    {
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
        Bttn_Inspect.onClick.AddListener(() => { currentTool = "inspect"; });
        Bttn_Paint.onClick.AddListener(() => { currentTool = "paint"; });
        layerButtons[0].onClick.AddListener(() => { currentLayer = 0;});
        layerButtons[1].onClick.AddListener(() => { currentLayer = 1;});
        layerButtons[2].onClick.AddListener(() => { currentLayer = 2;});
        sublayerButtons[0].onClick.AddListener(() => { currentSublayer = 0;});
        sublayerButtons[1].onClick.AddListener(() => { currentSublayer = 1;});
        sublayerButtons[2].onClick.AddListener(() => { currentSublayer = 2;});
        sublayerButtons[3].onClick.AddListener(() => { currentSublayer = 3;});
        sublayerToggleButton.onClick.AddListener(ToggleSublayerMenu);
        hotBarButtons[0].onClick.AddListener(() => { currentHotBarIndex = 0; });
        hotBarButtons[1].onClick.AddListener(() => { currentHotBarIndex = 1; });
        hotBarButtons[2].onClick.AddListener(() => { currentHotBarIndex = 2; });
        hotBarButtons[3].onClick.AddListener(() => { currentHotBarIndex = 3; });
        hotBarButtons[4].onClick.AddListener(() => { currentHotBarIndex = 4; });
        hotBarButtons[5].onClick.AddListener(() => { currentHotBarIndex = 5; });
        hotBarButtons[6].onClick.AddListener(() => { currentHotBarIndex = 6; });
        hotBarButtons[7].onClick.AddListener(() => { currentHotBarIndex = 7; });
        hotBarButtons[8].onClick.AddListener(() => { currentHotBarIndex = 8; });
        hotBarButtons[9].onClick.AddListener(() => { currentHotBarIndex = 9; });
        hotBarButtons[10].onClick.AddListener(ToggleInventoryOpen); 
    }

    private void UserInput()
    {
        // Place/Erase
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            if (currentTool == "paint")
                Place();

        if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject())
            if (currentTool == "paint")
                Erase();

        // Save/Open
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S)) levelManager.LevelFile("Save");
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.O)) levelManager.LevelFile("Load");

        // Undo/Redo
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Z)) Redo();
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z)) Undo();

        // Tab toggle inventory
        if (Input.GetKeyDown(KeyCode.Tab)) ToggleInventoryOpen();

        // Scroll wheel hotBar
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            currentHotBarIndex = (currentHotBarIndex + hotBarTileID.Length - 1) % hotBarTileID.Length;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            currentHotBarIndex = (currentHotBarIndex + hotBarTileID.Length - -1) % hotBarTileID.Length;
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
                            levelManager.assetsRoot.transform.GetChild(i).gameObject.transform.position.z +
                            currentDepth))
                        Destroy(levelManager.assetsRoot.transform.GetChild(i).gameObject);

                break;
            }
        }
    }

    private static void Undo()
    {
    }

    private static void Redo()
    {
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

    private void UpdateHotBarImages()
    {
        for (var i = 0; i < hotBarButtons.Length - 1; i++)
        {
            // Set sprite for each hotBar tile
            var hotBarPreview = hotBarButtons[i].transform.GetChild(0).GetComponent<Image>();
            if (levelManager.GetTileFromMemory(hotBarTileID[i]))
            {
                hotBarPreview.enabled = true;
                hotBarPreview.sprite = levelManager.GetTileFromMemory(hotBarTileID[i]).sprite;
            }
            else if (levelManager.GetAssetFromMemory(hotBarTileID[i]))
            {
                hotBarPreview.enabled = true;
                hotBarPreview.sprite = levelManager.GetAssetFromMemory(hotBarTileID[i]).GetComponent<SpriteRenderer>()
                    .sprite;
            }
            else
            {
                hotBarPreview.enabled = false;
            }

            // Show hotBar selection indicator
            var hotBarSelection = hotBarButtons[i].transform.GetChild(1).gameObject;
            if (i != currentHotBarIndex) hotBarSelection.SetActive(false);
            else hotBarSelection.SetActive(true);
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

    private void ToggleSublayerMenu()
    {
        var sublayerMenu = sublayerToggleButton.transform.parent.GetChild(1).gameObject;
        sublayerMenu.SetActive(!sublayerMenu.activeInHierarchy);
    }

    private void UpdateLayerSelection()
    {
        // Hide the menu if not in the painting tool mode
        if (currentTool != "paint") { layerButtons[0].transform.parent.parent.gameObject.SetActive(false); return; }
        
        // Show the menu if in the painting tool mode
        layerButtons[0].transform.parent.parent.gameObject.SetActive(true);
        
        if (currentPaintMode != 0) { sublayerToggleButton.transform.parent.gameObject.SetActive(false); }
        else { sublayerToggleButton.transform.parent.gameObject.SetActive(true); }

        // Set the layer text colors
        for (int i = 0; i < layerButtons.Length; i++)
        {
            layerButtons[i].transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(1, 1, 1, 0.15f);
        }
        layerButtons[currentLayer].transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(1, 1, 1, 1);
        
        // Set the paint mode
        switch (currentPaintMode)
        {
            case 0:
                // Select the ground layer for each tile map
                currentTilemap = levelManager.tilemaps[currentLayer*3];
                break;
            case 1:
                break;
            case 2:
                // Select the collision layer for each tile map
                currentTilemap = levelManager.tilemaps[currentLayer*3+2];
                break;
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
        topBarButtons[3].gameObject.SetActive(false);
        topBarButtons[4].gameObject.SetActive(true);
        FindObjectOfType<GameInstance>().CreateNewPlayerCharacter(testingGamemode, true, false);
    }

    private void StopTest()
    {
        Destroy(FindObjectOfType<GameInstance>().localPlayerCharacter.gameObject);
        FindObjectOfType<GameInstance>().localPlayerCharacter = editorPlayer;
        editorPlayer.gameObject.SetActive(true);
        topBarButtons[3].gameObject.SetActive(true);
        topBarButtons[4].gameObject.SetActive(false);
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    public void SetCurrentHotBarTile(string tileID)
    {
        hotBarTileID[currentHotBarIndex] = tileID;
    }
}