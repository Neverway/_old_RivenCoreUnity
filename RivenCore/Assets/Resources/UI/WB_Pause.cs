//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using UnityEngine;
using UnityEngine.UI;

public class WB_Pause : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private GameInstance gameInstance;
    private System_SceneLoader sceneLoader;
    [SerializeField] private Button Bttn_Return, Bttn_Settings, Bttn_Quit;
    [SerializeField] private GameObject UI_Settings;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        gameInstance = FindObjectOfType<GameInstance>();
        sceneLoader = FindObjectOfType<System_SceneLoader>();
        Bttn_Return.onClick.AddListener(delegate { OnClick("Return"); });
        Bttn_Settings.onClick.AddListener(delegate { OnClick("Settings"); });
        Bttn_Quit.onClick.AddListener(delegate { OnClick("Quit"); });
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void OnClick(string button)
    {
        switch (button)
        {
            case "Return":
                Destroy(gameObject);
                break;
            case "Settings":
                //gameInstance.AddWidget(UI_Settings);
                break;
            case "Quit":
                sceneLoader.LoadScene("_Title");
                break;
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
