using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerFloor : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject m_EnemyPrefab = null;
    void Start()
    {
        Instantiate(m_EnemyPrefab, transform.position, Quaternion.Euler(0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
