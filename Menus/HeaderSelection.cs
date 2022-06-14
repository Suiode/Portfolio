using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HeaderSelection : MonoBehaviour
{
    public GameObject currentlySelected;
    //public GameObject newButtonClicked;

    public Image currentlySelectedImage;
    public GameObject gameplayOptions;
    public GameObject videoOptions;
    public GameObject audioOptions;
    public GameObject miscOptions;

    public void SetNewActive(GameObject newSelected, Image selectedBackgroundImage)
    {
        //currentlySelectedImage.gameObject.SetActive(false);
        if (newSelected != currentlySelected)
        {
            //currentlySelectedImage.gameObject.SetActive(false);
            DeselectCurrentActive(selectedBackgroundImage);
            currentlySelected = newSelected;
            currentlySelectedImage = selectedBackgroundImage;
            selectedBackgroundImage.gameObject.SetActive(true);
            currentlySelected.SetActive(true);
        }
        
        //Image selectedBackgroundImage = this.gameObject.GetComponent<Image>();
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
