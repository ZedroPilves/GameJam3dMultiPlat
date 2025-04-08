using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerInput playerInputs;
    InputAction moveAction;
    InputAction dogeAction;
    [SerializeField] PlayerStatus playerStatus;   

    [SerializeField] Rigidbody rb;

    [Header("Movement Variables")]
    [SerializeField] float moveSpeed = 5f;

    [Header("Dodge Value")]
    [SerializeField] float dodgeSpeed = 10f;
    [SerializeField] float dodgeTime = 0.5f;    

    void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        playerInputs = GetComponent<PlayerInput>();
        moveAction = playerInputs.actions["Move"];
        dogeAction = playerInputs.actions["Dodge"];
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStatus.canMove)
        {
            if (moveAction != null)
            {
                rb.linearVelocity = new Vector3(moveAction.ReadValue<Vector2>().x * moveSpeed, rb.linearVelocity.y, moveAction.ReadValue<Vector2>().y * moveSpeed);


            }
            else { rb.linearVelocity = Vector3.zero; }


            if (dogeAction.triggered)
            {
               StartCoroutine(Dodge()); 
            }
        }


       
    }



    IEnumerator Dodge()
    {
        playerStatus.canMove = false;
        playerStatus.isInvincible = true;

        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        if (moveInput.sqrMagnitude > 0.01f)
        {
            Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = targetRotation;

            rb.AddForce(moveDir * dodgeSpeed, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(transform.forward * dodgeSpeed, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(dodgeTime);

        playerStatus.canMove = true;
        playerStatus.isInvincible = false;
    }

}