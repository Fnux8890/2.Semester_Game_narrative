using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

[RequireComponent(typeof(AudioSource))]

public class SoundManager : Singleton<SoundManager>
{
    private AudioSource component;
    private AudioClip hurt;
    private AudioClip attack;
    private AudioClip heal;
    private AudioClip magic;
    private AudioClip explosion;
    private AudioClip teleport;
    private AudioClip animepunch;

    private AudioClip shield;

    private void Start()
    {
        component = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        hurt = Resources.Load<AudioClip>("Sounds/Hurt");
        heal = Resources.Load<AudioClip>("Sounds/Heal");
        attack = Resources.Load<AudioClip>("Sounds/Attack");
        magic = Resources.Load<AudioClip>("Sounds/Magic");
        explosion = Resources.Load<AudioClip>("Sounds/Explosion");
        teleport = Resources.Load<AudioClip>("Sounds/Teleport");
        animepunch = Resources.Load<AudioClip>("Sounds/strongpunch");
        shield = Resources.Load<AudioClip>("Sounds/Shield");
        
    }

    public void PlayHurt()
    {
        component.clip = hurt;
        component.Play();
    }

    public void PlayHeal()
    {
        component.clip = heal;
        component.Play();
    }
    
    public void PlayAttack()
    {
        component.clip = attack;
        component.Play();
    }

    public void PlayMagic()
    {component.clip = magic;
        component.Play();
    }

    public void PlayExplosion()
    {
        component.clip = explosion;
        component.Play();
    }

    public void PlayTeleport()
    {
        component.clip = teleport;
        component.Play();
    }

    public void PlayAnimePunch()
    {
        component.clip = animepunch;
        component.Play();
    }

    public void PlayShield()
    {
        component.clip = shield;
        component.Play();
    }
    
}
