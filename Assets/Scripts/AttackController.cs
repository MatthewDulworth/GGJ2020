using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack")]
public class AttackController : MonoBehaviour
{
    //What angle enemies will fly back at
    public Vector2 knockbackAngle;
}
