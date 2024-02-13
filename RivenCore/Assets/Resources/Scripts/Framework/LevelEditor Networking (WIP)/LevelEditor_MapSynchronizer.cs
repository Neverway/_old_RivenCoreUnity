//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LevelEditor_MapSynchronizer : NetworkBehaviour
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
    
    

    //=-----------------=
    // Internal Functions
    //=-----------------=
    // SYNC MAP WHEN JOINING HOST
    // Upon joining a server,
    // get the host to save the map to a buffer json string
    // get the host to send the json string to the new client
    // get the client to save the map to a buffer file
    // clear the map path to avoid saving over the buffer
    // get the new client to load the json string as map data


    //=-----------------=
    // External Functions
    //=-----------------=
}
