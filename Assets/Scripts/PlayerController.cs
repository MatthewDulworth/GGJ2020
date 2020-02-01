using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //PlayerInfo
    PlayerInfo info;
    //Reference to current stage
    StageController stageCntrl;
    //Controller
    public ControlScheme.Controller controller;
    private ControlScheme cntrlSchm;
   
    //Shooting variables
    private GunController gun;

    //Sprite Facing
    private Direction dir;
    private SpriteRenderer sprtRend;

    //Animation
    private Animator anim;
    public float spriteFlipDelay;

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

    //Wall Jump
    //The object that is actually used for wall jump detection
    private GameObject wallCheck;
    //The object the wallCheck swaps position with when turning around
    private GameObject altWallCheckPos;
    private bool checkIsLeft = true;
    private GameObject rightWallCheck;
    public float wallJumpSpeed;
    public GameObject reflectHitbox;
    
    public enum Direction
    {
        left, right
    }
    // Start is called before the first frame update
    void Start()
    {
        jumpTimer = minJumpTime;
        jumpKeyUp = true;
        dir = Direction.left;
        sprtRend = GetComponent<SpriteRenderer>();
        cntrlSchm = GetComponent<ControlScheme>();
        cntrlSchm.SetControlScheme(controller);
        rb = GetComponent<Rigidbody2D>();
        foreach(Transform child in transform)
        {
            if(child.name == "GroundCheck")
            {
                groundCheck = child.gameObject;
            }
            if(child.name == "AltWallCheck")
            {
                altWallCheckPos = child.gameObject;
            }
            if(child.name == "WallCheck")
            {
                wallCheck = child.gameObject;
            }

        }
        anim = GetComponent<Animator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            info.CurrentState = PlayerInfo.State.dead;
            print(gameObject.name + " got hit by " + collision.gameObject.GetComponent<ProjectileController>().idString());
            stageCntrl.CheckRoundEnd();
            Physics2D.IgnoreCollision(transform.GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>(), true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Portal"))
        {
            collision.GetComponent<PortalController>().CheckTeleport(gameObject);
        }
        else if (collision.gameObject.tag.Equals("KillZone"))
        {
            Die();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (info.CurrentState == PlayerInfo.State.alive)
        {
            CheckShoot();
        }
    }
    private void FixedUpdate()
    {
        if (info.CurrentState == PlayerInfo.State.alive)
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

        //Wall Jump Checks
        bool onWallCheck = Physics2D.OverlapCircle(wallCheck.transform.position, .3f, ground);
        anim.SetBool("WallSliding", onWallCheck);

        //Horiz. vert. input 
        float horizontalInput = cntrlSchm.HorizontalInput();
        float verticalInput = cntrlSchm.VerticalInput();

        //Sprite Flip
        if(horizontalInput < 0 && dir == Direction.right)
        {
            dir = Direction.left;
            gun.gameObject.GetComponent<SpriteRenderer>().sortingOrder = sprtRend.sortingOrder + 1;
            Invoke("DoFlip", spriteFlipDelay);

        }
        else if(horizontalInput > 0 && dir == Direction.left)
        {
            dir = Direction.right;
            gun.gameObject.GetComponent<SpriteRenderer>().sortingOrder = sprtRend.sortingOrder - 1;
            Invoke("DoFlip", spriteFlipDelay);

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
        //Wall Jump
        else if(cntrlSchm.JumpPressed() && jumpKeyUp && (onWallCheck))
        {
            jumpTimer = minJumpTime;
            if (onWallCheck)
            {
                Vector2 jumpVel = new Vector2(wallJumpSpeed / 2, wallJumpSpeed);
                if(!checkIsLeft)
                {
                    jumpVel = new Vector2(-jumpVel.x, jumpVel.y);
                }
                rb.velocity = jumpVel;
            }
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
            rb.AddForce(-transform.up * fastFallMultiplier * 3, ForceMode2D.Impulse);
        }
    }

    public void CheckShoot()
    {
        if (cntrlSchm.ShootPressed())
        {
            gun.Shoot();
        }
    }
    public ControlScheme GetControlScheme()
    {
        return GetComponent<ControlScheme>();
    }
    public bool isDead()
    {
        return info.CurrentState == PlayerInfo.State.dead;
    }
    public PlayerInfo.PlayerID GetID()
    {
        return info.ID;
    }

    /*
     * Set values for variables based on the new PlayerInfo
     */
        // Hi my name jeff
    public void SetInfo(PlayerInfo newInfo, StageController newStage)
    {
        info = newInfo;
        cntrlSchm = info.CntrlSchm;
        controller = cntrlSchm.Type;
        stageCntrl = newStage;
        foreach(Transform e in transform)
        {
            if (e.name.Contains("Tag"))
            {
                e.GetChild(0).GetComponent<Text>().text = newInfo.GamerTag;
                e.GetChild(0).GetComponent<Text>().color = PlayerInfo.colors[info.ColorIndex];
            }
        }
        var colorSwapper = GetComponent<ColorSwapController>();
        colorSwapper.SetupPlayerColor(PlayerInfo.colors[info.ColorIndex]);
        gun = GetComponentInChildren<GunController>();
        var gunColorSwapper = gun.GetComponent<ColorSwapController>();
        gunColorSwapper.SetupPlayerColor(PlayerInfo.colors[info.ColorIndex]);
        gun.SetInfo(newInfo);
    }
    //Will include death animation, effects, probably slow down and sound effect
    public void Die()
    {

    }
    public void DoFlip()
    {
        Vector2 mainCheckPos = wallCheck.transform.localPosition;
        wallCheck.transform.localPosition = altWallCheckPos.transform.localPosition;
        altWallCheckPos.transform.localPosition = mainCheckPos;
        checkIsLeft = !checkIsLeft;

        sprtRend.flipX = !sprtRend.flipX;
    }
}




