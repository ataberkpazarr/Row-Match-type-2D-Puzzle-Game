using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnConfetti : Singleton<SpawnConfetti>
{
    public GameObject confettiFx;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public  void SpawnTHEConfetti() // creating the confetti object
    {
        GameObject ob = Instantiate(confettiFx);
        Destroy(ob, 2.5f);
    }
}
