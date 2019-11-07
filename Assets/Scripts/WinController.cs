using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        FindObjectOfType<Text>().text =
            GameController.Instance.playerOne.Points > GameController.Instance.playerTwo.Points
                ? "Player One"
                : "Player Two";
    }
}
