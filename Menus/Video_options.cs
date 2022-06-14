using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Video_options : MonoBehaviour
{
    public int framerateTarget;
    public Toggle fullscreenToggle;
    public Toggle vsyncToggle;


    public TMP_InputField framerateInput;

    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;
    List<int> shortResolutionList = new List<int>();
    public bool hasStarted = false;



    public TextMeshProUGUI oldRes;
    public TextMeshProUGUI newRes;
    public int oldResValue;
    public int newResValue;
    public GameObject keepNewRes;
    public float waitForUserConfirmTime = 15f;
    IEnumerator coRoutine;
    public bool hasResChanged = false;
    public Slider Timer;




    // Start is called before the first frame update
    void Start()
    {
        //Get resolutions when we start the game
        resolutions = Screen.resolutions;
        
        //Set resolution in the inspector before starting the game



        //If fullscreen is on, set the toggle to show that
        if (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen)
        {
            fullscreenToggle.isOn = true;
        }
        else
        {
            fullscreenToggle.isOn = false;
        }

        //If vSync is on, set the toggle to show that
        if (QualitySettings.vSyncCount == 0)
        {
            vsyncToggle.isOn = false;
        }
        else
        {
            vsyncToggle.isOn = true;
            framerateInput.interactable = false;
        }



        //Resolution dropdown menu
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        string option;
        for (int i = 0; i < resolutions.Length; i++)
        {

            //option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRate + "Hz";
            
            option = resolutions[i].width + " x " + resolutions[i].height;

            if (!options.Contains(option))
            {
                options.Add(option);
                shortResolutionList.Add(i);
            }



            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height && !options.Contains(resolutions[i].width + " x " + resolutions[i].height))
            {
            currentResolutionIndex = i;
            oldResValue = currentResolutionIndex;
            }
        }

        
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        
        
        if (!hasStarted)
        {
            RevertResolutionChanges();
            hasStarted = true;
        }
        

}

    // Update is called once per frame
    void Update()
    {
        if(hasResChanged)
        {
            //Timer.maxValue = waitForUserConfirmTime;
            Timer.value -= Time.unscaledDeltaTime;

            if(Timer.value <= 0)
            {
                hasResChanged = false;
                RevertResolutionChanges();
            }
        }
    }


    public void ApplyGraphics()
    {
        //Apply fullscreen
        if (fullscreenToggle.isOn)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }

        //Apply vSync
        if(vsyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
            framerateInput.interactable = false;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
            framerateInput.interactable = true;
        }

        //Apply framerate
        Application.targetFrameRate = framerateTarget;

    }

    

    public void UpdateValueFromStringFramerate()
    {
        bool successs = int.TryParse(framerateInput.text, out framerateTarget);
    }










    //Methods to change resolve and confirm whether the new resolution works
    public void ChangeResolution()
    {
        //oldResValue = resolutions[shortResolutionList[]];
        Resolution resolution = resolutions[shortResolutionList[resolutionDropdown.value]];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
        keepNewRes.SetActive(true);
        newResValue = resolutionDropdown.value;
        oldRes.text = "OLD:    " + resolutions[shortResolutionList[oldResValue]].width + " x " + resolutions[shortResolutionList[oldResValue]].height;
        newRes.text = "NEW:    " + resolution.width + " x " + resolution.height;
        Timer.maxValue = waitForUserConfirmTime;
        Timer.value = Timer.maxValue;

        hasResChanged = true;


    }

    //Buttons to keep or revert the current changes
    public void KeepResolutionChanges()
    {
        hasResChanged = false;
        Resolution resolution = resolutions[shortResolutionList[newResValue]];
        oldResValue = newResValue;
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
        keepNewRes.SetActive(false);
    }
    public void RevertResolutionChanges()
    {
        hasResChanged = false;
        resolutionDropdown.value = oldResValue;
        Resolution resolution = resolutions[shortResolutionList[oldResValue]];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
        keepNewRes.SetActive(false);
    }


}
