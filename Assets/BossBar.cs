using System.Collections;
using System.Collections.Generic;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossBar : MonoBehaviour
{
    [SerializeField]private Slider health, easeHealth;
    [SerializeField]private float lerpSpeed;
    [SerializeField]private float healthFill;
    [SerializeField]private TextMeshProUGUI bossName;
    [SerializeField]private GameObject barContainer;
    // Start is called before the first frame update
    void Start()
    {
        healthFill = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(health.value != healthFill)
        {
            health.value = healthFill;
        }
        if(health.value != easeHealth.value)
        {
            easeHealth.value = Mathf.Lerp(easeHealth.value, healthFill, lerpSpeed);
        }
    }

    public void SetHealth(int health, int maxhealth)
    {
        healthFill = (float)health/(float)maxhealth;
    }

    public void setBossName(string name)
    {
        bossName.text = name;
    }

    public void EnableBar()
    {
        barContainer.SetActive(true);
    }

    public void DisableBar()
    {
        barContainer.SetActive(false);
    }
}
