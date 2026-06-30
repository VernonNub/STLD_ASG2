using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    [Header("Collectible Information")]
    public int cash;
    public int scraps;
    public string itemName;
    public AudioClip collectibleSFX;

    public enum CollectibleType
    {
        Cash,
        Scraps,
        Objectives,
        Buffs
    }
    public CollectibleType collectibleType;

    public void CollectItems(PlayerManager player)
    {
        switch(collectibleType)
        {
            case CollectibleType.Cash:
                player.cash += cash;
                Destroy(gameObject);
                break;
            case CollectibleType.Scraps:
                player.scraps += scraps;
                Destroy(gameObject);
                break;
            case CollectibleType.Objectives:
                player.inventory.Add(itemName);
                Destroy(gameObject);
                break;
            case CollectibleType.Buffs:
                AddBuffs(player);
                Destroy(gameObject);
                break;
        }
    }

    private void AddBuffs(PlayerManager player)
    {
        
    }

    private void PlaySoundFX()
    {
        AudioSource.PlayClipAtPoint(collectibleSFX, gameObject.transform.position);
        Destroy(gameObject);
    }
}
