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
    public bool grounded;
    private GameObject groundCheck;
    public bool jumpKeyUp;
    public float minJumpTime;
    private float jumpTimer;

    //Fast Fall variable
    public float fastFallMultiplier;

    //Roll Variables
    public float rollSpeed;
    public float rollCooldown;
    public float rollDuration;
    private bool rolling;    
    private float rollTimer;

    //Attack Variables
    public GameObject attack;
    private bool attacking;
    public Attack[] attacks;
    
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
            attacking = CheckAttacking();
            if (!rolling)
            {
                if (!attacking)
                {
                    CheckFlip();
                    CheckAttack();
                }
                if (!(attacking && grounded))
                {
                    CheckPlayerMovement();
                }
            }
            if (jumpTimer >= 0)
            {
                jumpTimer -= Time.deltaTime;
            }
            if(rollTimer >= 0)
            {
                rollTimer -= Time.deltaTime;
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
            rb.velocity = new Vector2(rb.velocity.x / 1.2f, rb.velocity.y);
        }
        //Check if jmp key down
        if (grounded)
        {
            jumpKeyUp = true;
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
        
        //Fast Fall
        if (verticalInput < 0 && !grounded)
        {
            rb.AddForce(-transform.up * fastFallMultiplier, ForceMode2D.Impulse);
        }

        //Roll
        if (cntrlSchm.RollPressed() && rollTimer < 0)
        {
            int direction = (dir == Direction.left) ? -1 : 1;
            rb.velocity = new Vector2(rollSpeed * (direction), rb.velocity.y / 1.5f);

            rolling = true;
            rollTimer = rollCooldown;
            Invoke("ResetRoll", rollDuration);
        }
    }
    private void ResetRoll()
    {
        rolling = false;
    }
    public void CheckAttack()
    {
        
        //Horiz. vert. input 
        float horizontalInput = cntrlSchm.HorizontalInput();
        float verticalInput = cntrlSchm.VerticalInput();

        Attack attackInputted = null;
        if (Input.GetAxis(cntrlSchm.AttackAxis) > 0)
        {
            attackInputted = FindAttack("Jab");
        }
        //Upward attack
        else if (Input.GetAxis(cntrlSchm.AttackAxis) > 0 && verticalInput > 0)
        {
            attackInputted = FindAttack("Upward Attack");
            attacking = true;
        }

        else if(Input.GetAxis(cntrlSchm.AttackAxis) > 0 && verticalInput < 0)
        {

        }
        if(attackInputted != null)
        {
            var spawnedAttack = Instantiate(attack, transform);
            spawnedAttack.GetComponent<AttackController>().SetAttack(attackInputted);
        }
        
    }
    //Will include death animation, effects, probably slow down and sound effect
    public void Die()
    {

    }
    public void FlipCharacter()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
    }
    private Attack FindAttack(string name)
    {
        foreach(Attack e in attacks)
        {
            if (e.name.Equals(name))
            {
                return e;
            }
        }
        return null;
    }
    //Is the player attacking?
    private bool CheckAttacking()
    {
        foreach(Transform e in transform)
        {
            if(e.tag == "Attack")
            {
                return true;
            }
        }
        return false;
    }

    private void CheckFlip()
    {
        //Horiz. vert. input 
        float horizontalInput = cntrlSchm.HorizontalInput();

        //Sprite Flip
        if (horizontalInput < 0 && dir == Direction.right)
        {
            dir = Direction.left;
            Invoke("FlipCharacter", spriteFlipDelay);
        }
        else if (horizontalInput > 0 && dir == Direction.left)
        {
            dir = Direction.right;
            Invoke("FlipCharacter", spriteFlipDelay);
        }
    }
}




