using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] Transform leftEdge;
    [SerializeField] Transform rightEdge;
    [Header("Enemy")]
    [SerializeField] Transform enemy;
    [Header("Movement Parameters")]
    [SerializeField] float speed;
    Vector3 initScale;
    bool movingLeft;

    [Header("Idle Behavior")]
    [SerializeField] float idleDuration;
    float idleTimer;

    [Header("Animation")]
    [SerializeField] Animator animator;


    private void Awake()
    {
        initScale = enemy.localScale;
    }


    private void Update()
    {
        //goes the wrong way and/or faces the wrong way
        //MoveInDirection(-1);
        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
                MoveInDirection(-1);
            else
                DirectionChange();
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
                MoveInDirection(1);
            else
                DirectionChange();
        }
    }
    private void OnDisable()
    {
        animator.SetBool("moving", false);
    }

    void DirectionChange()
    {
        animator.SetBool("moving", false);
        idleTimer += Time.deltaTime;
        if (idleTimer > idleDuration)
            movingLeft = !movingLeft;
    }

    void MoveInDirection(int direction)
    {
        idleTimer = 0;
        animator.SetBool("moving", true);
        //make enemy face direction
        enemy.localScale = new Vector3(-Mathf.Abs(initScale.x) * direction, initScale.y, initScale.z);
        //move in that direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * direction * speed,
            enemy.position.y, enemy.position.z);
    }

}
