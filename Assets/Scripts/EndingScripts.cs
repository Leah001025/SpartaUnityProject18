using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingScripts : MonoBehaviour
{
    public Text EndingTxt;

    public string[] EndingDialogue;
    public string[] dialogues;

    public float TextSpeed = 1;
    public float seqSpeed = 1;

    public int talkNum;
    // Start is called before the first frame update
    void Start()
    {
        StartTalk(EndingDialogue);
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Typing(string talk)
    {
        EndingTxt.text = "";

        for (int i = 0; i < talk.Length; i++)
        {
            EndingTxt.text += talk[i];
            yield return new WaitForSeconds(TextSpeed);
        }

        yield return new WaitForSeconds(seqSpeed);
        NextTalk();
    }

    public void StartTalk(string[] talks)
    {
        dialogues = talks;
        StartCoroutine(Typing(dialogues[talkNum]));
    }

    public void NextTalk()
    {
        EndingTxt.text = null;
        talkNum++;

        if (talkNum == dialogues.Length)
        {
            EndTalk();
            return;
        }

        StartCoroutine(Typing(dialogues[talkNum]));
    }

    private void EndTalk()
    {
        talkNum = 0;

        StartCoroutine(LoadStartSceneAfterDelay(2f));
    }

    IEnumerator LoadStartSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("StartScene");
    }
}
