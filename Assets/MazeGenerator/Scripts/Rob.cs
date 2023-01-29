using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rob : MonoBehaviour
{
    public GameObject Wall = null;
    public AudioClip HitSound = null;
    public AudioClip CoinSound = null;
    public AudioClip GlitchSound = null;

    public float BoostWhenTransformingAmount = 0.1f;
    public int HeightBoostAmount = 2;

    private Rigidbody mRigidBody = null;
    private AudioSource mAudioSource = null;
    private bool mFloorTouched = false;
    private int espressosCollected = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GlitchWall(GameObject oldWall)
    {
        var glitchWalls = GameObject.FindGameObjectsWithTag("GlitchWall");
        var wall = glitchWalls[Random.Range(0, glitchWalls.Length)];

        SimpleSampleCharacterControl script = gameObject.GetComponent<SimpleSampleCharacterControl>();

        Vector3 newPlayPos = wall.transform.position + wall.transform.forward * -0.6f;
        newPlayPos.y += 0.5f;

        Vector3 newRot = script.m_currentDirection;
        newRot.y = wall.transform.rotation.eulerAngles.y;

        //the rotate stuff does not work correctly but whatever i tried lmao
        gameObject.transform.position = newPlayPos;
        gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, wall.transform.rotation.y + 180, gameObject.transform.rotation.z);
        script.m_currentDirection = wall.transform.forward * -1;
        script.m_currentH = 0;
        script.m_currentV = 0;

        //Replace the wall that was touched
        GameObject test = Instantiate(Wall, oldWall.transform.position, oldWall.transform.rotation);
        test.transform.parent = oldWall.transform.parent;
        Destroy(oldWall);

        //and replace the wall that the ball is teleported too

        GameObject repWall = Instantiate(Wall, wall.transform.position, wall.transform.rotation);
        repWall.transform.parent = wall.transform.parent;
        Destroy(wall);

        Debug.Log("I have glitched");
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag.Equals("GlitchWall"))
        {
            Debug.Log("Glitch wall collide");
            GlitchWall(coll.gameObject);
            mAudioSource.PlayOneShot(GlitchSound);
        }

        if (coll.gameObject.tag.Equals("Floor"))
        {
            mFloorTouched = true;
            if (mAudioSource != null && HitSound != null && coll.relativeVelocity.y > .5f)
            {
                mAudioSource.PlayOneShot(HitSound, coll.relativeVelocity.magnitude);
            }
        }
        else
        {
            if (mAudioSource != null && HitSound != null && coll.relativeVelocity.magnitude > 2f)
            {
                mAudioSource.PlayOneShot(HitSound, coll.relativeVelocity.magnitude);
            }
        }

    }

    void OnCollisionExit(Collision coll)
    {
        if (coll.gameObject.tag.Equals("Floor"))
        {
            mFloorTouched = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("EspressoCollectable"))
        {
            if (mAudioSource != null && CoinSound != null)
            {
                mAudioSource.PlayOneShot(CoinSound);
            }

            espressosCollected++;
            if (espressosCollected == 3)
            {

            }

            Destroy(other.gameObject);
        }
    }
}
