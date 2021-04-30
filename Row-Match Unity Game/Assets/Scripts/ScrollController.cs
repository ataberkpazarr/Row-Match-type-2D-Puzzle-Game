using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] LevelPrefabs;
    public GameObject LevelPrefab;




    void Start()
    {
        Instantiate(LevelPrefabs[1], new Vector3(1, 1, 0), Quaternion.identity);
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
