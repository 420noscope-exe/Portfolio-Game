using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class UpdateFPSMeter : MonoBehaviour
{
    [SerializeField]private GameObject fpsMeter;
    [SerializeField]private TextMeshProUGUI fpsText;
    private float[] frames = new float[50];
    private int frameIndex;
    private float frameAverage;
    // Start is called before the first frame update
    void Start()
    {
        fpsText = fpsMeter.GetComponent<TextMeshProUGUI>();
        frameIndex = 0;
        frameAverage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        frames[frameIndex] = Time.deltaTime;

        foreach(float frame in frames)
        {
            frameAverage += frame;
        }

        frameAverage = frameAverage/frames.Length;

        //print(1f/Time.deltaTime);
        fpsText.text = Mathf.RoundToInt(1f/frameAverage).ToString() + " FPS";

        frameIndex++;
        if(frameIndex > frames.Length - 1)
        {
            frameIndex = 0;
        }
    }
}
