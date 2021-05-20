using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource hurt;
    public AudioSource attack;
    public AudioSource heal;

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
    
}
