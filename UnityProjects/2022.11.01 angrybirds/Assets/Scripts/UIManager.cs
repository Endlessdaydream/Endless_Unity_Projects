using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text playerNum;
    public Text pigNum;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int lives = GameMode.Instance.playerLives;
        playerNum.text = lives.ToString();

        int enemyNum = GameMode.Instance.enemyNum;
        pigNum.text = enemyNum.ToString();
    }
}
