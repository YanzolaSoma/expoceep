using UnityEngine;
using UnityEngine.Events; // Necessário para UnityEvents

// SistemaDeVida.cs
public class SistemaDeVida : MonoBehaviour
{
    // --- Configuração ---
    [SerializeField]
    private int vidaMaxima = 100;
    
    // --- Estado Atual ---
    private int vidaAtual;

    // --- Eventos (Opcional, mas muito útil!) ---
    // Você pode arrastar funções do Inspector para cá.
    // Ex: Chamar um "animator.SetTrigger("Morreu")"
    [Space(10)]
    [Header("Eventos")]
    [Tooltip("Chamado quando o objeto recebe dano.")]
    public UnityEvent<int> AoReceberDano; // Envia a quantidade de dano

    [Tooltip("Chamado quando a vida chega a 0.")]
    public UnityEvent AoMorrer;

    // --- Propriedades (para outros scripts lerem) ---
    public int VidaAtual
    {
        get { return vidaAtual; }
    }
    public int VidaMaxima
    {
        get { return vidaMaxima; }
    }
    public bool EstaMorto
    {
        get { return vidaAtual <= 0; }
    }

    // --- Inicialização ---
    void Awake()
    {
        // Começa com a vida cheia
        vidaAtual = vidaMaxima;
    }

    // --- Método Público Principal ---

    /// <summary>
    /// Aplica uma quantidade de dano a este objeto.
    /// </summary>
    /// <param name="quantidadeDano">O valor do dano a ser aplicado.</param>
    public void ReceberDano(int quantidadeDano)
    {
        // Se já estiver morto, não faz nada
        if (EstaMorto)
        {
            return;
        }

        // Garante que o dano não é negativo
        if (quantidadeDano < 0)
        {
            quantidadeDano = 0;
        }
        
        // Reduz a vida
        vidaAtual -= quantidadeDano;
        Debug.Log(gameObject.name + " recebeu " + quantidadeDano + " de dano. Vida restante: " + vidaAtual);

        // Chama o evento de dano (para UI, sons, etc.)
        if (AoReceberDano != null)
        {
            AoReceberDano.Invoke(quantidadeDano);
        }

        // Verifica se morreu
        if (vidaAtual <= 0)
        {
            vidaAtual = 0; // Trava em 0
            Morrer();
        }
    }

    /// <summary>
    /// Cura o objeto em uma certa quantidade.
    /// </summary>
    public void Curar(int quantidadeCura)
    {
        if (EstaMorto || quantidadeCura <= 0)
        {
            return;
        }

        vidaAtual += quantidadeCura;

        // Garante que a cura não passe do máximo
        if (vidaAtual > vidaMaxima)
        {
            vidaAtual = vidaMaxima;
        }
    }

    // --- Método Privado ---

    private void Morrer()
    {
        Debug.Log(gameObject.name + " morreu.");

        // Chama o evento de morte
        if (AoMorrer != null)
        {
            AoMorrer.Invoke();
        }

        // Lógica de morte Padrão (pode ser customizada pelos eventos)
        
        // Se for um inimigo, podemos destruí-lo
        if (gameObject.CompareTag("Inimigo"))
        {
            // Desativa o script de seguir para ele parar
            var scriptFollow = GetComponent<InimigoSeguir2D>();
            if (scriptFollow) scriptFollow.enabled = false;
            
            // Destrói o objeto após 2 segundos (para dar tempo para animação/som)
            Destroy(gameObject, 2.0f); 
        }
        // Se for o jogador, podemos desativá-lo ou chamar um "Game Over"
        else if (gameObject.CompareTag("Player"))
        {
            // Apenas desativa o jogador por enquanto
            // (Você chamaria seu GameManager aqui)
            gameObject.SetActive(false);
            Debug.Log("GAME OVER!");
        }
    }
}