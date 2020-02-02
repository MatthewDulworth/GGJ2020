using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hitbox", menuName = "Hitbox")]
public class Hitbox : ScriptableObject
{
   //How long before the hitbox becomes active
   public float delay;

   //What angle enemies will fly back at
   public float knockbackAngle;

   //How far the enemy will be launched
   public float knockback;

   //If enemy is in hitstun, priority determines if they are hit again or not
   public int priority;

   //How long an enemy will not be able to attack after being hit with this attack
   public float hitstun;

   //How long the hitbox stay out
   public float hitboxDuration;

   //How big the hitbox will be
   public Vector2 scale;

   //what position relative to the player the hitbox will be instantiated
   public Vector2 offset;

   public float damage;

   public void Awake()
   {
      damage = 1;
   }

   public float shakeDuration;
   public float shakeIntensity;
}
