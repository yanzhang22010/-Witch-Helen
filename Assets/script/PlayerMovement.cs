using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private Animator animator;
    private float inputX, inputY;
    private float stopX, stopY;
    private bool isPickingUp; // 是否正在采摘
    private bool isAttacking; // 是否正在攻击
    public bool canPickUp = false;

    public int A, B, C,D;

    // private int books;//计数变量
    // public Text BooksText;//卷轴计数的UI组件

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
     

        A = GameObject.Find("Player").gameObject.GetComponent<ChannelScript>().A;
        B = GameObject.Find("Player").gameObject.GetComponent<ChannelScript>().B;
        C = GameObject.Find("Player").gameObject.GetComponent<ChannelScript>().C;
        D = GameObject.Find("Player").gameObject.GetComponent<ChannelScript>().D;
        Attack();
       // inputX = Input.GetAxisRaw("Horizontal");
       // inputY = Input.GetAxisRaw("Vertical");
        inputX = A - 1;
        inputY = B - 1;
        Vector2 input = (transform.right * inputX + transform.up * inputY).normalized;
        rb.velocity = input * speed;

        if (input != Vector2.zero)
        {
            animator.SetBool("isMoving", true);
            stopX = inputX;
            stopY = inputY;

        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        // 处理采摘动作
        if (canPickUp && (Input.GetKeyDown(KeyCode.Space) || C == 1))
        //if (canPickUp && (Input.GetKeyDown(KeyCode.Space)))
            {
            isPickingUp = true;
            animator.SetTrigger("isPickingUp");
            Debug.Log("down");
            // 计算面向方向并设置PickDirection
            Vector2 foodPosition = GetClosestFoodPosition();
            float direction = transform.position.x > foodPosition.x ? -1 : 1;

            animator.SetFloat("PickDirection", direction);
            SoundManager.PlayHealSpellClip(); // 播放回复血量音频
            //GameObject.Find("Player").gameObject.GetComponent<ChannelScript>().C = 0;
        }

        animator.SetFloat("InputX", stopX);
        animator.SetFloat("InputY", stopY);

    }

    Vector2 GetClosestFoodPosition()
    {
        GameObject[] foods = GameObject.FindGameObjectsWithTag("food");
        float closestDistance = float.MaxValue;
        Vector2 closestPosition = Vector2.zero;
        foreach (var food in foods)
        {
            float distance = Vector2.Distance(transform.position, food.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPosition = food.transform.position;
            }
        }
        return closestPosition;
    }

    public void EndPickAni() // Call this function at the end of the pick animation
    {
        isPickingUp = false;
    }

    void Attack()
    {
        if (Input.GetButtonDown("Attack")||D==1)
        //if (Input.GetButtonDown("Attack"))
            {

            animator.SetTrigger("Attack");
            isAttacking = true;
            SoundManager.PlayPlayerAttackClip(); // 播放攻击音频
            GameObject.Find("Player").gameObject.GetComponent<ChannelScript>().D = 0;
        }
    }

    public void AttackFinished()  // Call this function at the end of the attack animation
    {
        isAttacking = false;
    }

    public void EnablePickUp()
    {
        canPickUp = true;
    }

    public void DisablePickUp()
    {
        canPickUp = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (isAttacking)  // Only damage the enemy when the player is attacking
            {
                other.GetComponent<EnemyMovement>().TakeDamage(30);
            }
        }
    }
}
