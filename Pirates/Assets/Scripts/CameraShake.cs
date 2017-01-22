using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    float speed = 1.0f;
    Vector3 origPos, finalPos;

    void Start()
    {
        InvokeRepeating("GenNewPos", 0, 4f);
    }

    void Update()
    {
        transform.position = Vector3.Lerp(origPos, finalPos, Time.deltaTime * speed);
    }

    private void GenNewPos()
    {
        origPos = transform.position;
        finalPos = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), transform.position.z);
    }

}
