using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour
{

    public TextMeshProUGUI GameOverText;
    private bool showText = true;

    public AudioSource Source;
    public AudioClip[] SentencesAudio;
    private int AudioIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
      GameOverText.text = "";
      AudioIndex = Random.Range(0, 3);
    }

    // Update is called once per frame
    void Update()
    {
      if(Input.GetKeyDown(KeyCode.Space)){
        if(showText)
        {
          GameOverText.text = "YOU WIN!";
          Source.clip = SentencesAudio[AudioIndex];
          Source.Play();
        }
      }
    }
}
