using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    private float lenght;
    private float startPos;

    public GameObject cam;

    public float paralaxPower;

    void Start()
    {
        startPos = transform.position.x;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - paralaxPower));
        float distance = (cam.transform.position.x * paralaxPower);
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (temp > startPos + lenght)
        {
            print(1);
            startPos += lenght;
        }
        else if (temp < startPos - lenght)
        {
            print(2);
            startPos -= lenght;
        }
    }
}
