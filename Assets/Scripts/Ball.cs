using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class Ball : NetworkBehaviour
{

    [SerializeField]
    float velocity;

    NetworkVariable<Color> color = new NetworkVariable<Color>(
        readPerm: NetworkVariableReadPermission.Everyone,
        writePerm: NetworkVariableWritePermission.Owner);

    float timer = 0;


    public override void OnNetworkSpawn()
    {
        color.OnValueChanged += OnColorChanged;
    }

    void Update()
    {
        timer += Time.deltaTime;


        if (IsOwner)
        {
            Move();

            bool spacePressed = Input.GetKey(KeyCode.Space);
            if (spacePressed)
            {
                color.Value = Random.ColorHSV();
            }

            // bool enterPressed = Input.GetKey(KeyCode.Return);
            // if (enterPressed)
            // {
            //     Action_ServerRpc(timer);
            // }
        }
    }


    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.position += new Vector3(h, v, 0) * velocity * Time.deltaTime;
    }

    void OnColorChanged(Color previous, Color current)
    {
        GetComponent<SpriteRenderer>().color = current;
    }

}


    // [ServerRpc(RequireOwnership = true)]
    // void Action_ServerRpc(float time, ServerRpcParams rpcParams = default)
    // {
    //     //Debug.Log($"Client: {rpcParams.Receive.SenderClientId} ha pulsado en t: {time}");
    //     Action_ClientRpc(time);
    // }

    // [ClientRpc]
    // void Action_ClientRpc(float time, ClientRpcParams rpcParams = default)
    // {
    //     Debug.Log($"Alguien ha pulsado Enter en t: {time}");
    // }

    // [ServerRpc(RequireOwnership = false)]
    // void Action_ServerRpc(float time, ServerRpcParams rpcParams = default)
    // {
    //     Debug.Log($"Client: {rpcParams.Receive.SenderClientId} ha pulsado en t: {time}");
    // }