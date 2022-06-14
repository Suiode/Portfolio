using UnityEngine;

public class SwitchWeapons : MonoBehaviour
{

    public int selectedWeapon = 0;
    //public PauseScript pauseScript;


    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;
        if(Input.GetAxis("Mouse ScrollWheel") > 0 && !PauseScript.gameIsPaused)
        {
            if (selectedWeapon >= transform.childCount - 1)
            {
                selectedWeapon = 0;
            }
            else
            selectedWeapon++;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && !PauseScript.gameIsPaused)
        {
            if (selectedWeapon <= 0)
            {
                selectedWeapon = transform.childCount -1;
            }
            else
                selectedWeapon--;
        }

        if(Input.GetKeyDown(KeyCode.Alpha1) && !PauseScript.gameIsPaused)
        {
            selectedWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && !PauseScript.gameIsPaused)
        {
            selectedWeapon = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && !PauseScript.gameIsPaused)
        {
            selectedWeapon = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && !PauseScript.gameIsPaused)
        {
            selectedWeapon = 3;
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;

        foreach(Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
                weapon.gameObject.SetActive(false);

            i++;
        }

    }
}
