using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
   // -------------------------------------------------
   // Editor Variables
   // -------------------------------------------------
   [SerializeField] protected float health;
   [SerializeField] protected float walkSpeed;
   [SerializeField] int healthBar;
   [SerializeField] LayerMask ground;


   // -------------------------------------------------
   // Protected Variables
   // -------------------------------------------------
   protected int hitboxPriority = -1;
   protected float hitstun = 0;
   protected int direction = 1;
   protected Rigidbody2D rb;
   protected GameObject groundCheck;


   // -------------------------------------------------
   // States
   // -------------------------------------------------
   protected bool grounded;
   protected State state;
   protected enum State
   {
      hitStun, dead, active, attacking, inactive
   }


   // -------------------------------------------------
   // MonoBehaviour
   // -------------------------------------------------
   public virtual void Start()
   {
      state = State.active;
      foreach (Transform child in transform)
      {
         if (child.name == "GroundCheck")
         {
            groundCheck = child.gameObject;
         }
      }
      rb = GetComponent<Rigidbody2D>();
   }
   public virtual void Update()
   {
      grounded = Physics2D.OverlapCircle(groundCheck.transform.position, 0.1f, ground);
      handleHitStun();
   }


   // -------------------------------------------------
   // public methods
   // -------------------------------------------------
   public abstract void handleMovement();
   public abstract void handleAttacks();


   // -------------------------------------------------
   // Handle Hits
   // -------------------------------------------------
   protected void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.gameObject.tag == "Attack")
      {
         Hitbox hitbox = collision.gameObject.GetComponent<HitboxController>().hitbox;

         if (hitbox.priority > this.hitboxPriority)
         {
            this.hitboxPriority = hitbox.priority;

            this.hitstun = hitbox.hitstun;
            this.state = State.hitStun;

            float direction = collision.transform.parent.parent.localScale.x * -1;
            rb.velocity = getHitVector(hitbox.knockback, hitbox.knockbackAngle, direction);
            StartCoroutine(ResetPriority(hitbox.hitboxDuration));
         }
      }
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
   // Handle Hitstun
   // -------------------------------------------------
   private void handleHitStun()
   {
      if (state == State.hitStun)
      {
         hitstun -= Time.deltaTime;

         if (hitstun <= 0 && grounded)
         {
            state = State.active;
         }
      }
   }


   // -------------------------------------------------
   // Get / Set 
   // -------------------------------------------------
   public float Health { get => health; }
   public float WalkSpeed { get => walkSpeed; }
   public float HealthBar { get => healthBar; }
}
