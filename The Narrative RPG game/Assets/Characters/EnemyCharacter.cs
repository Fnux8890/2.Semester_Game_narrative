using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Enemy", menuName = "enemy")]
public class EnemyCharacter : ScriptableObject
{
    public new string name;
    
    public Sprite EnemySprite;
    public int attack;
    public int defense;
    public int maxHealth;
    public int HP;


   void AttackRand()
    {
    
    }

   void Dodge()
    {

    }

    void Heal()
    {

    }

    void giveXP()
    {

    }

}
