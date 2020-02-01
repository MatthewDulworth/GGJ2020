using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack")]
public class AttackController : ScriptableObject
{
   // What angle enemies will fly back at
   public Vector2 knockbackAngle;

   // How far the enemy will be launched
   public float knockback;

   // How much Damage the enemy takes
   public float damage;
}
