using UnityEngine;

public class MoneyPickup : MonoBehaviour
{
    [SerializeField] private int amount;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MoneyManager.Instance.AddMoney(amount);
            Destroy(gameObject);
        }
    }
}
