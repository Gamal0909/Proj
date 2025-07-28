using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Chest : MonoBehaviour
{
    //[SerializeField] private PlayerInput input;
    [SerializeField] private ChestType chestType;
    [SerializeField] private int moneyAmount = 100; 
    [SerializeField] private List<GameObject> lootPrefabs; // For loot drop
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float interactionDistanceOnX = 3f;
    [SerializeField] private float waitTime = 1f;

    private bool isOpened;

    private void Awake()
    {
        //if (input == null) Debug.LogError("PlayerInput is null");
        isOpened = false;
    }

    private void Update()
    {
        if (isOpened) return;

        float distanceOnX = Mathf.Abs(transform.position.x - playerTransform.position.x);
        if (distanceOnX <= interactionDistanceOnX && Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(OpenChest());
            isOpened = true;
        }
    }

    private IEnumerator OpenChest()
    {
        // Optional: play open animation here
        yield return new WaitForSeconds(waitTime);

        switch (chestType)
        {
            case ChestType.AutoAddMoney:
                MoneyManager.Instance.AddMoney(moneyAmount);
                break;

            case ChestType.DropLoot:
                if (lootPrefabs.Count > 0)
                {
                    foreach (var item in lootPrefabs)
                    {
                        Vector3 spawnPos = transform.position + Vector3.up * 1f;
                        GameObject loot = Instantiate(item, spawnPos, Quaternion.identity);

                        // Apply throw force on X axis
                        Rigidbody rb = loot.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            // Throw direction: right (+X) with upward arc
                            Vector3 throwDir = (Vector3.right + Vector3.up * 0.5f).normalized;

                            // Optional randomness for spread
                            throwDir += new Vector3(0.2f, 0, 0);
                            rb.AddForce(throwDir.normalized * 5f, ForceMode.Impulse);
                        }

                        yield return new WaitForSeconds(0.3f); // Slight delay between each
                    }
                }
                else
                {
                    Debug.LogWarning("No money prefab assigned for DropLoot chest.");
                }
                break;
        }

        this.enabled = false;
    }
}
