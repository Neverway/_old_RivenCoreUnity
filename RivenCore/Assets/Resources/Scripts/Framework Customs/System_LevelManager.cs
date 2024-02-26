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
    public Tile missingTileFallback;
    public GameObject missingObjectFallback;
    public Tile missingSpriteFallback;
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
        FileBrowser.AddQuickLink("Data Path", Application.persistentDataPath);
        FileBrowser.AddQuickLink("Application Path", Application.dataPath);
        FileBrowser.AddQuickLink("Editor Level Path", Application.dataPath + "/Resources/Levels/");
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
    /// <summary>
    /// Saves the current state of the level to a file.
    /// </summary>
    /// <param name="levelFile">The file path to save the level data to.</param>
    public void SaveLevel(string levelFile)
    { 
        // Create a new LevelData instance to store the level information.
        var data = new LevelData();

        // Save tile data
        foreach (var tilemap in tilemaps)
        {
            // Get the size of the tilemap
            var bounds = tilemap.cellBounds;
        
            // Iterate through each tile in the tilemap
            for (var x = bounds.min.x; x < bounds.max.x; x++)
            {
                for (var y = bounds.min.y; y < bounds.max.y; y++)
                {
                    // Check each tileMemory group to see if the tile exists
                    TileBase tempTile = null;
                    foreach (var group in tileMemory)
                    {
                        // If the tile exists in tileMemory, save its data
                        if (group.tiles.Find(t => t == tilemap.GetTile(new Vector3Int(x, y, 0))))
                        {
                            tempTile = group.tiles.Find(t => t == tilemap.GetTile(new Vector3Int(x, y, 0)));
                            break;
                        }
                    }

                    // If the tile is not found, skip it
                    if (tempTile == null) continue;
                
                    // Add the tile data to the level data
                    SpotData newSpotData = new SpotData();
                    newSpotData.id = tempTile.name; // Tile name
                    newSpotData.position = new Vector3Int(x, y, 0); // Tile position
                    newSpotData.layer = tilemaps.IndexOf(tilemap); // Tilemap layer
                    newSpotData.layerID = tilemap.name; // Tilemap name
                    data.tiles.Add(newSpotData); // Add tile data to the level data
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
                    // If the asset is found in assetMemory, set it as tempAsset
                    tempAsset = assetsRoot.transform.GetChild(i).gameObject;
                    break;
                }
            }
            
            // If the asset is not found in assetMemory, skip it
            if (tempAsset == null) tempAsset = missingObjectFallback;
            
            // Create a new SpotData instance to store the asset data
            SpotData newSpotData = new SpotData();
            newSpotData.id = tempAsset.name; // Asset name
            newSpotData.unsnappedPosition = tempAsset.transform.position; // Asset position
            
            // If the asset has an Object_RuntimeDataInspector component, inspect it for variable data
            if (tempAsset.GetComponent<Object_RuntimeDataInspector>())
            {
                tempAsset.GetComponent<Object_RuntimeDataInspector>().Inspect();
                newSpotData.assetData = tempAsset.GetComponent<Object_RuntimeDataInspector>().storedVariableData;
            }
            else
            {
                newSpotData.assetData = new List<VariableData>();
            }
            
            // NEED LAYER/DEPTH ASSIGNMENT HERE
            // newSpotData.layer = tempAsset.layer; // Assign layer information
            
            // Add the asset data to the level data
            data.assets.Add(newSpotData);
        }
        
        // Convert the level data to JSON format
        var json = JsonUtility.ToJson(data, true);
        
        // Write the JSON data to the specified file
        File.WriteAllText(levelFile, json); // Save JSON data to file
    }
    
    
    
    /// <summary>
    /// Loads a level from a file.
    /// </summary>
    /// <param name="_levelFile">The file path to load the level data from.</param>
     public void LoadLevel(string levelFile)
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
            //int tempUniqueId = 0;
            List<VariableData> tempData = new List<VariableData>();

            foreach (var group in assetMemory)
            {
                if (group.assets.Find(t => t.name == data.assets[i].id))
                {
                    tempAsset = group.assets.Find(t => t.name == data.assets[i].id);
                    tempPosition = data.assets[i].unsnappedPosition;
                    tempData = data.assets[i].assetData;
                    //tempUniqueId = data.assets[i].uniqueId;
                    break;
                }
            }
            
            if (tempAsset == null) tempAsset = missingObjectFallback;
            var assetRef = Instantiate(tempAsset, tempPosition, new Quaternion(0, 0, 0, 0), assetsRoot.transform);
            assetRef.name = assetRef.name.Replace("(Clone)", "").Trim();
            /*if (assetRef.GetComponent<Asset_UniqueInstanceId>())
            {
                assetRef.GetComponent<Asset_UniqueInstanceId>().Id = tempUniqueId;
            }*/
            if (assetRef.GetComponent<Object_RuntimeDataInspector>())
            {
                assetRef.GetComponent<Object_RuntimeDataInspector>().storedVariableData = tempData;
                assetRef.GetComponent<Object_RuntimeDataInspector>().SendVariableDataToScripts();
            }
        }
    }
    
    /// <summary>
    /// Loads a level from a string
    /// </summary>
    /// <param name="_levelFile">The file path to load the level data from.</param>
     public void LoadRawLevel(string rawleveldata)
    {
        var data = JsonUtility.FromJson<LevelData>(rawleveldata);
        
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
            //int tempUniqueId = 0;
            List<VariableData> tempData = new List<VariableData>();

            foreach (var group in assetMemory)
            {
                if (group.assets.Find(t => t.name == data.assets[i].id))
                {
                    tempAsset = group.assets.Find(t => t.name == data.assets[i].id);
                    tempPosition = data.assets[i].unsnappedPosition;
                    tempData = data.assets[i].assetData;
                    //tempUniqueId = data.assets[i].uniqueId;
                    break;
                }
            }
            
            if (tempAsset == null) continue;
            var assetRef = Instantiate(tempAsset, tempPosition, new Quaternion(0, 0, 0, 0), assetsRoot.transform);
            assetRef.name = assetRef.name.Replace("(Clone)", "").Trim();
            /*if (assetRef.GetComponent<Asset_UniqueInstanceId>())
            {
                assetRef.GetComponent<Asset_UniqueInstanceId>().Id = tempUniqueId;
            }*/
            if (assetRef.GetComponent<Object_RuntimeDataInspector>())
            {
                assetRef.GetComponent<Object_RuntimeDataInspector>().storedVariableData = tempData;
                assetRef.GetComponent<Object_RuntimeDataInspector>().SendVariableDataToScripts();
            }
        }
    }
    
    /// <summary>
    /// Saves the current state of the level to a file.
    /// </summary>
    /// <param name="levelFile">The file path to save the level data to.</param>
    public string GetRawMapData()
    { 
        // Create a new LevelData instance to store the level information.
        var data = new LevelData();

        // Save tile data
        foreach (var tilemap in tilemaps)
        {
            // Get the size of the tilemap
            var bounds = tilemap.cellBounds;
        
            // Iterate through each tile in the tilemap
            for (var x = bounds.min.x; x < bounds.max.x; x++)
            {
                for (var y = bounds.min.y; y < bounds.max.y; y++)
                {
                    // Check each tileMemory group to see if the tile exists
                    TileBase tempTile = null;
                    foreach (var group in tileMemory)
                    {
                        // If the tile exists in tileMemory, save its data
                        if (group.tiles.Find(t => t == tilemap.GetTile(new Vector3Int(x, y, 0))))
                        {
                            tempTile = group.tiles.Find(t => t == tilemap.GetTile(new Vector3Int(x, y, 0)));
                            break;
                        }
                    }

                    // If the tile is not found, skip it
                    if (tempTile == null) continue;
                
                    // Add the tile data to the level data
                    SpotData newSpotData = new SpotData();
                    newSpotData.id = tempTile.name; // Tile name
                    newSpotData.position = new Vector3Int(x, y, 0); // Tile position
                    newSpotData.layer = tilemaps.IndexOf(tilemap); // Tilemap layer
                    newSpotData.layerID = tilemap.name; // Tilemap name
                    data.tiles.Add(newSpotData); // Add tile data to the level data
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
                    // If the asset is found in assetMemory, set it as tempAsset
                    tempAsset = assetsRoot.transform.GetChild(i).gameObject;
                    break;
                }
            }
            
            // If the asset is not found in assetMemory, skip it
            if (tempAsset == null) continue;
            
            // Create a new SpotData instance to store the asset data
            SpotData newSpotData = new SpotData();
            newSpotData.id = tempAsset.name; // Asset name
            newSpotData.unsnappedPosition = tempAsset.transform.position; // Asset position
            
            // If the asset has an Object_RuntimeDataInspector component, inspect it for variable data
            if (tempAsset.GetComponent<Object_RuntimeDataInspector>())
            {
                tempAsset.GetComponent<Object_RuntimeDataInspector>().Inspect();
                newSpotData.assetData = tempAsset.GetComponent<Object_RuntimeDataInspector>().storedVariableData;
            }
            else
            {
                newSpotData.assetData = new List<VariableData>();
            }
            
            // NEED LAYER/DEPTH ASSIGNMENT HERE
            // newSpotData.layer = tempAsset.layer; // Assign layer information
            
            // Add the asset data to the level data
            data.assets.Add(newSpotData);
        }
        
        // Convert the level data to JSON format
        var json = JsonUtility.ToJson(data, true);
        
        // Write the JSON data to the specified file
        return json;
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
