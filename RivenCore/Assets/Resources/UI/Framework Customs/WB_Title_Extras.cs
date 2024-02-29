//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using UnityEngine;
using UnityEngine.UI;

public class WB_Title_Extras : MonoBehaviour
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
    private System_SceneLoader sceneLoader;
    [SerializeField] private Button Bttn_Back;
    [SerializeField] private Button Bttn_Extra1, Bttn_Extra2, Bttn_Extra3;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        Bttn_Back.onClick.AddListener(() => { Destroy(gameObject); });
        Bttn_Extra1.onClick.AddListener(() =>
        {
            if (!sceneLoader)
            {
                sceneLoader = FindObjectOfType<System_SceneLoader>();
            }
            sceneLoader.LoadScene("Dev_LevelEditor");
        });
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
