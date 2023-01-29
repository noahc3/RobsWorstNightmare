using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour
{
    public AudioSource Source;
    public AudioClip[] SentencesAudio;
    private int AudioIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
      AudioIndex = Random.Range(0, 3);
      Source.clip = SentencesAudio[AudioIndex];
      Source.Play();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
