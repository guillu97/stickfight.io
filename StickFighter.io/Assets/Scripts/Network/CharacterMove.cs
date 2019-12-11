using UnityEngine;
using System.Collections;
using UnitySocketIO;
using UnitySocketIO.Events;



[RequireComponent(typeof(BoxCollider2D))]
public class CharacterMove : MonoBehaviour
{
 
    private class Movement {
        public float moveInput;
    }

    SocketIOController io;

    bool isLocalPlayer;

    // script from https://roystan.net/articles/character-controller-2d.html

    [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    float speed = 9;

    [SerializeField, Tooltip("Acceleration while grounded.")]
    float walkAcceleration = 75;

    [SerializeField, Tooltip("Acceleration while in the air.")]
    float airAcceleration = 30;

    [SerializeField, Tooltip("Deceleration applied when character is grounded and not attempting to move.")]
    float groundDeceleration = 70;

    [SerializeField, Tooltip("Max height the character will jump regardless of gravity")]
    float jumpHeight = 4;

    private BoxCollider2D boxCollider;

    private Vector2 velocity;

    private Vector3 oldPosition;


    /// <summary>
    /// Set to true when the character intersects a collider beneath
    /// them in the previous frame.
    /// </summary>
    private bool grounded;

    private void Start()
    {
        io = GameObject.Find("NetworkManager").GetComponent<ConnectToServer>().io;
    }

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
    
    private void Update()
    {
        // if this instance is an instance of the local player
        if(GetComponent<LocalPlayer>().isLocalPlayer){

            
            // sharing position if the position changed
            if(Vector3.Distance(transform.position, oldPosition) > 0.01f){
                Debug.Log(Vector3.Distance(transform.position, oldPosition));
                io.Emit("playerMoved",JsonUtility.ToJson(transform.position));
                oldPosition = transform.position;
            }
            
            




            float moveInput = 0;
            // Use GetAxisRaw to ensure our input is either 0, 1 or -1.
            moveInput = Input.GetAxisRaw("Horizontal");

            if (grounded)
            {
                velocity.y = 0;
                
                if (Input.GetButtonDown("Jump"))
                {
                    io.Emit("jump");
                    // Calculate the velocity required to achieve the target jump height.
                    velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y));
                }
                
            }
            

            float acceleration = grounded ? walkAcceleration : airAcceleration;
            float deceleration = grounded ? groundDeceleration : 0;

            if (moveInput != 0)
            {

                Movement m = new Movement();
                m.moveInput = moveInput;
                io.Emit("moveInput", JsonUtility.ToJson(m));
                velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInput, acceleration * Time.deltaTime);
            }
            else
            {
                velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
            }

            velocity.y += Physics2D.gravity.y * Time.deltaTime;
            transform.Translate(velocity * Time.deltaTime);
            grounded = false;


            // send the transform position if the local player moves
            //if(GetComponent<>)

            

        }
        // if this instance in not the local player
        else
        {
            /*
            float moveInput = 0;
            moveInput = GetComponent<PlayerState>().getMoveInput();
            if (grounded)
            {
                velocity.y = 0;
                if(GetComponent<PlayerState>().getJump())
                {
                    // Calculate the velocity required to achieve the target jump height.
                    velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y));
                }
            }
            
            float acceleration = grounded ? walkAcceleration : airAcceleration;
            float deceleration = grounded ? groundDeceleration : 0;

            if (moveInput != 0)
            {
                velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInput, acceleration * Time.deltaTime);
            }
            else
            {
                velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
            }
            velocity.y += Physics2D.gravity.y * Time.deltaTime;

            transform.Translate(velocity * Time.deltaTime);
            grounded = false;
            */
            //transform.position = GetComponent<PlayerState>().getPosition();
            //Debug.Log(GetComponent<PlayerState>().getPosition());
        }

/*
        float moveInput = 0;
        if(GetComponent<LocalPlayer>().isLocalPlayer){
            // Use GetAxisRaw to ensure our input is either 0, 1 or -1.
            moveInput = Input.GetAxisRaw("Horizontal");
        }
        else{
            moveInput = GetComponent<PlayerState>().getMoveInput();
        }
        if (grounded)
        {
            velocity.y = 0;
            if(GetComponent<LocalPlayer>().isLocalPlayer){
                if (Input.GetButtonDown("Jump"))
                {
                    io.Emit("jump");
                    // Calculate the velocity required to achieve the target jump height.
                    velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y));
                }
            }
            else{
                if(GetComponent<PlayerState>().getJump()){
                    // Calculate the velocity required to achieve the target jump height.
                    velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y));
                }
            }
        }

        float acceleration = grounded ? walkAcceleration : airAcceleration;
        float deceleration = grounded ? groundDeceleration : 0;

        
        if (moveInput != 0)
        {
            if(GetComponent<LocalPlayer>().isLocalPlayer){
                Movement m = new Movement();
                m.moveInput = moveInput;
                io.Emit("moveInput", JsonUtility.ToJson(m));
            }
            velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInput, acceleration * Time.deltaTime);
        }
        else
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
        }

        velocity.y += Physics2D.gravity.y * Time.deltaTime;

        transform.Translate(velocity * Time.deltaTime);

        grounded = false;
*/
        
        // Retrieve all colliders we have intersected after velocity has been applied.
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);

        foreach (Collider2D hit in hits)
        {
            // Ignore our own collider.
            if (hit == boxCollider)
                continue;

            ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

            // Ensure that we are still overlapping this collider.
            // The overlap may no longer exist due to another intersected collider
            // pushing us out of this one.
            if (colliderDistance.isOverlapped)
            {
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);

                // If we intersect an object beneath us, set grounded to true. 
                if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && velocity.y < 0)
                {
                    grounded = true;
                }
            }
        }

        
    }
}