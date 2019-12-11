using UnityEngine;
using System.Collections;
using UnitySocketIO;
using UnitySocketIO.Events;


public class ConnectToServer : MonoBehaviour {

/*
    [System.Serializable]
    private class Players{
        public string playerId;
        //public Position pos;
        public Vector3 pos;
    }

    [System.Serializable]
    private class ListPlayers{
        public List<Players> Players = new List<Players>();
    }
    */

    

/*
    private class Position{
        public Vector3 pos;
    }
*/

    private class PlayerIdJSON{
        public string playerId;
    }

    private class Movement {
        public string playerId;
        public float moveInput;
    }

    private class PlayerPos {
        public string playerId;
        public Vector3 pos;
    }

    public SocketIOController io;
    public GameObject playerPrefab;

	void Start() {
        io = GameObject.Find("SocketIOController").GetComponent<SocketIOController>();
        io.On("connect", (SocketIOEvent e) => {
            Debug.Log("SocketIO connected");
            
            

            io.Emit("test-event1");

            TestObject t = new TestObject();
            t.test = 123;
            t.test2 = "test1";

            io.Emit("test-event2", JsonUtility.ToJson(t));

            TestObject t2 = new TestObject();
            t2.test = 1234;
            t2.test2 = "test2";

            io.Emit("test-event3", JsonUtility.ToJson(t2), (string data) => {
                Debug.Log(data);
            });

        });

        io.On("allPlayersPositions", (SocketIOEvent ev) => {
            Debug.Log("allPlayersPositions received");
            Debug.Log(ev.data.ToString());
            PlayersList playersList = new PlayersList();
            playersList = JsonUtility.FromJson<PlayersList>(ev.data);
            foreach (Players player in playersList.Players)
            {
                Debug.Log(player.playerId);
                Debug.Log(player.pos);
                string playerName = "Player" + player.playerId;
                GameObject playerInstance = Instantiate(playerPrefab, player.pos, Quaternion.identity);
                playerInstance.name = playerName;
                LocalPlayer localPlayer = playerInstance.GetComponent<LocalPlayer>();
                localPlayer.playerId = player.playerId;
            }
        });

        // delete an instance of the disconnected player
        io.On("playerDisconnected", (SocketIOEvent ev) => {
            string playerId = JsonUtility.FromJson<PlayerIdJSON>(ev.data).playerId;
            string playerName = "Player" + playerId;
            Destroy(GameObject.Find(playerName));
        });

        io.On("assignPlayerId", (SocketIOEvent ev) => {
            Debug.Log("assignPlayerId received");
            string playerId = JsonUtility.FromJson<PlayerIdJSON>(ev.data).playerId;
            Debug.Log(playerId);
            string playerName = "Player" + playerId;
            // Instantiate at position (0, 0, 0) and zero rotation.
            GameObject player = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            player.name = playerName;
            LocalPlayer localPlayer = player.GetComponent<LocalPlayer>();
            localPlayer.isLocalPlayer = true;
            localPlayer.playerId = playerId;
        });

        io.On("moveInput", (SocketIOEvent ev) => {
            Debug.Log("moveInput received : " + ev.data.ToString());
            Movement movement = JsonUtility.FromJson<Movement>(ev.data);
            string playerName = "Player" + movement.playerId;
            GameObject.Find(playerName).GetComponent<PlayerState>().moveInput = movement.moveInput; 
        });

        io.On("playerMoved",(SocketIOEvent ev) => {
            Debug.Log("playerMoved received : " + ev.data.ToString());
            PlayerPos playerPos = JsonUtility.FromJson<PlayerPos>(ev.data);
            string playerId = playerPos.playerId;
            string playerName = "Player" + playerId;
            Debug.Log(playerName);
            GameObject playerToMove = GameObject.Find(playerName);
            
            if(playerToMove == null){
                playerToMove = Instantiate(playerPrefab, playerPos.pos, Quaternion.identity);
                playerToMove.name = playerName;
                playerToMove.GetComponent<LocalPlayer>().setPlayerId(playerId);
            }
            playerToMove.transform.position = playerPos.pos;
            Debug.Log("playerPos.pos");
            Debug.Log(playerPos.pos);
            /*if(playerToMove != null){
                playerToMove.transform.position = playerPos.pos;
            }
            else{
                Debug.Log("cannot find the playerToMove!");
            }*/
        });

        io.On("jump", (SocketIOEvent ev) => {
            Debug.Log("jump received");
            string playerName = "Player" + ev.data;
            GameObject.Find(playerName).GetComponent<PlayerState>().jump = true;
        });
        

        /*io.On("anotherPlayerConnected", (SocketIOEvent e) => {
            Debug.Log(e.data);
            Instantiate(myPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        });*/

        io.Connect();

        io.On("test-event", (SocketIOEvent e) => {
            Debug.Log(e.data);
        });
    }

}
