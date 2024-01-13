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
using System.IO;

public class LevelManager : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public static LevelManager instance;
    public Tilemap tilemap;
    public List<TileBase> tiles = new List<TileBase>();


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.A)) SaveLevel();
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.W)) LoadLevel();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void SaveLevel()
    {
        Debug.Log("Saved");
        BoundsInt bounds = tilemap.cellBounds;

        LevelData levelData = new LevelData();
        
        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                TileBase temp = tilemap.GetTile(new Vector3Int(x, y, 0));
                TileBase tempTile = tiles.Find(t => t == temp);
                
                if (tempTile != null)
                {
                    levelData.tiles.Add(tempTile.name);
                    levelData.poses.Add(new Vector3Int(x, y, 0));
                }
            }
        }

        string json = JsonUtility.ToJson(levelData, true);
        File.WriteAllText(Application.dataPath + "/testLevel.json",json);
    }
    
    public void LoadLevel()
    {
        Debug.Log("Loaded");
        string json = File.ReadAllText(Application.dataPath + "/testLevel.json");
        LevelData data = JsonUtility.FromJson<LevelData>(json);
        
        tilemap.ClearAllTiles();

        for (int i = 0; i < data.poses.Count; i++)
        {
            tilemap.SetTile(data.poses[i], tiles.Find(t => t.name == data.tiles[i]));
        }
    }
}

public class LevelData
{
    public List<string> tiles = new List<string>();
    public List<Vector3Int> poses = new List<Vector3Int>();
}