//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Instance : MonoBehaviour
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


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {

    }

    private void Update()
    {

    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void CreateNewPlayerCharacter(Gamemode _gamemode)
    {
        // NEED FUNCTION TO FIND VALID PLAYER START POINT
        localPlayerCharacter = Instantiate(_gamemode.playerCharacter).GetComponent<Entity>();
        localPlayerCharacter.isPossessed = false;
    }
    
    public void CreateNewPlayerCharacter(Gamemode _gamemode, bool _isLocalPlayer)
    {
        // NEED FUNCTION TO FIND VALID PLAYER START POINT
        localPlayerCharacter = Instantiate(_gamemode.playerCharacter).GetComponent<Entity>();
        localPlayerCharacter.isPossessed = _isLocalPlayer;
    }
}
