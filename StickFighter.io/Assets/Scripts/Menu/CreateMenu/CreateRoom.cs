using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnitySocketIO;
using UnitySocketIO.Events;
using TMPro;
using UnityEngine.SceneManagement;

public class CreateRoom : MonoBehaviour
{

    private class RoomName{
        public string roomName;
    }

    private class RoomAvailability{
        public bool roomAvailable;
    }

    public  TextMeshProUGUI roomNameTextMesh;

    public TextMeshProUGUI notAvailableTextMesh;

    private  string roomName;

    public  void SendName(){

        roomName = roomNameTextMesh.text;
        

        if(roomName != ""){
            SocketIOController io = GameObject.Find("SocketIOController").GetComponent<SocketIOController>();
            Debug.Log("room name received: " + roomName);
            RoomName JSONobj = new RoomName();
            JSONobj.roomName = roomName;
            Debug.Log("test:"+JSONobj.roomName+":");
            // then send the name to the server
            io.Emit("createRoom",JsonUtility.ToJson(JSONobj),(string data) => {
                Debug.Log(data);
                bool roomAvailable = JsonUtility.FromJson<RoomAvailability>(data).roomAvailable;
                Debug.Log(roomAvailable);
                if(roomAvailable == true){
                    SceneManager.LoadScene("Arena-1", LoadSceneMode.Single);
                }
                else{
                    notAvailableTextMesh.gameObject.SetActive(true);
                }
            });
        }
        

    }
}
