using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Attack attack;

    //Angle at which enemy is hit back at
    private float endLag;
    private float hitboxDuration;
    private float startupTime;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = attack.scale;
        transform.localPosition = attack.offeset;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        startupTime -= Time.deltaTime;
        if(startupTime <= 0)
        {
            GetComponent<Collider2D>().enabled = true;

            hitboxDuration -= Time.deltaTime;
            if(hitboxDuration <= 0)
            {
                GetComponent<Collider2D>().enabled = false;
                endLag -= Time.deltaTime;
            }
            
            if (endLag <= 0)
            {
                Die();
            }
        }
    }
    //Assigns all values in controller through attack scriptable
    public void SetAttack(Attack inAttack)
    {
        attack = inAttack;
        endLag = attack.endLag;
        hitboxDuration = attack.hitboxDuration;
        startupTime = attack.startupTime;
    }

    //Will destroy this attack object. Includes any effects
    private void Die()
    {
        Destroy(gameObject);
    }
}
