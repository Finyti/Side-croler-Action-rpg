using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class GameManager : MonoBehaviour
{
    public string scenename1;
    public string scenename2;

    public GameObject UI;
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            SceneManager.LoadScene(scenename1);
        }
        if (Input.GetKeyDown("2"))
        {
            SceneManager.LoadScene(scenename2);
        }
        if (Input.GetKeyDown("e"))
        {
            UI.SetActive(!UI.active);
            //if(Time.timeScale == 1)
            //{
            //    Time.timeScale = 0.0f;
            //}
            //else
            //{
            //    Time.timeScale = 1.0f;
            //}

        }
    }
}
