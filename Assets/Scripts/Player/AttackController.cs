using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
   public Attack attack;
   public GameObject hitbox;
   public bool isAerial;

   private float endLag;
   private float maxHitboxDuration;
   private float startupTime;


   // Update is called once per frame
   void FixedUpdate()
   {
      startupTime -= Time.deltaTime;
      if (startupTime <= 0)
      {
         maxHitboxDuration -= Time.deltaTime;


         // kill aerials on landing
         if (isAerial && transform.parent.gameObject.GetComponent<PlayerController>().grounded)
         {
            Die();
         }
         else if (maxHitboxDuration <= 0)
         {
            // handle endLag
            endLag -= Time.deltaTime;
            if (endLag <= 0)
            {
               Die();
            }
         }
      }
   }

   //Assigns all values in controller through attack scriptable
   public void SetAttack(Attack inAttack)
   {
      attack = inAttack;
      endLag = attack.endLag;
      startupTime = attack.startupTime;

      foreach (Hitbox e in inAttack.hitboxes)
      {
         if (e.delay + e.hitboxDuration > maxHitboxDuration)
         {
            maxHitboxDuration = e.delay + e.hitboxDuration;
         }
         StartCoroutine(SpawnHitbox(e, startupTime + e.delay));
      }
   }

   IEnumerator SpawnHitbox(Hitbox hb, float delay)
   {
      yield return new WaitForSeconds(delay);
      var spawnedHB = Instantiate(hitbox, transform);
      spawnedHB.GetComponent<HitboxController>().SetHitbox(hb);
   }

   //Will destroy this attack object. Includes any effects
   private void Die()
   {
      Destroy(gameObject);
   }
}
