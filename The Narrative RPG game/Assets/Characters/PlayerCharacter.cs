using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "player")]
public class PlayerCharacter : ScriptableObject
{
    public new string name;

    public Sprite PlayerSprite;
    public int attack;
    public int defense;
    public int maxHealth;
    public int HP;
    public int lvl;

    void Attack()
    {

    }

    void Dodge()
    {

    }

    void Heal()
    {

    }

    void Run()
    {

    }

}
