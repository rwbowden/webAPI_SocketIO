using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;

public class Network : MonoBehaviour
{
    public static SocketIOComponent socket;
    public GameObject playerPrefab;

    public Dictionary<string, GameObject> players;

    // Use this for initialization
    void Start()
    {
        socket = GetComponent<SocketIOComponent>();
        socket.On("open", OnConnected);
        socket.On("spawn player", OnSpawned);
        socket.On("disconnected", OnDisconnected);
        socket.On("move", OnMove);
        players = new Dictionary<string, GameObject>();
    }

    void OnMove(SocketIOEvent e)
    {
        //Debug.Log("Networked player is moving" + e.data);
        var id = e.data["id"].ToString();

        var player = players[id];


        var netMove = player.GetComponent<CharacterMovement>();

        var pos = new Vector3(GetFloatFromJson(e.data, "x"), 0 ,GetFloatFromJson(e.data, "y"));

        var h = GetFloatFromJson(e.data, "h");
        var v = GetFloatFromJson(e.data, "v");

        netMove.NetworkMovement(pos, h, v);

        Debug.Log("Pos: " + pos);
    }

    float GetFloatFromJson(JSONObject data, string key)
    {
        return float.Parse(data[key].ToString().Replace("\"", ""));
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
