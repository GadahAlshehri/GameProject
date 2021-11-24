using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameButtons : MonoBehaviour
{
    bool isPaused =false;


    public void Restart(){


        SceneManager.LoadScene(SceneManager.GetActiveScene().name);



    }

    public void Quit()
    {

        Application.Quit();
    }

    public void Play()
    {

        SceneManager.LoadScene("JoyStick");

    }

    public void Back()
    {

        SceneManager.LoadScene("StartScene");

    }

    public void Cridts()
    {

        SceneManager.LoadScene("Credits");

    }




    public void PauseGame()
    {
        if (isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;

        }
        else
        {
            Time.timeScale = 0;
            isPaused = true;
        }
    }

}
