using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ryu : Character
{

    public WaterPoints waterpoints;

    public int waterpointsMax;

    public int startingWaterPoints;

    public HUD hudPrefab;

    HUD hud;





    //Time it takes for Ryu to regenerate one point of water.
    [Header("Water Regeneration")]
    [Tooltip("Time needed to Regenerate Water")]
    public float waterRegenerationTime = 1.0f;
    public int waterRegenerationAmount = 1;

    float lastRegeneration;

    //We'll need this PlatformerBehavior within the player.
    PlatformerBehavior pb;


    [Header("Hurt Behavior")]
    [Tooltip("What to do in case Ryu is Hurt.")]
    public float hurtRecoverTime;
    public float hurtDuration;
    float lastTimeDamaged;

    
    
    [Header ("Recovery Effects")]
    public float recoveryTime = 1.5f;
    public float recoveryFlashTime = 0.2f;
    public Color recoverColor;
    float recoveryStartTime;
    float recoveryLastFlash;
    
    bool recovering;
    bool hurt;
    bool dying;
    bool transparent;

    SpriteRenderer sr;


    [Header("Dying Behavior")]
    [Tooltip("What to do in case Ryu has suffered a Mortal Blow.")]
    public int crumpleDamageThreshold;
    public float crumpleForceThreshold;


    // Start is called before the first frame update
    void Start()
    {
        initializeHitPoints();
        initializeWaterPoints();

        recovering = false;
        transparent = false;

        pb = GetComponent<PlatformerBehavior>();

        sr = GetComponent<SpriteRenderer>();
        
    }

    void Update()
    {
      RegenerateWater();


        if(recovering == true)
        {
            RecoverAnimation();
            EndRecover();
        }
       


    }

    

    void initializeWaterPoints()
    {
        waterpoints.value = startingWaterPoints;

    }

    void setWater(int val)
    {
        waterpoints.value += val;

        if(waterpoints.value > waterpointsMax)
        {
            waterpoints.value = waterpointsMax;
        }

        if(waterpoints.value < 0)
        {
            waterpoints.value = 0;
        }

    }

    public int getMaxHitPoints()
    {
        return hitpointsMax;
    }

    public int getMaxWaterPoints()
    {
        return waterpointsMax;
    }


    public void RegenerateWater()
    {
        if(waterRegenerationTime <= 0.0f || waterpoints.value >= waterpointsMax)
        {
            return;
        }

        if(lastRegeneration == null || Time.time - lastRegeneration >= waterRegenerationTime)
        {
            waterpoints.value += waterRegenerationAmount;

            lastRegeneration = Time.time;
        }

    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        //You can't pick stuff up when you're dead.
        if(dying == true)
        {
            return;
        }

        if(collision.gameObject.CompareTag("CanBePickedUp") && !dead)
        {
            Item hitObject = collision.gameObject.GetComponent<Consumable>().item;

            DeterminePickupValue(hitObject);
            

            collision.gameObject.SetActive(false);

            Destroy(collision.gameObject);
        }

        if(collision.gameObject.CompareTag("Hazard") && recovering == false)
        {
            AttackData attackData = collision.gameObject.GetComponent<Hazard>().attackData;

            int damage = attackData.damage;
            float forceMultiplier = attackData.forceMultiplier;
            float recoveryTime = attackData.recoveryTime;

            Hurt(damage, forceMultiplier, recoveryTime);


        }
    }


    void Hurt(int val, float forceMultiplier, float recoveryTime)
    {
        adjustHitPoints(-val);
        
        if(hitpoints.value <= 0)
        {
            Dying();

            pb.Dying(val, forceMultiplier);
        }
        else{

            StartRecovery();

            pb.Hurt(forceMultiplier);

        }
    }
    

    void StartRecovery()
    {
        recovering = true;

        recoveryLastFlash = recoveryStartTime = Time.time;

    }

    void RecoverAnimation()
    {
        if(Time.time - recoveryLastFlash >= recoveryFlashTime)
        {
            if(transparent == false)
            {
                transparent = true;
                
                sr.color = recoverColor;

            }
            else if(transparent == true)
            {
                transparent = false;
                sr.color = Color.white;
            }

            
        }

    }

    void EndRecover()
    {
        if(Time.time - recoveryStartTime >= recoveryTime)
        {
            sr.color = Color.white;
            transparent = false;
            recovering = false;
        }

    }

    void Dying()
    {
        //We are Dying.  Time to die dramatically.

        recovering = false;
        dying = true;


    }



    void DeterminePickupValue(Item p)
    {
        switch(p.type)
        {
            case 0:
                adjustHitPoints(p.quantity);
            break;

        }


    }
}
