using UnityEngine;
using System.Collections;
using UnitySocketIO;
using UnitySocketIO.Events;

using Platformer.Mechanics;


public class ConnectToServer : MonoBehaviour {


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

    private class PlayerVelocity {
        public string playerId;
        public Vector2 velocity;
    }

    [HideInInspector] // Hides var below
    public SocketIOController io;
    public GameObject playerPrefab;
    public GameObject playerSpawn;

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
                playerInstance.tag = "Player";
                /*LocalPlayer localPlayer = playerInstance.GetComponent<LocalPlayer>();
                localPlayer.playerId = player.playerId;
                */
            }
        });

        // delete an instance of the disconnected player
        io.On("playerDisconnected", (SocketIOEvent ev) => {
            string playerId = JsonUtility.FromJson<PlayerIdJSON>(ev.data).playerId;
            string playerName = "Player" + playerId;
            Destroy(GameObject.Find(playerName));
        });

        io.On("spawnPlayer", (SocketIOEvent ev) => {
            playerSpawn.GetComponent<SpawnPlayer>().spawnLocalPlayer();
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
                playerToMove.tag = "Player";
                playerToMove.GetComponent<LocalPlayer>().setPlayerId(playerId);
            }

            // TODO: playerToMove.Move(player.pos);

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

        io.On("playerRotated",(SocketIOEvent ev) => {
            Debug.Log("playerRotated received : " + ev.data.ToString());
            PlayerPos playerPos = JsonUtility.FromJson<PlayerPos>(ev.data);
            string playerId = playerPos.playerId;
            string playerName = "Player" + playerId;
            Debug.Log(playerName);
            GameObject playerToMove = GameObject.Find(playerName);

            playerToMove.transform.eulerAngles = playerPos.pos;
            Debug.Log("playerPos.pos");
            Debug.Log(playerPos.pos);
        });

        io.On("velocityChanged",(SocketIOEvent ev) => {
            Debug.Log("velocityChanged received : " + ev.data.ToString());
            PlayerVelocity playerVelocity = JsonUtility.FromJson<PlayerVelocity>(ev.data);
            string playerId = playerVelocity.playerId;
            string playerName = "Player" + playerId;
            Debug.Log(playerName);
            GameObject playerToMove = GameObject.Find(playerName);

            playerToMove.GetComponent<PlayerController>().ComputeVelocity(playerVelocity.velocity);
            /*if (playerVelocity.velocity.x == 0)
            {
                playerToMove.GetComponent<Animator>.SetBool("IsRunning", false);
            }
            else
            {
                playerToMove.GetComponent<Animator>.SetBool("IsRunning", true);
            }*/
            Debug.Log("playerVelocity.velocity");
            Debug.Log(playerVelocity.velocity);
        });

        

        io.On("playerAttack",(SocketIOEvent ev) => {
            Debug.Log("playerAttack received");
            string playerId = JsonUtility.FromJson<PlayerIdJSON>(ev.data).playerId;
            string playerName = "Player" + playerId;
            GameObject playerInstance = GameObject.Find(playerName);
            playerInstance.GetComponent<PlayerState>().isAttacking = true;
            playerInstance.GetComponent<Animator>().SetTrigger("Attack");
        });

        io.On("playerEndAttack",(SocketIOEvent ev) => {
            Debug.Log("playerAttack received");
            string playerId = JsonUtility.FromJson<PlayerIdJSON>(ev.data).playerId;
            string playerName = "Player" + playerId;
            GameObject.Find(playerName).GetComponent<PlayerState>().isAttacking = false;
        });

        io.On("jump", (SocketIOEvent ev) => {
            Debug.Log("jump received");
            string playerId = JsonUtility.FromJson<PlayerIdJSON>(ev.data).playerId;
            string playerName = "Player" + playerId;
            GameObject.Find(playerName).GetComponent<PlayerState>().jump = true;
        });
        

        io.On("playerKilled", (SocketIOEvent ev) => {
            Debug.Log("another player killed by a player");
            string playerId = JsonUtility.FromJson<PlayerIdJSON>(ev.data).playerId;
            string playerName = "Player" + playerId;
            Destroy(GameObject.Find(playerName));
        });

        io.On("playerDeathByDeathFloor", (SocketIOEvent ev) => {
            Debug.Log("another player killed by death floor");
            string playerId = JsonUtility.FromJson<PlayerIdJSON>(ev.data).playerId;
            string playerName = "Player" + playerId;
            Destroy(GameObject.Find(playerName));
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
