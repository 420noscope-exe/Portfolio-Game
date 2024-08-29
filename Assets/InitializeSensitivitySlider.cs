using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitializeSensitivitySlider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Slider>().value = PlayerPrefs.GetFloat("sensitivity", 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
