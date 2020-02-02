using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    public float decay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Color newColor = GetComponent<SpriteRenderer>().color;
        newColor.a -= Time.deltaTime * 2;
        GetComponent<SpriteRenderer>().color = newColor;
        transform.localScale = new Vector3(transform.localScale.x - Time.deltaTime * 2, transform.localScale.y - Time.deltaTime * 2, 1);
    }
}
