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

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float moveInputX;
    private bool estaNoChao;

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
    }

    private void Update()
    {

        // Atualiza as variáveis do Animator (garanta que o nome seja exatamente idêntico)
        animator.SetBool("isGrounded", estaNoChao);
        animator.SetBool("isWalking", Mathf.Abs(moveInputX) > 0.1f);

        // Inverte a direção do Sprite
        InverterSprite();
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

    public int vida = 10;

    public void TomarDano()
    {
        vida--;

        Debug.Log("Tomou dano" + vida);

        if (vida <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

