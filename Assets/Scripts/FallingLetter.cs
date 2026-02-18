using UnityEngine;

public class FallingLetter : MonoBehaviour
{
    bool dying;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (dying || !other.CompareTag("FinishLine")) return;
        dying = true;

        // 1) Fizi�i durdur
        var rb = GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.simulated = false;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        // 2) Bu obje ve �ocuklar�ndaki t�m scriptleri (Update atanlar�) kapat
        var behaviours = GetComponentsInChildren<MonoBehaviour>(true);
        foreach (var b in behaviours)
            if (b && b != this) b.enabled = false;

        // 3) Son olarak yok et (ayn� frame�de g�venli)
        Destroy(gameObject);
    }

    private void OnDisable() { dying = true; }
}
