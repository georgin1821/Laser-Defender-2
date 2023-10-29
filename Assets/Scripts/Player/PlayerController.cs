using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : SimpleSingleton<PlayerController> 
{

    private float xMin, xMax, yMin, yMax;
    private Camera viewCamera;
    public float speed;

    public bool mobileRelease;
    public Vector2 dir;
    override protected void Awake()
    {
        base.Awake();
        viewCamera = Camera.main;

        speed = speed / 1000;
    }

    private void Start()
    {
        xMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        xMax = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        yMax = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
        yMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;

    }
    private void Update()
    {
        if (!GamePlayController.Instance.andOfAnimation) return;
        if (GamePlayController.Instance.state == GameState.PLAY)
        {
            Move();
            RestrictPlayerToScreen();
        }
    }

    private void Move()
    {
        if (mobileRelease)
        {
            MobileInput();
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 mousePosition = viewCamera.ScreenToWorldPoint(Input.mousePosition);
                transform.position = Vector2.MoveTowards(transform.position, mousePosition, speed * 3000 * Time.deltaTime);
            }
        }

        Vector3 translation = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;
        translation *= speed * 2000 * Time.deltaTime;
        this.transform.Translate(translation);

    }

    private void MobileInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                dir = touch.deltaPosition;
                transform.Translate(dir * speed, Space.World);

                if (dir.x > 0)
                {
                    transform.Rotate(0, -6, 0);
                    if (transform.rotation.eulerAngles.y > -40)
                    {
                        transform.rotation = Quaternion.Euler(0, -40, 0);
                    }
                }
                else if (dir.x < 0)
                {
                    transform.Rotate(0, 6, 0);
                    if (transform.rotation.eulerAngles.y > 40)
                    {
                        transform.rotation = Quaternion.Euler(0, 40, 0);
                    }
                }
            }

            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Ended)
            {
               // transform.rotation = Quaternion.Euler(0, 0, 0);
                var fromAngle = transform.rotation;
                var toAngle = Quaternion.Euler(0, 0, 0);
                for (var t = 0f; t < 1; t += Time.deltaTime / 0.2f)
                {
                    transform.rotation = Quaternion.Lerp(fromAngle, toAngle, t);
                }

            }
        }
    }

    private void RestrictPlayerToScreen()
    {
        Vector3 temp = this.transform.position;
        temp.x = Mathf.Clamp(temp.x, xMin, xMax);
        temp.y = Mathf.Clamp(temp.y, yMin, yMax);

        this.transform.position = temp;
    }

}
