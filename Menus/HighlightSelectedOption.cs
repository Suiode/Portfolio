using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class HighlightSelectedOption : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    //public Button menuButton;
    public HeaderSelection headerMenus;
    public Image backgroundImage;
    public GameObject optionsMenu;

    // Start is called before the first frame update


    public void OnSelect(BaseEventData eventData)
    {
        headerMenus.SetNewActive(optionsMenu, backgroundImage);
    }

    public void OnDeselect(BaseEventData data)
    {

    }

}