using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Vector2 moveInput;
    public Animator animator; 

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();    
        

    }

    // Update is called once per frame
    void Update()
    {
        moveInput = playerMovement.moveAction.ReadValue<Vector2>();
        animator.SetFloat("movX", moveInput.x);
        animator.SetFloat("movY", moveInput.y); 
    }
}
