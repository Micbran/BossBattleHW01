using UnityEngine;

public class CullBullets : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Bullet otherBullet = other.GetComponent<Bullet>();
        if (otherBullet != null)
        {
            Destroy(otherBullet.gameObject);
        }
    }
}
