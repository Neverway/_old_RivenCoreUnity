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
using UnityEngine.UI;
using SimpleFileBrowser;

public class LevelManager : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public static LevelManager instance;
    public List<Tilemap> tilemaps;
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

    [SerializeField] private UISkin fileBrowserSkin;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        FileBrowser.Skin = fileBrowserSkin;
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
                foreach (var tilemap in tilemaps) tilemap.ClearAllTiles();
                break;
            case "Load":
                StartCoroutine( ShowLoadDialogCoroutine() );
                break;
            case "Save":
                StartCoroutine( ShowSaveDialogCoroutine() );
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

    IEnumerator ShowLoadDialogCoroutine()
    {		
        FileBrowser.SetFilters( false, new FileBrowser.Filter( "CT Maps", ".ctmap"));
        yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.FilesAndFolders, false, Application.persistentDataPath, null, "Load Cartographer Map File", "Load" );
        if (FileBrowser.Success)
        {
            LoadLevel(FileBrowser.Result[0]);
        }
    }

    IEnumerator ShowSaveDialogCoroutine()
    {		
        FileBrowser.SetFilters( false, new FileBrowser.Filter( "CT Maps", ".ctmap"));
        yield return FileBrowser.WaitForSaveDialog( FileBrowser.PickMode.FilesAndFolders, false, Application.persistentDataPath, null, "Save Cartographer Map File", "Save" );
        if (FileBrowser.Success)
        {
            SaveLevel(FileBrowser.Result[0]);
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    public void SaveLevel(string levelFile)
    {
        Debug.Log("Saved");
            
        LevelData levelData = new LevelData();

        for (int i = 0; i < tilemaps.Count; i++)
        {
            BoundsInt bounds = tilemaps[i].cellBounds;
            
            for (int x = bounds.min.x; x < bounds.max.x; x++)
            {
                for (int y = bounds.min.y; y < bounds.max.y; y++)
                {
                    Tilemap tempTileMap = tilemaps[i];
                    TileBase temp = tilemaps[i].GetTile(new Vector3Int(x, y, 0));
                    TileBase tempTile = masterTileIndex.Find(t => t == temp);
                    
                    if (tempTile != null)
                    {
                        levelData.tilemap.Add(i);
                        levelData.tiles.Add(tempTile.name);
                        levelData.poses.Add(new Vector3Int(x, y, 0));
                    }
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
        
        foreach (var tilemap in tilemaps) tilemap.ClearAllTiles();

        for (int i = 0; i < data.poses.Count; i++)
        {
            tilemaps[data.tilemap[i]].SetTile(data.poses[i], masterTileIndex.Find(t => t.name == data.tiles[i]));
        }
    }
}

public class LevelData
{
    public List<int> tilemap = new List<int>();
    public List<string> tiles = new List<string>();
    public List<Vector3Int> poses = new List<Vector3Int>();
}