﻿using System.Collections;
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
    public GameObject uiIdle;

   
    public GameState gameState = GameState.Idle;

    public GameObject player;
    public GameObject enemyGenerator;

    private AudioSource musicPlayer;

	// Use this for initialization
	void Start () {
        musicPlayer = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

        bool userAction = Input.GetKeyDown("up") || Input.GetMouseButtonDown(0);

        if (gameState == GameState.Idle && userAction) {
            gameState = GameState.Playing;
            uiIdle.SetActive(false);
            player.SendMessage("UpdateState", "PlayerRun");
            enemyGenerator.SendMessage("StartGenerator");
            musicPlayer.Play();
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
        SceneManager.LoadScene("Nivel");
    }
}
