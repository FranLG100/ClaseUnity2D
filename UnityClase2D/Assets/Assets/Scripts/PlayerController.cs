using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject game;
    public GameObject enemyGenerator;
    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        //animator.Play("PlayerRun");
	}
	
	// Update is called once per frame
	void Update () {
        bool gamePlaying = game.GetComponent<GameController>().gameState == GameState.Playing;
        if (gamePlaying && (Input.GetKeyDown("up") || Input.GetMouseButtonDown(0))) {
            UpdateState("PlayerJump");
        }
	}

    public void UpdateState(string state) {
        if (state != null) {
            animator.Play(state);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            UpdateState("PlayerDie");
            game.GetComponent<GameController>().gameState = GameState.Ended;
            enemyGenerator.SendMessage("CancelGenerator",true);
        }
    }
}
