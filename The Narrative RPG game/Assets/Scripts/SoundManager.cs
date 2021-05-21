using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource hurt;
    public AudioSource attack;
    public AudioSource heal;
    public AudioSource magic;
    public AudioSource explosion;
    public AudioSource teleport;
    public AudioSource animepunch;

    public AudioSource shield;

    public void PlayHurt()
    {
        hurt.Play();
    }

    public void PlayHeal()
    {
        heal.Play();
    }
    
    public void PlayAttack()
    {
        attack.Play();
    }

    public void PlayMagic()
    {
        magic.Play();
    }

    public void PlayExplosion()
    {
        explosion.Play();
    }

    public void PlayTeleport()
    {
        teleport.Play();
    }

    public void PlayAnimePunch()
    {
        animepunch.Play();
    }

    public void PlayShield()
    {
        shield.Play();
    }
    
}
