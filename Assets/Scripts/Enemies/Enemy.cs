using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
   // -------------------------------------------------
   // Editor Variables
   // -------------------------------------------------
   [SerializeField] protected float health;
    private float totalHealth;
   [SerializeField] protected float walkSpeed;
   [SerializeField] protected float range;
   [SerializeField] protected LayerMask ground;
   [SerializeField] protected GameObject attack;
   [SerializeField] protected Attack[] attacks;
   [SerializeField] protected float maxCoolDown;

   // -------------------------------------------------
   // Protected Variables
   // -------------------------------------------------
   protected float coolDown = 0;
   protected int hitboxPriority = -1;
   protected float hitstun = 0;
   protected bool facingLeft = true;
   protected Rigidbody2D rb;
   protected GameObject groundCheck;
   protected bool attacking;
   protected Transform target;

   protected const int left = 1;
   protected const int right = -1;

    protected GameObject healthBar;

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
      totalHealth = health;
      target = FindObjectOfType<PlayerController>().transform;
      state = State.idle;
      foreach (Transform child in transform)
      {
         if (child.name == "GroundCheck")
         {
            groundCheck = child.gameObject;
         }
         if(child.name == "Health Bar")
            {
                healthBar = child.gameObject;
            }
      }
      rb = GetComponent<Rigidbody2D>();
      StartCoroutine(Init(0.5f));
   }

   public virtual void Update()
   {
      grounded = Physics2D.OverlapCircle(groundCheck.transform.position, 0.1f, ground);
      handleHitStun();

      if (coolDown > 0)
      {
         coolDown -= Time.deltaTime;
      }

      var newColor = GetComponent<SpriteRenderer>().color;
      newColor = new Color(newColor.r + Time.deltaTime, newColor.g + Time.deltaTime, newColor.b + Time.deltaTime);
      GetComponent<SpriteRenderer>().color = newColor;
   }

   public virtual void FixedUpdate()
   {
      attacking = CheckAttacking();

      if (state == State.active)
      {
         Active();
      }
      else if (state == State.idle)
      {
         Idle();
      }
   }


   // -------------------------------------------------
   // Handle Hits
   // -------------------------------------------------
   protected virtual void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.gameObject.tag == "Attack")
      {
         Hitbox hitbox = collision.gameObject.GetComponent<HitboxController>().hitbox;

         if (hitbox.priority > this.hitboxPriority)
         {
            this.hitboxPriority = hitbox.priority;
            HaltAttacks();

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
                healthBar.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100 * health / totalHealth);

            this.health -= hitbox.damage;
            if (this.health <= 0)
            {
               Die();
            }
         }
      }
   }

   protected void HaltAttacks()
   {
      foreach (Transform t in transform)
      {
         if (t.CompareTag("EnemyAttack"))
         {
            Destroy(t.gameObject);
         }
      }
   }

   protected void ResetTimeScale()
   {
      Time.timeScale = 1;
   }

   protected IEnumerator ResetPriority(float delay)
   {
      yield return new WaitForSeconds(delay);
      hitboxPriority = -1;
   }

   protected Vector2 getHitVector(float magnitude, float angle, float direction)
   {
      angle *= Mathf.Deg2Rad;
      return new Vector2(Mathf.Cos(angle) * direction, Mathf.Sin(angle)) * magnitude;
   }


   // -------------------------------------------------
   // Handle Hitstun
   // -------------------------------------------------
   protected virtual void handleHitStun()
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

   protected void Ready()
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

   public AttackController DoAttack(string name)
   {
      AttackController spawnedAttack = Instantiate(this.attack, this.transform).GetComponent<AttackController>();
      spawnedAttack.isAerial = false;
      spawnedAttack.SetAttack(FindAttack(name));
      attacking = true;
      return spawnedAttack;
   }


   // -------------------------------------------------
   // Targeting
   // -------------------------------------------------
   protected float TargetDirection()
   {
      return Mathf.Sign(this.transform.position.x - target.transform.position.x);
   }

   protected float TargetDist()
   {
      return Vector2.Distance(this.transform.position, target.position);
   }

   protected bool InRange()
   {
      return TargetDist() <= this.range;
   }

   protected void CheckFlips()
   {
      if (TargetDirection() == left && !facingLeft)
      {
         Flip();
         facingLeft = true;
      }
      else if (TargetDirection() == right && facingLeft)
      {
         Flip();
         facingLeft = false;
      }
   }


   // -------------------------------------------------
   // States
   // -------------------------------------------------
   protected virtual void Active() { }

   protected virtual void Idle() { }

   protected virtual void Die()
   {
      Destroy(gameObject);
   }

   private IEnumerator Init(float delay)
   {
      yield return new WaitForSeconds(delay);
      state = State.active;
   }

   // -------------------------------------------------
   // Get / Set 
   // -------------------------------------------------
   public float Health { get => health; }
   public float WalkSpeed { get => walkSpeed; }
}