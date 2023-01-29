using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTest : MonoBehaviour
{

    public Transform player;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = player.transform.position + new Vector3(0, 5, -4);
        transform.rotation = Quaternion.Euler(30, 0, 0);
    }
}
