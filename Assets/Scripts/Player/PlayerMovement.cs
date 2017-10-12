using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //controlls how fast player is 
    //f in the end means floating variable.
    public float speed = 6f;

    //store movement for player
    Vector3 movement;
    
    //reference to animation
    Animator _playerAnimator;

    //reference to rigidbody
    Rigidbody _playerRigidbody;
    int floorMask;
    float camRayLength = 100f;

    //mono behavior functions
    private void Awake()
    {
        //establish reference
        floorMask        = LayerMask.GetMask("Floor");
        _playerAnimator  = GetComponent<Animator>();
        _playerRigidbody = GetComponent<Rigidbody>();

    }

    //mono behavior functions
    private void FixedUpdate()
    {
        //DEFAULTS:
        //only value of -1, 0, 1 
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
        Animating(h, v);

    }

    void Move(float h, float v)
    {
        //x and z are flat on the ground.
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;
        _playerRigidbody.MovePosition(transform.position + movement);

    }

    void Turning()
    {
        //find the point underneath the mouse
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        if(Physics.Raycast(cameraRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            //way of storing a rotation
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            //change entire rotation, NO OFFSET
            _playerRigidbody.MoveRotation(newRotation);
        }
    }

    void Animating(float h, float v)
    {
        //if h or v is 0 then walking is false.
        bool walking = h != 0f || v != 0f;
        _playerAnimator.SetBool("IsWalking", walking);


    }
}
