using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateMenu : MonoBehaviour
{
    public bool isReturn;
    
    void OnMouseUp(){
        if(isReturn)
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
        /*
        if (isCreate)
        {
            Application.Quit();
        }
        */
    } 
}
