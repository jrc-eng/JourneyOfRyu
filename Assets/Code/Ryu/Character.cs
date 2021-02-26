using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public HitPoints hitpoints;

    public int hitpointsMax;

    public int startingHitPoints = 75;

    public bool dead;

    enum CharStatus
    {
        Good = 0,
        Dead = 1



    }

    void Start()
    {
        initializeHitPoints();



    }

    protected void initializeHitPoints()
    {
        hitpoints.value = startingHitPoints;

    }

    void Update()
    {


    }

    protected void adjustHitPoints(int val)
    {

        hitpoints.value += val;

        if(hitpoints.value > hitpointsMax)
        {
            hitpoints.value = hitpointsMax;
        }

        else if(hitpoints.value < 0)
        {
            hitpoints.value = 0;
        }


    }

    void checkIfDead()
    {
        if(hitpoints.value <= 0)
        {
            dead = true;
            //Death Details go here.
        }

    }

    public virtual void KillCharacter()
    {
        Destroy(gameObject);
    }

    public virtual void TakeDamage(int damageValue)
    {
        adjustHitPoints(-damageValue);
    }

    public virtual void RestoreHitPoints(int restoreValue)
    {
        adjustHitPoints(restoreValue);
    }

}
