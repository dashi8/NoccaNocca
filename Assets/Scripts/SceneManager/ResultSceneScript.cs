using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PiecesBoard;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultSceneScript : MonoBehaviour
{
    public GameObject resultTextObject;
    bool winner;

    // Start is called before the first frame update
    void Start()
    {
        winner = PiecesManager.getWinner();
        writeText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void writeText()
    {
        var resultText = resultTextObject.GetComponent<Text>();
        if (winner)
        {
            resultText.text = "あなたの勝ちです！";
        }
        else
        {
            resultText.text = "あなたの負けです";
        }
    }

    public void toMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void toPlayScene()
    {
        SceneManager.LoadScene("PlayScene");
    }
}
