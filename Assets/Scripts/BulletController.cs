using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20f;
    [SerializeField] float selfDestructTime = 3f;

    Rigidbody2D myRigidbody;
    float xSpeed;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

        // Find the player safely
        PlayerMovement player = FindFirstObjectByType<PlayerMovement>();

        if (player != null)
        {
            // 1. Get direction (1 for right, -1 for left)
            float direction = Mathf.Sign(player.transform.localScale.x);

            // 2. Calculate horizontal speed
            xSpeed = direction * bulletSpeed;

            // 3. Set velocity once (physics engine handles it from here)
            myRigidbody.linearVelocity = new Vector2(xSpeed, 0f);

            // 4. FIX SCALE: Set size to 0.1 while maintaining direction
            transform.localScale = new Vector3(direction * 0.1f, 0.1f, 1f);
        }

        // Automatic cleanup so your hierarchy doesn't get cluttered
        Destroy(gameObject, selfDestructTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore the player entirely so the bullet doesn't delete itself or the player
        if (other.CompareTag("Player")) return;

        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else
        {
            // Hits ground, walls, etc.
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Extra safety check for non-trigger colliders
        if (collision.gameObject.CompareTag("Player")) return;

        Destroy(gameObject);
    }
}