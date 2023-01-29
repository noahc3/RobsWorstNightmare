using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentMove : MonoBehaviour
{
    // Adjust the speed for the application.
    public float speed = 1.0f;

    public SimpleSampleCharacterControl script;

    public bool canMove = true;
    public int countMove = 0;

    public GameObject m_EnemyPrefab;

    // The target (cylinder) position.
    private Transform target;

    void Awake()
    {
        // target = m_EnemyPrefab.transform;
    }

    void Update()
    {
        target = GameObject.FindGameObjectsWithTag("Player")[0].transform;

        Debug.Log("Target Pos:" + target.position.x + " " + target.position.z);
        // Move our position a step closer to the target.
        var step =  speed * Time.deltaTime; // calculate distance to move
        var distance = Vector3.Distance(transform.position, target.position);


        if(distance < 1 && canMove){ //touch rob
            // change controllers
            // script.changeController();
            // student stops
            canMove = false;
        }  //FollowRob
        else if (distance < 7 && canMove) { //Follow Rob
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);

            Vector3 targetDirection = target.position - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 3 * step, 0.0f);

            transform.rotation = Quaternion.LookRotation(newDirection);
        }

        if(!canMove){
            countMove++;
        }

        if(countMove > 400){
            canMove = true;
        }
    }
}