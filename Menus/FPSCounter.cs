using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSCounter : MonoBehaviour
{

    public TextMeshProUGUI fpsText;
    public Toggle FPSCounterToggle;


    // Start is called before the first frame update
    void Start()
    {
        if (FPSCounterToggle.isOn)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float FPSfloat = (int)(1f / Time.unscaledDeltaTime);
        fpsText.text = FPSfloat.ToString();
    }
}
