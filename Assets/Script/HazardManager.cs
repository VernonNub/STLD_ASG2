using UnityEngine;

public class HazardManager : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    public enum HazardType
    {
        Instance,
        DoT,
        SpeedChange,
        MaxHealthChange
    }

    [Header("Hazard Details")]
    public HazardType hazardType;
    public float effectValue;
    public bool hasActivated = false;
    public float newMoveSpeed = 0;

    private void OnTriggerStay(Collider other)
    {
        if(other.name == "Player")
        {
            playerManager = other.GetComponent<PlayerManager>();
            OvertimeEffects(playerManager);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        newMoveSpeed = Mathf.Max(other.GetComponent<PlayerManager>().tpc.MoveSpeed + effectValue, 1);

        if (other.name == "Player")
        {
            playerManager = other.GetComponent<PlayerManager>();
            InstanceEffects(playerManager);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<PlayerManager>().tpc.MoveSpeed = 2;
        other.GetComponent<PlayerManager>().tpc.SprintSpeed = 5.335f;
    }

    //For DoT and slow which is only active when player in inside the area
    private void OvertimeEffects(PlayerManager player)
    {
        switch(hazardType)
        {
            case HazardType.DoT:
                player.UpdateHealth(effectValue * Time.deltaTime, 0);
                break;
            case HazardType.SpeedChange:
                player.tpc.MoveSpeed = newMoveSpeed;
                player.tpc.SprintSpeed = newMoveSpeed;
                break;
        }
    }

    private void InstanceEffects(PlayerManager player)
    {
        switch (hazardType)
        {
            //All of my damagedone is handled wiith the update health method in my playermanager 
            //More details look at player manager script.
            case HazardType.Instance:
                player.UpdateHealth(effectValue, 0);
                if(effectValue > 0)
                {
                    hasActivated = true;
                }
                break;

            case HazardType.MaxHealthChange:
                if (!hasActivated)
                    player.UpdateHealth(0, effectValue);
                hasActivated = true;
                break;
        }
    }
}
