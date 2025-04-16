using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CoopSpawner : MonoBehaviour
{
  
    [SerializeField] CinemachineTargetGroup targetGroup;
    [SerializeField] GameObject canva;
    [SerializeField] Transform spawnPoint;
    [SerializeField] InputAction inputActionSpawn;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] bool canSpawnPlayer = true;
    void Start()
    {
        inputActionSpawn = playerInput.actions["Player2Join"];
        playerInput = GetComponent<PlayerInput>();  

    }

    // Update is called once per frame
    void Update()
    {
        if(inputActionSpawn.IsPressed() && canSpawnPlayer)
        {
            canSpawnPlayer = false; 
            transform.position = spawnPoint.position;
            canva.SetActive(true);
            targetGroup.AddMember(gameObject.transform, 1, 0.5f);   
        }
    }

    
}
