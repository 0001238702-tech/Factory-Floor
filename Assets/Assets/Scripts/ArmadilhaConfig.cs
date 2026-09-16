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

    [SerializeField] private AudioClip somDisparo;
    private AudioSource audioSource;



    private Vector3 posInicial;
    private Animator anim;
    private bool emAtraso = true;
    private float cronometro = 0f;
 
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        
        
        emAtraso = true;
        posInicial = transform.position;

        anim = GetComponent<Animator>();
        
        if (colisorDano == null) colisorDano = GetComponent<Collider2D>();

        if (tipo == TipoArmadilha.Espetos && atrasoInicial > 0)
        {
            
        }
    }


    private void Update()
    {
        switch (tipo)
        {
            case TipoArmadilha.Espetos:

                AtivarEspetos();
                break;

            case TipoArmadilha.Torreta:
                AtualizarTorreta();
                break;
        }
    }


    private void AtivarEspetos()
    {
        cronometro += Time.deltaTime;

        if (cronometro <= atrasoInicial && emAtraso == true)
        {
                        
            // Começa no Estado 1
            anim.SetInteger("Estado", 0);
        }


        else if (cronometro < tempoAtivo + atrasoInicial)
        {
            Debug.Log("Ativando o Espeto");
            anim.SetInteger("Estado", 1);
           colisorDano.enabled = true;
        }
        else if (cronometro < tempoAtivo + tempoInativo + atrasoInicial)
        {
                Debug.Log("Desativando o Espeto");
                anim.SetInteger("Estado", 2);
            colisorDano.enabled= false;
        }
        else 
        { 
        cronometro = 0f;
            emAtraso = false;
            atrasoInicial = 0f;
        }
    }

    private void AtualizarTorreta()
    {
        cronometro += Time.deltaTime;

        if (cronometro >= tempoEntreDisparos)
        {


            Disparar();

            cronometro = 0f;
        }
    }

    public void Disparar()
    {
        GameObject projetil = Instantiate(prefabProjetil, pontoDisparo.position, pontoDisparo.rotation);
        audioSource.PlayOneShot(somDisparo);
    }
}