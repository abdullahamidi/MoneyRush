using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int value;
    public bool isDestroyed = false;

    public Collectible(int value)
    {
        this.value = value;
    }
}
