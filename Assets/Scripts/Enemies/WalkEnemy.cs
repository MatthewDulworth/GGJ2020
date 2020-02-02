using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkEnemy : Enemy
{

    // -------------------------------------------------
    // MonoBehaviour
    // -------------------------------------------------
    public override void Start()
   {
      base.Start();
   }
   public override void Update()
   {
      base.Update();
   }
   public override void FixedUpdate()
   {
      base.FixedUpdate();
   }

   // -------------------------------------------------
   // MonoBehaviour
   // -------------------------------------------------
   protected override void Active()
   {
      if(grounded) 
      {
         CheckFlips();

         if(InRange()) {
            this.rb.velocity = Vector2.zero;

            if(coolDown <= 0) 
            {
               coolDown = maxCoolDown;
               DoAttack("E_Jab");
            }
         }
         else {
            this.rb.velocity = Vector2.right * this.walkSpeed * -TargetDirection();
         }
      }
   }
}
