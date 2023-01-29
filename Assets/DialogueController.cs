using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueController : MonoBehaviour
{
  // Text 
  public TextMeshProUGUI DialogueText;
  public string[] Sentences;
  private int SentenceIndex = 0;
  public float DialogueSpeed;
  private bool NextText = false;

  public TextMeshProUGUI ContinueText;
  public TextMeshProUGUI SkipText;

  // Text effects
  private float TextMinAlpha;
  private float TextMaxAlpha;
  private float TextCurrentAlpha;
  private bool reduceColor;

  // Audio 
  public AudioSource Source;
  public AudioClip[] SentencesAudio;
  private int AudioIndex = 0;

  // Start is called before the first frame update
  void Start()
  {
    NextSentence();
    Source.clip = SentencesAudio[AudioIndex];
    Source.Play();

    TextMinAlpha = 0.2f;
    TextMaxAlpha = 1f;
    TextCurrentAlpha = 1f;
    reduceColor = true;
  }

  // Update is called once per frame
  void Update()
  {
    AlphaComments();
    if(Input.GetKeyDown(KeyCode.Space))
    {
      if(NextText)
      {
        ContinueText.text = "";
        NextSentence();
        Source.clip = SentencesAudio[AudioIndex];
        Source.Play();
        NextText = false;
      }
    }
    if(Input.GetKeyDown(KeyCode.Return))
    {
      SceneManager.LoadScene(2);
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
    ContinueText.text = "Press Space to continue.....";

  }

  public void AlphaComments()
  {
    if(reduceColor)
    {
      TextCurrentAlpha = TextCurrentAlpha - 0.0025f;
      if(TextCurrentAlpha <= TextMinAlpha)
      {
        reduceColor = !reduceColor;
      }
    }
    else
    {
      TextCurrentAlpha = TextCurrentAlpha + 0.0025f;
      if(TextCurrentAlpha >= TextMaxAlpha)
      {
        reduceColor = !reduceColor;
      }
    }

    SkipText.color = new Color(Color.white.r,Color.white.g, Color.white.b, TextCurrentAlpha);
    ContinueText.color = new Color(Color.white.r,Color.white.g, Color.white.b, TextCurrentAlpha);

  }

}
