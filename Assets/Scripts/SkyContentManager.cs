using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SkyContentManager : MonoBehaviour
{

    public GameObject skyManager;
    public Transform mainCamera;

    public GameObject cloud1Prefab;
    public GameObject cloud2Prefab;
    public GameObject cloud3Prefab;
    public GameObject cloud4Prefab;
    public GameObject cloud5Prefab;
    public GameObject cloud6Prefab;
    public GameObject cloud7Prefab;

    public Sprite starsSky;
    public Sprite cloudsMassive;

    private bool cloudsActive = false;
    private bool cloudsMassiveActive = false;
    private bool starsActive = false;

    private List<GameObject> Clouds = new List<GameObject>();


    public float cloudSpeed = 0.02f;
    void Start()
    {
        cloudsActive = skyManager.GetComponent<SkyManager>().cloudsActive;
        cloudsMassiveActive = skyManager.GetComponent<SkyManager>().cloudsMassiveActive;
        starsActive = skyManager.GetComponent<SkyManager>().starsActive;


        if (starsActive)
        {
            GetComponent<SpriteRenderer>().sprite = starsSky;
        }
        if (cloudsMassiveActive)
        {
            GetComponent<SpriteRenderer>().sprite = cloudsMassive;
        }
        if (cloudsActive)
        {
            CloudInstantiate(2);
            foreach (var cloud in Clouds)
            {
                cloud.transform.parent = mainCamera.transform;
                cloud.transform.position = new Vector3(Random.Range(-70, 70), Random.Range(8, 17), 1);
            }
        }
    }

    void FixedUpdate()
    {
        if (starsActive)
        {
            GetComponent<SpriteRenderer>().sprite = starsSky;
            transform.position =  new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y + 5, 1);
        }
        if (cloudsMassiveActive)
        {
            GetComponent<SpriteRenderer>().sprite = cloudsMassive;
            transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y + 5, 1);
        }
        if (cloudsActive)
        {
            foreach (GameObject cloud in Clouds)
            {
                CloudManager(cloud);
            }
        }
    }

    public void CloudManager(GameObject cloud)
    {
        cloud.transform.position += new Vector3(cloudSpeed, 0, 0);
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(cloud.transform.position);
        Vector3 WorldPoint = Camera.main.ViewportToWorldPoint(new Vector3(-1.5f, 0, 0));
        print(WorldPoint.x);
        if (viewportPoint.x < -2 || viewportPoint.x > 3)
        {
            cloud.transform.position = new Vector3(Random.Range(WorldPoint.x, WorldPoint.x + 40), Random.Range(8, 17), 1);
        }

    }

    public void CloudInstantiate(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject cloud1 = Instantiate(cloud1Prefab, new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 1), mainCamera.transform.rotation);
            GameObject cloud2 = Instantiate(cloud2Prefab, new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 1), mainCamera.transform.rotation);
            GameObject cloud3 = Instantiate(cloud3Prefab, new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 1), mainCamera.transform.rotation);
            GameObject cloud4 = Instantiate(cloud4Prefab, new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 1), mainCamera.transform.rotation);
            GameObject cloud5 = Instantiate(cloud5Prefab, new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 1), mainCamera.transform.rotation);
            GameObject cloud6 = Instantiate(cloud6Prefab, new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 1), mainCamera.transform.rotation);
            GameObject cloud7 = Instantiate(cloud7Prefab, new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 1), mainCamera.transform.rotation);
            Clouds.Add(cloud1);
            Clouds.Add(cloud2);
            Clouds.Add(cloud3);
            Clouds.Add(cloud4);
            Clouds.Add(cloud5);
            Clouds.Add(cloud6);
            Clouds.Add(cloud7);
        }

    }


}
