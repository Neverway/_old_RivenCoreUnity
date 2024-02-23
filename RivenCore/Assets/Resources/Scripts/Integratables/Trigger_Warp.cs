//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using UnityEngine;

public class Trigger_Warp : Trigger
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public string warpID;
    public string exitWarpID;
    public float exitOffsetX, exitOffsetY;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private Transform exitTransform;
    private Vector3 exitOffset;


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void OnTriggerEnter2D(Collider2D _other)
    {
        base.OnTriggerEnter2D(_other); // Call the base class method
        if (!GetExitWarp()) return; 
        if (targetEnt)
        {
            targetEnt.transform.position = GetExitWarp().position+exitOffset;
            if (targetEnt.isPossessed) { // TODO Add player screen fade here
            }
        }
        if (targetProp)
        {
            targetProp.transform.position = GetExitWarp().position+exitOffset;
        }
    }


    //=-----------------=
    // Internal Functions
    //=-----------------=
    private Transform GetExitWarp()
    {
        exitOffset = new Vector3(exitOffsetX, exitOffsetY);
        
        if (exitWarpID == "") return null;
        
        foreach (var warp in FindObjectsOfType<Trigger_Warp>())
        {
            if (warp.warpID == exitWarpID)
            {
                return warp.transform;
            }
        }

        return null;
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
