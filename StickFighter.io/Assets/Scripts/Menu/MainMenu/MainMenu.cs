using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public bool isJoin;
    public bool isCreate;

    void OnMouseUp(){
        if(isJoin)
        {
            SceneManager.LoadScene("JoinMenu", LoadSceneMode.Single);
        }
        if (isCreate)
        {
            SceneManager.LoadScene("CreateMenu", LoadSceneMode.Single);
        }
    } 
}
