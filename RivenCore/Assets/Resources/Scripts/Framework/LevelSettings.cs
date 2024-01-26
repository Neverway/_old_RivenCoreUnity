//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: stores information for how the level should act. At the moment, it
// will take a specified gamemode, which is essentially just an object reference
// that tells the game what type of player to spawn when the level begins. It
// also has parameters for enabling a “Death Barrier Range“ that will automatically
// call the entities death function if an entity is ever found outside of the specified
// range.
// Notes:
//
//=============================================================================

using System;
using UnityEngine;

public class LevelSettings : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Tooltip("This will determine what type of player character should be spawned when the level begins")]
    public Gamemode defaultLevelGamemode;
    [Tooltip("Disable this if you always want to spawn the player at the world origin")]
    public bool usePlayerStart = true;
    [Tooltip("This will determine how far out a player character can travel before being killed")]
    public int levelDeathBarrierRange;
    public bool enableLevelDeathBarrier;
    public bool debugShowLevelDeathBarrierRange;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private GameInstance gameInstance;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        gameInstance = FindObjectOfType<GameInstance>();
        if (!gameInstance.localPlayerCharacter)
        {
            gameInstance.CreateNewPlayerCharacter(defaultLevelGamemode, true, usePlayerStart);
        }
    }

    private void Update()
    {
        if (!enableLevelDeathBarrier) return;
        CheckForOutOfBoundsEntities();
    }

    void OnDrawGizmos()
    {
        if (!debugShowLevelDeathBarrierRange) return;
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawWireCube(transform.position, new Vector3(levelDeathBarrierRange*2, levelDeathBarrierRange*2, levelDeathBarrierRange*2));
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void CheckForOutOfBoundsEntities()
    {
        foreach (var _entity in FindObjectsOfType<Entity>())
        {
            var distanceToEntity = Vector3.Distance(_entity.gameObject.transform.position,  new Vector3(0,0,0));
            if (distanceToEntity >= levelDeathBarrierRange || distanceToEntity <= (levelDeathBarrierRange * -1))
            {
                Destroy(_entity.gameObject);
                // Fail-Safe: If the local player character was the out of bounds entity, spawn the default character specified for the level
                if (!gameInstance.localPlayerCharacter) gameInstance.CreateNewPlayerCharacter(defaultLevelGamemode, true);
            }
        }
    }



    //=-----------------=
    // External Functions
    //=-----------------=
}
