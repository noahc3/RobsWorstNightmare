using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DialogueController : MonoBehaviour
{

    public TextMeshProUGUI DialogueText;
    public string[] Sentences;
    private int Index = 0;
    public float DialogueSpeed;
    private bool NextText = false;

    // Start is called before the first frame update
    void Start()
    {
        NextSentence();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
          if(NextText)
          {
            NextSentence();
            NextText = false;
          }
        }
    }

    void NextSentence()
    {
      if(Index <= Sentences.Length - 1)
        {
          DialogueText.text = "";
          StartCoroutine(WriteSentence());
        }
    }

    IEnumerator WriteSentence()
    {
      foreach(char Character in Sentences[Index].ToCharArray())
      {
        DialogueText.text += Character;
        yield return new WaitForSeconds(DialogueSpeed);

      }
      Index++;
      NextText = true;
    }

}
