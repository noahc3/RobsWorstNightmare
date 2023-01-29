using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerFloor : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] m_EnemyPrefabs = null;
    void Start()
    {
        var numb = Random.Range(0,m_EnemyPrefabs.Length);
        Instantiate(m_EnemyPrefabs[numb], transform.position, Quaternion.Euler(0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
