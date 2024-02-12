//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Handles the saving and loading of map data
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
using UnityEngine.SceneManagement;

public class System_LevelManager : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Tooltip("A list of all the tiles and their categories used for the current project")]
    public List<tileMemoryGroup> tileMemory;
    [Tooltip("A list of all the objects and their categories used for the current project")]
    public List<AssetMemoryGroup> assetMemory;
    [Tooltip("A list of all the sprites that can be used for decor props for the current project")]
    public List<Sprite> spriteMemory;
    [Header("READ-ONLY (Don't touch!)")]
    [Tooltip("A list of all of the 'tile layers' used in the scene")]
    public List<Tilemap> tilemaps;
    public GameObject assetsRoot;
    public string filePath;


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
    private void Awake()
    {
        InitializeSceneReferences();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeSceneReferences();
    }

    private void InitializeSceneReferences()
    {
        tilemaps.Clear();
        if (tilemaps.Count == 0 && GameObject.FindWithTag("Map_Tilemaps"))
        {
            var tileMapGroups = GameObject.FindWithTag("Map_Tilemaps").transform;
            for (int i = 0; i < tileMapGroups.childCount; i++)
            {
                for (int ii = 0; ii < tileMapGroups.GetChild(i).childCount; ii++)
                {
                    tilemaps.Add(tileMapGroups.GetChild(i).GetChild(ii).GetComponent<Tilemap>());
                }
            }
        }
        if (!assetsRoot) assetsRoot = GameObject.FindGameObjectWithTag("Map_Assets");
    }
    
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
        filePath = FileBrowser.Result[0];
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
    public void SaveLevel(string levelFile)
    {
        var data = new LevelData();
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
                    newSpotData.layerID = tilemap.name;
                    data.tiles.Add(newSpotData);
                }
            }
        }
        // Save object data
        
        // Find all objects with an asset_instanceID component
        // For each one, see if we can find the root asset in assetMemory
            // If not, skip it
        // else, add the asset data for name, unique id, position, and any variable data
        for (int i = 0; i < assetsRoot.transform.childCount; i++)
        {
            GameObject tempAsset = null;
            foreach (var group in assetMemory)
            {
                if (group.assets.Find(t => t.name == assetsRoot.transform.GetChild(i).gameObject.name))
                {
                    tempAsset = assetsRoot.transform.GetChild(i).gameObject;
                    break;
                }
            }
            
            // Exit if we couldn't find the asset in the assetMemory
            if (tempAsset == null) continue;
            // Else add the asset data to the level data
            SpotData newSpotData = new SpotData();
            newSpotData.id = tempAsset.name;
            newSpotData.unsnappedPosition = tempAsset.transform.position;
            if (tempAsset.GetComponent<Asset_UniqueInstanceId>())
            {
                newSpotData.uniqueId = tempAsset.GetComponent<Asset_UniqueInstanceId>().Id;
            }
            if (tempAsset.GetComponent<Object_RuntimeDataInspector>())
            {
                tempAsset.GetComponent<Object_RuntimeDataInspector>().Inspect();
                newSpotData.assetData = tempAsset.GetComponent<Object_RuntimeDataInspector>().variableData;
            }
            else
            {
                newSpotData.assetData = new List<VariableData>();
            }
            // NEED LAYER/DEPTH ASSIGNMENT HERE
            //newSpotData.layer = tempAsset.layer;
            data.assets.Add(newSpotData);
        }
        
        var json = JsonUtility.ToJson(data, true);
        File.WriteAllText(levelFile, json);
    }
    
    private void LoadLevel(string levelFile)
    {
        var json = File.ReadAllText(levelFile);
        var data = JsonUtility.FromJson<LevelData>(json);
        
        foreach (var tilemap in tilemaps) tilemap.ClearAllTiles();
        for (int i = 0; i < assetsRoot.transform.childCount; i++)
        {
            Destroy(assetsRoot.transform.GetChild(i).gameObject);
        }

        // Load tiles
        for (var i = 0; i < data.tiles.Count; i++)
        {
            TileBase tempTile = null;

            foreach (var group in tileMemory)
            {
                if (group.tiles.Find(t => t.name == data.tiles[i].id))
                {
                    tempTile = group.tiles.Find(t => t.name == data.tiles[i].id);
                    break;
                }
            }
            
            if (tempTile == null) continue;
            tilemaps[data.tiles[i].layer].SetTile(data.tiles[i].position, tempTile);
        }
        
        // Load assets
        for (int i = 0; i < data.assets.Count; i++)
        {
            GameObject tempAsset = null;
            Vector3 tempPosition = new Vector3();
            int tempUniqueId = 0;
            List<VariableData> tempData = new List<VariableData>();

            foreach (var group in assetMemory)
            {
                if (group.assets.Find(t => t.name == data.assets[i].id))
                {
                    tempAsset = group.assets.Find(t => t.name == data.assets[i].id);
                    tempPosition = data.assets[i].unsnappedPosition;
                    tempData = data.assets[i].assetData;
                    tempUniqueId = data.assets[i].uniqueId;
                    break;
                }
            }
            
            if (tempAsset == null) continue;
            var assetRef = Instantiate(tempAsset, tempPosition, new Quaternion(0, 0, 0, 0), assetsRoot.transform);
            assetRef.name = assetRef.name.Replace("(Clone)", "").Trim();
            if (assetRef.GetComponent<Asset_UniqueInstanceId>())
            {
                assetRef.GetComponent<Asset_UniqueInstanceId>().Id = tempUniqueId;
            }
            if (assetRef.GetComponent<Object_RuntimeDataInspector>())
            {
                assetRef.GetComponent<Object_RuntimeDataInspector>().variableData = tempData;
                assetRef.GetComponent<Object_RuntimeDataInspector>().SendVariableDataToScript();
            }
            // NEED LAYER/DEPTH ASSIGNMENT HERE
        }
    }

    public void ModifyLevelFile(string mode)
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

    public GameObject GetAssetFromMemory(string assetID)
    {
        foreach (var assetMemoryGroup in assetMemory)
        {
            foreach (var asset in assetMemoryGroup.assets)
            {
                if (asset.name == assetID) return asset;
            }
        }

        return null;
    }
}
