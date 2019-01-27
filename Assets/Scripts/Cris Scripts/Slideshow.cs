﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slideshow : MonoBehaviour
{
    ///Made specifically for the final cutscene

    public SpriteRenderer sceneImage;
    public SpriteRenderer transImage;
    public List<Sprite> backgrounds;
    public List<int> timings; //List of timings, every other timing is a fade

    private int currentBackgroundIndex;
    private int currentTimingIndex;
    private float timer;
    private bool fading;

    private int drawDepth = -1000;
    private float alpha = 1.0f;
    private int fadeDirection = -1;

    void Start()
    {
        sceneImage.sprite = backgrounds[0];
        transImage.sprite = backgrounds[1];

        currentBackgroundIndex = 0;
        currentTimingIndex = 0;
        timer = 0;
        fading = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTimingIndex >= timings.Count || currentBackgroundIndex >= backgrounds.Count)
            return;

        if (fading)
            Fade();

        timer += Time.deltaTime;
        //Debug.Log(currentTimingIndex);
        if(timer >= timings[currentTimingIndex])
        {
            if (fading)
            {
                Debug.Log(backgrounds[currentBackgroundIndex]);
                if (currentBackgroundIndex + 2 <= backgrounds.Count)
                {
                    sceneImage.sprite = backgrounds[currentBackgroundIndex++];
                    transImage.sprite = backgrounds[currentBackgroundIndex];
                }

                sceneImage.color = Color.white;
                transImage.color = Color.white;

                //change timing vars
                currentTimingIndex++;
                timer = 0;
                fading = false;
            }
            else
            {
                StartCoroutine(FadeWait(timings[currentTimingIndex]));
            }
        }
    }

    void Fade()
    {
        float a = timer / timings[currentTimingIndex];
        sceneImage.color = new Color(1, 1, 1, 1-a);
        transImage.color = new Color(1, 1, 1, a);
    }

    IEnumerator FadeWait(int timeWait)
    {
        yield return new WaitForSecondsRealtime(timeWait);
        fading = true;
    }

}