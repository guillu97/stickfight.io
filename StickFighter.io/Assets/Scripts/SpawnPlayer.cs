using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySocketIO;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject playerPrefab;

    private SocketIOController io;
    // Start is called before the first frame update
    void Start()
    {
        io = GameObject.Find("SocketIOController").GetComponent<SocketIOController>();
        spawnLocalPlayer();
        requestPlayersPosition();
        // TODO : request start => check if there are 4 players in the room
    }

    public void spawnLocalPlayer(){
        // create the scene name of the player with the socketID
        string playerName = "Player" + io.SocketID;
        // Instantiate at the position of the playerSpawn object and zero rotation.
        GameObject player = Instantiate(playerPrefab, transform.position, Quaternion.identity);

            
        player.name = playerName;
        LocalPlayer localPlayer = player.GetComponent<LocalPlayer>();
        localPlayer.isLocalPlayer = true;
        localPlayer.tag = "Knight";
        localPlayer.playerId = io.SocketID;
    }

    public void requestPlayersPosition()
    {
        io.Emit("requestPlayersPos");
    }
}
