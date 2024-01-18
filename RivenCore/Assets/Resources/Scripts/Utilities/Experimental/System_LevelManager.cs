//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Handles the saving and loading of map data
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using SimpleFileBrowser;

public class System_LevelManager : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Tooltip("A list of all of the 'tile layers' used in the scene")]
    public List<Tilemap> tilemaps;
    [Tooltip("A list of all the tiles and their categories used for the current project")]
    public List<tileMemoryGroup> tileMemory;
    [Tooltip("A list of all the objects and their categories used for the current project")]
    public List<AssetMemoryGroup> assetMemory;


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private LevelData levelData;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private UISkin fileBrowserSkin;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Update()
    {
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=
    public IEnumerator ShowFileDialogCoroutine(string mode)
    {
        FileBrowser.SetFilters(false, new FileBrowser.Filter("CT Maps", ".ctmap"));
        yield return mode switch
        {
            "Load" => FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, false, Application.persistentDataPath, null, "Load Cartographer Map File", "Load"),
            "Save" => FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.FilesAndFolders, false, Application.persistentDataPath, null, "Save Cartographer Map File", "Save"),
            _ => null
        };

        if (!FileBrowser.Success) yield break;
        var filePath = FileBrowser.Result[0];
        switch (mode)
        {
            case "Load":
                LoadLevel(filePath);
                break;
            case "Save":
                SaveLevel(filePath);
                break;
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    private void SaveLevel(string levelFile)
    {
        var levelData = new LevelData();
        // Save tile data
        foreach (var tilemap in tilemaps)
        {
            // Get the size of the tilemap
            var bounds = tilemap.cellBounds;
            // Iterate through each tile in the tile map
            for (var x = bounds.min.x; x < bounds.max.x; x++)
            {
                for (var y = bounds.min.y; y < bounds.max.y; y++)
                {
                    // Check in each tileMemory group
                    TileBase tempTile = null;
                    foreach (var group in tileMemory)
                    {
                        if (group.tiles.Find(t => t == tilemap.GetTile(new Vector3Int(x, y, 0))))
                        {
                            tempTile = group.tiles.Find(t => t == tilemap.GetTile(new Vector3Int(x, y, 0)));
                            break;
                        }
                    }

                    // Exit if we couldn't find the tile in the tileMemory
                    if (tempTile == null) continue;
                    // Else add the tile data to the level data
                    SpotData newSpotData = new SpotData();
                    newSpotData.id = tempTile.name;
                    newSpotData.position = new Vector3Int(x, y, 0);
                    newSpotData.layer = tilemaps.IndexOf(tilemap);
                    levelData.tiles.Add(newSpotData);
                }
            }
        }
        // Save object data
        //foreach (var tilemap in tilemaps)
        //{
        //}
        
        var json = JsonUtility.ToJson(levelData, true);
        File.WriteAllText(levelFile, json);
    }
    
    private void LoadLevel(string levelFile)
    {
        var json = File.ReadAllText(levelFile);
        var data = JsonUtility.FromJson<LevelData>(json);
        
        foreach (var tilemap in tilemaps) tilemap.ClearAllTiles();

        for (var i = 0; i < data.tiles.Count; i++)
        {
            TileBase tempTile = null;

            foreach (var tileMemoryGroup in tileMemory)
            {
                if (tileMemoryGroup.tiles.Find(t => t.name == data.tiles[i].id))
                {
                    tempTile = tileMemoryGroup.tiles.Find(t => t.name == data.tiles[i].id);
                }
            }
            
            if (tempTile == null) continue;
            print(tempTile);
            tilemaps[data.tiles[i].layer].SetTile(data.tiles[i].position, tempTile);
        }
    }

    public void LevelFile(string mode)
    {
        StartCoroutine(ShowFileDialogCoroutine(mode));
    }

    public Tile GetTileFromMemory(string tileID)
    {
        foreach (var tileMemoryGroup in tileMemory)
        {
            foreach (var tile in tileMemoryGroup.tiles)
            {
                if (tile.name == tileID) return tile;
            }
        }

        return null;
    }
}
