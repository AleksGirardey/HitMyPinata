using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RoundController : MonoBehaviour {
    public GameObject Break;
    public Text timer;

    public RawImage playerOneColor;
    public RawImage playerTwoColor;

    public Text playerOneScore;
    public Text playerTwoScore;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() {
        if (GameController.Instance.phase.phase == EPhase.ColorAssignment) {
            Break.SetActive(false);
            
        } else if (GameController.Instance.phase.phase == EPhase.Hitting) {
            timer.text = "" + ((float) Math.Truncate(GameController.Instance.elapsedTime * 10.0f) / 10.0f);
            playerOneScore.text = "" + GameController.Instance.playerOne.Points;
            playerTwoScore.text = "" + GameController.Instance.playerTwo.Points;
        } else if (GameController.Instance.phase.phase == EPhase.Break) {
            Break.SetActive(true);
        }
        
        playerOneColor.color = GameController.Instance.playerOne.AssignedKey.Color;
        playerTwoColor.color = GameController.Instance.playerTwo.AssignedKey.Color;
    }
}