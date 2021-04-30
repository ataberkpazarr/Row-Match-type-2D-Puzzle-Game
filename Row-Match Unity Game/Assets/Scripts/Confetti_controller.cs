using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Confetti_controllerr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        System.Threading.Thread.Sleep(1000);
        SceneManager.LoadScene("_levelScene_");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
