using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStartPosition : MonoBehaviour
{
    public Transform[] startPosition1;
    public Transform[] startPosition2;
    public GameObject room1;
    public GameObject room2;

    void Start()
    {
        int randomStartPos1 = Random.Range(0, startPosition1.Length);
         int randomStartPos2 = Random.Range(0, startPosition2.Length);

        transform.position = startPosition1[randomStartPos1].position;
        Instantiate(room1, transform.position, Quaternion.identity);
        transform.position = startPosition2[randomStartPos2].position;       
        Instantiate(room2, transform.position, Quaternion.identity);
    }

}
