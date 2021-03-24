using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{

    //fields
    int level;
    int baseAttack;
    int baseDeffence;
    int hp;
    int maxHp;
    int id;

    public string name { get; }

    public List<Actions> Actions { get; }







}
