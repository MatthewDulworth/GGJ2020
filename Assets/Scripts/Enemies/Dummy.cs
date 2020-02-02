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

      if (state == State.active)
      {
         // Wander();
      }
      else if (state != State.hitStun)
      {
         rb.velocity = Vector2.zero;
      }
   }

   // -------------------------------------------------
   // private methods
   // -------------------------------------------------
   private void Idle() {
      if(grounded) {

      }
   }


   private void Active() {

   }
}
