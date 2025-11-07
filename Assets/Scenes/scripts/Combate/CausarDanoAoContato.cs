using UnityEngine;

// CausarDanoAoContato.cs
[RequireComponent(typeof(Collider2D))]
public class CausarDanoAoContato : MonoBehaviour
{
    [SerializeField]
    private int dano = 10; // Dano que este objeto causa

    [SerializeField]
    private string tagDoAlvo = "Player"; // Quem este objeto pode machucar

    // Usamos OnTriggerEnter2D para que o inimigo não "empurre" o jogador.
    // O Collider2D do inimigo (ou uma parte dele) deve estar marcado como "Is Trigger".
    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o objeto que entrou no Trigger tem a tag correta
        if (other.gameObject.CompareTag(tagDoAlvo))
        {
            // Tenta encontrar o script de vida no objeto que colidiu
            SistemaDeVida vidaDoAlvo = other.gameObject.GetComponent<SistemaDeVida>();

            // Se o alvo tiver um SistemaDeVida, aplica o dano
            if (vidaDoAlvo != null)
            {
                vidaDoAlvo.ReceberDano(dano);
            }
        }
    }

    /* // --- VERSÃO ALTERNATIVA COM COLISÃO (OnCollisionEnter2D) ---
    // Use esta se você NÃO estiver usando "Is Trigger".
    // Isso fará com que os objetos batam e empurrem (física).

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(tagDoAlvo))
        {
            SistemaDeVida vidaDoAlvo = collision.gameObject.GetComponent<SistemaDeVida>();
            if (vidaDoAlvo != null)
            {
                vidaDoAlvo.ReceberDano(dano);
            }
        }
    }
    */
}