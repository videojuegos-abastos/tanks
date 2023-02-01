using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{

    [SerializeField]
    float velocity;

    NetworkVariable<Color> color = new NetworkVariable<Color>(
        readPerm: NetworkVariableReadPermission.Everyone,
        writePerm: NetworkVariableWritePermission.Owner);

    void Start()
    {
        color.OnValueChanged += OnColorChanged;

    }

    void OnColorChanged3()
    {
        Debug.Log("Color has changed");
    }

    void Update()
    {


        if (IsOwner)
        {
            Move();

            bool spacePressed = Input.GetKey(KeyCode.Space);
            if (spacePressed)
            {
                color.Value = Random.ColorHSV();
            }
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