using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayNowAudio : MonoBehaviour
{

    public AudioSource Source;
    public AudioClip[] SentencesAudio;
    private int AudioIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void playNow(bool playAudio){
      if(playAudio)
      {
        AudioIndex = Random.Range(0, 3);
        Source.clip = SentencesAudio[AudioIndex];
        Source.Play();
        
        StartCoroutine(changeScene());
      }
      
    }

    IEnumerator changeScene()
    {
      yield return new WaitForSeconds(1.50f);
      SceneManager.LoadScene(1);
    }

}
