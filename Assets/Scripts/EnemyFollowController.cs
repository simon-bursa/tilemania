using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Animator))]
public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 4f;
    public float stoppingDistance = 0.2f;

    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float distanceX = player.position.x - rb.position.x;
        bool shouldMove = Mathf.Abs(distanceX) > stoppingDistance;
        animator.SetBool("isEnemyMoving", shouldMove);
        animator.SetBool("isEnemyIdle", !shouldMove);

        if (shouldMove)
        {
            float moveDir = Mathf.Sign(distanceX);
            Vector2 newPos = new Vector2(rb.position.x + moveDir * speed * Time.fixedDeltaTime, rb.position.y);
            rb.MovePosition(newPos);

            if (moveDir > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else if (moveDir < 0)
                transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform == player)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}