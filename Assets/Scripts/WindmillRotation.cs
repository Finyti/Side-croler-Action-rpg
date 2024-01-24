using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillRotation : MonoBehaviour
{

    public float speed = 1f;

    void Update()
    {
        transform.eulerAngles += new Vector3(0, 0, speed);
    }
}
