using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    public int bulletsToAddOnHit = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("bullet"))
        {
            // Debug.Log("destroying grenade");
            UIManager.Instance.AddGrenades(1);
            Destroy(gameObject);
        }
    }

}

