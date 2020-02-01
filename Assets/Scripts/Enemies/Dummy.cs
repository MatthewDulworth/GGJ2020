using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Enemy
{
   // -------------------------------------------------
   // Variables
   // -------------------------------------------------
   [SerializeField] private float maxWanderDist;
   [SerializeField] private float maxChoiceTime;
   private Vector3 startPos;
   private float choiceTimer = 0;

   // -------------------------------------------------
   // MonoBehaviour
   // -------------------------------------------------
   public override void Start()
   {
      base.Start();
      startPos = this.transform.position;
   }
   public override void Update()
   {
      base.Update();
      handleMovement();
   }

   // -------------------------------------------------
   // public methods
   // -------------------------------------------------
   public override void handleMovement()
   {
      if (state == States.active)
      {
         Wander();
      }
      else if (state != States.hitStun)
      {
         rb.velocity = Vector2.zero;
      }
   }

   public override void handleAttacks()
   {

   }

   // -------------------------------------------------
   // private methods
   // -------------------------------------------------
   private void Wander()
   {
      this.choiceTimer -= Time.deltaTime;

      if(Mathf.Abs(this.startPos.x - this.transform.position.x) > this.maxWanderDist){
         this.direction *= -1;
      }
      else if (choiceTimer <= 0)
      {
         this.choiceTimer = this.maxChoiceTime;
         float choice = Random.Range(0f, 1.0f);

         if (choice < 0.5)
         {
            this.direction = 0;
         } 
         else if (choice < 0.7)
         {
            if(this.direction == 0) {
               this.direction = 1;
            }
            this.direction *= -1;
         }
      }

      this.rb.velocity = Vector2.right * this.walkSpeed * direction;
   }
}
