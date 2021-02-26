using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "AttackData")]
public class AttackData : ScriptableObject
{
    public int damage;

    public float forceMultiplier = 1.0f;

    public float recoveryTime = 1.5f;

    public string [] doNotHurtList;
}
