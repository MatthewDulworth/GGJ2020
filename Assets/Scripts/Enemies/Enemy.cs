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
   [SerializeField] protected GameObject attack;
   [SerializeField] private Attack[] attacks;


   // -------------------------------------------------
   // Protected Variables
   // -------------------------------------------------
   protected int hitboxPriority = -1;
   protected float hitstun = 0;
   protected int direction = 1;
   protected Rigidbody2D rb;
   protected GameObject groundCheck;
   protected bool attacking;
   protected Transform target;


   // -------------------------------------------------
   // States
   // -------------------------------------------------
   public bool grounded;
   protected State state;
   protected enum State
   {
      hitStun,
      landStun,
      dead,
      active,
      idle
   }


   // -------------------------------------------------
   // MonoBehaviour
   // -------------------------------------------------
   public virtual void Start()
   {
      target = FindObjectOfType<PlayerController>().transform;
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
   public virtual void FixedUpdate()
   {
      attacking = CheckAttacking();
   }


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

            // knockback
            float direction = collision.transform.parent.parent.localScale.x * -1;
            rb.velocity = getHitVector(hitbox.knockback, hitbox.knockbackAngle, direction);
            StartCoroutine(ResetPriority(hitbox.hitboxDuration));
            
            // effects
            GameObject.Find("Preloaded").GetComponent<EffectsController>().CameraShake(hitbox.shakeDuration, hitbox.shakeIntensity);
            Time.timeScale = 1 - hitbox.shakeIntensity;
            Invoke("ResetTimeScale", .3f);
            GetComponent<AudioSource>().Play();
            collision.gameObject.GetComponent<ParticleSystem>().Play();

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
   // Handle Hitstun
   // -------------------------------------------------
   private void handleHitStun()
   {
      if (state == State.hitStun)
      {
         hitstun -= Time.deltaTime;

         if (hitstun <= 0 && grounded)
         {
            state = State.landStun;
            Invoke("Ready", .2f);
         }
      }
   }

   private void Ready()
   {
      if (state == State.landStun)
      {
         state = State.active;
      }
   }

   protected void Flip()
   {
      transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
   }


   // -------------------------------------------------
   // Attacks 
   // -------------------------------------------------
   protected Attack FindAttack(string name)
   {
      foreach (Attack attack in attacks)
      {
         if (attack.name.Equals(name))
         {
            return attack;
         }
      }
      return null;
   }

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

   public void DoAttack(string name)
   {
      AttackController spawnedAttack = Instantiate(this.attack, this.transform).GetComponent<AttackController>();
      spawnedAttack.isAerial = false;
      spawnedAttack.SetAttack(FindAttack(name));
      attacking = true;
   }


   // -------------------------------------------------
   // Targeting
   // -------------------------------------------------
   protected float TargetDirection()
   {
      return this.transform.position.x - target.transform.position.x;
   }

   protected float TargetDist()
   {
      return Vector2.Distance(this.transform.position, target.position);
   }


   // -------------------------------------------------
   // Death
   // -------------------------------------------------
   private void Die() 
   {
      Debug.Log("Enemy Killed");
   }

   // -------------------------------------------------
   // Get / Set 
   // -------------------------------------------------
   public float Health { get => health; }
   public float WalkSpeed { get => walkSpeed; }
   public float HealthBar { get => healthBar; }
}