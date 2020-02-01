using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public GameObject panel1;
    public GameObject panel2;

    public float minDist;
    public Transform endPos;
    public Transform spawnPos;
    public float scrollSpeed;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var direction = (endPos.position - transform.position) / (endPos.position - transform.position).magnitude;
        panel1.transform.Translate(scrollSpeed * direction);
        panel2.transform.Translate(scrollSpeed * direction);

        if(Vector3.Distance(panel1.transform.position, endPos.position) < minDist)
        {
            panel1.transform.position = spawnPos.position;
        }
        if (Vector3.Distance(panel2.transform.position, endPos.position) < minDist)
        {
            panel2.transform.position = spawnPos.position;
        }
    }
}
