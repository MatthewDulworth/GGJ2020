using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
   // -------------------------------------------------
   // Variables 
   // -------------------------------------------------
   [SerializeField] private float health = 100;
   [SerializeField] private float maxPriorityDelay = 0.5f;
   private State state = State.alive;
   private AttackMachine attackMachine;
   private ControlScheme cntrlSchm;

   //Sprite Facing
   private Direction dir;
   private SpriteRenderer sprtRend;

   //Animation
   private Animator anim;
   [SerializeField] private float spriteFlipDelay;

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

   //Roll Variables
   public float rollSpeed;
   public float rollCooldown;
   public float rollDuration;
   private bool rolling;
   private float rollTimer;
   private bool rollKeyDown;

   //Attack Variables
   public GameObject attack;
   private bool attacking;
   public Attack[] attacks;

   // Hit Vars
   private int hitboxPriority = -1;
   private float hitstun = 0;

    // Run sound
    private AudioSource runSound;
    private AudioSource hitSound;
   public enum State
   {
      alive, dead, disabled
   }
   public enum Direction
   {
      left, right
   }


   // -------------------------------------------------
   // Start
   // -------------------------------------------------
   void Start()
   {
      cntrlSchm = GetComponent<ControlScheme>();
      cntrlSchm.SetControlScheme();
      attackMachine = new AttackMachine(cntrlSchm, attacks);
      jumpKeyUp = true;
      dir = Direction.left;
      sprtRend = GetComponent<SpriteRenderer>();
      rb = GetComponent<Rigidbody2D>();
      foreach (Transform child in transform)
      {
         if (child.name == "GroundCheck")
         {
            groundCheck = child.gameObject;
         }
         if(child.name == "Run Sound")
            {
                runSound = child.gameObject.GetComponent<AudioSource>();
            }
            if (child.name == "Hit Sound")
            {
                hitSound = child.gameObject.GetComponent<AudioSource>();
            }
        }
      anim = GetComponent<Animator>();
   }

   // -------------------------------------------------
   // Collision
   // -------------------------------------------------
   private void OnCollisionEnter2D(Collision2D collision)
   {
      if (collision.gameObject.tag == "Ground")
      {
         rb.velocity *= 1.5f;
      }
   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.gameObject.tag == "EnemyAttack")
      {
         Hitbox hitbox = collision.gameObject.GetComponent<HitboxController>().hitbox;

         if (hitbox.priority > this.hitboxPriority)
         {
            this.hitboxPriority = hitbox.priority;

            float direction = 1;
            if (collision.transform.parent.parent != null)
            {
               direction = collision.transform.parent.parent.localScale.x * -1;
            }

            rb.velocity = getHitVector(hitbox.knockback, hitbox.knockbackAngle, direction);
            StartCoroutine(ResetPriority(Mathf.Min(hitbox.hitboxDuration, maxPriorityDelay)));

           // GameObject.Find("Preloaded").GetComponent<EffectsController>().CameraShake(hitbox.shakeDuration, hitbox.shakeIntensity);
            Time.timeScale = 1 - hitbox.shakeIntensity;
            Invoke("ResetTimeScale", .2f);
                hitSound.Play();
                collision.gameObject.GetComponent<ParticleSystem>().Play();
                // damage
                this.health -= hitbox.damage;
            if(this.health <= 0) {
               Die();
            }
         }
      }
   }
    private void ResetTimeScale()
    {
        Time.timeScale = 1;
    }
    IEnumerator ResetPriority(float delay)
   {
      yield return new WaitForSeconds(delay);
      hitboxPriority = -1;
   }

   private Vector2 getHitVector(float magnitude, float angle, float direction)
   {
      angle *= Mathf.Deg2Rad;
      return new Vector2(Mathf.Cos(angle) * direction, Mathf.Sin(angle)) * magnitude;
   }


   // -------------------------------------------------
   // Fixed Update
   // -------------------------------------------------
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
         if (rollTimer >= 0)
         {
            rollTimer -= Time.deltaTime;
         }
      }
   }

   private void Update()
   {
      if (!cntrlSchm.RollPressed())
      {
         rollKeyDown = false;
      }
   }
   // -------------------------------------------------
   // Movement
   // -------------------------------------------------
   public bool CheckRoll()
   {
      return cntrlSchm.RollPressed();
   }

   public void CheckPlayerMovement()
   {
      //Grounded Checks
      grounded = Physics2D.OverlapCircle(groundCheck.transform.position, .2f, ground);
      anim.SetBool("Grounded", grounded);

      //Horiz. vert. input 
      float horizontalInput = cntrlSchm.HorizontalInput();
      float verticalInput = cntrlSchm.VerticalInput();

      //Walk 
      rb.AddForce(playerSpeed * horizontalInput * transform.right);
      if (horizontalInput != 0)
      {
         if (!runSound.isPlaying)
         {
            runSound.Play();
         }
         anim.SetBool("Running", true);
      }
      else
      {
         runSound.Stop();
         anim.SetBool("Running", false);
      }
      if (Mathf.Abs(rb.velocity.x) > maxSpeedX)
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
      else if (jumpTimer < 0)
      {
         if (!cntrlSchm.JumpPressed() && !grounded)
         {
            jumpKeyUp = true;
            if (rb.velocity.y > 0)
            {
               rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 4);
            }
         }
      }

      //Roll
      if (cntrlSchm.RollPressed() && rollTimer < 0)
      {
         int direction = (dir == Direction.left) ? -1 : 1;
         rb.velocity = new Vector2(rollSpeed * (direction), rb.velocity.y * 1.5f);

         rolling = true;
         rollTimer = rollCooldown;
         Invoke("ResetRoll", rollDuration);
         rollKeyDown = true;
      }
   }

   private void ResetRoll()
   {
      rolling = false;
   }

   public void FlipCharacter()
   {
      transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
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


   // -------------------------------------------------
   // Attacks
   // -------------------------------------------------
   public void CheckAttack()
   {
      attackMachine.Update(grounded, CheckAttacking());

      if (attackMachine.Attacking)
      {
         attacking = true;
         GetComponent<AudioSource>().Play();
         AttackController spawnedAttack = Instantiate(attack, transform).GetComponent<AttackController>();
         spawnedAttack.isAerial = attackMachine.IsAerial;
         spawnedAttack.SetAttack(attackMachine.CurrentAttack);
      }
   }

   //Is the player attacking?
   public bool CheckAttacking()
   {
      foreach (Transform e in transform)
      {
         if (e.tag == "Attack")
         {
            return true;
         }
      }
      return false;
   }


   // -------------------------------------------------
   // Death
   // -------------------------------------------------
   // Will include death animation, effects, probably slow down and sound effect
   public void Die()
   {
      //   if (!GameObject.Find("Spawn Controller").GetComponent<SpawnController>().gameover)
      //   {
      //       Debug.Log("Player Death");
      //       GameObject.Find("Spawn Controller").GetComponent<SpawnController>().gameover = true;
      //       GameObject.Find("Game Over Screen").GetComponent<MoveTowards>().enabled = true;
      //       GameObject.Find("Game Over Text").GetComponent<Text>().text += GameObject.Find("Spawn Controller").GetComponent<SpawnController>().currentWave;
      //       state = State.dead;
      //       anim.speed = 0;
      //       runSound.Stop();
      //   }
   }
}




