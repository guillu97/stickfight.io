using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySocketIO;
using UnitySocketIO.Events;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class JoinRoom : MonoBehaviour
{
    private class RoomName{
        public string roomName;
    }
    private class RoomAvailability{
        public bool roomAvailable;
    }

    public GameObject notAvailableTextMesh;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Join()
    {
        Debug.Log("Join a room is clicked:" + EventSystem.current.currentSelectedGameObject.name);

        string roomName = GameObject.Find(EventSystem.current.currentSelectedGameObject.name).GetComponentInChildren<TextMeshProUGUI>().text;
        roomName = roomName.Split('\n')[0];

        SocketIOController io = GameObject.Find("SocketIOController").GetComponent<SocketIOController>();

        RoomName JSONobj = new RoomName();
        JSONobj.roomName = roomName;

        // socket io emit {join, room name}
        io.Emit("joinRoom",JsonUtility.ToJson(JSONobj), (string data) => {
            Debug.Log(data);
            bool roomAvailable = JsonUtility.FromJson<RoomAvailability>(data).roomAvailable;
            Debug.Log(roomAvailable);
            if(roomAvailable == true){
                    SceneManager.LoadScene("Arena-1", LoadSceneMode.Single);
            }
            else{
                GameObject notAvailableTextMeshInstance = Instantiate(notAvailableTextMesh,new Vector3(411,470,0), Quaternion.identity) as GameObject;
                // wait 1 second and delete message
                //Start the coroutine we define below named ExampleCoroutine.
                StartCoroutine(ExampleCoroutine(notAvailableTextMeshInstance));

            }
        });
    }

    IEnumerator ExampleCoroutine(GameObject notAvailableTextMeshInstance)
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(1);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        Destroy(notAvailableTextMeshInstance);
    }
}
