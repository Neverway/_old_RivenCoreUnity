//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Handles input and UI events for controlling level editor
// Notes:
//
//=============================================================================

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
    [SerializeField] private Tilemap currentTilemap;
    [SerializeField] private int currentHotbarIndex;
    [SerializeField] private string[] hotbarTileID;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private System_LevelManager levelManager;
    [SerializeField] private Camera viewCamera;
    [SerializeField] private Button[] topbarButtons;
    [SerializeField] private Button[] sidebarButtons;
    [SerializeField] private Button[] hotbarButtons;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        levelManager = FindObjectOfType<System_LevelManager>();
        currentTilemap = levelManager.tilemaps[0];
    }

    private void Update()
    {
        viewCamera = FindObjectOfType<Camera>();
        var pos = currentTilemap.WorldToCell(viewCamera.ScreenToWorldPoint(Input.mousePosition));

        UserInput();
        UpdateHotbarImages();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void UserInput()
    {
        // Place/Erase
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) { /*PlaceTile(pos);*/ }
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
            //ToggleInventoryOpen();
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
            print(levelManager.GetTileFromMemory(hotbarTileID[i]).name);
            if (levelManager.GetTileFromMemory(hotbarTileID[i]))
            {
                hotbarPreview.sprite = levelManager.GetTileFromMemory(hotbarTileID[i]).sprite;
            }
            
            // Show hotbar selection indicator
            var hotbarSelection = hotbarButtons[i].transform.GetChild(1).gameObject;
            if (i != currentHotbarIndex) hotbarSelection.SetActive(false);
            else hotbarSelection.SetActive(true);
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
