using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private State state = State.alive;
    private ControlScheme cntrlSchm;

    //Sprite Facing
    private Direction dir;
    private SpriteRenderer sprtRend;

    //Animation
    private Animator anim;
    [SerializeField]
    private float spriteFlipDelay;

    //Side Movement
    public float playerSpeed;
    private Rigidbody2D rb;
    public float maxSpeedX;

    //Jump variables
    public float jumpForce;
    public LayerMask ground;
    private bool grounded;
    private GameObject groundCheck;
    private bool jumpKeyUp;
    public float minJumpTime;
    private float jumpTimer;

    //Fast Fall variable
    public float fastFallMultiplier;

    //Attack cooldown
    private bool attacking;
    
    public enum State
    {
        alive, dead, disabled
    }
    public enum Direction
    {
        left, right
    }
    // Start is called before the first frame update
    void Start()
    {
        cntrlSchm = GetComponent<ControlScheme>();
        cntrlSchm.SetControlScheme();
        jumpKeyUp = true;
        dir = Direction.left;
        sprtRend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        foreach(Transform child in transform)
        {
            if(child.name == "GroundCheck")
            {
                groundCheck = child.gameObject;
            }

        }
        anim = GetComponent<Animator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
    
    private void FixedUpdate()
    {
        if (state == State.alive)
        {
            CheckPlayerMovement();
            if (jumpTimer >= 0)
            {
                jumpTimer -= Time.deltaTime;
            }
        }
    }
    public bool CheckRoll()
    {
        return cntrlSchm.RollPressed();
    }
    public void CheckPlayerMovement()
    {
        //Grounded Checks
        grounded = Physics2D.OverlapCircle(groundCheck.transform.position, .4f, ground);
        anim.SetBool("Grounded", grounded);

        //Horiz. vert. input 
        float horizontalInput = cntrlSchm.HorizontalInput();
        float verticalInput = cntrlSchm.VerticalInput();

        //Sprite Flip
        if(horizontalInput < 0 && dir == Direction.right)
        {
            dir = Direction.left;
            Invoke("FlipCharacter", spriteFlipDelay);
        }
        else if(horizontalInput > 0 && dir == Direction.left)
        {
            dir = Direction.right;
            Invoke("FlipCharacter", spriteFlipDelay);
        }

        //Walk 
        rb.AddForce(playerSpeed * horizontalInput * transform.right);
        if(horizontalInput != 0)
        {
            anim.SetBool("Running", true);
        }
        else
        {
            anim.SetBool("Running", false);
        }
        if(Mathf.Abs(rb.velocity.x) > maxSpeedX)
        {
            rb.velocity = new Vector2(rb.velocity.x / 1.1f, rb.velocity.y);
        }
        //Jump
        if (cntrlSchm.JumpPressed() && grounded && jumpKeyUp)
        {
            jumpTimer = minJumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            
            jumpKeyUp = false;
        }
        //Smaller Jump
        else if (jumpTimer < 0) { 
             if (!cntrlSchm.JumpPressed() && !grounded)
             {
                jumpKeyUp = true;
                if (rb.velocity.y > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 4);
                }
            }
        }
        if(!cntrlSchm.JumpPressed())
        {
            jumpKeyUp = true;
        }
        //Fast Fall
        if (verticalInput < 0 && !grounded)
        {
            rb.AddForce(-transform.up * fastFallMultiplier, ForceMode2D.Impulse);
        }
    }
    public void CheckAttack()
    {
        if (!attacking)
        {
            //Horiz. vert. input 
            float horizontalInput = cntrlSchm.HorizontalInput();
            float verticalInput = cntrlSchm.VerticalInput();

            //Upward attack
            if (Input.GetAxis(cntrlSchm.AttackAxis) > 0 && verticalInput > 0)
            {
                attacking = true;
            }
        }
    }
    //Will include death animation, effects, probably slow down and sound effect
    public void Die()
    {

    }
    public void FlipCharacter()
    {
        sprtRend.flipX = !sprtRend.flipX;
    }
}




