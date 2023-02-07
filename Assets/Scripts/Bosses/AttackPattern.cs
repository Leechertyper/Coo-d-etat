using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern : MonoBehaviour
{
    // The attacks can be staggered. Checks if the player wants a staggered attack.
    [Header("Will this attack be staggered?")]
    public bool isStaggered = false;

    [Header("Attack weight (between 1-5, any other values will be rounded)")]
    [SerializeField] private int weight;

    // The positions this pattern will strike
    [Header("Make sure you know the size of the grid is the same size as the attack pattern. Otherwise it will be excluded in the grid script")]
    [SerializeField] private Vector2[] positions;

    [Header("Give the indexes of the attacks location in \'positions\'")]
    [SerializeField] private Vector2[] firstAttack;

    [Header("Give the indexes of the attacks location in \'positions\'")]
    [SerializeField] private Vector2[] secondAttack;

    [Header("Give the indexes of the attacks location in \'positions\'")]
    [SerializeField] private Vector2[] thirdAttack;

    // the amount of attacks filled out
    private int _attacksCount;

    // the attacks that are in use
    private bool[] _usedAttacks = { false, false, false };

    // get and set functions for each value
    public Vector2[] Positions { get => positions; set => positions = value; }
    public Vector2[] FirstAttack { get => firstAttack; set => firstAttack = value; }
    public Vector2[] SecondAttack { get => secondAttack; set => secondAttack = value; }
    public Vector2[] ThirdAttack { get => thirdAttack; set => thirdAttack = value; }
    public bool[] UsedAttacks { get => _usedAttacks; set => _usedAttacks = value; }
    public int AttacksCount { get => _attacksCount; set => _attacksCount = value; }
    public int Weight { get => weight; set => weight = value; }


    // Start is called before the first frame update
    void Start()
    {
        if (positions == null)
        {
            throw new System.Exception("No Attack Positions Entered");
        }
        if (weight > 5) weight = 5;
        if (weight < 1) weight = 1;
        if (isStaggered)
        {
            if(firstAttack == null)
            {
                _attacksCount++;
                _usedAttacks[0] = true;
            }
            if(secondAttack == null)
            {
                _attacksCount++;
                _usedAttacks[1] = true;
            }
            if(secondAttack == null)
            {
                _attacksCount++;
                _usedAttacks[2] = true;
            }
            if(_attacksCount == 0)
            {
                throw new System.Exception("Staggered Attacks Selected, But Not Supplied");
            }

        }
        else
        {
            _attacksCount = 1;
        }
    }


}

