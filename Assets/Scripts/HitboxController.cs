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
        
    }
    private void FixedUpdate()
    {
        hitboxDuration -= Time.deltaTime;
    }
    public void SetHitbox(Hitbox hb)
    {
        hitbox = hb;
        hitboxDuration = hb.hitboxDuration;
    }
}
