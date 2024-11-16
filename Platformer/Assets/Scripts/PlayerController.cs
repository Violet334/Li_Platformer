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
        rb.AddForce(movement * speed);
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    public bool IsWalking()
    {
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
