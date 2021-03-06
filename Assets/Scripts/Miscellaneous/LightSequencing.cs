﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSequencing : MonoBehaviour
{
    public enum SequenceType
    {
        Flash,
        Fade
    };

    public SequenceType sequenceType;
    public List<Light> lights = new List<Light>();
    public Color startingColour = Color.red;
    public Color endColour = Color.green;
    public float timeBetweenFlashes = 1.0f;
    public float timeUntilFlashingEnds = 10.0f;

    private Color targetColor;

    // Only used for Fade.
    public AnimationCurve intensity;

    // Start is called before the first frame update
    void Start()
    {
        if (sequenceType == SequenceType.Flash)
        {
            targetColor = endColour;
            ChangeLightColour();

            Invoke("EndRepeating", timeUntilFlashingEnds);
            InvokeRepeating("ChangeLightColour", timeBetweenFlashes, timeBetweenFlashes);            
        }
        else if (sequenceType == SequenceType.Fade)
        {
            InvokeRepeating("ChangeLightIntensity", 0.0f, timeBetweenFlashes);
        }
    }

    private void ChangeLightIntensity()
    {
        float randTime = Random.Range(0.0f, 1.0f);

        foreach (Light element in lights)
        {
            element.intensity = intensity.Evaluate(randTime);
        }
    }

    private void EndRepeating()
    {
        targetColor = endColour;
        foreach (Light element in lights)
        {
            element.color = targetColor;
        }

        CancelInvoke("ChangeLightColour");
    }

    private void ChangeLightColour()
    {
        targetColor = (targetColor == startingColour) ? endColour : startingColour;

        foreach (Light element in lights)
        {
            element.color = targetColor;
        }
    }

}
