using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    // You can adjust the value as needed
    public int bulletsToAddOnHit = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object is a bullet
        if (collision.gameObject.tag == "bullet")
        {
            Debug.Log("Collision detected with: " + collision.gameObject.name);
            // Call the AddBullets method from UIManager
            UIManager.Instance.AddGrenades(bulletsToAddOnHit);
            Destroy(gameObject);

            // Optional: Destroy the grenade after being hit
            // Destroy(gameObject);
        }
    }
}
