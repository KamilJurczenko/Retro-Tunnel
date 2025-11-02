//using UnityEngine.VFX;
using UnityEngine;

public class TunnelExit : MonoBehaviour
{
    [SerializeField] ParticleSystem particlesSystem;
    [SerializeField] SettingColor colorsClass;
    [SerializeField] TunnelManager speedTunnel;
    [SerializeField] StageManager stageManager;
    [SerializeField] AudioSource inTunnelSoundFX;
    [SerializeField] AudioSource exitTunnelSoundFX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !TunnelManager.tunnelExitTrigger)
        {
            GameObjectSpawner.respawnCheckpoint = TunnelManager.GetCurrentPositions()[1].z;
            TunnelManager.tunnelExitTrigger = true;
            particlesSystem.transform.parent = null;
            Vector3 contactPoint = PlayerController.playerPosition;
            contactPoint += new Vector3(0,0,0);
            particlesSystem.transform.position = contactPoint;
            particlesSystem.Play();
            inTunnelSoundFX.Stop();
            if(GameManager.playSoundFX)
                exitTunnelSoundFX.Play();
            stageManager.SetNextStage();
            colorsClass.SetColor();
            speedTunnel.SetTunnelPosition(StageManager.currentNextStagePoint / PlayerController.playerMetersFactor);
        }
    }
}
