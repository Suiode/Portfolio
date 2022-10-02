using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseLook : MonoBehaviour
{
    [Header("Mouse variables")]
    [SerializedField] private float mouseX;
    [SerializedField] private float mouseY;
    [SerializedField] private float yMouseSens = 100f;
    [SerializedField] private float xMouseSens = 100f;
    private float xRotation;
    private float yRotation;

    [Header("Game Objects")]
    [SerializedField] private Transform playerBody;
    [SerializedField] private Transform eyes;
    [SerializedField] private Camera playerCam;

    ////Future use with UI
    //public bool linkedXY = true;
    //public Slider mouseSensitivityX;
    //public Slider mouseSensitivityY;
    //public TMP_InputField mouseSensXNum;
    //public TMP_InputField mouseSensYNum;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //xMouseSens = xMouseSens * mouseSensitivity.value;
        //yMouseSens = yMouseSens * mouseSensitivity.value;
        //mouseX = Input.GetAxis("Mouse X") * (xMouseSens * mouseSensitivityX.value)/10 * Time.deltaTime;
        //mouseY = Input.GetAxis("Mouse Y") * (yMouseSens * mouseSensitivityY.value)/10 * Time.deltaTime;


        mouseX = Input.GetAxis("Mouse X") * (xMouseSens) * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * (yMouseSens) * Time.deltaTime;

        xRotation -= mouseY;
        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(0, yRotation, 0);
        eyes.transform.localRotation = Quaternion.Euler(xRotation, 0, 0); ;


    }




    //Scripts for UI elements, currently not used

    /*public void UpdateValueFromFloatX(float value)
    {
        value = Mathf.Round(value * 10) / 10;
        if (mouseSensitivityX) { mouseSensitivityX.value = value; }
        if (mouseSensXNum) { mouseSensXNum.text = value.ToString(); }

        if(linkedXY)
        {
            if (mouseSensitivityY) { mouseSensitivityY.value = value; }
            if (mouseSensYNum) { mouseSensYNum.text = value.ToString(); }
        }
    }

    public void UpdateValueFromStringX(string value)
    {
        value = string.Format("{0:0.##}", value);
        if (mouseSensitivityX) { mouseSensitivityX.value = float.Parse(value); }
        if (mouseSensXNum) { mouseSensXNum.text = value; }

        if (linkedXY)
        {
            if (mouseSensitivityY) { mouseSensitivityY.value = float.Parse(value); }
            if (mouseSensYNum) { mouseSensYNum.text = value; }
        }

    }


    public void UpdateValueFromFloatY(float value)
    {
        value = Mathf.Round(value * 10) / 10;
        if (mouseSensitivityY) { mouseSensitivityY.value = value; }
        if (mouseSensYNum) { mouseSensYNum.text = value.ToString(); }

        if (linkedXY)
        {
            if (mouseSensitivityX) { mouseSensitivityX.value = value; }
            if (mouseSensXNum) { mouseSensXNum.text = value.ToString(); }
        }
    }

    public void UpdateValueFromStringY(string value)
    {
        value = string.Format("{0:0.##}", value);
        if (mouseSensitivityY) { mouseSensitivityY.value = float.Parse(value); }
        if (mouseSensYNum) { mouseSensYNum.text = value; }

        if (linkedXY)
        {
            if (mouseSensitivityX) { mouseSensitivityX.value = float.Parse(value); }
            if (mouseSensXNum) { mouseSensXNum.text = value; }
        }
    }

    public void ToogleLink()
    {
        if(linkedXY)
        {
            linkedXY = false;
        }
        else
        {
            linkedXY = true;
        }
    }*/

}


