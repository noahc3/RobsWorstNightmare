using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EspressoSpin : MonoBehaviour
{
    public float tiltDeg = 25.0f;
    public float degPerSecond = 90.0f;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.Rotate(Vector3.forward, tiltDeg);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(Vector3.up, degPerSecond * Time.deltaTime * -1.0f * 2, Space.Self);
        gameObject.transform.Rotate(Vector3.up, degPerSecond * Time.deltaTime, Space.World);
    }
}
