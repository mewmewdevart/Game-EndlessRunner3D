using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject gameOver;

    public float score;
    public Text scoreText;
    public int scoreCoin;
    public Text scoreCoinText;


    private Player player;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.isDead)//Enquanto o player estiver vivo, execute
        {
        //Score : Quanto maior for o 10f mais rapido é a soma da pontuação
        score += Time.deltaTime * 5f;
        scoreText.text = Mathf.Round(score).ToString() + "m"; //Arredonda o float e depois castando o valor, estou dizendo que o float virara string
        }

    }
    public void ShowGameOver()
    {
        gameOver.SetActive(true);
    }

    public void AddCoin()
    {
        scoreCoin++;
        scoreCoinText.text = scoreCoin.ToString();
    }
}
