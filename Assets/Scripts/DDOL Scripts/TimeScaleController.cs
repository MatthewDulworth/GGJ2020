using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleController : MonoBehaviour
{
    public float timeScale;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = timeScale;
    }

}
