using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    [SerializeField]private List<GameObject> guns = new List<GameObject>();
    [SerializeField]private GameObject currentGun;
    [SerializeField]private GameObject gunParent;  //"Gun" gameobject in the player prefab.  Position and rotation is already set.
    [SerializeField]private Image AmmoMeter;

    // Start is called before the first frame update
    void Start()
    {
        //foreach(GameObject gun in guns)
        //{
        //    GameObject temp = Instantiate(gun, gunParent.transform);
        //
        //}
        AmmoMeter = GameObject.Find("AmmoMeter").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        setCurrentGun();
        setAmmoMeter();
    }

    void setAmmoMeter()
    {
        if(currentGun == null)
        {
        AmmoMeter.fillAmount = 0;
        }
        else
        {
        float ammoPercentage = currentGun.GetComponent<Gun>().getAmmo();
        AmmoMeter.fillAmount = ammoPercentage;
        }
    }

    public int checkButtons()
    {
        if(Input.GetButtonDown("Gun1"))
        {
            return 0;
        }
        else if(Input.GetButtonDown("Gun2"))
        {
            return 1;
        }
        else if(Input.GetButtonDown("Gun3"))
        {
            return 2;
        }
        else if(Input.GetButtonDown("Gun4"))
        {
            return 3;
        }
        else if(Input.GetButtonDown("Gun5"))
        {
            return 4;
        }
        return -1;
    }

    public void pickupGun(GameObject gun)
    {
        deactivateGun();
        GameObject temp = Instantiate(gun, gunParent.transform);
        guns.Add(temp);
        currentGun = temp;
    }

    private void setCurrentGun()
    {
        int temp = checkButtons();
        if(temp > -1 && temp < guns.Count)
        {
            deactivateGun();
            currentGun = guns[temp];
            activateGun();
        }
    }

    private void setCurrentGun(int num)
    {
        deactivateGun();
        currentGun = guns[num];
        activateGun();
    }

    private void deactivateGun()
    {
        if(currentGun != null)
        {
            currentGun.SetActive(false);
        }
        
    }

    private void activateGun()
    {
        currentGun.SetActive(true);
    }
}
