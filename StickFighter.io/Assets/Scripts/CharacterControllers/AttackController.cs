using UnityEngine;
using UnitySocketIO;
using UnitySocketIO.Events;

namespace Platformer.Mechanics
{
    public class AttackController : MonoBehaviour
    {
        private class PlayerIdJSON{
            public string playerId;
        }

        private PlayerController scriptPlayerController;
        public bool isLeft = true;

        private SocketIOController io;

        void Start()
        {
            scriptPlayerController = 
                GameObject.FindGameObjectWithTag("Knight").GetComponent<PlayerController>();
            io = GameObject.Find("NetworkManager").GetComponent<ConnectToServer>().io;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (isLeft)
            {
                if (scriptPlayerController.isAttackingLeft())
                {
                    if (other.gameObject.tag == "Player")
                    {
                        Debug.Log("Other character died on left");
                        Destroy(other.gameObject);
                        // TODO: io send playerDeath
                        string playerName = other.gameObject.name;
                        string playerId = playerName.Remove(0,6); //Player
                        PlayerIdJSON obj = new PlayerIdJSON();
                        obj.playerId = playerId;
                        io.Emit("playerKilled", JsonUtility.ToJson(obj));
                    }
                }
            }
            else
            {
                if (scriptPlayerController.isAttackingRight())
                {
                    if (other.gameObject.tag == "Player")
                    {
                        Debug.Log("Other character died on right");
                        Destroy(other.gameObject);
                        // TODO: io send playerDeath
                        Destroy(other.gameObject);
                        // TODO: io send playerDeath
                        string playerName = other.gameObject.name;
                        string playerId = playerName.Remove(0,6); //Player
                        PlayerIdJSON obj = new PlayerIdJSON();
                        obj.playerId = playerId;
                        io.Emit("playerKilled", JsonUtility.ToJson(obj));
                    }
                }
            }
        }
    }
}