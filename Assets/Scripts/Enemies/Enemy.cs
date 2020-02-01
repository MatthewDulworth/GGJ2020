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


   // -------------------------------------------------
   // Protected Variables
   // -------------------------------------------------
   protected int hitStunPriority = -1;
   protected float hitStunTimer = 0;
   protected int direction = 1;


   // -------------------------------------------------
   // States
   // -------------------------------------------------
   protected States state;
   protected enum States
   {
      hitStun, dead, active, attacking
   }


   // -------------------------------------------------
   // MonoBehaviour
   // -------------------------------------------------
   public virtual void Start() 
   { 
      state = States.active;
   }
   public virtual void Update() 
   {

   }


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
         
         hitStunPriority++;
      }
   }


   // -------------------------------------------------
   // Get / Set 
   // -------------------------------------------------
   public float Health { get => health; }
   public float WalkSpeed { get => walkSpeed; }
   public float HealthBar { get => healthBar; }
}
