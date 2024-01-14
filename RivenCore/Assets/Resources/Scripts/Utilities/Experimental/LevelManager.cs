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
using UnityEditor;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public static LevelManager instance;
    public Tilemap tilemap;
    public List<Tile> tiles = new List<Tile>();


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private Button Bttn_New, Bttn_Load, Bttn_Save;

    [SerializeField] private InputField InptFld_levelName;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
        Bttn_New.onClick.AddListener(delegate { OnClick("New"); });
        Bttn_Load.onClick.AddListener(delegate { OnClick("Load"); });
        Bttn_Save.onClick.AddListener(delegate { OnClick("Save"); });
    }

    private void Update()
    {
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void OnClick(string button)
    {
        switch (button)
        {
            case "New":
                tilemap.ClearAllTiles();
                break;
            case "Load":
                LoadLevel(EditorUtility.OpenFilePanel("Selected level file", Application.persistentDataPath, "ctmap"));
                break;
            case "Save":
                SaveLevel(EditorUtility.SaveFilePanel("Selected level file", Application.persistentDataPath, "NewLevel", "ctmap"));
                break;
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    public void SaveLevel(string levelFile)
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
        File.WriteAllText(levelFile,json);
    }
    
    public void LoadLevel(string levelFile)
    {
        Debug.Log("Loaded");
        string json = File.ReadAllText(levelFile);
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