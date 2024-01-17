using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkyManager : MonoBehaviour
{

    public Transform mainCamera;

    public GameObject skyContent;

    public Sprite sunnySky;
    public Sprite cloudySky;
    public Sprite seaSky;
    public Sprite darkSky;


    private SpriteRenderer sp;

    public bool cloudsActive = false;
    public bool cloudsMassiveActive = false;
    public bool starsActive = false;

    private int CurrentSky;

    void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
        CurrentSky = Random.Range(2, 3);

        if (CurrentSky == 1)
        {
            sp.sprite = sunnySky;
            cloudsActive = true;
        }
        if (CurrentSky == 2)
        {
            sp.sprite = cloudySky;
            cloudsMassiveActive = true;
        }
        if (CurrentSky == 3)
        {
            sp.sprite = seaSky;
            cloudsActive = true;
        }
        if (CurrentSky == 4)
        {
            sp.sprite = darkSky;
            starsActive = true;
        }

    }

    void FixedUpdate()
    {
        transform.position = new Vector3(mainCamera.position.x, mainCamera.position.y, 0);
    }

    public bool Content(int content)
    {
        switch(content)
        {
            case 0:
            return cloudsActive;
            case 1:
            return cloudsMassiveActive;
            case 2:
            return starsActive;
        }

        return false;
    }
}