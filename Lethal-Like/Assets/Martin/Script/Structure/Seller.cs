using System.Collections.Generic;
using UnityEngine;

public class Seller : MonoBehaviour
{
    [Header("Selling Area")]
    [SerializeField] private BoxCollider sellArea;
    [SerializeField] private LayerMask itemLayer;

    public void SellItems()
    {
        if (Economy_Manager.instance == null || sellArea == null) return;

        Vector3 center = sellArea.transform.TransformPoint(sellArea.center);

        Vector3 scale = sellArea.transform.lossyScale;

        Vector3 halfExtents = Vector3.Scale(sellArea.size * 0.5f, 
            new Vector3(Mathf.Abs(scale.x), Mathf.Abs(scale.y), Mathf.Abs(scale.z)));

        Collider[] hits = Physics.OverlapBox(center, halfExtents, sellArea.transform.rotation, itemLayer, QueryTriggerInteraction.Ignore);

        HashSet<Item> itemsToSell = new HashSet<Item>();

        foreach (Collider hit in hits)
        {
            Item item = hit.GetComponentInParent<Item>();

            if (item == null) continue;

            Rigidbody itemRb = item.GetComponent<Rigidbody>();

            if (itemRb == null || itemRb.isKinematic) continue;

            itemsToSell.Add(item);
        }

        if (itemsToSell.Count == 0)
        {
            Debug.Log("No items to sell");
            return;
        }

        int totalValue = 0;

        foreach (Item item in itemsToSell)
        {
            totalValue += Mathf.Max(0, item.Value);

            Destroy(item.gameObject);
        }

        Economy_Manager.instance.SellItem(totalValue);

        Debug.Log( $"Sold {itemsToSell.Count} items for ${totalValue}");
    }
}
