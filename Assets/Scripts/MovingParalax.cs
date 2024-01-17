using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingParalax : MonoBehaviour
{
    private float lenght;
    private float startPos;

    public GameObject cam;

    public float paralaxPower;

    public float moveSpeed = 0.1f;

    public float accuracyCorrection = 0;



    void Start()
    {
        startPos = transform.position.x;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - paralaxPower));
        float distance = (cam.transform.position.x * paralaxPower);
        startPos += moveSpeed;
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (temp > startPos + lenght)
        {
            print(1);
            startPos += lenght + (lenght * 0.1f);
        }
        else if (temp < startPos - lenght)
        {
            print(2);
            startPos -= lenght - (lenght * 0.1f);
        }
    }
}
