using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [BoxGroup("Movimento")]
    public float speed;
    public Rigidbody2D rig;

    [BoxGroup("Configurações do Dash")]
    public float dashForce = 10f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 3f;

    // Variáveis de Controle de Estado
    public bool isDashReady = true;
    private bool isDashing = false;   // Bloqueia outras ações enquanto o dash acontece

    public float cooldownTimer;
    public float dashTimer;

    private Vector2 movementDirection;

    public CinemachineCamera virtualCamera;
    public float dashZoomSize = 12f;

    private float defaulZoom = 10f;

    public static PlayerController Intance;

   private void Awake()
    {
        Intance = this;
    }

    private void FixedUpdate()
    {
        // Máquina de estados que diz se o Player no meio de um Dash, 
        // para abortar a atualização de movimento padrão!
        if (isDashing) return;

        rig.linearVelocity = movementDirection.normalized * speed;
    }

    private void Update()
    {
        // 1. Gerencia o tempo de DURAÇÃO do Dash
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                isDashing = false; // Dash acabou e devolve o controle ao FixedUpdate
                virtualCamera.Lens.OrthographicSize = defaulZoom; // Reset da câmera para o tamanho original
            }
        }

        // 2. Gerencia o tempo de RECARGA do Dash
        if (!isDashReady)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                isDashReady = true;
            }
        }
    }

    void OnMove(InputValue value)
    {
        movementDirection = value.Get<Vector2>();
    }

    void OnDash()
    {
        // Só permite o Dash se estiver pronto e se não estiver no meio de um Dash
        if (isDashReady && !isDashing && movementDirection != Vector2.zero)
        {
            isDashing = true;
            defaulZoom = virtualCamera.Lens.OrthographicSize; // Salva o zoom atual para restaurar depois
            virtualCamera.Lens.OrthographicSize = dashZoomSize; // Aplica o zoom out da câmera durante o dash
            isDashReady = false;

            // Inicia os dois cronômetros simultaneamente
            dashTimer = dashDuration;
            cooldownTimer = dashCooldown;

            // Aplica a força real! Como isDashing agora é true, o FixedUpdate não vai cancelar essa força
            rig.AddForce(movementDirection.normalized * dashForce, ForceMode2D.Impulse);
        }
    }
}