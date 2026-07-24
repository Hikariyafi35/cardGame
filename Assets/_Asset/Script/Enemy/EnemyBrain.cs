using System.Collections;
using UnityEngine;
using DG.Tweening; // Untuk animasi serangan

public class EnemyBrain : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int attackDamage = 5; // Jumlah damage yang diberikan musuh

    // Fungsi ini akan dipanggil oleh TurnManager
    public IEnumerator TakeTurn()
    {
        Debug.Log($"{gameObject.name} bersiap menyerang...");

        // 1. Animasi ancang-ancang: Musuh maju sedikit ke arah kiri (asumsi player di kiri)
        Vector3 originalPos = transform.position;
        transform.DOMoveX(originalPos.x - 1f, 0.2f).SetLoops(2, LoopType.Yoyo);

        // Tunggu sebentar sampai animasi maju selesai
        yield return new WaitForSeconds(0.3f); 

        // 2. Cari Player dan berikan damage
        // Pastikan objek Player-mu sudah diberi Tag "Player" di Inspector!
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        if (player != null)
        {
            HealthManager playerHealth = player.GetComponent<HealthManager>();
            if (playerHealth != null)
            {
                Debug.Log($"{gameObject.name} menyerang Player dengan {attackDamage} damage!");
                playerHealth.TakeDamage(attackDamage);
            }
        }

        // 3. Jeda sejenak sebelum giliran musuh benar-benar selesai
        yield return new WaitForSeconds(0.7f);
    }
}