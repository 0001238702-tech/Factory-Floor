using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class ControleJogador2D : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Efeitos Sonoros do Jogador")]
    [SerializeField] private AudioClip somPulo;
    [SerializeField] private AudioClip somPassos;
    [SerializeField] private float intervaloPassos = 0.35f; // Tempo entre cada som de passada

    private AudioSource audioSource;
    private float timerPassos;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float moveInputX;
    private bool estaNoChao;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // ... restante da inicialização
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Método disparado pelo Player Input (Action: Move)
    public void OnMove(InputValue value)
    {
        Vector2 inputVector = value.Get<Vector2>();
        moveInputX = inputVector.x;
    }

    // Método disparado pelo Player Input (Action: Jump)
    public void OnJump(InputValue value)
    {
        // Pula apenas se estiver no chão
        if (value.isPressed && estaNoChao)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            estaNoChao = false; // Força a saída do chão imediatamente no frame do pulo
        }

        TocarSomPulo();
    }

    public void TocarSomPulo()
    {
        if (audioSource != null && somPulo != null)
        {
            audioSource.PlayOneShot(somPulo);
        }
    }


    private void Update()
    {

        // Atualiza as variáveis do Animator (garanta que o nome seja exatamente idêntico)
        animator.SetBool("isGrounded", estaNoChao);
        animator.SetBool("isWalking", Mathf.Abs(moveInputX) > 0.1f);

        // Inverte a direção do Sprite
        InverterSprite();

        // --- LÓGICA DE PASSOS ---
        // Toca passos apenas enquanto houver input horizontal e o jogador estiver no chão
        if (Mathf.Abs(moveInputX) > 0.1f && estaNoChao)
        {
            timerPassos += Time.deltaTime;
            if (timerPassos >= intervaloPassos)
            {
                if (audioSource != null && somPassos != null)
                {
                    audioSource.PlayOneShot(somPassos);
                }
                timerPassos = 0f;
            }
        }
        else
        {
            timerPassos = intervaloPassos; // Reseta para tocar instantaneamente assim que voltar a andar
        }


    }

    private void FixedUpdate()
    {
        // Aplica velocidade horizontal mantendo a força Y da gravidade/pulo
        rb.linearVelocity = new Vector2(moveInputX * moveSpeed, rb.linearVelocity.y);
    }

    private void InverterSprite()
    {
        if (moveInputX > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInputX < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            estaNoChao = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            estaNoChao = false;
        }
    }
}