using UnityEngine;
using TMPro;
using DG.Tweening; 

public class CardViews : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text mana;
    [SerializeField] private SpriteRenderer imageSR;
    [SerializeField] private GameObject wrapper;
    
    public Card Card { get; private set; }

    private Vector3 originalPosition;
    private bool isDragging = false;

    public void Setup(Card card)
    {
        Card = card;
        title.text = card.Title;
        description.text = card.Description;
        mana.text = card.Mana.ToString();
        imageSR.sprite = card.Image;
    }

    void OnMouseEnter()
    {
        // Cegah efek hover muncul saat kartu sedang ditarik
        if (isDragging) return; 

        wrapper.SetActive(false);
        Vector3 pos = new(transform.position.x, -2, 0);
        CardViewHoverSystem.Instance.Show(Card, pos);
    }
    
    void OnMouseExit()
    {
        if (isDragging) return;

        CardViewHoverSystem.Instance.Hide();
        wrapper.SetActive(true);
    }

    void OnMouseDown()
    {
        isDragging = true;
        
        // Simpan posisi awal di tangan
        originalPosition = transform.position; 
        
        CardViewHoverSystem.Instance.Hide(); 
        wrapper.SetActive(true); 
    }

    void OnMouseDrag()
    {
        // Membaca jarak Z secara dinamis agar pergerakan kartu di ruang 3D tetap stabil dan rata
        float zDistance = Camera.main.WorldToScreenPoint(transform.position).z;
        Vector3 mouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDistance);
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        
        transform.position = mouseWorldPos; 
    }

    void OnMouseUp()
    {
        isDragging = false;
        bool droppedOnTarget = false;

        // Menembakkan Raycast 3D dari posisi kursor untuk mendeteksi target
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        
        foreach (RaycastHit hit in hits)
        {
            // Abaikan jika raycast menabrak collider kartu ini sendiri
            if (hit.collider.gameObject == this.gameObject) continue;

            // Deteksi tag musuh atau player
            if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Player"))
            {
                // --- 1. VALIDASI TARGET ---
                bool isValidTarget = false;

                // Kartu Attack HANYA bisa dijatuhkan ke Enemy
                if (Card.Type == CardType.Attack && hit.collider.CompareTag("Enemy"))
                {
                    isValidTarget = true;
                }
                // Kartu Buff HANYA bisa dijatuhkan ke Player
                else if (Card.Type == CardType.Buff && hit.collider.CompareTag("Player"))
                {
                    isValidTarget = true;
                }

                // --- 2. EKSEKUSI JIKA TARGET SESUAI ---
                if (isValidTarget)
                {
                    // Cek apakah mana cukup sebelum mengeksekusi PlayCard
                    if (ManaManager.Instance.TryUseMana(Card.Mana))
                    {
                        PlayCard(hit.collider.gameObject);
                        droppedOnTarget = true;
                    }
                    else
                    {
                        Debug.Log("Gagal dimainkan: Mana tidak mencukupi!");
                    }
                    
                    break; // Hentikan pencarian karena target yang valid sudah ditemukan
                }
                else
                {
                    // Jika target tidak valid (misal: Buff dilempar ke Enemy), 
                    // loop akan terus berjalan atau kartu akan memantul kembali ke tangan.
                    Debug.Log("Salah sasaran! Target tidak cocok dengan tipe kartu.");
                }
            }
        }

        // Jika kartu dilepas di tempat kosong (meleset), kembalikan ke tangan
        if (!droppedOnTarget)
        {
            transform.DOMove(originalPosition, 0.2f);
        }
    }

    private void PlayCard(GameObject target)
    {
        Debug.Log($"Kartu '{Card.Title}' berhasil dimainkan ke: {target.name}");
        
        HealthManager targetHealth = target.GetComponent<HealthManager>();
        
        if (targetHealth != null)
        {
            // Cek tipe kartu
            if (Card.Type == CardType.Attack)
            {
                targetHealth.TakeDamage(Card.Damage); 
            }
            else if (Card.Type == CardType.Buff)
            {
                // Eksekusi efek Heal & Shield
                if (Card.HealAmount > 0) targetHealth.Heal(Card.HealAmount);
                if (Card.ShieldAmount > 0) targetHealth.AddShield(Card.ShieldAmount);
            }
        }

        // 1. Beritahu DeckManager untuk memindahkan data kartu ini ke Discard Pile
        DeckManager.Instance.DiscardCard(Card);

        // 2. Beritahu HandView untuk menghapus visual kartu ini dari daftar tangannya
        // (Menggunakan FindAnyObjectByType karena ini adalah prototype yang cepat)
        FindAnyObjectByType<HandView>().RemoveCard(this);

        // 3. Hancurkan objek visual kartu ini dari scene
        Destroy(gameObject); 
    }
}