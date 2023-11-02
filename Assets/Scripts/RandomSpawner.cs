using System.Collections;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject grenadePrefab;
    public GameObject shieldPrefab;
    public Transform[] spawnPoints;
    private GameObject currentGrenade;
    private GameObject currentShield;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true) // This will keep the routine running indefinitely
        {
            yield return new WaitForSeconds(5f);  // Wait for 5 seconds

            // If there's already a grenade or shield in the scene, destroy them
            if (currentGrenade)
            {
                Destroy(currentGrenade);
            }
            if (currentShield)
            {
                Destroy(currentShield);
            }

            // Choose a random spawn point from our array
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            
            // Instantiate the grenade at the spawn point's position
            currentGrenade = Instantiate(grenadePrefab, randomSpawnPoint.position, Quaternion.identity);

            // Instantiate the shield at the same spawn point's position
            currentShield = Instantiate(shieldPrefab, randomSpawnPoint.position, Quaternion.identity);

            yield return new WaitForSeconds(10f);  // Grenade and shield stay for 10 seconds

            // Destroy the grenade and shield
            if (currentGrenade)
            {
                Destroy(currentGrenade);
            }
            if (currentShield)
            {
                Destroy(currentShield);
            }
        }
    }
}
