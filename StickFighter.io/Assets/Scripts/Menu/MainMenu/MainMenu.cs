using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public bool isJoin;
    public bool isCreate;

    void OnMouseUp(){
        if(isJoin)
        {
            Application.LoadLevel(1);
        }
        if (isCreate)
        {
            Application.LoadLevel(2);
        }
    } 
}
