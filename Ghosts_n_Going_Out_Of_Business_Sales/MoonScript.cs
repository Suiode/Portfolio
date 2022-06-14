using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonScript : MonoBehaviour
{
    public Light moonlight;
    public Camera camera;
    public bool bloodMoonEnabled;
    public float defaultChangeTime = 0.2f;
    public float changeTime = 0;

    public bool shiftingPhase = false;



    // Start is called before the first frame update
    void Start()
    {
        if (moonlight.color == Color.red)
        {
            StartCoroutine(MoonShift(moonlight.color, Color.red));
            bloodMoonEnabled = true;
        }
        else
        {
            bloodMoonEnabled = false;
            StartCoroutine(MoonShift(moonlight.color, Color.gray));
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && !bloodMoonEnabled)
        {
            //shiftingPhase = true;
            StartCoroutine(MoonShift(moonlight.color, Color.red));
            bloodMoonEnabled = true;
        }
        else if(Input.GetKeyDown(KeyCode.F) && bloodMoonEnabled)
        {
            //shiftingPhase = true;
            StartCoroutine(MoonShift(moonlight.color, Color.gray));
            bloodMoonEnabled = false;
        }
    }


    public IEnumerator MoonShift(Color oldColor, Color newColor)
    {
        shiftingPhase = true;
        changeTime = 0;


        while (shiftingPhase)
        {
            yield return null;
            changeTime += Time.deltaTime;
            moonlight.color = Color.Lerp(oldColor, newColor, (changeTime / defaultChangeTime));
            camera.backgroundColor = Color.Lerp(oldColor, newColor, (changeTime / defaultChangeTime));
            

            if (changeTime >= defaultChangeTime)
            {
                shiftingPhase = false;
                changeTime = 0;
            }
        }
        
    }


}



