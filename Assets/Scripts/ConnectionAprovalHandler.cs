using UnityEngine;
using Unity.Netcode;

public class ConnectionAprovalHandler : MonoBehaviour
{
    private NetworkManager networkManager;

    private void Start()
    {
        networkManager = GetComponent<NetworkManager>();
        if (networkManager != null)
        {
            networkManager.ConnectionApprovalCallback = ApprovalCheck;
            networkManager.OnClientDisconnectCallback += OnClientDisconnectCallback;
        }
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        byte[] bytes = request.Payload;
        string password = System.Text.Encoding.ASCII.GetString(bytes);

        Debug.Log($"Cliente conectando con contraseña: {password}");

        response.Approved = true;
        response.CreatePlayerObject = true;
        //response.Reason = "Testing the declined approval message";
    }

    private void OnClientDisconnectCallback(ulong obj)
    {
        Debug.Log("Me han desconectado");
        // if (!networkManager.IsServer && networkManager.DisconnectReason != string.Empty)
        // {
        //     Debug.Log($"Approval Declined Reason: {networkManager.DisconnectReason}");
        // }
    }

    // private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    // {
    //     // The client identifier to be authenticated
    //     var clientId = request.ClientNetworkId;

    //     // Additional connection data defined by user code
    //     var connectionData = request.Payload;

    //     // Your approval logic determines the following values
    //     response.Approved = true;
    //     response.CreatePlayerObject = true;

    //     // The Prefab hash value of the NetworkPrefab, if null the default NetworkManager player Prefab is used
    //     response.PlayerPrefabHash = null;

    //     // Position to spawn the player object (if null it uses default of Vector3.zero)
    //     response.Position = Vector3.zero;

    //     // Rotation to spawn the player object (if null it uses the default of Quaternion.identity)
    //     response.Rotation = Quaternion.identity;
        
    //     // If response.Approved is false, you can provide a message that explains the reason why via ConnectionApprovalResponse.Reason
    //     // On the client-side, NetworkManager.DisconnectReason will be populated with this message via DisconnectReasonMessage
    //     response.Reason = "Some reason for not approving the client";

    //     // If additional approval steps are needed, set this to true until the additional steps are complete
    //     // once it transitions from true to false the connection approval response will be processed.
    //     response.Pending = false;
    // }
}