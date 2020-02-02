using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEnemy : Enemy
{
   // -------------------------------------------------
   // MonoBehaviour
   // -------------------------------------------------
   [SerializeField] float rangedCoolDownMax;
   private float rangedCoolDown;


   // -------------------------------------------------
   // MonoBehaviour
   // -------------------------------------------------
   public override void Start()
   {
      base.Start();
      this.state = State.active;
   }

   public override void Update()
   {
      base.Update();
      rangedCoolDown -= Time.deltaTime;
   }

   public override void FixedUpdate()
   {
      base.FixedUpdate();
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
      Debug.Log("Yeet");
   }
}
