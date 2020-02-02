using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEnemy : Enemy
{
   // -------------------------------------------------
   // MonoBehaviour
   // -------------------------------------------------
   [SerializeField] float rangedCoolDownMax;
   [SerializeField] float launchAngle;
   [SerializeField] float launchForce;
   [SerializeField] float ascendVelocity;
   [SerializeField] float floatHeight;
   [SerializeField] float landStun = 0.5f;
   private float rangedCoolDown;
   private List<AttackController> fireballs;

   private FireState fireState;
   private enum FireState
   {
      floating,
      descending,
      ascending,
      landStun,
   }

   // -------------------------------------------------
   // MonoBehaviour
   // -------------------------------------------------
   public override void Start()
   {
      base.Start();
      fireballs = new List<AttackController>();
      fireState = FireState.descending;
      this.floatHeight = this.transform.position.y + this.floatHeight;
   }

   public override void Update()
   {
      base.Update();
      rangedCoolDown -= Time.deltaTime;
      Debug.Log(this.fireState);
   }

   public override void FixedUpdate()
   {
      base.FixedUpdate();

      for (int i = fireballs.Count - 1; i >= 0; i--)
      {
         if (fireballs[i] == null)
         {
            fireballs.RemoveAt(i);
         }
      }

      foreach (AttackController f in fireballs)
      {
         if (Physics2D.OverlapCircle(f.transform.position, 0.4f, ground))
         {
            f.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
         }
      }
   }


   // -------------------------------------------------
   // Hits
   // -------------------------------------------------
   protected override void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.gameObject.tag == "Attack")
      {
         Hitbox hitbox = collision.gameObject.GetComponent<HitboxController>().hitbox;

         if (hitbox.priority > this.hitboxPriority)
         {
            // state
            if (fireState == FireState.floating)
            {
               fall();
            }
            else if (fireState == FireState.ascending)
            {
               fall();
            }

            this.hitboxPriority = hitbox.priority;
            HaltAttacks();

            this.hitstun = hitbox.hitstun;
            this.state = State.hitStun;

            // knockback
            float direction = collision.transform.parent.parent.localScale.x * -1;
            rb.velocity = getHitVector(hitbox.knockback, hitbox.knockbackAngle, direction);
            StartCoroutine(ResetPriority(hitbox.hitboxDuration));

            // effects
            //GameObject.Find("Preloaded").GetComponent<EffectsController>().CameraShake(hitbox.shakeDuration, hitbox.shakeIntensity);
            Time.timeScale = 1 - hitbox.shakeIntensity;
            Invoke("ResetTimeScale", .3f);
            GetComponent<AudioSource>().Play();
            collision.gameObject.GetComponent<ParticleSystem>().Play();

            this.health -= hitbox.damage;
            if (this.health <= 0)
            {
               Die();
            }
         }
      }
   }

   // -------------------------------------------------
   // Handle Hitstun
   // -------------------------------------------------
   protected override void handleHitStun()
   {
      if (state == State.hitStun)
      {
         hitstun -= Time.deltaTime;

         if (hitstun <= 0)
         {
            state = State.active;
            Invoke("Ready", .2f);
         }
      }
   }

   // -------------------------------------------------
   // Active
   // -------------------------------------------------
   protected override void Active()
   {
      if (this.fireState == FireState.floating)
      {
         CheckFlips();

         if(Mathf.Abs(this.transform.position.x - target.transform.position.x) <= this.range) {
            this.rb.velocity = Vector2.zero;
            if (rangedCoolDown <= 0)
            {
               rangedCoolDown = rangedCoolDownMax;
               FireAttack();
            }
         }
         else 
         {
            this.rb.velocity = Vector2.right * this.walkSpeed * -TargetDirection();
         }

         if(this.transform.position.y > this.floatHeight)
         {
            ascend();
         }

      }

      else if (this.fireState == FireState.ascending)
      {
         if (this.transform.position.y >= this.floatHeight)
         {
            this.rb.velocity = Vector2.zero;
            this.fireState = FireState.floating;
         }

         int modifier = 1;
         if(this.transform.position.y > this.floatHeight) {
            modifier = -1;
         }
         rb.velocity = Vector2.up * ascendVelocity * modifier;
      }

      else if (this.fireState == FireState.descending)
      {
         if (grounded)
         {
            StartCoroutine(LandStun());
            this.fireState = FireState.landStun;
         }
      }
   }

   IEnumerator LandStun() {
      yield return new WaitForSeconds(this.landStun);
      ascend();
   }


   private void fall()
   {
      this.fireState = FireState.descending;
      rb.gravityScale = 1;
   }

   private void ascend()
   {
      this.fireState = FireState.ascending;
      rb.gravityScale = 0;
   }


   // -------------------------------------------------
   // FireAttack
   // -------------------------------------------------
   private void FireAttack()
   {
      AttackController fireball = DoAttack("Fireball");
      fireball.transform.parent = null;
      fireball.gameObject.AddComponent<Rigidbody2D>();
      fireball.GetComponent<Rigidbody2D>().velocity = getHitVector(this.launchForce, this.launchAngle, -TargetDirection());
      fireballs.Add(fireball);
   }
}
