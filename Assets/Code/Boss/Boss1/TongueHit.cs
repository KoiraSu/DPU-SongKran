using UnityEngine;

public class TongueHit : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("โดนลิ้น");
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.TakeDamage(damage);
                Debug.Log("-1");
            }
        }
    }
}