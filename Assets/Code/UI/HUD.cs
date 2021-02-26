using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public HitPoints hitpoints;

    public WaterPoints waterpoints;

    public Ryu ryu;

    public int hitpointsMax;

    int waterpointsMax;

    public Image waterImage;

    public Image hitpointsImage;



    [Header("Meter Ghost Images")]
    public Image hitpointsGhostImage;
    public Image waterpointsGhostImage;
    public float ghostMoveTime = 0.2f;

    
    public int ghostHitPoints = 50;
    public int ghostWaterPoints = 50;

    float lastGhostMove;
    


    public int currentHitPoints;

    public Text healthText;
    public Text waterText;

   
    
    void Start()
    {
        this.hitpointsMax = ryu.getMaxHitPoints();

        this.waterpointsMax = ryu.getMaxWaterPoints();

        lastGhostMove = 0.0f;
        

    }

    void Update()
    {
        if(ryu == null)
        {
            findRyu();
        }
        else
        {
            setHitPointMeter();

            setWaterMeter();

            currentHitPoints = hitpoints.value;

            setText();
        }

        


    }

    void FixedUpdate()
    {

        if(Time.time - ghostMoveTime >= lastGhostMove)
        {
            setGhostHitPointMeter();
            setGhostWaterPointMeter();

            lastGhostMove = Time.time;
        }
    }

    void setHitPointMeter()
    {
        hitpointsImage.fillAmount = (float)hitpoints.value / hitpointsMax;

    }

    void setWaterMeter()
    {

        waterImage.fillAmount = (float)waterpoints.value / waterpointsMax;
    }


    void findRyu()
    {



    }

    void setText()
    {
        healthText.text = "" + hitpoints.value;

        waterText.text = "" + waterpoints.value;

    }

    void setGhostHitPointMeter()
    {
        if(ghostHitPoints == hitpoints.value)
        {
            return;
        }
        else if(ghostHitPoints > hitpoints.value)
        {
            ghostHitPoints -= 1;

        }
        else if(ghostHitPoints < hitpoints.value)
        {
            ghostHitPoints += 1;

        }

        hitpointsGhostImage.fillAmount = (float)ghostHitPoints / hitpointsMax;
    }

    void setGhostWaterPointMeter()
    {
        if(ghostWaterPoints == waterpoints.value)
        {
            return;
        }
        else if(ghostWaterPoints > waterpoints.value)
        {
            ghostWaterPoints -= 1;

        }
        else if(ghostWaterPoints < waterpoints.value)
        {
            ghostWaterPoints += 1;

        }

        waterpointsGhostImage.fillAmount = (float)ghostWaterPoints / waterpointsMax;


    }
}
