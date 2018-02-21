using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Network : MonoBehaviour
{
    static SocketIOComponent socket;
    public GameObject playerPrefab;

    public Dictionary<string, GameObject> players;

    // Use this for initialization
    void Start()
    {
        socket = GetComponent<SocketIOComponent>();
        socket.On("open", OnConnected);
        socket.On("spawn player", OnSpawned);
        socket.On("disconnected", OnDisconnected);
        players = new Dictionary<string, GameObject>();
    }

    // Tells us we are connected
    void OnConnected(SocketIOEvent e)
    {
        Debug.Log("We are connected");
        socket.Emit("playerhere");
    }

    void OnDisconnected(SocketIOEvent e)
    {
        Debug.Log("Player disconnected " + e.data);

        var id = e.data["id"].ToString();
        var player = players[id];

        Destroy(player);
        players.Remove(id);

    }

    void OnSpawned(SocketIOEvent e)
    {
        Debug.Log("Player Spawned!" + e.data);

        players.Add(e.data["id"].ToString(), Instantiate(playerPrefab));
        Debug.Log("Count " + players.Count);
    }
}
