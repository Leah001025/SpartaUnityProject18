using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroScripts : MonoBehaviour
{
    public Text IntroTxt;

    public string[] IntroDialogue;
    public string[] dialogues;

    public float TextSpeed = 1;
    public float seqSpeed = 1;

    public int talkNum;
    // Start is called before the first frame update
    void Start()
    {
        StartTalk(IntroDialogue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Typing (string talk)
    {
        IntroTxt.text = "";

        for(int i = 0; i < talk.Length; i++)
        {
            IntroTxt.text += talk[i];
            yield return new WaitForSeconds(TextSpeed);
        }

        yield return new WaitForSeconds (seqSpeed);
        NextTalk();
    }

    public void StartTalk (string[] talks)
    {
        dialogues = talks;
        StartCoroutine(Typing(dialogues[talkNum]));
    }

    public void NextTalk()
    {
        IntroTxt.text = null;
        talkNum++;

        if(talkNum == dialogues.Length)
        {
            EndTalk();
            return;
        }

        StartCoroutine(Typing(dialogues[talkNum]));
    }

    private void EndTalk()
    {
        talkNum = 0;
        SceneManager.LoadScene("Map");
    }
}
