using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class healingHouse : MonoBehaviour
{
    public GameObject initialVirusPrefab;
    public GameObject humanNShieldPrefab;
    public GameObject impactPrefab;
    private static Stack<GameObject> infectedStack;

    private HashSet<GameObject> humansToReceiveShield = new HashSet<GameObject>();

    // keeping record of numbers of healed humans
    public int counter;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("grenade"))
        {
            Instantiate(impactPrefab, transform.position, Quaternion.identity);
            // Destroy the healingHouse
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

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
                // Debug.Log(other.gameObject.name + " has been healed!");
                counter += 1;

                infectedStack = BulletScript.GetInfectedStack();

                // Remove the healed object from the infected stack
                if (infectedStack.Count > 0 && infectedStack.Peek() == other.gameObject)
                {
                    infectedStack.Pop();
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
                else if (infectedStack.Count > 0 && infectedStack.Peek() != other.gameObject)
                {
                    Stack<GameObject> tempStack = new Stack<GameObject>();
                    bool found = false;

                    // Move elements to the temp stack until we find the target game object
                    while (infectedStack.Count > 0 && infectedStack.Peek() != other.gameObject)
                    {
                        tempStack.Push(infectedStack.Pop());
                    }

                    // If we find the target game object, pop it from the stack
                    if (infectedStack.Count > 0 && infectedStack.Peek() == other.gameObject)
                    {
                        infectedStack.Pop();
                        found = true;
                    }

                    // Move elements back from the temp stack to the infected stack
                    while (tempStack.Count > 0)
                    {
                        GameObject stackObject = tempStack.Pop();
                        infectedStack.Push(stackObject);
                    }

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

                // Add the healed human to the HashSet
                humansToReceiveShield.Add(other.gameObject);
            }
            else
            {
                Debug.Log(other.gameObject.name + " does not have the InitialVirus.");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (humansToReceiveShield.Contains(other.gameObject))
        {
            AttachShieldToHuman(other.transform);
            humansToReceiveShield.Remove(other.gameObject);
        }
    }

    private void AttachShieldToHuman(Transform humanTransform)
    {
        // Check if the human already has a shield
        foreach (Transform child in humanTransform)
        {
            if (child.name.StartsWith("HS"))
            {
                Debug.Log("Human already has a shield.");
                return;
            }
        }

        // Instantiate and attach the shield to the human
        GameObject newShield = Instantiate(humanNShieldPrefab, humanTransform.position, Quaternion.identity);
        if (newShield != null)
        {
            Debug.Log("Shield attached successfully.");
        }
        else
        {
            Debug.LogError("Shield attachment failed.");
            return;
        }
        newShield.transform.SetParent(humanTransform);
        newShield.transform.localPosition = new Vector3(0, 0, 0);
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
