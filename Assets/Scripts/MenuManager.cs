using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    PlayerManager playerManager;
    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        
        playerManager.OnPlayerAdded.AddListener(UpdateNames);
        playerManager.OnPlayerReadyChanged.AddListener(UpdateReady);
    }

    void UpdateNames()
    {
        int i = 0;
        foreach (Transform t in transform)
        {
            if (i > playerManager.players.Count - 1) break;

            t.GetComponent<Text>().text = $"Jugador {i}";
        
            i++;
        }
    }

    void UpdateReady()
    {
        Color _color;

        int i = 0;
        foreach (Transform t in transform)
        {
            if (i > playerManager.players.Count - 1) break;

            _color = (playerManager.players[i].ready.Value) ? Color.yellow : Color.white;
            
            t.GetComponent<Text>().color = _color;
        
            i++;
        }

        _color = (Player.owner.ready.Value) ? Color.yellow : Color.white;
        // No es la forma!
        GameObject.Find("Ready").GetComponent<Image>().color = _color;
    }
}
