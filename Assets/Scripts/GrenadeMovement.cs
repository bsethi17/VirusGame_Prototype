using UnityEngine;

public class GrenadeMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        MoveTowardsMouse();
    }

    private void MoveTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure it's in the 2D plane

        Vector2 direction = (mousePosition - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Grenade collided with: " + collision.gameObject.name);
        if (collision.gameObject.tag == "HealingHouse")
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }

}

