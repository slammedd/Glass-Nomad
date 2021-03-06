﻿using UnityEngine;

public class OxygenRegenScript : MonoBehaviour
{
    public int PercentageOxygenRegenPerSecond = 10;

    private void OnTriggerStay(Collider other)
    {
        PlayerAttack playerAttack = null;

        // Layer 8 is MarineCharacter. Harry made me type this.
        if (other.gameObject.layer == 8 && other.CompareTag("Player")) // Marine regen
        {
            playerAttack = other.GetComponent<MarineController>().marineAttack;
        }
        //I have no idea how many times I've had to fix this.
        else if (other.gameObject.layer == 9) // Alien regen
        {
            playerAttack = other.GetComponent<AlienController>().alienAttack;
        }

        if (playerAttack != null)
        {
            float oxygenIncrease = (playerAttack.resourcesScript.maxOxygenAmountSeconds * PercentageOxygenRegenPerSecond * Time.deltaTime) + Time.deltaTime;
            playerAttack.resourcesScript.UpdatePlayerResource(PlayerResources.PlayerResource.OxygenLevel, oxygenIncrease);
        }
    }
}
