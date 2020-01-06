using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySocketIO;
using UnitySocketIO.Events;
using UnityEngine.UI;
using TMPro;

public class RefreshRooms : MonoBehaviour
{
    
    [System.Serializable]
    private class RoomsList{
        public List<Room> Rooms = new List<Room>();
    }

    [System.Serializable]
    private class Room{
        public string roomName;
        public int numPlayers;
    }

    public GameObject buttonPrefab;
    public GameObject content;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        RefreshRoomsList();
    }

    public void RefreshRoomsList()
    {
        Debug.Log("refresh room list");
        SocketIOController io = GameObject.Find("SocketIOController").GetComponent<SocketIOController>();
        io.Emit("refreshRoomsList", JsonUtility.ToJson(""), (string data) => {
            Debug.Log(data);
            RoomsList roomsList = new RoomsList();
            roomsList = JsonUtility.FromJson<RoomsList>(data);
            foreach (Room room in roomsList.Rooms)
            {
                Debug.Log(room.roomName);
                GameObject buttonPrefabInstance = Instantiate(buttonPrefab,content.transform, false) as GameObject;
                buttonPrefabInstance.name = "Button" + room.roomName;
                //buttonPrefabInstance.transform.parent = content.transform;
                buttonPrefabInstance.GetComponentInChildren<TextMeshProUGUI>().text = room.roomName + "\nnumber of players: " + room.numPlayers.ToString() + "/4";
            }

        });
    }
}
