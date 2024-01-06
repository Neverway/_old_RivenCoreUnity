//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Entity_Controller_Player", menuName="Neverway/ScriptableObjects/Entity/Controller/Player")]
public class Entity_Controller_Player : Entity_Controller
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    private Vector2 movement;
    private bool is3DEntity;
    
    
    //=-----------------=
    // Reference Variables
    //=-----------------=
    

    //=-----------------=
    // Internal Functions
    //=-----------------=
    public override void EntityAwake(Entity entity)
    {
    }
    
    public override void Think(Entity entity)
    {
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
