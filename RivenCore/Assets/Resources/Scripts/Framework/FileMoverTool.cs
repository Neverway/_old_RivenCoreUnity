//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.IO;
using UnityEditor;
using UnityEngine;

public class FileMoverTool : EditorWindow
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    private string sourceFolder = "Assets/Resources/Maps";
    private string targetFolder = ""; // Leave empty to export to the build directory
    private string fileExtension = "*.ctmap";
    
    private const string TargetFolderKey = "FileMoverTool_TargetFolder";

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    [MenuItem("Neverway/Export CTMaps")]
    public static void ShowWindow()
    {
        GetWindow<FileMoverTool>("Export CTMap Files");
    }

    void OnGUI()
    {
        GUILayout.Label("Export Settings", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Source Folder", sourceFolder);
        EditorGUILayout.LabelField("Target Folder", targetFolder);

        if (GUILayout.Button("Browse Target"))
        {
            BrowseTargetFolder();
        }

        if (GUILayout.Button("Export Files"))
        {
            ExportFiles();
        }
    }

    private void BrowseTargetFolder()
    {
        string selectedFolder = EditorUtility.OpenFolderPanel("Select Target Folder", "", "");
        if (!string.IsNullOrEmpty(selectedFolder))
        {
            targetFolder = selectedFolder;
            // Save the selected folder path
            EditorPrefs.SetString(TargetFolderKey, selectedFolder);
        }
    }

    private void ExportFiles()
    {
        // Check if the target folder is specified
        if (string.IsNullOrEmpty(targetFolder))
        {
            Debug.LogError("Target folder is not specified!");
            return;
        }

        // Get the path to the build directory
        string buildDirectory = Path.GetDirectoryName(Application.dataPath);

        // Combine the build directory path with the target folder name
        string exportFolderPath = Path.Combine(buildDirectory, targetFolder);

        // Append "maps" to the target folder path
        string mapsFolder = Path.Combine(targetFolder, "Maps");

        // Create the "maps" folder if it doesn't exist
        if (!Directory.Exists(mapsFolder))
        {
            Directory.CreateDirectory(mapsFolder);
        }

        // Validate the source directory
        if (!Directory.Exists(sourceFolder))
        {
            Debug.LogError("Source folder does not exist!");
            return;
        }

        // Get all files in the source directory with the specified extension
        string[] files = Directory.GetFiles(sourceFolder, fileExtension, SearchOption.AllDirectories);
        foreach (string file in files)
        {
            string fileName = Path.GetFileName(file);
            string destFile = Path.Combine(mapsFolder, fileName);

            // Copy the file to the "maps" folder
            File.Copy(file, destFile, true); // Overwrite if exists
            Debug.Log($"Exported {fileName} to {destFile}");
        }
        AssetDatabase.Refresh();
    }
    
    // Load the target folder path when the window is opened
    private void OnEnable()
    {
        targetFolder = EditorPrefs.GetString(TargetFolderKey, "");
    }

    // Save the target folder path when the window is closed
    private void OnDisable()
    {
        EditorPrefs.SetString(TargetFolderKey, targetFolder);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
