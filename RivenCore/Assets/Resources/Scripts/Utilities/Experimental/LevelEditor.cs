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
    public TileBase currentTile { get { return LevelManager.instance.masterTileIndex[selectedTileIndex]; } }
    public int selectedTileIndex;
    public int selectedTilemapIndex;


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

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) { PlaceTile(pos); }
        if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject()) { EraseTile(pos); }

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
