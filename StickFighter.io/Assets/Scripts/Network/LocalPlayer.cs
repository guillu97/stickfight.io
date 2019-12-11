using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer : MonoBehaviour
{
    public bool isLocalPlayer = false;
    public string playerId = "";

    public void setPlayerId(string newPlayerId){
        this.playerId = newPlayerId;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
