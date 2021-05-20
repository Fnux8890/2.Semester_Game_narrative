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
    
}
