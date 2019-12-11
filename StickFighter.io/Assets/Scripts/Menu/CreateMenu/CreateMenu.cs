using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMenu : MonoBehaviour
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
