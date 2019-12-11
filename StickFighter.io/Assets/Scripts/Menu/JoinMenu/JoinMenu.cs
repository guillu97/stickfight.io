using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinMenu : MonoBehaviour
{
    public bool isReturn;
    
    void OnMouseUp(){
        if(isReturn)
        {
            Application.LoadLevel(0);
        }
        /*
        if (isCreate)
        {
            Application.Quit();
        }
        */
    } 
}
