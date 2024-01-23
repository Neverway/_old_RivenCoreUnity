//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class old_LevelEditor : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public Tilemap currentTilemap;
    public int selectedTileIndex;

    //=-----------------=
    // Private Variables
    //=-----------------=
    private int selectedTilemapIndex;
    private Stack<TileChange> undoStack = new Stack<TileChange>();
    private Stack<TileChange> redoStack = new Stack<TileChange>();
    private TileChange lastChange;
    
    
    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private Camera viewCamera;
    [SerializeField] private TileBase currentTile { get { return old_LevelManager.instance.masterTileIndex[selectedTileIndex]; } }

    
    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Update()
    {
        viewCamera = FindObjectOfType<Camera>();
        var pos = currentTilemap.WorldToCell(viewCamera.ScreenToWorldPoint(Input.mousePosition));

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) { PlaceTile(pos); }
        if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject()) { EraseTile(pos); }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Z)) Redo();
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z)) Undo();
    }

    
    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void PlaceTile(Vector3Int pos)
    {
        var previousTile = currentTilemap.GetTile(pos);
        currentTilemap.SetTile(pos, currentTile);
        RecordChange(pos, previousTile);
        Debug.Log($"Placed tile with selectedTileIndex: {selectedTileIndex}");
    }

    private void EraseTile(Vector3Int pos)
    {
        var previousTile = currentTilemap.GetTile(pos);
        currentTilemap.SetTile(pos, null);
        RecordChange(pos, previousTile);
    }

    private void RecordChange(Vector3Int pos, TileBase previousTile)
    {
        var tileChange = new TileChange(pos, previousTile);
        if (tileChange.Equals(lastChange)) { return; }
        undoStack.Push(tileChange);
        redoStack.Clear();
        lastChange = tileChange;
    }

    private void Undo()
    {
        if (undoStack.Count <= 0) return;
        var undoChange = undoStack.Pop();
        var currentTile = currentTilemap.GetTile(undoChange.Position);
        redoStack.Push(new TileChange(undoChange.Position, currentTile));
        currentTilemap.SetTile(undoChange.Position, undoChange.PreviousTile);
        lastChange = undoStack.Count > 0 ? undoStack.Peek() : new TileChange(Vector3Int.zero, null);
    }

    private void Redo()
    {
        if (redoStack.Count <= 0) return;
        var redoChange = redoStack.Pop();
        var currentTile = currentTilemap.GetTile(redoChange.Position);
        undoStack.Push(new TileChange(redoChange.Position, currentTile));
        currentTilemap.SetTile(redoChange.Position, redoChange.PreviousTile);
        lastChange = redoChange;
    }

    private struct TileChange
    {
        public Vector3Int Position;
        public TileBase PreviousTile;

        public TileChange(Vector3Int position, TileBase previousTile)
        {
            Position = position;
            PreviousTile = previousTile;
        }

        public bool Equals(TileChange other)
        {
            return Position.Equals(other.Position) && PreviousTile == other.PreviousTile;
        }
    }

    
    //=-----------------=
    // External Functions
    //=-----------------=
}