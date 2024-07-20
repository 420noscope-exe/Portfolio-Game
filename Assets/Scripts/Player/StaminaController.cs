using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{
    [SerializeField] private float stamina;
    [SerializeField] private float maxStamina;
    private float staminaPercentage;
    private PlayerController playerController;
    [SerializeField] private Image staminaMeter;

    // Start is called before the first frame update
    void Start()
    {
        stamina = maxStamina;
        staminaPercentage = stamina/maxStamina;
        playerController = gameObject.GetComponent<PlayerController>();
        staminaMeter = GameObject.Find("StaminaMeter").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        SetStaminaMeter();
    }
    
    // FixedUpdate is called once every .02 seconds
    void FixedUpdate()
    {
        RegenStamina();
        MaxStaminaCheck();
    }

    private void SetStaminaMeter()
    {
        staminaPercentage = stamina/maxStamina;
        staminaMeter.fillAmount = staminaPercentage;
    }

    private void MaxStaminaCheck() //checks to see if player is over maxStamina, and will set stamina=maxStamina if this happens
    {
        if(stamina > 3.0f)
        {
            stamina = 3.0f;
        }
    }

    public bool CanDash() //checks to see if the player can dash
    {
        if(stamina >= 1.0f)
        {
            return true;
        }
        return false;
    }

    public void UseStamina()
    {
        stamina = stamina - 1.0f;
    }

    private void RegenStamina()
    {
        if(!playerController.GetIsDashing())
        {
            stamina += 1.0f * Time.fixedDeltaTime;
        }
    }
}
