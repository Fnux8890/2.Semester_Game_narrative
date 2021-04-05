using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{

    //fields
    string Name;
    int level;
    int baseAttack;
    int baseDefence;
    int hp;
    int maxHp;
    int id;

    public string CharacterName { get; }

    public List<Actions> Actions { get; }

    public int HP { get => hp; }

    public Character(string name, int level, int baseAttack,
            int baseDefence, int hp, 
            List<Actions> actions)
    {
        this.level = level;
        this.baseAttack = baseAttack;
        this.baseDefence = baseDefence;
        this.CharacterName = name;
        this.hp = hp;
        this.maxHp = hp;
        this.Actions = actions;
    }







}
