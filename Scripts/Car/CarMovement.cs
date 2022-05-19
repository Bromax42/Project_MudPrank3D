using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    // movement
    public float steerSpeed; //Horizontal speed - only player input
    public float speed;
    public float maxSpeed;
    public float acceleration;

    // touch input
    private Touch touch;
    private Vector3  inputVect;
    float x = 0;
    float diff = 0;

    private Vector3 moveDir;

    private float horizontal = 0;


    public float currentHorizontalPosition;



    public Rigidbody rb;

    void Awake()
    {


        rb = GetComponent<Rigidbody>();

        currentHorizontalPosition = transform.position.x;
    }
    private void Update()
    {
        if (Application.isEditor)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            horizontal = GetTouchDirection().x;
        }
    }
    void FixedUpdate()
    {
        if (horizontal != 0)
        {
            moveDir = (transform.forward);
            rb.velocity = moveDir * speed;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(moveDir + (transform.right * horizontal)), steerSpeed * Time.fixedDeltaTime);
        }
        else
        {
            moveDir = transform.forward * speed;
            rb.velocity = moveDir;
        }


        if (speed < maxSpeed)
        {
            speed += acceleration;
        }
    }
    private Vector3 GetTouchDirection()
    {
        float rate = 0;
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                x = touch.position.x;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                diff = touch.position.x - x;
                rate = (diff / Screen.width);
                inputVect.x = rate;
            }

        }
        else
        {
            inputVect.x = 0;
        }
        return inputVect;
    }


}
