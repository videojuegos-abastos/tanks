using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    const int MAX_PLAYERS = 4;

    public UnityEvent OnPlayerAdded = new UnityEvent();
    public UnityEvent OnPlayerReadyChanged = new UnityEvent();

    public List<Player> players { get; private set; } = new List<Player>();

    public void AddPlayer(Player _player)
    {
        // Comprobamos máximo de jugadores
        // Habría que desconectar al jugador
        if (players.Count >= MAX_PLAYERS) return;

        players.Add(_player);
        _player.ready.OnValueChanged += OnReadyChanged;

        OnPlayerAdded.Invoke();
    }

    void OnReadyChanged(bool previous, bool current)
    {
        OnPlayerReadyChanged.Invoke();
    }

}
