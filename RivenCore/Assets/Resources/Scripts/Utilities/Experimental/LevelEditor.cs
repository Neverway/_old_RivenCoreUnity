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

public class LevelEditor : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public Tilemap currentTilemap;
    [SerializeField] private Camera viewCamera;

    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private TileBase currentTile { get { return LevelManager.instance.masterTileIndex[selectedTileIndex]; } }
    public int selectedTileIndex;
    [SerializeField] private int selectedTilemapIndex;
    [SerializeField] private Stack<TileChange> undoStack = new Stack<TileChange>();
    [SerializeField] private Stack<TileChange> redoStack = new Stack<TileChange>();
    private TileChange lastChange;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Update()
    {
        viewCamera = FindObjectOfType<Camera>();
        Vector3Int pos = currentTilemap.WorldToCell(viewCamera.ScreenToWorldPoint(Input.mousePosition));

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
        TileBase previousTile = currentTilemap.GetTile(pos);
        currentTilemap.SetTile(pos, currentTile);
        RecordChange(pos, previousTile);
    }

    private void EraseTile(Vector3Int pos)
    {
        TileBase previousTile = currentTilemap.GetTile(pos);
        currentTilemap.SetTile(pos, null);
        RecordChange(pos, previousTile);
    }

    private void RecordChange(Vector3Int pos, TileBase previousTile)
    {
        TileChange tileChange = new TileChange(pos, previousTile);

        // Check if the current change is the same as the previous one
        if (tileChange.Equals(lastChange))
        {
            return; // Skip adding to undo stack
        }

        undoStack.Push(tileChange);
        redoStack.Clear(); // Clear redo stack when a new change is made

        lastChange = tileChange;
    }

    private void Undo()
    {
        if (undoStack.Count > 0)
        {
            TileChange undoChange = undoStack.Pop();
            TileBase currentTile = currentTilemap.GetTile(undoChange.Position);

            redoStack.Push(new TileChange(undoChange.Position, currentTile));

            currentTilemap.SetTile(undoChange.Position, undoChange.PreviousTile);

            lastChange = undoStack.Count > 0 ? undoStack.Peek() : new TileChange(Vector3Int.zero, null);
        }
    }

    private void Redo()
    {
        if (redoStack.Count > 0)
        {
            TileChange redoChange = redoStack.Pop();
            TileBase currentTile = currentTilemap.GetTile(redoChange.Position);

            undoStack.Push(new TileChange(redoChange.Position, currentTile));

            currentTilemap.SetTile(redoChange.Position, redoChange.PreviousTile);

            lastChange = redoChange;
            Debug.Log($"Redo: {redoChange.Position} {redoChange.PreviousTile} {redoStack.Count}");
        }
    }

    [Serializable]
    public struct TileChange
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
