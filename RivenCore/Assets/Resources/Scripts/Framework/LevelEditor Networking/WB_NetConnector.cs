//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class WB_NetConnector : MonoBehaviour
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
    private UnityTransport unityTransport;
    private NetworkManager networkManager;
    [SerializeField] private TMP_InputField hostAddressField;
    [SerializeField] private TMP_InputField joinAddressField;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        unityTransport = FindObjectOfType<UnityTransport>();
        networkManager = FindObjectOfType<NetworkManager>();
    }
    

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void SendAddressToTransport(bool _isHostAddress)
    {
        string targetAddress = "";
        if (_isHostAddress) targetAddress = hostAddressField.text.Trim(); // Get the text from the input field and trim any leading or trailing spaces
        else targetAddress = joinAddressField.text.Trim(); // Get the text from the input field and trim any leading or trailing spaces

        // Check if the host address contains a port separator (":")
        int portSeparatorIndex = targetAddress.IndexOf(':');
        if (portSeparatorIndex != -1)
        {
            // Extract the address and port
            string address = targetAddress.Substring(0, portSeparatorIndex);
            string portString = targetAddress.Substring(portSeparatorIndex + 1);

            // Try parsing the port string to an integer
            if (ushort.TryParse(portString, out ushort port))
            {
                // Set the address and port in the UnityTransport
                unityTransport.SetConnectionData(address, port);
                Debug.Log($"Address: {address}, Port: {port}");
            }
            else
            {
                Debug.LogError("Invalid port number.");
            }
        }
        else
        {
            Debug.LogError("Invalid address format. Please include the port number (e.g., 192.168.0.1:25565).");
        }
    }

    public void StartHost()
    {
        networkManager.StartHost();
    }

    public void StartClient()
    {
        networkManager.StartClient();
    }

    public void ShutdownConnection()
    {
        networkManager.Shutdown();
    }
}
