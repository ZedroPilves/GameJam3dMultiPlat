using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    [Header("Velocidade de movimento")]
    public float moveSpeed = 5f;

    void Update()
    {
        // Pega o input horizontal: -1 (esquerda), 0 (nada), 1 (direita)
        float inputX = Input.GetAxisRaw("Horizontal");

        // Calcula o movimento
        Vector3 move = new Vector3(inputX, 0f, 0f) * moveSpeed * Time.deltaTime;

        // Aplica o movimento
        transform.Translate(move);
    }
}
