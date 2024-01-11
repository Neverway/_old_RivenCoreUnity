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
    [Tooltip("This will determine how far out a player character can travel before being killed")]
    public int deathBarrierRange;
    public bool enableLevelDeathBarrier;
    public bool debugShowDeathBarrierRange;


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
            gameInstance.CreateNewPlayerCharacter(defaultLevelGamemode, true);
        }
    }

    private void Update()
    {
        CheckForOutOfBoundsEntities();
    }

    void OnDrawGizmos()
    {
        if (!debugShowDeathBarrierRange) return;
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawWireCube(transform.position, new Vector3(deathBarrierRange*2, deathBarrierRange*2, deathBarrierRange*2));
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void CheckForOutOfBoundsEntities()
    {
        foreach (var _entity in FindObjectsOfType<Entity>())
        {
            var distanceToEntity = Vector3.Distance(_entity.gameObject.transform.position,  new Vector3(0,0,0));
            if (distanceToEntity >= deathBarrierRange || distanceToEntity <= (deathBarrierRange * -1))
            {
                Destroy(_entity.gameObject);
            }
        }
    }



    //=-----------------=
    // External Functions
    //=-----------------=
}
