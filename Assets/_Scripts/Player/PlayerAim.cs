using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerInput playerInputs;
    [SerializeField] PlayerStatus playerStatus;
    [SerializeField] NearestEnemy nearestEnemy;


    private InputAction aimAction;
    private InputAction lockAction;


    [SerializeField] bool isAimlocked = false;
    
    void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        playerInputs = GetComponent<PlayerInput>();
        aimAction = playerInputs.actions["Aim"];
        lockAction = playerInputs.actions["LockAim"];
        nearestEnemy = GetComponent<NearestEnemy>();
    }

    void Update()
    {
        if (playerStatus.usingController && playerStatus.canMove && !isAimlocked)
        {
            Vector2 aimInput = aimAction.ReadValue<Vector2>();

            // Only rotate if the stick is being pushed (to avoid snapping to 0 when idle)
            if (aimInput.sqrMagnitude > 0.1f && playerStatus.canMove)
            {
                float angle = Mathf.Atan2(aimInput.x, aimInput.y) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
                transform.rotation = rotation;
            }
        }
        else if(!playerStatus.usingController && playerStatus.canMove && !isAimlocked) 
        {

            RotateTowardMouse();
        }

        if(lockAction.triggered && playerStatus.canMove)
        {
            isAimlocked = !isAimlocked;
          
           
        }


        if(isAimlocked)
        {
           
        }


    }





    void RotateTowardMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f))
        {
            Vector3 lookPoint = hitInfo.point;
            Vector3 direction = (lookPoint - transform.position);
            direction.y = 0f; // Keep rotation on the horizontal plane only

            if (direction.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }

}
