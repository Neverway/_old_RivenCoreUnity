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

public class LevelEditor : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public Tilemap currentTilemap;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private TileBase currentTile
    {
        get { return LevelManager.instance.tiles[selectedTileIndex]; }
    }
    private List<Tile> currentTiles
    {
        get { return LevelManager.instance.tiles; }
    }

    public int selectedTileIndex;
    public int selectedHotbarTile;
    public bool inventoryOpen;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    public Camera viewCamera;

    [SerializeField] private Button Bttn_Tile1, Bttn_Tile2, Bttn_Tile3, Bttn_Tile4, Bttn_Tile5, Bttn_Tile6, Bttn_Tile7, Bttn_Tile8, Bttn_Tile9, Bttn_TileInventory;
    [SerializeField] private Image[] Img_Tile;

    [SerializeField] private int[] hotbarTiles;
    [SerializeField] private TMP_Text debugText;
    [SerializeField] private GameObject inventory;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        Bttn_Tile1.onClick.AddListener(delegate { OnClick("Tile1"); });
        Bttn_Tile2.onClick.AddListener(delegate { OnClick("Tile2"); });
        Bttn_Tile3.onClick.AddListener(delegate { OnClick("Tile3"); });
        Bttn_Tile4.onClick.AddListener(delegate { OnClick("Tile4"); });
        Bttn_Tile5.onClick.AddListener(delegate { OnClick("Tile5"); });
        Bttn_Tile6.onClick.AddListener(delegate { OnClick("Tile6"); });
        Bttn_Tile7.onClick.AddListener(delegate { OnClick("Tile7"); });
        Bttn_Tile8.onClick.AddListener(delegate { OnClick("Tile8"); });
        Bttn_Tile9.onClick.AddListener(delegate { OnClick("Tile9"); });
        Bttn_TileInventory.onClick.AddListener(delegate { OnClick("TileInventory"); });
    }

    private void Update()
    {
        viewCamera = FindObjectOfType<Camera>();
        Vector3Int pos = currentTilemap.WorldToCell(viewCamera.ScreenToWorldPoint(Input.mousePosition));

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            PlaceTile(pos);
        }

        if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            EraseTile(pos);
        }

        UpdateHotbarTileImages();
        selectedTileIndex = hotbarTiles[selectedHotbarTile];
        debugText.text = $"Hotbar: {selectedHotbarTile},\n Tile: {currentTile.name},\n Layer: {LevelManager.instance.tilemap.name}";
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void PlaceTile(Vector3Int pos)
    {
        currentTilemap.SetTile(pos, currentTile);
    }

    private void EraseTile(Vector3Int pos)
    {
        currentTilemap.SetTile(pos, null);
    }

    private void OnClick(string button)
    {
        switch (button)
        {
            case "Tile1":
                selectedHotbarTile = 0;
                break;
            case "Tile2":
                selectedHotbarTile = 1;
                break;
            case "Tile3":
                selectedHotbarTile = 2;
                break;
            case "Tile4":
                selectedHotbarTile = 3;
                break;
            case "Tile5":
                selectedHotbarTile = 4;
                break;
            case "Tile6":
                selectedHotbarTile = 5;
                break;
            case "Tile7":
                selectedHotbarTile = 6;
                break;
            case "Tile8":
                selectedHotbarTile = 7;
                break;
            case "Tile9":
                selectedHotbarTile = 8;
                break;
            case "TileInventory":
                ToggleInventoryOpen();
                break;
        }
    }

    private void UpdateHotbarTileImages()
    {
        for (int i = 0; i < Img_Tile.Length; i++)
        {
            if (currentTiles[hotbarTiles[i]])
            {
                Img_Tile[i].GetComponent<Image>().enabled = true;
                Img_Tile[i].sprite = currentTiles[hotbarTiles[i]].sprite;
            }
            else
            {
                Img_Tile[i].GetComponent<Image>().enabled = false;
            }
        }
    }

    private void ToggleInventoryOpen()
    {
        inventoryOpen = !inventoryOpen;
        if (inventoryOpen) inventory.SetActive(true);
        else inventory.SetActive(false);
    }


//=-----------------=
    // External Functions
    //=-----------------=
}
