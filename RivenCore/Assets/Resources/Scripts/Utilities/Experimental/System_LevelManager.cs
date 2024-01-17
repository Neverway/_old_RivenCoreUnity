//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
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


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private UISkin fileBrowserSkin;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S)) StartCoroutine(ShowFileDialogCoroutine("Save"));
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.O)) StartCoroutine(ShowFileDialogCoroutine("Load"));
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=
    private IEnumerator ShowFileDialogCoroutine(string mode)
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
        var testData = new LevelData();
        Debug.Log("Saving level...");
        // Save tile data
        foreach (var tilemap in tilemaps)
        {
            Debug.Log($"In {tilemap.name}");
            // Get the size of the tilemap
            var bounds = tilemap.cellBounds;
            // Iterate through each tile in the tile map
            for (var x = bounds.min.x; x < bounds.max.x; x++)
            {
                for (var y = bounds.min.y; y < bounds.max.y; y++)
                {
                    
                    TileBase tempTile = null;
                    var tileGroup = "";
                    
                    // Check in each tileMemory group
                    foreach (var group in tileMemory)
                    {
                        Debug.Log($"In {group.name}");
                        if (group.tiles.Find(t => t == tilemap.GetTile(new Vector3Int(x, y, 0))))
                        {
                            tempTile = group.tiles.Find(t => t == tilemap.GetTile(new Vector3Int(x, y, 0)));
                            Debug.Log($"Found tile {tempTile}");
                            break;
                        }
                    }

                    // Exit if we couldn't find the tile in the tileMemory
                    if (tempTile == null) continue;
                    // Else add the tile data to the level data
                    SpotData newSpotData = new SpotData();
                    newSpotData.id = tempTile.name;
                    newSpotData.position = new Vector3(x, y, 0);
                    newSpotData.layer = tilemaps.IndexOf(tilemap);
                    Debug.Log($"Writing {newSpotData.id}");
                    testData.tiles.Add(newSpotData);
                }
            }
        }
        // Save object data
        //foreach (var tilemap in tilemaps)
        //{
        //}
        
        Debug.Log($"SAVING FILE");
        var json = JsonUtility.ToJson(testData, true);
        File.WriteAllText(levelFile, json);
    }
    private void LoadLevel(string levelFile)
    {
        var json = File.ReadAllText(levelFile);
        var data = JsonUtility.FromJson<LevelData>(json);
    }
}
