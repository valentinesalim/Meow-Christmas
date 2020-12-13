using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameMain : MonoBehaviour {

    public GameObject cat;
    public GameObject readyPic;
    public GameObject tipPic;
    public GameObject scoreMgr;
    public GameObject pipeSpawner;

    private bool gameStarted = false;

	void Start () {

	}

	void Update () {
        if (!gameStarted && Input.GetButtonDown("Fire1"))
        {
            gameStarted = true;
            StartGame();
        }
    }

    private void StartGame()
    {
        CatControl control = cat.GetComponent<CatControl>();
        control.inGame = true;
        control.JumpUp();

        readyPic.GetComponent<SpriteRenderer>().material.DOFade(0f, 0.2f);
        tipPic.GetComponent<SpriteRenderer>().material.DOFade(0f, 0.2f);

        scoreMgr.GetComponent<ScoreMgr>().SetScore(0);
        pipeSpawner.GetComponent<PipeSpawner>().StartSpawning();
    }
    public void Replayfunction()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
