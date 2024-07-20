using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface HealthController
{

    public bool IsDead(); //checks to see if the player should be dead

    public void TakeDamage(int damage); //for taking damage

    public void TakeHeal(int heal); //for healing or increasing health

    public void Kill(); //kills player is they are supposed to be dead, disables controls, and bring up DeathMenu

    public void MaxHealthCheck();  //checks to see if player is over maxHealth, and will set health=maxhealth if this happens

}
