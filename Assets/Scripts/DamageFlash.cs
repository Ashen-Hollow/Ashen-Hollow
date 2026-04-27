using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [Header("Configurações do Flash")]
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashDuration = 0.2f;

    private SpriteRenderer spriteRenderer;
    private Material material;
    
    // Usar o ID da propriedade é muito mais rápido que usar a String
    private int flashAmountID = Shader.PropertyToID("_FlashAmount");
    private int flashColorID = Shader.PropertyToID("_FlashColor");

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Ao acessar .material, o Unity cria uma instância única para este inimigo
        material = spriteRenderer.material;
        
        // Garante que a cor inicial do flash seja a definida no script
        material.SetColor(flashColorID, flashColor);
    }

    // Função principal que você chamará do seu script de Health
    public void CallDamageFlash()
    {
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // Ativa o efeito (FlashAmount = 1)
        material.SetFloat(flashAmountID, .4f);

        // Espera o tempo definido
        yield return new WaitForSeconds(flashDuration);

        // Desativa o efeito (FlashAmount = 0)
        material.SetFloat(flashAmountID, 0f);
    }
}
