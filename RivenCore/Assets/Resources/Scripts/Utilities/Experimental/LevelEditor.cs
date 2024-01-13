//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelEditor : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public Tilemap currentTilemap;

    private TileBase currentTile
    {
        get
        {
            return LevelManager.instance.tiles[selectedTileIndex];
        }
    }


    //=-----------------=
    // Private Variables
    //=-----------------=
    private int selectedTileIndex;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    public Camera viewCamera;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Update()
    {
        viewCamera = FindObjectOfType<Camera>();
        Vector3Int pos = currentTilemap.WorldToCell(viewCamera.ScreenToWorldPoint(Input.mousePosition));

        if (Input.GetMouseButton(0))
        {
            PlaceTile(pos);
        }
        if (Input.GetMouseButton(1))
        {
            EraseTile(pos);
        }

        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            Debug.Log(LevelManager.instance.tiles[selectedTileIndex].name);
            selectedTileIndex++;
            if (selectedTileIndex >= LevelManager.instance.tiles.Count) selectedTileIndex = 0;
        }

        if (Input.GetKey(KeyCode.KeypadMinus))
        {
            Debug.Log(LevelManager.instance.tiles[selectedTileIndex].name);
            selectedTileIndex--;
            if (selectedTileIndex < 0) selectedTileIndex = LevelManager.instance.tiles.Count - 1;
        }
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


    //=-----------------=
    // External Functions
    //=-----------------=
}
