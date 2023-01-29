using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using TMPro;

public class Rob : MonoBehaviour
{
    public GameObject Wall = null;
    public AudioClip HitSound = null;
    public AudioClip oofSound = null;
    public AudioClip EspressoSound = null;
    public AudioClip GlitchSound = null;

    public AudioClip[] RobHitSounds;
    public AudioClip[] EspressoHit;

    public float BoostWhenTransformingAmount = 0.1f;
    public int HeightBoostAmount = 2;

    private Rigidbody mRigidBody = null;
    private AudioSource mAudioSource = null;
    private int espressosCollected = 0;

    public Stopwatch timer;

    // Start is called before the first frame update
    void Start()
    { 
        mRigidBody = GetComponent<Rigidbody> ();
        mAudioSource = GetComponent<AudioSource> ();

        timer = new Stopwatch(); 
        timer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        UnityEngine.Debug.Log(timer.Elapsed);
        string timeSpan = timer.Elapsed.Minutes + ":" + timer.Elapsed.Seconds + "." + timer.Elapsed.Milliseconds;
        GameObject.Find("timerText").GetComponent<TextMeshProUGUI>().text = timeSpan;
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

        //Debug.Log("I have glitched");
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag.Equals("GlitchWall"))
        {
            //Debug.Log("Glitch wall collide");
            GlitchWall(coll.gameObject);
            mAudioSource.PlayOneShot(GlitchSound);
        }

        if (coll.gameObject.tag.Equals("NormalWall") || coll.gameObject.tag.Equals("Pillar"))
        {
           Debug.Log($"{RobHitSounds.Length}");
            oofSound = RobHitSounds[Random.Range(0, 3)];
            mAudioSource.clip = oofSound;
            mAudioSource.Play();
            // mAudioSource.PlayOneShot(oofSound, coll.relativeVelocity.magnitude);
        }
    }

    void OnCollisionExit(Collision coll)
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("EspressoCollectable"))
        {
            if (mAudioSource != null)
            {
              EspressoSound = EspressoHit[Random.Range(0, 3)];
              mAudioSource.clip = EspressoSound;
              mAudioSource.Play();
            }

            espressosCollected++;
            if (espressosCollected == 1)
            {
                Image espresso1 =  GameObject.Find("espresso1").GetComponent<Image>();
                var tempColor = espresso1.color;
                tempColor.a = 1f;
                espresso1.color = tempColor;
            }
            else if (espressosCollected == 2)
            {
                Image espresso1 =  GameObject.Find("espresso2").GetComponent<Image>();
                var tempColor = espresso1.color;
                tempColor.a = 1f;
                espresso1.color = tempColor;
            }
            else // 3
            {
                Image espresso1 =  GameObject.Find("espresso3").GetComponent<Image>();
                var tempColor = espresso1.color;
                tempColor.a = 1f;
                espresso1.color = tempColor;

                timer.Stop();
                
                //find if the user made it to the scoreboard and update the string in game over scene
                SceneManager.LoadScene(3);
            }

            Destroy(other.gameObject);
        }
    }
}
