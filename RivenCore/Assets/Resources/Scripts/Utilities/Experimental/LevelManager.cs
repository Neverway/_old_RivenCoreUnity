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
    public List<Tile> masterTileIndex = new List<Tile>();


    //=-----------------=
    // Private Variables
    //=-----------------=
    private bool isTesting;
    private Entity editorPlayer;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private Button Bttn_New, Bttn_Load, Bttn_Save, Bttn_StartTest, Bttn_StopTest;
    [SerializeField] private Gamemode testingGamemode;


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
        Bttn_StartTest.onClick.AddListener(delegate { OnClick("StartTest"); });
        Bttn_StopTest.onClick.AddListener(delegate { OnClick("StopTest"); });
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
            case "StartTest":
                if (!editorPlayer) editorPlayer = FindObjectOfType<GameInstance>().localPlayerCharacter;
                editorPlayer.gameObject.SetActive(false);
                Bttn_StartTest.gameObject.SetActive(false);
                Bttn_StopTest.gameObject.SetActive(true);
                FindObjectOfType<GameInstance>().CreateNewPlayerCharacter(testingGamemode, true, false);
                break;
            case "StopTest":
                Destroy(FindObjectOfType<GameInstance>().localPlayerCharacter.gameObject);
                FindObjectOfType<GameInstance>().localPlayerCharacter = editorPlayer;
                editorPlayer.gameObject.SetActive(true);
                Bttn_StartTest.gameObject.SetActive(true);
                Bttn_StopTest.gameObject.SetActive(false);
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
                TileBase tempTile = masterTileIndex.Find(t => t == temp);
                
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
            tilemap.SetTile(data.poses[i], masterTileIndex.Find(t => t.name == data.tiles[i]));
        }
    }
}

public class LevelData
{
    public List<string> tiles = new List<string>();
    public List<Vector3Int> poses = new List<Vector3Int>();
}