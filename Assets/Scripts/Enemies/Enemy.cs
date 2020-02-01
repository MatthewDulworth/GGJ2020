using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
   // -------------------------------------------------
   // Variables
   // -------------------------------------------------
   [SerializeField] protected float health;
   [SerializeField] protected float walkSpeed;
   public int healthBar;

   protected States state;
   protected enum States
   {
      hitStun, dead, active, attacking
   }

   // -------------------------------------------------
   // MonoBehaviour
   // -------------------------------------------------
   void Start() { }
   void Update() { }

   // -------------------------------------------------
   // public methods
   // -------------------------------------------------
   public abstract void handleMovement();
   public abstract void handleAttacks();

   protected void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.gameObject.tag == "Attack")
      {
         AttackController attack = collision.gameObject.GetComponent<AttackController>();
         Debug.Log("attacked");
      }
   }


   // -------------------------------------------------
   // Get / Set 
   // -------------------------------------------------
   public float Health { get => health; }
   public float WalkSpeed { get => walkSpeed; }
}
