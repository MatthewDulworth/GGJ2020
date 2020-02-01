using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxController : MonoBehaviour
{
    public Hitbox hitbox;

    private float hitboxDuration;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(hitbox.scale.x, hitbox.scale.y, 1);
    }
    private void FixedUpdate()
    {
        hitboxDuration -= Time.deltaTime;
        if(hitboxDuration <= 0)
        {
            GetComponent<Collider2D>().enabled = false;
        }
    }
    public void SetHitbox(Hitbox hb)
    {
        hitbox = hb;
        hitboxDuration = hb.hitboxDuration;
    }
}
