//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using Unity.Netcode;
using UnityEngine;

public class RpcTest : NetworkBehaviour
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
    // Mono Functions
    //=-----------------=
    public override void OnNetworkSpawn()
    {
        // If we're not the owner of this object
        if (!IsOwner)
        {
            // Set child object to active
            transform.GetChild(0).gameObject.SetActive(true);
        }        
    }

    public void Update()
    {
        // If we're the owner of this object
        // Set this objects position to cursor position
        if (IsOwner) transform.position = GameObject.FindGameObjectWithTag("CRTG_Cursor").transform.position;
    }    

    
    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}