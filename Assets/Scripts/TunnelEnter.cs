using UnityEngine;

public class TunnelEnter : MonoBehaviour
{
    [SerializeField] PlayerController pc;
    [SerializeField] AudioSource inTunnelSoundFX;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !TunnelManager.tunnelEnterTrigger)
        {
            if(GameManager.playSoundFX)
                inTunnelSoundFX.Play();
            TunnelManager.tunnelEnterTrigger = true;
            Vector3 vec = TunnelManager.GetCurrentPositions()[1];
            if (PlayerController.maxSpeedIncreaseCount > PlayerController.speedIncreaseCounter)
            {
                StartCoroutine(pc.SetPlayerZVelocity(PlayerController.zCurrentPlayerVelocity + 2f));
                PlayerController.speedIncreaseCounter++;
            }
            else Debug.Log("Max Player Speed achieved. No Speed Increase.");
            StartCoroutine(pc.MoveToPoint(vec, PlayerController.zCurrentPlayerVelocity * 2,true));
        }
    }
}
