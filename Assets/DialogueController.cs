using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueController : MonoBehaviour
{

    

    public TextMeshProUGUI DialogueText;
    public string[] Sentences;
    private int SentenceIndex = 0;
    public float DialogueSpeed;
    private bool NextText = false;

    public TextMeshProUGUI ContinueText;
    // public string empty = "";
    // public string continueText = "Press Space to continue....";

    public AudioSource Source;
    public AudioClip[] SentencesAudio;
    private int AudioIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
      NextSentence();
      Source.clip = SentencesAudio[AudioIndex];
      Source.Play();
    }

    // Update is called once per frame
    void Update()
    {
      if(Input.GetKeyDown(KeyCode.Space)){
        if(NextText)
        {
          ContinueText.text = "";
          NextSentence();
          Source.clip = SentencesAudio[AudioIndex];
          Source.Play();
          NextText = false;
        }
      }
    }

    void NextSentence()
    {
      if(SentenceIndex <= Sentences.Length - 1)
        {
          DialogueText.text = "";
          StartCoroutine(WriteSentence());
        }else
        {
          SceneManager.LoadScene(2);
        }
    }

    IEnumerator WriteSentence()
    {
      foreach(char Character in Sentences[SentenceIndex].ToCharArray())
      {
        DialogueText.text += Character;
        yield return new WaitForSeconds(DialogueSpeed);

      }
      SentenceIndex++;
      AudioIndex++;
      NextText = true;
      ContinueText.text = "Press Space to continue....";

    }

}
