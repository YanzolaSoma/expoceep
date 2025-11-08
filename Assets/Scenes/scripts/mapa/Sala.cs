using UnityEngine;
using System.Collections.Generic;

public class Sala : MonoBehaviour
{
    [System.Serializable]
    public class Saida
    {
        public Direcao direcao;
        public Transform pontoDeConexao;
        public bool estaConectada;
    }

    [Tooltip("Saídas da sala (Norte, Sul, Leste, Oeste).")]
    public List<Saida> saidas = new List<Saida>();

    [Tooltip("Tamanho da sala (X e Z).")]
    public Vector2 tamanhoDaSala = new Vector2(20f, 20f);

    [HideInInspector]
    public Vector2Int indiceDeGrade;

    public void MarcarSaidaConectada(Direcao dir)
    {
        foreach (var s in saidas)
        {
            if (s.direcao == dir)
            {
                s.estaConectada = true;
                return;
            }
        }
    }
}
