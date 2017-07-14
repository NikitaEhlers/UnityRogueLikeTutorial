using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    public int playerDamamge;

    private Animator animator;
    private Transform target;
    private bool skipMove;

	// Use this for initialization
	protected override void Start () {
        GameManager.instance.AddEmemyToList(this);

        animator = GetComponent<Animator>();

        target = GameObject.FindGameObjectWithTag("Player").transform;

        base.Start();
	}

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }
        
        base.AttemptMove<T>(xDir, yDir);

        skipMove = true;
    }

    public void MoveEnemy ()
    {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
        {
            //If the y coordinate of the target's (player) position is greater than the y coordinate of this enemy's position set y direction 1 (to move up). If not, set it to -1 (to move down).
            yDir = target.position.y > transform.position.y ? 1 : -1;
        }

        else
        {
            //Check if target x position is greater than enemy's x position, if so set x direction to 1 (move right), if not set to -1 (move left).
            xDir = target.position.x > transform.position.x ? 1 : -1;
        }

        AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;

        hitPlayer.LoseFood(playerDamamge);

        animator.SetTrigger("enemyAttack");
    }

    
}
