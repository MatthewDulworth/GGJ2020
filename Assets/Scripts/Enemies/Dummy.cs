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
   [SerializeField] private Attack[] attacks;
   private Vector3 startPos;
   private float choiceTimer = 0;

   // -------------------------------------------------
   // MonoBehaviour
   // -------------------------------------------------
   public override void Start()
   {
      base.Start();
      startPos = this.transform.position;
      this.state = State.active;
   }
   public override void Update()
   {
      base.Update();

      if (state == State.active)
      {
         Active();
      }
   }

   // -------------------------------------------------
   // private methods
   // -------------------------------------------------
   private void Idle()
   {
      if (grounded)
      {

      }
   }

   private void Active()
   {
      choiceTimer -= Time.deltaTime;

      if(choiceTimer <= 0) {
         choiceTimer = maxChoiceTime;
         AttackController spawnedAttack = Instantiate(attack, transform).GetComponent<AttackController>();
         spawnedAttack.gameObject.tag = "EnemyAttack";
         spawnedAttack.isAerial = false;
         spawnedAttack.SetAttack(FindAttack("Jab"));
         Debug.Log(spawnedAttack.tag);
      }
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

   private Attack FindAttack(string name)
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
}
