using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 2f;
    Vector2 movement;
    Rigidbody2D rb;

    FacingDirection direction = FacingDirection.left;
    public enum FacingDirection
    {
        left, right
    }

    // Start is called before the first frame update
    void Start()
    {
        //call rigidbody
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //The input from the player needs to be determined and then passed in the to the MovementUpdate which should
        //manage the actual movement of the character.
        Vector2 playerInput = new Vector2();
        MovementUpdate(playerInput);
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        //take the player's arrow key inputs and translate into movement
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.AddForce(movement * speed);
    }

    public bool IsWalking()
    {
        //if there's movement then set iswalking to true
        if (movement.x > 0||movement.x < 0)
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
            Debug.Log("grounded");
            return true;
        }
        else
        {
            Debug.Log("not grounded");
            return false;
        }
    }

    public FacingDirection GetFacingDirection()
    {
        //change directions based on positive/negative force on x axis
        if (movement.x > 0)
        {
            direction = FacingDirection.right;
        }
        else if (movement.x < 0)
        {
            direction = FacingDirection.left;
        }

        return direction;
    }
}
