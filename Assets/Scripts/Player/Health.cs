using UnityEngine;
using Unity.Netcode;

public class Health : NetworkBehaviour
{
    public const int MaxLife = 100;
    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>(MaxLife);

    public delegate void HealthChanged(int previous, int current);
    public event HealthChanged OnHealthChanged;

    public event System.Action OnDeath;


    private void Awake()
    {
        CurrentHealth.OnValueChanged += HandleHealthChanged;
    }

    public override void OnDestroy()
    {
        CurrentHealth.OnValueChanged -= HandleHealthChanged;
    }

    [Rpc(SendTo.Server)]
    public void ApplyDamageRpc(int amount)
    {
        if (!IsServer) return;

        if (CurrentHealth.Value > 0)
        {
            CurrentHealth.Value -= amount;
            if (CurrentHealth.Value <= 0)
            {
                Die();
            }
        }
    }

    [Rpc(SendTo.Server)]
    public void HealRpc(int amount)
    {
        if (!IsServer) return;

        CurrentHealth.Value = Mathf.Min(CurrentHealth.Value + amount, MaxLife);
    }

    private void HandleHealthChanged(int previous, int current)
    {
        OnHealthChanged?.Invoke(previous, current);
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }
}
