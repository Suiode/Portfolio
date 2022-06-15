using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HeaderSelection : MonoBehaviour
{
    public GameObject currentlySelected;

    public Image currentlySelectedImage;
    public GameObject gameplayOptions;
    public GameObject videoOptions;
    public GameObject audioOptions;
    public GameObject miscOptions;

    public void SetNewActive(GameObject newSelected, Image selectedBackgroundImage)
    {
        if (newSelected != currentlySelected)
        {
            DeselectCurrentActive(selectedBackgroundImage);
            currentlySelected = newSelected;
            currentlySelectedImage = selectedBackgroundImage;
            selectedBackgroundImage.gameObject.SetActive(true);
            currentlySelected.SetActive(true);
        }
        
    }

    public void DeselectCurrentActive(Image selectedBackgroundImage)
    {
        if (currentlySelected != null)
        {
            currentlySelectedImage.gameObject.SetActive(false);
            currentlySelected.SetActive(false);
            currentlySelected = null;

        }
    }

}
