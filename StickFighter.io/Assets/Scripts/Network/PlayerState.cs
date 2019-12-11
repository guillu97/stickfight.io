using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public float moveInput = 0;
    public bool jump = false;
    private Vector3 position;

    public float getMoveInput()
    {
        float cpMoveInput = this.moveInput;
        this.moveInput = 0;
        return cpMoveInput;
    }

    public bool getJump()
    {
        bool cpJump = this.jump;
        this.jump = false;
        return cpJump;
    }

    public void setPosition(Vector3 newPosition){
        this.position = newPosition;
    }

    public Vector3 getPosition(){
        return this.position;
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
