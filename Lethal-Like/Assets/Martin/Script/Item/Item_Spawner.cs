using System.Collections.Generic;
using UnityEngine;

public class Item_Spawner : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] private List<Item> itemPrefabs = new();

    [Header("Spawn Points")]
    [SerializeField] private List<Transform> spawnPoints = new();

    [Header("Settings")]
    [SerializeField] private int minimumTotalValue = 500;

    private void Start()
    {
        SpawnItems();
    }

    private void SpawnItems()
    {
        List<Item> validItems = new();
        List<Transform> availablePoints = new();

        int maxItemValue = 0;

        foreach (Item item in itemPrefabs)
        {
            if (item == null || item.Value <= 0) continue;

            validItems.Add(item);
            maxItemValue = Mathf.Max(maxItemValue, item.Value);
        }

        foreach (Transform point in spawnPoints)
        {
            if (point != null) availablePoints.Add(point);
        }

        if (validItems.Count == 0 || availablePoints.Count == 0)
            return;

        if ((long)maxItemValue * availablePoints.Count < minimumTotalValue)
        {
            Debug.LogWarning("Not enough spawn points or item value!");
            return;
        }

        for (int i = 0; i < availablePoints.Count; i++)
        {
            int randomIndex = Random.Range(i, availablePoints.Count);

            (availablePoints[i], availablePoints[randomIndex]) =
                (availablePoints[randomIndex], availablePoints[i]);
        }

        long totalValue = 0;
        int spawnedCount = 0;

        for (int i = 0; i < availablePoints.Count; i++)
        {
            if (totalValue >= minimumTotalValue) break;

            int remainingPoints = availablePoints.Count - i - 1;

            long requiredValue = minimumTotalValue - totalValue - (long)remainingPoints * maxItemValue;

            List<Item> candidates = validItems.FindAll(item => item.Value >= requiredValue);

            if (candidates.Count == 0) break;

            Item selectedItem = candidates[Random.Range(0, candidates.Count)];

            Transform point = availablePoints[i];

            Instantiate(selectedItem, point.position, point.rotation);

            totalValue += selectedItem.Value;
            spawnedCount++;
        }

        Debug.Log($"Spawned {spawnedCount} items. Total value: ${totalValue}");
    }
}
