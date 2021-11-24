using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    // Start is called before the first frame update
    int score = 0;
     [SerializeField] int lives = 3;
     [SerializeField] Text scoreText;
     [SerializeField] Text livesText;
     [SerializeField] GameObject ballPrefab;
    public void IncreaseScore()
     {
    score++;
    scoreText.text = score.ToString();
     }
    void Start()
    {
        
    }
    public void DecreaseLives()
    {
        lives--;
        livesText.text = lives.ToString();
        if(lives > 0)
        {
            RespawnPlayer();
        }
    }

    private void RespawnPlayer()
    {
         Vector3 respawnPosition = new Vector3(0, -4, 0);
         Instantiate(ballPrefab, respawnPosition, Quaternion.identity);
    }
    private void Awake()
    {
         LevelController[] objs = FindObjectsOfType<LevelController>();
         if (objs.Length > 1)
         {
             Destroy(gameObject);
         }
          DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
