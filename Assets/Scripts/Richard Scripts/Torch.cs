﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : Interactable
{
    [Header("Torch Values")]
    [Range(0f, 5f)]
    public float setLightTimer = 2;

    private float lightTimer;

    // TEMPORARY
    private SpriteRenderer sr;

    public void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        base.Update();

        if (active)
        {
            lightTimer -= Time.deltaTime;

            if (lightTimer <= 0f)
            {
                sr.color = Color.white;

                active = false;
            }
        }
    }

    public override void mothTriggerEffect()
    {
        return;
    }

    public override void lightTriggerEffect()
    {
        active = true;

        sr.color = Color.red;

        lightTimer = setLightTimer;
    }
}
