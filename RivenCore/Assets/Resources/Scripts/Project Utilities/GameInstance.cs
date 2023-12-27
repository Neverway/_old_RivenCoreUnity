//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: A persistant script that stores values of variables globally for
// all players to access
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : MonoBehaviour
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
    public Entity localPlayerCharacter;
    [SerializeField] private GameObject WB_Title;
    [SerializeField] private GameObject WB_Loading;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // Core External Functions
    //=-----------------=
    public void CreateNewPlayerCharacter(Gamemode _gamemode)
    {
        // NEED FUNCTION TO FIND VALID PLAYER START POINT
        var startpoint = GetPlayerStartPoint().transform;
        localPlayerCharacter = Instantiate(_gamemode.playerCharacter, startpoint.position, startpoint.rotation).GetComponent<Entity>();
        localPlayerCharacter.isPossessed = false;
    }
    
    public void CreateNewPlayerCharacter(Gamemode _gamemode, bool _isLocalPlayer)
    {
        // NEED FUNCTION TO FIND VALID PLAYER START POINT
        var startpoint = GetPlayerStartPoint().transform;
        localPlayerCharacter = Instantiate(_gamemode.playerCharacter, startpoint.position, startpoint.rotation).GetComponent<Entity>();
        localPlayerCharacter.isPossessed = _isLocalPlayer;
    }

    public Transform GetPlayerStartPoint()
    {
        var allPossibleStartPoints = FindObjectsOfType<PlayerStart>();
        var allValidStartPoints = new List<PlayerStart>();
        
        foreach (var possibleStartPoint in allPossibleStartPoints)
        {
            if (possibleStartPoint.playerStartFilter == "") //|| possibleStartPoint.playerStartFilter == Insert reference to the playerteam)
            {
                allValidStartPoints.Add(possibleStartPoint);
            }
        }

        if (allValidStartPoints.Count != 0)
        {
            var random = Random.Range(0, allValidStartPoints.Count);
            return allValidStartPoints[random].transform;
        }

        return null;
    }
    
    public void AddWidget(GameObject WidgetBluprint)
    {
        var canvas = FindObjectOfType<Canvas>();
        var newWidget = Instantiate(WidgetBluprint);
        newWidget.transform.SetParent(canvas.transform, false);
        newWidget.transform.localScale = new Vector3(1, 1, 1);
    }
    
    
    //=-----------------=
    // User External Functions
    //=-----------------=
    public void UI_ShowTitle()
    {
        AddWidget(WB_Title);
    }
    public void UI_ShowLoading()
    {
        AddWidget(WB_Loading);
    }
}
