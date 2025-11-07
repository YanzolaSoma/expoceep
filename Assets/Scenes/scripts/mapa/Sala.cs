using UnityEngine;
using System.Collections.Generic;

public class Sala : MonoBehaviour
{
    // NOVO: Struct simples para gerenciar a saída.
    [System.Serializable]
    public struct Saida
    {
        public string direcao; // Ex: "Norte", "Sul", "Leste", "Oeste"
        public Transform pontoDeConexao; // O GameObject vazio na porta
        public bool estaConectada; // Se já tem uma sala vizinha
    }

    [Tooltip("Lista de todas as saídas da sala (Norte, Sul, Leste, Oeste).")]
    public List<Saida> saidas = new List<Saida>();
    
    // Tamanho da sala (usado para calcular a posição da próxima sala)
    [Tooltip("Dimensões X e Z da sala para cálculo de posição.")]
    public Vector2 tamanhoDaSala = new Vector2(20f, 20f); 

    // Posição na grade (para controle do Gerador)
    [HideInInspector]
    public Vector2 indiceDeGrade;

    /// <summary>
    /// Retorna uma saída que ainda não foi usada para conexão.
    /// </summary>
    public Saida? GetSaidaLivre()
    {
        foreach (var saida in saidas)
        {
            if (!saida.estaConectada)
            {
                return saida;
            }
        }
        return null;
    }
}