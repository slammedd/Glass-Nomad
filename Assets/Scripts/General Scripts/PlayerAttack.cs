﻿using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerAttack : MonoBehaviourPunCallbacks
{
    // Used to change the health bar slider above the player.
    [SerializeField] 
    public Image healthSlider = null; 

    // Used to set the player's health the max, on initialisation.
    [SerializeField] 
    private int maxHealth = 100; 
    
    // Used to control the health of this player.
    public PlayerHealth healthScript;     
    // Used to disable/enable the camera so that we only control our local player's camera.
    private GameObject cameraGO; 

    private float deltaTime = 0.0f;

    static WeaponClass rifle;

    private void Start()
    {
        healthScript = new PlayerHealth(this.gameObject, maxHealth);

        // Gets the camera child on the player.
        cameraGO = this.GetComponentInChildren<Camera>().gameObject; 

        rifle = new WeaponClass(3, 2, 20, 50, 40);
        deltaTime = rifle.fireRate;
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (Input.GetButton("Fire1"))
        {
            deltaTime += Time.deltaTime;
            
            if (canFire(deltaTime, rifle.fireRate, rifle.bulletsInCurrentMag))
            {
                // Calls the 'Attack' method on all clients, meaning that the health will be synced across all clients.
                photonView.RPC("FireWeapon", RpcTarget.All, cameraGO.transform.position, cameraGO.transform.forward, rifle.range, rifle.damage);
                deltaTime -= rifle.fireRate;
            }
            
        }

        if (Input.GetButtonUp("Fire1"))
        {
            // Means there is no delay before firing when the button is first pressed.
            deltaTime = rifle.fireRate;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            rifle.ReloadWeapon();
        }
    }

    private bool canFire(float deltaTime, float fireRate, int bulletsInCurrentMag)
    {
        if (bulletsInCurrentMag > 0)
        {
            if (deltaTime > fireRate)
            {
                return true;
            }
        }
        else
        {
            Debug.Log("You are out of bullets in your magazine.");
        }

        return false;
    }

    [PunRPC]
    protected void FireWeapon(Vector3 cameraPos, Vector3 cameraForward, float range, int damage)
    {
        Debug.Log(photonView.Owner.NickName + " did a light attack");

        RaycastHit hit;
        if (Physics.Raycast(cameraPos, cameraForward, out hit, range))
        {
            PlayerAttack hitPlayer = hit.transform.gameObject.GetComponent<PlayerAttack>();
            if (hitPlayer != null)
            {
                PlayerHealth hitPlayerHealth = hitPlayer.healthScript;

                hitPlayerHealth.PlayerHit(damage);
                hitPlayer.healthSlider.fillAmount = hitPlayerHealth.fillAmount;

                Debug.Log(photonView.Owner.NickName + " hit player: " + hitPlayer.gameObject.name);
            }
        }
    }
}
