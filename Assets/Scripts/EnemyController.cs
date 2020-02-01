using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Attack")
        {
            AttackController attack = collision.gameObject.GetComponent<AttackController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
