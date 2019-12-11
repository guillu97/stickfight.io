using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour
{
    public GameObject inputField;
    public string roomName;
    public void SendName(){
        roomName = inputField.GetComponent<Text>().text;

        // then send the name to the server

    }
}
