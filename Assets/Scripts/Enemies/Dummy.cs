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
   private Rigidbody2D rb;
   private Vector3 startLocation;
   private float choiceTimer = 0;

   // -------------------------------------------------
   // MonoBehaviour
   // -------------------------------------------------
   void Start()
   {
      startLocation = this.transform.position;
      state = States.active;
   }
   void Update() { }

   // -------------------------------------------------
   // public methods
   // -------------------------------------------------
   public override void handleMovement()
   {
      if (state == States.active)
      {
         Wander();
      }
      else if(state != States.hitStun) {
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
      choiceTimer -= Time.deltaTime * Random.Range(0.5f, 1);
      if (choiceTimer <= 0)
      {
         choiceTimer = maxChoiceTime;
         int direction = (Random.Range(0, 1) < 0.5) ? 1 : -1;
         rb.velocity = Vector2.right * walkSpeed * direction;
      }
   }
}
