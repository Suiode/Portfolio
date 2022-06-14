using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Misc_options : MonoBehaviour
{
    public Toggle FPSCounterToggle;
    public GameObject FPSCounter;
    public bool fpsOn;

    public Toggle infiniteAmmoToggle;
    public bool infiniteAmmo;
    public Toggle laserGunToggle;
    public bool laserGun;



    // Start is called before the first frame update
    void Start()
    {
        if(FPSCounterToggle.isOn)
        {
            fpsOn = true;
            FPSCounter.gameObject.SetActive(true);
        }
        else
        {
            fpsOn = false;
            FPSCounter.gameObject.SetActive(false);
        }

        if(infiniteAmmoToggle.isOn)
        {
            infiniteAmmo = true;
        }
        else
        {
            infiniteAmmo = false;

        }
        if (laserGunToggle.isOn)
        {
            laserGun = true;
        }
        else
        {
            laserGun = false;

        }
    }

    // Update is called once per frame
    void Update()
    {


    }



    public void ToggleFPSCounter()
    {
        if(fpsOn)
        {
            fpsOn = false;
            FPSCounter.gameObject.SetActive(false);
        }
        else
        {
            fpsOn = true;
            FPSCounter.gameObject.SetActive(true);
        }
    }

    public void ToggleInfiniteAmmo()
    {
        if (infiniteAmmo)
        {
            infiniteAmmo = false;
        }
        else
        {
            infiniteAmmo = true;
        }
    }
    public void ToggleLaserGun()
    {
        if (laserGun)
        {
            laserGun = false;
        }
        else
        {
            laserGun = true;
        }
    }

}
