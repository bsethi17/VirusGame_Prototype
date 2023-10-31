using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healingHouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the entering object is one of the HumanNV objects
        if (other.gameObject.name.StartsWith("HumanNV"))
        {
            bool virusFound = false;

            // Loop through all child objects
            foreach (Transform child in other.transform)
            {
                // Check if the child's name starts with "InitialVirus"
                if (child.name.StartsWith("InitialVirus"))
                {
                    // If the child object exists, destroy it
                    Destroy(child.gameObject);
                    Debug.Log(other.gameObject.name + " has been healed!");
                    virusFound = true;
                    break; // Stop the loop if the virus is found
                }
            }

            if (!virusFound)
            {
                Debug.Log(other.gameObject.name + " does not have the InitialVirus.");
            }
        }
    }
}
