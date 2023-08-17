using System.Collections.Generic;
using UnityEngine;

public abstract class Inventory<T> : MonoBehaviour where T : Gun
{
    [SerializeField] private List<T> guns;

    private int currentGunIndex;
    public T CurrentGun => guns[currentGunIndex];

    public void SwitchGun(int index)
    {
        if (currentGunIndex == index || index >= guns.Count) return;

        CurrentGun.gameObject.SetActive(false);
        currentGunIndex = index;
        CurrentGun.gameObject.SetActive(true);
    }
}
