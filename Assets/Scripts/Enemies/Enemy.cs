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


   // -------------------------------------------------
   // Protected Variables
   // -------------------------------------------------
   protected int hitboxPriority = -1;
   protected float hitstun = 0;
   protected int direction = 1;
   protected Rigidbody2D rb;


   // -------------------------------------------------
   // States
   // -------------------------------------------------
   protected States state;
   protected enum States
   {
      hitStun, dead, active, attacking
   }


   // -------------------------------------------------
   // MonoBehaviour
   // -------------------------------------------------
   public virtual void Start()
   {
      state = States.active;
      rb = GetComponent<Rigidbody2D>();
   }
   public virtual void Update()
   {
      if(state == States.hitStun) 
      {
         hitstun -= Time.deltaTime;
         Debug.Log("stunned");
         if(hitstun <= 0) {
            state = States.active;
            Debug.Log("unstunned");
            hitboxPriority = -1;
         }
      }
   }


   // -------------------------------------------------
   // public methods
   // -------------------------------------------------
   public abstract void handleMovement();
   public abstract void handleAttacks();

   protected void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.gameObject.tag == "Attack")
      {
         Hitbox hitbox = collision.gameObject.GetComponent<HitboxController>().hitbox;

         if (hitbox.priority > this.hitboxPriority)
         {
            this.hitboxPriority = hitbox.priority;
            this.hitstun = hitbox.hitstun;
            this.state = States.hitStun;

            print(hitbox.name + ", " + hitbox.knockback + ", " + hitbox.knockbackAngle + ", " + this.gameObject);

            // float direction = collision.transform.parent.parent.localScale.x;
            // Vector2 hitVector = getHitVector(hitbox.knockbackAngle, hitbox.knockback) * direction;
            rb.velocity = Vector2.up * hitbox.knockback * 30;
         }
      }
   }

   private Vector2 getHitVector(float magnitude, float angle){
      angle = Mathf.Deg2Rad * angle;
      return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * magnitude;
   }


   // -------------------------------------------------
   // Get / Set 
   // -------------------------------------------------
   public float Health { get => health; }
   public float WalkSpeed { get => walkSpeed; }
   public float HealthBar { get => healthBar; }
}
