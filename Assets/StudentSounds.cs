using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentSounds : MonoBehaviour
{

    public List<AudioClip> StudentSoundList = new List<AudioClip>() { };
    private AudioSource mAudioSource = null;

    // Start is called before the first frame update
    void Start()
    {
        mAudioSource = GetComponent<AudioSource> ();
        
    }

    // Update is called once per frame
    void Update()
    {
    }


    void OnCollisionEnter(Collision coll)
    {
        var sound = StudentSoundList[Random.Range(0, StudentSoundList.Count)];
        
        if (mAudioSource != null && sound != null) {
            mAudioSource.PlayOneShot (sound, coll.relativeVelocity.magnitude);
        }
    }
}
