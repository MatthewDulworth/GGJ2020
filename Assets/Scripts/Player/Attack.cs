using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack")]
public class Attack : ScriptableObject
{
    //How long before this attack's hitbox comes out
    public float startupTime;

    //How much time before player can act out of attacking
    public float endLag;

    //Whether or not the attack prevent the attacker from being hit for its duration
    public bool invincible;

    //All hit boxes spwned in this attack
    public Hitbox[] hitboxes;
}
