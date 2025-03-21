using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, InputController.IPlayerInputActions, IDamage
{
    public static Player instance;
    Rigidbody rb;
    public float speed = 7.5f;
    private InputController ic;
    public GameObject weapon, bulletPrefab;
    Stack<GameObject> bullets;
    public float HP { get; set; }

    private void Awake()
    {
        HP = 10;
        rb = GetComponent<Rigidbody>();
        ic = new InputController();
        ic.PlayerInput.SetCallbacks(this);
        bullets = new Stack<GameObject>();
    }
    private void OnEnable()
    {
        ic.PlayerInput.Enable();
    }
    private void OnDisable()
    {
        ic.PlayerInput.Disable();
    }
    private void Update()
    {
        Move();
    }
    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        rb.velocity = new Vector3(move.x * speed, rb.velocity.y, move.z * speed);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameObject bullet;
            if (bullets.Count > 0)
            {
                bullet = Pop();
            }
            else
            {
                bullet = Instantiate(bulletPrefab, weapon.transform.position, weapon.transform.rotation);
            }
            bullet.GetComponent<Rigidbody>().AddForce(weapon.transform.forward * 1000);
        }
    }
    public void Push(GameObject obj)
    {
        obj.SetActive(false);
        bullets.Push(obj);
    }
    public GameObject Pop()
    {
        GameObject obj = bullets.Pop();
        obj.SetActive(true);
        obj.transform.position = weapon.transform.position;
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        return obj;
    }
    public GameObject Peek()
    {
        return bullets.Peek();
    }

    public void TakeDamage(float dmg)
    {
        HP -= dmg;
        if (HP <= 0)
        {
            Application.Quit();
        }
    }
}