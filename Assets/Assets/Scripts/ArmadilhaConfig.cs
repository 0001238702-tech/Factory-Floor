using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
public enum TipoArmadilha { Espetos, Torreta}

public class ArmadilhaConfig : MonoBehaviour
{
    [Header("Configuração Geral")]
    [SerializeField] private Collider2D colisorDano;
    [SerializeField] private Animator anim;
    [SerializeField] private TipoArmadilha tipo = TipoArmadilha.Espetos;
    [SerializeField] private float atrasoInicial = 0f;  // Atraso antes do 1º ciclo
    [Header("Configuração Espetos")]
    [SerializeField] private float tempoAtivo = 2f;     // Quanto tempo fica ligado
    [SerializeField] private float tempoInativo = 1.5f; // Quanto tempo fica desligado
    [Header("Configuração Torreta")]
    [SerializeField] private float tempoEntreDisparos = 2f;
    [SerializeField] private float prefabProjetil = 0f; // modificar
    [SerializeField] private float pontoDisparo = 0f; // modificar2
    [SerializeField] private float velocidadeProjetil = 0f;


    
    private void Start()
    {

    }


    private void Update()
    {
        switch (tipo)
        {
            case TipoArmadilha.Espetos:
                AtualizarEspetos();
                break;

            case TipoArmadilha.Torreta:
                AtualizarTorreta();
                break;
        }
    }

    private void AtualizarEspetos()
    {
        
    }
    private void AtualizarTorreta()
    {

    }
}
