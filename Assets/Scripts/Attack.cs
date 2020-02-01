using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack")]
public class Attack : ScriptableObject
{
    //What angle enemies will fly back at
    public float knockbackAngle;

    //How far the enemy will be launched
    public float knockback;

    //How long an enemy will not be able to attack after being hit with this attack
    public float hitstun;

    //How long before this attack's hitbox comes out
    public float startupTime;

    //How much time before player can act out of attacking
    public float endLag;

    //How long the hitbox stay out
    public float hitboxDuration;

    //Attack timing should follow this
    //complete attack time = startupTime + hitboxDuration + endLag
    //in that order.

    //How big the hitbox will be
    public Vector2 scale;

    //what position relative to the player the hitbox will be instantiated
    public Vector2 offeset;

    //Whether or not the attack prevent the attacker from being hit for its duration
    public bool invincible;
}
