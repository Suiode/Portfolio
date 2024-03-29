﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseLook : MonoBehaviour
{

    [SerializeField] private float mouseX;
    [SerializeField] private float mouseY;
    public bool linkedXY = true;
    public Slider mouseSensitivityX;
    public Slider mouseSensitivityY;
    public TMP_InputField mouseSensXNum;
    public TMP_InputField mouseSensYNum;
    [SerializeField] private Camera playerCam;

    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform eyes;


    public float yMouseSens = 10f;
    public float xMouseSens = 10f;
    float xRotation;
    float yRotation;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * (xMouseSens * mouseSensitivityX.value)/10 * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * (yMouseSens * mouseSensitivityY.value)/10 * Time.deltaTime;

        xRotation -= mouseY;
        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(0, yRotation, 0);
        eyes.transform.localRotation = Quaternion.Euler(xRotation, 0, 0); ;


        if (PauseScript.gameIsPaused)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if(!PauseScript.gameIsPaused)
            Cursor.lockState = CursorLockMode.Locked;

    }



    public void UpdateValueFromFloatX(float value)
    {
        value = Mathf.Round(value * 10) / 10;
        if (mouseSensitivityX) 
			mouseSensitivityX.value = value; 
        if (mouseSensXNum) 
			mouseSensXNum.text = value.ToString(); 

        if(linkedXY)
        {
            if (mouseSensitivityY) 
				mouseSensitivityY.value = value; 
            if (mouseSensYNum) 
				mouseSensYNum.text = value.ToString(); 
        }
    }

    public void UpdateValueFromStringX(string value)
    {
        value = string.Format("{0:0.##}", value);
        if (mouseSensitivityX) 
			mouseSensitivityX.value = float.Parse(value); 
        if (mouseSensXNum) 
			mouseSensXNum.text = value; 

        if (linkedXY)
        {
            if (mouseSensitivityY) 
				mouseSensitivityY.value = float.Parse(value); 
            if (mouseSensYNum) 
				mouseSensYNum.text = value;
        }

    }


    public void UpdateValueFromFloatY(float value)
    {
        value = Mathf.Round(value * 10) / 10;
        if (mouseSensitivityY) 
			mouseSensitivityY.value = value;
        if (mouseSensYNum) 
			mouseSensYNum.text = value.ToString();

        if (linkedXY)
        {
            if (mouseSensitivityX) 
				mouseSensitivityX.value = value;
            if (mouseSensXNum) 
				mouseSensXNum.text = value.ToString();
        }
    }

    public void UpdateValueFromStringY(string value)
    {
        value = string.Format("{0:0.##}", value);
        if (mouseSensitivityY) 
			mouseSensitivityY.value = float.Parse(value);
        if (mouseSensYNum) 
			mouseSensYNum.text = value;

        if (linkedXY)
        {
            if (mouseSensitivityX) 
				mouseSensitivityX.value = float.Parse(value);
            if (mouseSensXNum) {
				mouseSensXNum.text = value;
        }
    }

    public void ToogleLink()
    {
        if(linkedXY)
            linkedXY = false;
        else
            linkedXY = true;
    }

}

