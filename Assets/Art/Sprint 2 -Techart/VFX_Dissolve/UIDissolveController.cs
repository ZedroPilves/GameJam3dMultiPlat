using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class UIDissolveController : MonoBehaviour
{
    // ============================= //
    //        CONFIGURAÇÕES         //
    // ============================= //

    [Header("Material e Shader")]
    [Tooltip("Material com o shader de dissolve aplicado")]
    public Material dissolveMaterial;

    [Tooltip("Nome do parâmetro do dissolve no shader")]
    public string dissolveParameter = "_DissolveAmount";

    [Header("Intervalo da Dissolução")]
    [Tooltip("Valor inicial para a dissolução")]
    public float startDissolve = 0f;

    [Tooltip("Valor final para a dissolução")]
    public float endDissolve = 1.1f;

    [Header("Tempo e Curva")]
    [Tooltip("Tempo da transição em segundos")]
    public float transitionTime = 1.0f;

    [Tooltip("Curva personalizada de transição")]
    public AnimationCurve dissolveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    // ============================= //
    //          PRIVADOS            //
    // ============================= //

    private Coroutine currentCoroutine;

    // ============================= //
    //      FUNÇÕES DE CONTROLE     //
    // ============================= //

    public void Dissolve()
    {
        StartDissolve(startDissolve, endDissolve);
    }

    public void Appear()
    {
        StartDissolve(endDissolve, startDissolve);
    }

    public void StartDissolve(float from, float to)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(AnimateDissolve(from, to));
    }

    // ============================= //
    //        INTERPOLAÇÃO          //
    // ============================= //

    private IEnumerator AnimateDissolve(float from, float to)
    {
        float elapsed = 0f;

        while (elapsed < transitionTime)
        {
            float t = elapsed / transitionTime;
            float curveValue = dissolveCurve.Evaluate(t);
            float value = Mathf.Lerp(from, to, curveValue);
            dissolveMaterial.SetFloat(dissolveParameter, value);
            elapsed += Time.deltaTime;
            yield return null;
        }

        dissolveMaterial.SetFloat(dissolveParameter, to);
    }

    // ============================= //
    //       TESTE POR TECLAS       //
    // ============================= //

    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            Dissolve();
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            Appear();
        }
    }
}
