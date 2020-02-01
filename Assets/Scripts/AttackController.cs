using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Attack attack;
    public GameObject hitbox;

    //Angle at which enemy is hit back at
    private float endLag;
    private float maxHitboxDuration;
    private float startupTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Startup
        startupTime -= Time.deltaTime;
        if (startupTime <= 0)
        {
            //Any hitbox times
            maxHitboxDuration -= Time.deltaTime;
            if (maxHitboxDuration <= 0)
            {

                //EndLag
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

        foreach(Hitbox e in inAttack.hitboxes)
        {
            if(e.delay + e.hitboxDuration > maxHitboxDuration)
            {
                maxHitboxDuration = e.delay + e.hitboxDuration;
            }
            StartCoroutine(SpawnHitbox(e, startupTime + e.delay));
        }
    }
    IEnumerator SpawnHitbox(Hitbox hb, float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(hb, transform);
    }

    //Will destroy this attack object. Includes any effects
    private void Die()
    {
        Destroy(gameObject);
    }
}
