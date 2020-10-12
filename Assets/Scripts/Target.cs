using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public bool isHit;

    public AudioClip hitSound;
    public AudioClip downSound;
    public AudioClip upSound;
    public AudioSource audioSource;

    private void Update()
    {
        if (isHit)
        {
            audioSource.clip = hitSound;
            audioSource.Play();
            isHit = false;
        }
    }
}
