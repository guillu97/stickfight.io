using UnityEngine;
using UnitySocketIO;
using UnitySocketIO.Events;

namespace Platformer.Mechanics
{
    public class PlayerController : MonoBehaviour
    {
        
        public float maxSpeed = 7;
        public float jumpTakeOffSpeed = 7;

        //private bool isAttacking = false;
        private bool isGrounded = false;

        private BoxCollider2D boxCollider;

        private Rigidbody2D rb;
        //private bool isLookingToRight = true;
        private bool jump;
        private Animator anim;
        private Vector3 eulerAngles;

        private float moveInput;
        private BoxCollider2D deathFloor;



        public Vector2 targetedVelocity;


        private Vector3 oldPosition;
        private Vector3 oldEulerAngle;
        private Vector2 oldVelocity;


        private SocketIOController io;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            anim = GetComponent<Animator>();
            eulerAngles =  transform.eulerAngles;

            io = GameObject.Find("NetworkManager").GetComponent<ConnectToServer>().io;
            deathFloor = GameObject.Find("DeathFloor").GetComponent<BoxCollider2D>();
        }

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            boxCollider = GetComponent<BoxCollider2D>();
        }

        protected void Update()
        {
            if(GetComponent<LocalPlayer>().isLocalPlayer){
                if(Input.GetMouseButtonDown(0))
                {
                    anim.SetTrigger("Attack");
                    GetComponent<PlayerState>().isAttacking = true;
                    
                    // TODO: io send attack
                    io.Emit("attack");
                }
                
                targetedVelocity = Vector2.zero;

                moveInput = Input.GetAxisRaw("Horizontal");

                
                
                if (moveInput < 0)
                {
                    targetedVelocity.x = -1 * maxSpeed;
                    GetComponent<PlayerState>().isLookingToRight = false;
                    eulerAngles.y = -120;
                    transform.eulerAngles = eulerAngles;
                }
                else if (moveInput > 0)
                {
                    targetedVelocity.x = maxSpeed;
                    GetComponent<PlayerState>().isLookingToRight = true;
                    eulerAngles.y = 120;
                    transform.eulerAngles = eulerAngles;
                }

                if (Input.GetButtonDown("Jump"))
                {
                    jump = true;
                    Debug.Log("Need to jump");

                    io.Emit("jump");
                }

                
                ComputeVelocity(targetedVelocity);

                if(GetComponent<LocalPlayer>().isLocalPlayer){

                    if(targetedVelocity != oldVelocity)
                    {
                        io.Emit("velocityChanged", JsonUtility.ToJson(targetedVelocity));
                        oldVelocity = targetedVelocity;
                    }

                    // sharing position if the position changed
                    if(Vector3.Distance(transform.position, oldPosition) > 0.02f){
                        Debug.Log(Vector3.Distance(transform.position, oldPosition));
                        io.Emit("playerMoved",JsonUtility.ToJson(transform.position));
                        oldPosition = transform.position;
                    }

                    if(transform.eulerAngles != oldEulerAngle)
                    {
                        Debug.Log("Player rotated");
                        io.Emit("playerRotated",JsonUtility.ToJson(transform.eulerAngles));
                        oldEulerAngle = transform.eulerAngles;
                    }
                }
            }
        }

        public void ComputeVelocity(Vector2 velocity)
        {

            if (jump)
            {
                velocity.y = jumpTakeOffSpeed;
                jump = false;
            }
            

            if (velocity.x == 0)
            {
                anim.SetBool("IsRunning", false);
            }
            else
            {
                anim.SetBool("IsRunning", true);
            }

            rb.velocity = velocity;
        }

        private void EndAttack()
        {
            GetComponent<PlayerState>().isAttacking = false;
            // TODO: io send attack end
            io.Emit("endAttack");
        }

        public bool isAttackingLeft()
        {
            return GetComponent<PlayerState>().isAttacking && !GetComponent<PlayerState>().isLookingToRight;
        }

        public bool isAttackingRight()
        {
            return GetComponent<PlayerState>().isAttacking && GetComponent<PlayerState>().isLookingToRight;
        }
    }
}