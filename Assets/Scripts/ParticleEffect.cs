using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    public float decay;
    public float lifetime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lifetime -= Time.deltaTime;
        if(lifetime <= 0)
        {
            Destroy(gameObject);
        }
        Color newColor = GetComponent<SpriteRenderer>().color;
        newColor.a -= Time.deltaTime * decay;
        GetComponent<SpriteRenderer>().color = newColor;
        transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * decay, transform.localScale.y + Time.deltaTime * decay, 1);
    }
}
