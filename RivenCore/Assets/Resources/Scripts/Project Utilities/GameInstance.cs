//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: A persistant script that stores values of variables globally for
// all players to access
// Notes:
//
//=============================================================================

using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private GameObject WB_Pause;


    //=-----------------=
    // Mono Functions
    //=-----------------=


    //=-----------------=
    // Internal Functions
    //=-----------------=
    private static void SetAllEntitiesIsPaused(bool _isPaused)
    {
        foreach (var entity in FindObjectsOfType<Entity>()) entity.isPaused = _isPaused;
    }


    //=-----------------=
    // Core External Functions
    //=-----------------=
    public void CreateNewPlayerCharacter(Gamemode _gamemode)
    {
        // NEED FUNCTION TO FIND VALID PLAYER START POINT
        var startpoint = GetPlayerStartPoint().transform;
        localPlayerCharacter = Instantiate(_gamemode.playerCharacter, startpoint.position, startpoint.rotation)
            .GetComponent<Entity>();
        localPlayerCharacter.isPossessed = false;
        localPlayerCharacter.name = localPlayerCharacter.name.Replace("(Clone)", "").Trim();
    }

    public void CreateNewPlayerCharacter(Gamemode _gamemode, bool _isLocalPlayer)
    {
        // NEED FUNCTION TO FIND VALID PLAYER START POINT
        var startpoint = GetPlayerStartPoint().transform;
        localPlayerCharacter = Instantiate(_gamemode.playerCharacter, startpoint.position, startpoint.rotation)
            .GetComponent<Entity>();
        localPlayerCharacter.isPossessed = _isLocalPlayer;
        localPlayerCharacter.name = localPlayerCharacter.name.Replace("(Clone)", "").Trim();
    }

    public void CreateNewPlayerCharacter(Gamemode _gamemode, bool _isLocalPlayer, bool _usePlayerStart)
    {
        // NEED FUNCTION TO FIND VALID PLAYER START POINT
        if (_usePlayerStart)
        {
            var startpoint = GetPlayerStartPoint().transform;
            localPlayerCharacter = Instantiate(_gamemode.playerCharacter, startpoint.position, startpoint.rotation)
                .GetComponent<Entity>();
        }
        else
        {
            localPlayerCharacter =
                Instantiate(_gamemode.playerCharacter, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0))
                    .GetComponent<Entity>();
        }

        localPlayerCharacter.isPossessed = _isLocalPlayer;
        localPlayerCharacter.name = localPlayerCharacter.name.Replace("(Clone)", "").Trim();
    }

    private static Transform GetPlayerStartPoint(bool _usePlayerStart = true)
    {
        if (!_usePlayerStart) return null;
        var allPossibleStartPoints = FindObjectsOfType<PlayerStart>();
        var allValidStartPoints = allPossibleStartPoints.Where(_possibleStartPoint => _possibleStartPoint.playerStartFilter == "").ToList();

        if (allValidStartPoints.Count == 0) return null;
        var random = Random.Range(0, allValidStartPoints.Count);
        return allValidStartPoints[random].transform;

    }

    public static void AddWidget(GameObject _widgetBlueprint)
    {
        var canvas = FindObjectOfType<Canvas>();
        var newWidget = Instantiate(_widgetBlueprint, canvas.transform, false);
        newWidget.transform.localScale = new Vector3(1, 1, 1);
        newWidget.name = newWidget.name.Replace("(Clone)", "").Trim();
    }

    private static GameObject GetWidget(string _widgetName)
    {
        var canvas = FindObjectOfType<Canvas>();
        for (var i = 0; i < canvas.transform.childCount; i++)
        {
            var widget = canvas.transform.GetChild(i).gameObject;
            if (widget.name == _widgetName) return widget;
        }

        return null;
    }


    //=-----------------=
    // User External Functions
    //=-----------------=
    public void UI_ShowTitle() { AddWidget(WB_Title); }

    public void UI_ShowLoading() { AddWidget(WB_Loading); }

    public void UI_ShowPause()
    {
        if (GetWidget("WB_Pause") == null) { AddWidget(WB_Pause); SetAllEntitiesIsPaused(true); }
        else { Destroy(GetWidget("WB_Pause")); SetAllEntitiesIsPaused(false); }
    }
}