using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Gun
{
    // Start is called before the first frame update
    void fire();

    float getAmmo();
}
