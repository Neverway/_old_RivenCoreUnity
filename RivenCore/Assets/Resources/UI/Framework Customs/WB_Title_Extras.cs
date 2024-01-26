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
    [SerializeField] private Button Bttn_Back;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        Bttn_Back.onClick.AddListener(delegate { OnClick("Back"); });
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void OnClick(string button)
    {
        if (button == "Back")
        {
            Destroy(gameObject);
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
