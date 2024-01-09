//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName="Entity_Controller_Spectator", menuName="Neverway/ScriptableObjects/Entity/Controller/Spectator")]
public class Entity_Controller_Spectator : Entity_Controller
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
    public InputActions.SpectatorActions spectatorActions;
    

    //=-----------------=
    // Internal Functions
    //=-----------------=
    public override void EntityAwake(Entity entity)
    {
        gameInstance = FindObjectOfType<GameInstance>();
        spectatorActions = new InputActions().Spectator;
        spectatorActions.Enable();
    }
    
    public override void Think(Entity entity)
    {
        if (!entity.isPossessed) return;
        
        if (spectatorActions.Pause.WasPressedThisFrame())
        {
            gameInstance.UI_ShowPause();
        }
        
        // Add movement code here
        Debug.Log(spectatorActions.Move.ReadValue<Vector2>());
    }
    
    public override void FixedRateThink(Entity entity)
    {
        if (!entity.isPossessed) return;
        
        if (spectatorActions.Pause.WasPressedThisFrame())
        {
            gameInstance.UI_ShowPause();
        }
        
        // Add movement code here
        Debug.Log(spectatorActions.Move.ReadValue<Vector2>());
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
