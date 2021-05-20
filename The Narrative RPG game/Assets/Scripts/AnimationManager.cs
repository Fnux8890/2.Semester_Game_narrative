using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator swordguy;
    public Animator supportgirl;
    public Animator edgelord;
    public Animator catdog;

    public Animator goblin;
    public Animator redknight;

    public Animator earthElemental;

    public Animator explosion;

    public void Start()
    {
        Invoke(nameof(LateStart), 1);
    }

    private void LateStart()
    {
        GameObject Swordguy = GameObject.FindGameObjectWithTag("Swordguy");
        swordguy = Swordguy.GetComponent<Animator>();
        GameObject Supportgirl = GameObject.FindGameObjectWithTag("Supportgirl");
        supportgirl = Supportgirl.GetComponent<Animator>();
        GameObject Edgelord = GameObject.FindGameObjectWithTag("Edgelord");
        edgelord = Edgelord.GetComponent<Animator>();
        GameObject Catdog = GameObject.FindGameObjectWithTag("Catdog");
        catdog = Catdog.GetComponent<Animator>();
        
        GameObject Redknight = GameObject.FindGameObjectWithTag("Redknight");
        redknight = Redknight.GetComponent<Animator>();
        
        
        
    }

    public void EarthElementalAttack()
    {
        earthElemental.Play("Earth_Elemental");
        //earthElemental.Play("EarthElemental_Attack");
    }

    public void EarthElementalIdle()
    {
        earthElemental.Play("EarthElemental_Idle");
    }

    public void Explosion()
    {
        explosion.Play("Explosion_Attack");
    }

    public void ExplosionIdle()
    {
        explosion.Play("Explosion_Idle");
    }

    public void SwordguyAttack()
    {
        GameObject Goblin = GameObject.FindGameObjectWithTag("Goblin");
        goblin = Goblin.GetComponent<Animator>();
        
        swordguy.Play("Swordguy_Attack");
        
        swordguy.Play("Swordguy_Idle");
    }

    public void SwordguyIdle()
    {
        swordguy.Play("Swordguy_Idle");
    }

    public void SupportgirlIdle()
    {
        supportgirl.Play("Supportgirl_Idle");
    }

    public void SupportgirlAttack()
    {
        supportgirl.Play("Supportgirl_Attack");
    }
}
