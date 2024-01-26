//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Entity_Controller_Base", menuName="Neverway/ScriptableObjects/Entity/Controller/Base")]
public class Entity_Controller_Base : Entity_Controller
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
    

    //=-----------------=
    // Internal Functions
    //=-----------------=
    public override void EntityAwake(Entity entity)
    {
    }
    
    public override void Update(Entity entity)
    {
        if (!entity.isPossessed) return;
        if (entity.isPaused) return;
    }
    
    public override void FixedUpdate(Entity entity)
    {
        if (!entity.isPossessed) return;
        if (entity.isPaused) return;
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
