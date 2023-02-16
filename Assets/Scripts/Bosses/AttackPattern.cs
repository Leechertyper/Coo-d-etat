using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern : MonoBehaviour
{
    // The attacks can be in a chain. If this is a chain attack it will be true, false otherwise.
    [Header("Will another attack succeed this one?")]
    [SerializeField] private bool hasFollowingAttack = false;

    [Header("Attack weight (between 1-5, any other values will be rounded)")]
    [SerializeField] private int weight;

    // The positions this pattern will strike
    [Header("Make sure you know the size of the grid is the same size as the attack pattern. Otherwise it will be excluded in the grid script")]
    [SerializeField] private Vector2[] positions;

    // The object for the next attack
    [Header("Give another attack pattern object that will run after this one (if hasFollowingAttack is false, this will be ignored")]
    [SerializeField] private AttackPattern nextAttack;

    [Header("Give a delay in second, until the next attack runs")]
    [SerializeField] private float nextAttackDelay;

    /// <summary>
    /// A Vector2 array of the attack positions
    /// </summary>
    public Vector2[] Positions { get => positions; set => positions = value; }

    /// <summary>
    /// The weight of this attacks chance to be used
    /// </summary>
    public int Weight { get => weight; set => weight = value; }

    /// <summary>
    /// The attack to be run after this one
    /// </summary>
    public AttackPattern NextAttack { get => nextAttack; set => nextAttack = value; }

    /// <summary>
    /// Whether or not this attack has a successor
    /// </summary>
    public bool HasFollowingAttack { get => hasFollowingAttack; set => hasFollowingAttack = value; }
    /// <summary>
    /// The time in seconds until the next attack starts
    /// </summary>
    public float NextAttackDelay { get => nextAttackDelay; set => nextAttackDelay = value; }


    // Start is called before the first frame update
    void Start()
    {
        if (positions == null)
        {
            throw new System.Exception("No Attack Positions Entered");
        }
        // if isStaggered is false, this is the last attack in the chain
        if (!hasFollowingAttack && nextAttack == null)
        {
            nextAttack = null;
        }
        if (weight > 5) weight = 5;
        if (weight < 1) weight = 1;
    }
}

