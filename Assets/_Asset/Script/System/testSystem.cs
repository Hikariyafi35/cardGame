using UnityEngine;

public class TestSystem : MonoBehaviour
{
    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // Cukup panggil fungsi DrawCards dari DeckManager, 
            // minta tarik 1 kartu setiap kali Spasi ditekan.
            DeckManager.Instance.DrawCards(1);
        }
        
        // (Tambahan untuk tes) Tekan tombol D untuk pura-pura Draw 5 kartu sekaligus (seperti awal giliran)
        if(Input.GetKeyDown(KeyCode.D))
        {
            DeckManager.Instance.DrawCards(5);
        }
    }
}
