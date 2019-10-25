using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState { Idle, Playing, Ended, Ready };

public class GameController : MonoBehaviour {

    [Range(0.02f,0.2f)]
    public float parallaxSpeed = 0.02f;
    public RawImage background;
    public RawImage platform;
    public Text pointsText;
    public Text recordText;
    public GameObject uiIdle;
    public GameObject uiScore;
    public float scaleTime = 6f;
    public float scaleInc = 0.25f;

   
    public GameState gameState = GameState.Idle;

    public GameObject player;
    public GameObject enemyGenerator;

    private AudioSource musicPlayer;
    private int points = 0;

	// Use this for initialization
	void Start () {
        recordText.text= "BEST: " + GetMaxScore().ToString();
        musicPlayer = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

        bool userAction = Input.GetKeyDown("up") || Input.GetMouseButtonDown(0);

        if (gameState == GameState.Idle && userAction) {
            gameState = GameState.Playing;
            uiIdle.SetActive(false);
            uiScore.SetActive(true);
            player.SendMessage("UpdateState", "PlayerRun");
            player.SendMessage("DustPlay");
            enemyGenerator.SendMessage("StartGenerator");
            musicPlayer.Play();
            InvokeRepeating("GameTimeScale", scaleTime, scaleTime);
        }
        else if (gameState==GameState.Playing) {
            Parallax();
        }

        else if (gameState == GameState.Ready)
        {
            if (userAction)
            {
                RestartGame();
            }
        }

    }

    void Parallax() {
        float finalSpeed = parallaxSpeed * Time.deltaTime;
        background.uvRect = new Rect(background.uvRect.x + finalSpeed, 0f, 1f, 1f);
        platform.uvRect = new Rect(platform.uvRect.x + finalSpeed * 4, 0f, 1f, 1f);
    }

    public void RestartGame() {
        ResetTimeScale();
        SceneManager.LoadScene("Nivel");
    }

    public void GameTimeScale() {
        Time.timeScale += scaleInc;
    }

    public void ResetTimeScale(float newTimeScale=1f) {
        CancelInvoke("GameTimeScale");
        Time.timeScale = 1f;
    }

    public void IncreasePoints() {
        points++;
        pointsText.text = points.ToString();
        if (points >= GetMaxScore()) {
            recordText.text = "BEST: " + points.ToString();
            SaveScore(points);
        }
    }

    public int GetMaxScore() {
        return PlayerPrefs.GetInt("Max Points", 0);
    }

    public void SaveScore(int currentPoints)
    {
        PlayerPrefs.SetInt("Max Points", currentPoints);
    }
}
