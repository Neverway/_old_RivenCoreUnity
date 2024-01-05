//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using UnityEngine;
using UnityEngine.UI;

public class WB_Title : MonoBehaviour
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
    [SerializeField] private Button Bttn_MainGame, Bttn_Extras, Bttn_Ranking, Bttn_Settings, Bttn_Quit;
    [SerializeField] private GameObject UI_Extras, UI_Ranking, UI_Settings;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        gameInstance = FindObjectOfType<GameInstance>();
        sceneLoader = FindObjectOfType<System_SceneLoader>();
        Bttn_MainGame.onClick.AddListener(delegate { OnClick("MainGame"); });
        Bttn_Extras.onClick.AddListener(delegate { OnClick("Extras"); });
        Bttn_Ranking.onClick.AddListener(delegate { OnClick("Ranking"); });
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
            case "MainGame":
                if (!sceneLoader) sceneLoader = FindObjectOfType<System_SceneLoader>();
                sceneLoader.LoadScene("Dev_Test");
                break;
            case "Extras":
                if (!gameInstance) gameInstance = FindObjectOfType<GameInstance>();
                gameInstance.AddWidget(UI_Extras);
                break;
            case "Ranking":
                if (!gameInstance) gameInstance = FindObjectOfType<GameInstance>();
                gameInstance.AddWidget(UI_Ranking);
                break;
            case "Settings":
                if (!gameInstance) gameInstance = FindObjectOfType<GameInstance>();
                //gameInstance.AddWidget(UI_Settings);
                break;
            case "Quit":
                Application.Quit();
                break;
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
