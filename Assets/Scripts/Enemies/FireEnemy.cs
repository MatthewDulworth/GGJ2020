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
   private float rangedCoolDown;
   private List<AttackController> fireballs;


   // -------------------------------------------------
   // MonoBehaviour
   // -------------------------------------------------
   public override void Start()
   {
      base.Start();
      this.state = State.active;
      fireballs = new List<AttackController>();
   }

   public override void Update()
   {
      base.Update();
      rangedCoolDown -= Time.deltaTime;
   }

   public override void FixedUpdate()
   {
      base.FixedUpdate();

      for(int i=fireballs.Count - 1; i >= 0; i--) 
      {
         if(fireballs[i] == null) 
         {
            fireballs.RemoveAt(i);
         }
      }

      foreach(AttackController f in fireballs)
      {
         if(Physics2D.OverlapCircle(f.transform.position, 0.4f, ground)) 
         {
            f.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
         }
      }
   }


   // -------------------------------------------------
   // Active
   // -------------------------------------------------
   protected override void Active()
   {
      if (grounded)
      {
         CheckFlips();

         if (InRange())
         {
            if (coolDown <= 0)
            {
               coolDown = maxCoolDown;
               DoAttack("E_Jab");
            }

            this.rb.velocity = Vector2.right * this.walkSpeed * -TargetDirection();
         }
         else
         {
            this.rb.velocity = Vector2.zero;

            if (rangedCoolDown <= 0)
            {
               rangedCoolDown = rangedCoolDownMax;
               FireAttack();
            }
         }
      }
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
