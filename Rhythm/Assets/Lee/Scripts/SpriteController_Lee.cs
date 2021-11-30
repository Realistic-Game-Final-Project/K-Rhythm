using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteController_Lee : MonoBehaviour
{
    //Sprite Image
    public GameObject Bird;
    GameObject[] BirdObj = new GameObject[10];
    Vector3[] BirdPoint = new Vector3[10];
    // Start is called before the first frame update
    void Start()
    {
        MakeBird();
        BirdPos();

    }
    float CurrentTime = 0;
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 10; i++)
        {

            BirdObj[i].transform.position = Vector3.Lerp(BirdObj[i].transform.position, BirdPoint[i], Time.deltaTime);
        }
        CurrentTime += Time.deltaTime;
        if(CurrentTime > 3)
        {
            BirdPos();
            CurrentTime = 0;
        }
        
    }

    void MakeBird()
    {
       for(int i = 0; i<10; i++)
        {
            Vector3 createPoint = new Vector3(Random.Range(30, 800), Random.Range(80, 400),0);
            GameObject CloneBird = Instantiate(Bird, createPoint, Quaternion.identity, GameObject.Find("Canvas").transform);
            BirdObj[i] = CloneBird;
        }
    }

    void BirdPos()
    {
        for (int i = 0; i < 10; i++)
        {
            BirdPoint[i] = new Vector3(Random.Range(30, 800), Random.Range(80, 400), 0);
        }
    }

}
