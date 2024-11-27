using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 2f;
    Vector2 movement;
    Rigidbody2D rb;

    public float maxSpeed;
    public float accelerationTime;
    float acceleration;
    float deceleration;
    public float decelerationTime;
    bool isJumping;

    //jumping variables
    public float apexHeight;
    public float apexTime;
    float jumpSpeed;

    //terminal speed
    public float terminalSpeed;

    //coyote time
    float timer = 0;
    public float coyoteTime;

    public int health = 10;

    FacingDirection direction = FacingDirection.left;
    public enum FacingDirection
    {
        left, right
    }

    public enum CharacterState
    {
        idle, walk, jump, die
    }
    public CharacterState currentChatacterState = CharacterState.idle;
    public CharacterState prevCharacterState = CharacterState.idle;

    // Start is called before the first frame update
    void Start()
    {
        //call rigidbody
        rb = GetComponent<Rigidbody2D>();

        //get acceleration
        acceleration = maxSpeed / accelerationTime;
        deceleration = maxSpeed / decelerationTime;
        //jumpSpeed = apexHeight / apexTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(prevCharacterState != currentChatacterState)
        {

        }
        //The input from the player needs to be determined and then passed in the to the MovementUpdate which should
        //manage the actual movement of the character.
        if (IsGrounded() && Input.GetKeyDown(KeyCode.UpArrow))
        {
            isJumping = true;
        }

        switch (currentChatacterState)
        {
            case CharacterState.die:

                break;

            case CharacterState.walk:
                if (!IsWalking())
                {
                    currentChatacterState = CharacterState.idle;
                }
                if (!IsGrounded())
                {
                    currentChatacterState = CharacterState.jump;
                }
                break;
            case CharacterState.idle:
                if (IsWalking())
                {
                    currentChatacterState = CharacterState.walk;
                }
                if (!IsGrounded())
                {
                    Debug.Log("Jump switch triggered");
                    currentChatacterState = CharacterState.jump;
                }
                break;

            case CharacterState.jump:
                Debug.Log("Are we grounded["+IsGrounded().ToString()+"]");
                if (IsGrounded())
                {
                    if (IsWalking())
                    {
                        currentChatacterState = CharacterState.walk;
                    }
                    else
                    {
                        currentChatacterState = CharacterState.idle;
                    }
                }
                break;
        }

        if (IsDead())
        {
            currentChatacterState = CharacterState.die;
        }
    }
    private void FixedUpdate()
    {
        Vector2 playerInput = new Vector2();

        //set player input
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerInput += Vector2.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerInput += Vector2.right;
        }
        if (isJumping)
        {
            playerInput += Vector2.up;
            isJumping = false;
        }

        MovementUpdate(playerInput);
    }
    private void MovementUpdate(Vector2 playerInput)
    {
        /*
        //take the player's arrow key inputs and translate into movement
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        rb.AddForce(movement * speed);*/

        /*//Updated movement
        Vector2 currVelocity = rb.velocity;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            currVelocity += acceleration * Vector2.left * Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            currVelocity -= deceleration * Vector2.left * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            currVelocity += acceleration * Vector2.right * Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            currVelocity -= deceleration * Vector2.left * Time.deltaTime;
        }*/

        //new actually good movement
        Vector2 currVelocity = rb.velocity;
        if (playerInput.x != 0)
        {
            currVelocity += acceleration * playerInput * Time.deltaTime;
        }
        else
        {
            currVelocity = new Vector2(0, currVelocity.y);
        }

        //timer increments when player isn't touching the ground
        if (!IsGrounded())
        {
            timer++;
        }
        /*//add a jump mechanic
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            currVelocity += Vector2.up * jumpSpeed;
        } */
        //add coyote time
        else if (!IsGrounded() && timer < coyoteTime && Input.GetKeyDown(KeyCode.Space))
        {
            currVelocity += Vector2.up * jumpSpeed;
        }
        if (!IsGrounded() && currVelocity.y >= apexHeight)
        {
            currVelocity += Vector2.down * jumpSpeed;
        }
        //terminal speed
        else if (!IsGrounded())
        {
            currVelocity += acceleration * Vector2.down * Time.deltaTime;
        }
        if (Vector3.Magnitude(rb.velocity) < -terminalSpeed)
        {
            acceleration = -terminalSpeed;
        }

        rb.velocity = currVelocity;

        //reset coyote timer when the player hits the ground
        if (IsGrounded())
        {
            timer = 0;
        }
    }

    public bool IsWalking()
    {
        //if there's movement then set iswalking to true
        if (rb.velocity.x != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsGrounded()
    {
        //get reference to ground tilemap layer, then use in linecast to hit that specific collider
        LayerMask ground = LayerMask.GetMask("Ground");
        bool hit = Physics2D.Linecast(transform.position, new Vector2(transform.position.x, transform.position.y - 1f), ground);
        if (hit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsDead()
    {
        return health <= 0;
    }
    public FacingDirection GetFacingDirection()
    {
        //change directions based on positive/negative force on x axis
        if (rb.velocity.x > 0)
        {
            direction = FacingDirection.right;
        }
        else if (rb.velocity.x < 0)
        {
            direction = FacingDirection.left;
        }

        return direction;
    }

}
