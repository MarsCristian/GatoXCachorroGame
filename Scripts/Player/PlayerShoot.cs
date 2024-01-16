using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bullet;
    public GameObject bulletEnemy;
    public GameObject weaponThrow;

    private Transform weaponChild;
    private Transform parentChange;

    public float fireRate = 5;
    private float fireTimer = 0f;
    public int ammo = 0;
    public int maxAmmo = 30;

    public PlayerMovement alertIsTrue;
    public AIPath aiPath;
    private GameObject father;
    private GameObject grandfather;

    public LayerMask hitters;
    public float visionRange = 15f;

    void Start(){
        ammo = maxAmmo;
        weaponChild = this.gameObject.transform.GetChild(0);
        alertIsTrue = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (this.transform.parent != null){
            father = this.transform.parent.gameObject;
            GetComponent<CircleCollider2D>().enabled = false;
            if (father.transform.name == "PlayerWeapon"){
                if (Input.GetMouseButton(0) && fireTimer <= 0f && ammo > 0)
                {
                    if (!alertIsTrue.alert)
                        alertIsTrue.alert = true;
                    ammo--;
                    Instantiate(bullet, weaponChild.position, father.transform.rotation);
                    fireTimer = fireRate;
                }
                else if (Input.GetKeyDown(KeyCode.Q)){
                    this.transform.SetParent(Instantiate(weaponThrow, this.transform.position, father.transform.rotation).transform);
                }
                else
                    fireTimer -= Time.deltaTime;
            }
            else if(father.transform.name == "EnemyWeapon"){
                grandfather = father.transform.parent.gameObject;
                RaycastHit2D hit = Physics2D.Raycast(father.transform.position, this.transform.parent.TransformDirection(Vector2.up), visionRange, hitters);
                if (alertIsTrue.alert && hit.transform.tag == "Player" && fireTimer <= 0f && ammo > 0){
                    grandfather.GetComponent<AIPath>().canMove = false;
                    ammo--;
                    Instantiate(bulletEnemy, weaponChild.position, father.transform.rotation);
                    fireTimer = fireRate;
                }
                else if(alertIsTrue.alert && !(hit.transform.tag == "Player") && fireTimer <= 0f && ammo > 0){
                    grandfather.GetComponent<AIPath>().canMove = true;
                    fireTimer -= Time.deltaTime;
                }
                else {
                    fireTimer -= Time.deltaTime;
                }
            }
        }
        else {
            GetComponent<CircleCollider2D>().enabled = true;
        }
    }
}
