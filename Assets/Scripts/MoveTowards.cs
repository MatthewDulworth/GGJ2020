using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    public MoveType type;

    public Transform dest;

    //Goes from 0 (non-inclusive) to 1;
    public float smoothSpeed;
    public float speed;

    //Used for spring movement.
    //Describes at what speed the object should stop losing speed 
    //if the intended effect is for the object to come to a stop, set to 0
    public float minOscilationSpeed;
    //Used for spring movement
    //Describes how quickly an object comes to a stop
    public float springResistance;

    private Rigidbody2D rgdbdy;


    public enum MoveType
    {
        //linear -- moves the block the same amount each frame
        //smooth -- move the block more at the beginning of an iteration and less when it is close to target
        //spring -- moves the block more the farther it is from the block and will overshoot its target slightly
        smooth, linear, springy
    }
    // Start is called before the first frame update
    void Start()
    {
        rgdbdy = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (type == MoveType.linear)
        {
            Vector3 heading = dest.position - transform.position;
            if (heading.magnitude < speed * Time.fixedDeltaTime)
            {
                rgdbdy.MovePosition(dest.position);
            }
            else
            {
                rgdbdy.MovePosition(transform.position + heading / heading.magnitude * speed * Time.fixedDeltaTime);
            }
        }
        else if (type == MoveType.smooth)
        {
            //Constant is used to make smaller values of speed go through Lerp slower. 
            transform.position = Vector2.Lerp(transform.position, dest.position, smoothSpeed);
        }
        else if (type == MoveType.springy)
        {
            //Follows the formula for the force an oscilating spring exherts on an object. F = -kx
            rgdbdy.AddForce(speed * Vector2.Distance(dest.position, transform.position) * (dest.position - transform.position));
            if (rgdbdy.velocity.magnitude > minOscilationSpeed)
            {
                rgdbdy.velocity = rgdbdy.velocity / springResistance;
            }
        }
    }
    public void Setup(Transform destination, MoveType moveType, float speed)
    {
        type = moveType;
        dest = destination;
        if (moveType == MoveType.smooth)
        {
            smoothSpeed = speed;
        }
        else if (moveType == MoveType.linear || moveType == MoveType.springy)
        {
            this.speed = speed;
        }
        rgdbdy = GetComponent<Rigidbody2D>();
        if (moveType == MoveType.springy)
        {
            rgdbdy.bodyType = RigidbodyType2D.Dynamic;
        }
        else if (moveType == MoveType.smooth || moveType == MoveType.linear)
        {
            rgdbdy.bodyType = RigidbodyType2D.Kinematic;
        }
    }
    public void ChangeDest(Transform newDest)
    {
        dest = newDest;
    }
}
