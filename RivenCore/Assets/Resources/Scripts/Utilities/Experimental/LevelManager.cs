//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

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
        instance = this;
        Bttn_New.onClick.AddListener(() => OnClick("New"));
        Bttn_Load.onClick.AddListener(() => StartCoroutine(ShowFileDialogCoroutine("Load")));
        Bttn_Save.onClick.AddListener(() => StartCoroutine(ShowFileDialogCoroutine("Save")));
        Bttn_StartTest.onClick.AddListener(() => OnClick("StartTest"));
        Bttn_StopTest.onClick.AddListener(() => OnClick("StopTest"));
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
                StartCoroutine(ShowFileDialogCoroutine("Load"));
                break;
            case "Save":
                StartCoroutine(ShowFileDialogCoroutine("Save"));
                break;
            case "StartTest":
                StartTest();
                break;
            case "StopTest":
                StopTest();
                break;
        }
    }
    
    private void StartTest()
    {
        if (!editorPlayer) editorPlayer = FindObjectOfType<GameInstance>().localPlayerCharacter;
        editorPlayer.gameObject.SetActive(false);
        Bttn_StartTest.gameObject.SetActive(false);
        Bttn_StopTest.gameObject.SetActive(true);
        FindObjectOfType<GameInstance>().CreateNewPlayerCharacter(testingGamemode, true, false);
    }

    private void StopTest()
    {
        Destroy(FindObjectOfType<GameInstance>().localPlayerCharacter.gameObject);
        FindObjectOfType<GameInstance>().localPlayerCharacter = editorPlayer;
        editorPlayer.gameObject.SetActive(true);
        Bttn_StartTest.gameObject.SetActive(true);
        Bttn_StopTest.gameObject.SetActive(false);
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

        foreach (var tilemap in tilemaps)
        {
            var bounds = tilemap.cellBounds;

            for (var x = bounds.min.x; x < bounds.max.x; x++)
            {
                for (var y = bounds.min.y; y < bounds.max.y; y++)
                {
                    TileBase tempTile = masterTileIndex.Find(t => t == tilemap.GetTile(new Vector3Int(x, y, 0)));

                    if (tempTile == null) continue;
                    levelData.tilemap.Add(tilemaps.IndexOf(tilemap));
                    levelData.tiles.Add(tempTile.name);
                    levelData.poses.Add(new Vector3Int(x, y, 0));
                }
            }
        }

        var json = JsonUtility.ToJson(levelData, true);
        File.WriteAllText(levelFile, json);
    }

    private void LoadLevel(string levelFile)
    {
        var json = File.ReadAllText(levelFile);
        var data = JsonUtility.FromJson<LevelData>(json);

        foreach (var tilemap in tilemaps) tilemap.ClearAllTiles();

        for (var i = 0; i < data.poses.Count; i++)
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