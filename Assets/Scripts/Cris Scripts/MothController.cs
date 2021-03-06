﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothController : MonoBehaviour
{
    public Collider2D grandma; //to ignore collisions between players
    public GameObject halo; //to turn on when the moth is napping
    public float speed;
    public float sleepySpeed; //the speed the moth falls asleep

    private Animator anim;

    private Rigidbody2D rb2d;
    private Vector2 velocity;
    private float currentSpeed;
    private bool sleepyTime; //true if outside a light source
    public List<GameObject> collidingLights;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        velocity = new Vector2(0, 0);
        currentSpeed = speed;
        sleepyTime = true;
        collidingLights = new List<GameObject>();

        if (grandma)
            Physics2D.IgnoreCollision(grandma, GetComponent<Collider2D>());
    }

    void FixedUpdate()
    {
        updateAnimations();

        ///Adjusting current speed based on if it's outside a light source ("Glow")
        if (sleepyTime)
        {
            if (currentSpeed > 0.001)
                currentSpeed -= Time.fixedDeltaTime / sleepySpeed;
            rb2d.gravityScale += Time.fixedDeltaTime / sleepySpeed;
            if (!GetComponent<ParticleSystem>().isPlaying)
            {
                GetComponent<ParticleSystem>().Play();
            }
        }
        else
        {
            GetComponent<ParticleSystem>().Stop();
            currentSpeed = speed;
            rb2d.gravityScale = 0;
        }

        rb2d.MovePosition(rb2d.position + new Vector2(Input.GetAxisRaw("DPadHorizontal2"), Input.GetAxisRaw("DPadVertical2")) * Time.fixedDeltaTime * currentSpeed);

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Contains("Glow"))
        {
            collidingLights.Remove(other.gameObject);
        }
        if (collidingLights.Count == 0)
            sleepyTime = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Contains("Glow") && other.transform.localScale != new Vector3(0, 0, 0))
        {
            collidingLights.Add(other.gameObject);
            sleepyTime = false;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag.Contains("Glow") && other.transform.localScale != new Vector3(0, 0, 0))
        {
            if (!collidingLights.Contains(other.gameObject))
                collidingLights.Add(other.gameObject);
            sleepyTime = false;
        }
    }

    void updateAnimations()
    {
        anim.SetBool("sleeping", sleepyTime);
        halo.SetActive(sleepyTime);
        if (sleepyTime)
        { return; }

        halo.SetActive(false);
        //Flip sprite based on where the player is moving
        if (Input.GetAxis("DPadHorizontal2") == -1)
            GetComponent<SpriteRenderer>().flipX = true;
        else if (Input.GetAxis("DPadHorizontal2") == 1)
            GetComponent<SpriteRenderer>().flipX = false;
    }
}
