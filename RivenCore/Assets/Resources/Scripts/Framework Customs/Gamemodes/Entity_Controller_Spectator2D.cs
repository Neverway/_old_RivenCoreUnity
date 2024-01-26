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

[CreateAssetMenu(fileName="Entity_Controller_Spectator2D", menuName="Neverway/ScriptableObjects/Entity/Controller/Spectator2D")]
public class Entity_Controller_Spectator2D : Entity_Controller
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
        
        if (spectatorActions.Pause.WasPressedThisFrame()) gameInstance.UI_ShowPause();
        
        if (entity.isPaused) return;
        
        var movement = spectatorActions.Move.ReadValue<Vector2>();
        entity.Move(new Vector3(movement.x, movement.y, 0), "translate");
    }
    
    public override void FixedRateThink(Entity entity)
    {
        if (!entity.isPossessed) return;
        if (entity.isPaused) return;
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
