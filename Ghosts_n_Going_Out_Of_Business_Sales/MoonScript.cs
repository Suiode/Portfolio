using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonScript : MonoBehaviour
{
    [Header("Game Objects")]
    public Light moonlight;
    public Camera camera;


    [Header("Moon shift variables")]
    [SerializeField] private bool bloodMoonEnabled;
    [SerializeField] private float defaultChangeTime = 0.2f;
    [SerializeField] private float changeTime = 0;
    [SerializeField] private bool shiftingPhase = false;


    void Start()
    {
        //Change blood moon status based on startup color
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
        //Current setup to change color whenever I like. In the future this will all be handled by the MoonShift() coroutine
        if (Input.GetKeyDown(KeyCode.F) && !bloodMoonEnabled)
        {
            //shiftingPhase = true;
            StartCoroutine(MoonShift(moonlight.color, Color.red));
            bloodMoonEnabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.F) && bloodMoonEnabled)
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



