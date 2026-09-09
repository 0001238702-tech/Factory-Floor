using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum TipoArmadilha { Espetos, Torreta }

public class ArmadilhaConfig : MonoBehaviour
{
    [Header("Configuração Geral")]
    [SerializeField] private Collider2D colisorDano;
    
    [SerializeField] private TipoArmadilha tipo = TipoArmadilha.Espetos;
    [SerializeField] private float atrasoInicial = 0f;  
    [Header("Configuração Espetos")]
    [SerializeField] private float tempoAtivo = 2f;     
    [SerializeField] private float tempoInativo = 1.5f; 
    [Header("Configuração Torreta")]
    [SerializeField] private float tempoEntreDisparos = 2f;
    [SerializeField] private GameObject prefabProjetil;
    [SerializeField] private Transform pontoDisparo;
    [SerializeField] private float velocidadeProjetil = 5f;

    private Vector3 posInicial;
    private Animator anim;
    private bool emAtraso = false;
    private float cronometro;
 
    private void Start()
    {
        posInicial = transform.position;
        anim = GetComponent<Animator>();

        if (colisorDano == null) colisorDano = GetComponent<Collider2D>();

        if (tipo == TipoArmadilha.Espetos && atrasoInicial > 0)
        {
            emAtraso = true;
            DesligarEspetos();
        }
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
        if (emAtraso)
        {
            cronometro += Time.deltaTime;

            if (cronometro >= atrasoInicial)
            {
                emAtraso = false;
                cronometro = 0f;

                // Começa no Estado 1
                anim.SetInteger("Estado", 1);
            }

            return;
        }

        cronometro += Time.deltaTime;

        switch (anim.GetInteger("Estado"))
        {
            case 1:
                // Estado 1 - Subindo

                if (cronometro >= tempoInativo)
                {
                    anim.SetInteger("Estado", 2);
                    cronometro = 0f;

                    LigarEspetos();
                }

                break;

            case 2:
                // Estado 2 - Exposto

                LigarEspetos();

                if (cronometro >= tempoAtivo)
                {
                    anim.SetInteger("Estado", 1);
                    cronometro = 0f;

                    DesligarEspetos();
                }

                break;
        }
    }

    private void LigarEspetos()
    {
        if (colisorDano != null) colisorDano.enabled = true;
        if (anim != null) anim.SetBool("Ativado", true);
    }

    private void DesligarEspetos()
    {
        if (colisorDano != null) colisorDano.enabled = false;
        if (anim != null) anim.SetBool("Ativado", false);
    }

    private void AtualizarTorreta()
    {
        cronometro -= Time.deltaTime;
    }

    // --- DETECÇÃO DE DANO E REINÍCIO ---
    private void OnTriggerEnter2D(Collider2D collider)
    {
        ProcessarMorte(collider.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ProcessarMorte(collision.gameObject);
    }

    private void ProcessarMorte(GameObject objetoAtingido)
    {
        if (objetoAtingido.CompareTag("Player"))
        {
            Destroy(objetoAtingido);
            Invoke(nameof(ReiniciarCena), 1.5f);
        }
    }

    private void ReiniciarCena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}