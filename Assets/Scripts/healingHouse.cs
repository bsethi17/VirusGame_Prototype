using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healingHouse : MonoBehaviour
{
    public GameObject initialVirusPrefab;
    private static Stack<GameObject> infectedStack;
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
        if (other.gameObject.name.StartsWith("HumanNV"))
        {
            // Try to find the child object named "InitialVirus"
            Transform initialVirus = null;
            foreach (Transform child in other.transform)
            {
                if (child.name.StartsWith("InitialVirus"))
                {
                    initialVirus = child;
                    break;
                }
            }

            if (initialVirus != null)
            {
                // If the child object exists, destroy it
                Destroy(initialVirus.gameObject);
                Debug.Log(other.gameObject.name + " has been healed!");

                infectedStack = BulletScript.GetInfectedStack();

                // Remove the healed object from the infected stack
                if (infectedStack.Count > 0 && infectedStack.Peek() == other.gameObject)
                {
                    infectedStack.Pop();
                }

                // Transfer shooting capability to the previous shooter in the stack
                if (infectedStack.Count > 0 && infectedStack.Peek().name != other.gameObject.name)
                {
                    GameObject previousShooter = infectedStack.Peek();
                    Transform initialVirusChild = null;
                    // Find the "InitialVirus" child of the next infected NVHuman
                    foreach (Transform child in previousShooter.transform)
                    {

                        if (child.name.StartsWith("InitialVirus"))
                        {
                            initialVirusChild = child;
                            break;
                        }
                    }
                    if (initialVirusChild)
                    {
                        // Destroy the existing "InitialVirus" child
                        Destroy(initialVirusChild.gameObject);
                        InstantiateVirus(previousShooter.transform);
                    }
                }
            }
            else
            {
                Debug.Log(other.gameObject.name + " does not have the InitialVirus.");
            }
        }
    }
    private void InstantiateVirus(Transform parentTransform)
    {

        GameObject newVirus = Instantiate(initialVirusPrefab, parentTransform.position, Quaternion.identity);
        if (newVirus != null)
        {
            Debug.Log("Virus instantiated successfully.");
        }
        else
        {
            Debug.LogError("Virus instantiation failed.");
            return;
        }
        newVirus.transform.SetParent(parentTransform);
        SpriteRenderer sr = newVirus.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = 1; // Ensure it is rendered in front
        }
        newVirus.transform.localPosition = new Vector3(0, 0, 0);
        if (infectedStack.Count == 0 || infectedStack.Peek() != parentTransform.gameObject)
        {
            infectedStack.Push(parentTransform.gameObject);
        }
    }
}