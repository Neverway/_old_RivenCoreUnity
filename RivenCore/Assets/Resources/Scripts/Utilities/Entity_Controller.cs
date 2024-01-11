//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity_Controller : ScriptableObject
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public abstract void EntityAwake(Entity _entity);
    public abstract void Think(Entity _entity);
    public abstract void FixedRateThink(Entity _entity);


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
