using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // Start is called before the first frame update
    int brickCount;
    private void Start()
    {
        CountAllBricks();
    }
    private void CountAllBricks()
    {
        Brick[] allBricksInLevel = FindObjectsOfType<Brick>();
        brickCount = allBricksInLevel.Length;
    }
    public void DeductBrick()
    {
         brickCount--;
         if(brickCount <= 0)
         {
             LoadNextLevel();
         }
    }
    private void LoadNextLevel()
         {
             int sceneIndex = SceneManager.GetActiveScene().buildIndex;
             SceneManager.LoadScene(sceneIndex + 1);
         }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
