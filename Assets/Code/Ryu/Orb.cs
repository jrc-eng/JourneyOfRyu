using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{

    [Header("Follow Speed")]
    [Tooltip("Attributes for following Ryu the Master")]

    public float followSpeed;
    public float returnSpeed;
    public float sadSpeed;


    [Header("Attack 1")]
    [Tooltip("First Attack:  Orb-Punch")]
    [Range(0, 10f)]
    [SerializeField]
    private float boomerangDistance;


    [Header("Y Offset")]
    [Tooltip("Makes Ryu hold the orb correctly in their hands by offsetting the Y for following Ryu around.")]
    public float yOffset;


    public float rapidMoveSpeed;
    



    public GameObject leader;

    public bool hasLeader;

    public float followSharpness = 0.1f;

    Vector3 _followOffset;

    Rigidbody2D rb;

    Vector3 directionToPlayer;
    Vector3 localScale;


    enum AnimState
    {
        Normal = 0,
        Sad = 1,
        Stun = 2,
        Attack1 = 3,
        Returning = 4



    }
    public enum OrbState
    {
        Ready = 0,
        Sad = 1,
        Stun = 2,
        Attack1 = 3,
        Returning = 4

    }

    public OrbState orbState;

    void Start()
    {
        hasLeader = false;

        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate () 
    {
        if(hasLeader == false)
        {
            FindLeader();
        }
        else
        {
            DetermineAction();

        }
        
    }


    void DetermineAction()
    {
        int state = (int)orbState;

        switch(state)
        {
            case (int)OrbState.Ready:

                followLeader(followSpeed);

            break;

            case (int)OrbState.Sad:

                followLeader(followSpeed);

            break;

            case (int)OrbState.Returning:

                followLeader(returnSpeed);

            break;






        }

    }


    void FindLeader()
    {
        GameObject potentialLeader = GameObject.FindWithTag("Player");

        if(potentialLeader != null)
        {
            hasLeader = true;

            leader = potentialLeader;

        }

    }

    void followLeader(float speed)
    {

        Vector3 targetPosition = leader.transform.position;

        targetPosition.y += yOffset;

        directionToPlayer = (targetPosition - transform.position).normalized;

        rb.velocity = new Vector2(directionToPlayer.x, directionToPlayer.y) * speed;

    }



    //Animation Code:

    void determineAnimation()
    {







        
    }




}
