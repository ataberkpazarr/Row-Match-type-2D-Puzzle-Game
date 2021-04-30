using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScreenFader : MonoBehaviour
{
    public float solidAlpha = 1f; //alpha chanel value of our graphic this if or when it is opac

    public float clearAlpha = 0f; //transparent

    public float delay = 1f; //how long we wait before the fade begins

    public float timeToFade = 1f; //how long the fade take the transition

    MaskableGraphic m_graphic;

    void Start()
    {
        m_graphic = GetComponent<MaskableGraphic>();
        FadeOff();
        
    }

    IEnumerator FadeRoutine(float alpha)
    {
        yield return new WaitForSeconds(delay);

        m_graphic.CrossFadeAlpha(alpha, timeToFade, true); // target alpha value, time to fade value, ignore time scale boolean value. True means it will fade even if game pause

    }

    public void FadeOn()
    {
        StartCoroutine(FadeRoutine(solidAlpha)); //solidalpha value so it will be opac and overlay the screen
    }

    public void FadeOff()
    {
        StartCoroutine(FadeRoutine(clearAlpha)); //clearalpha value
    }

}


