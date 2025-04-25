using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class PlayerManager : NetworkBehaviour
{
    public const int BULLET_DAMAGE = 10;

    public NetworkVariable<FixedString128Bytes> username;
    public NetworkVariable<int> spawns = new NetworkVariable<int>(0);

    [SerializeField] Image m_HealthBarImage;
    [SerializeField] TMP_Text m_UsernameLabel;
    public TextMeshProUGUI txtHealth;
    public TextMeshProUGUI txtSpawns;

    private Health healthComponent;
    private GameObject playerSpawner;

    private void Awake()
    {
        username = new NetworkVariable<FixedString128Bytes>(Utilities.GetRandomUsername());
        healthComponent = GetComponent<Health>();
        healthComponent.OnHealthChanged += OnClientHealthChanged;
        healthComponent.OnDeath += HandleDeath;
        playerSpawner = GameObject.Find("PlayerSpawner");
    }

    public override void OnDestroy()
    {
        if (healthComponent != null)
        {
            healthComponent.OnHealthChanged -= OnClientHealthChanged;
            healthComponent.OnDeath -= HandleDeath;
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        spawns.OnValueChanged += OnSpawnsChanged;
        username.OnValueChanged += OnClientUsernameChanged;

        ChangeNameRpc(Utilities.GetRandomUsername());
        transform.position = playerSpawner.GetComponent<SpawnPointManager>().GetRandomSpawnPoint();
        OnClientHealthChanged(Health.MaxLife, Health.MaxLife);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        username.OnValueChanged -= OnClientUsernameChanged;
    }

    [Rpc(SendTo.Server)]
    public void ChangeNameRpc(FixedString128Bytes newValue)
    {
        if (!IsServer) return;
        username.Value = newValue;
    }

    [Rpc(SendTo.Server)]
    public void ApplySpawnRpc()
    {
        if (!IsServer) return;
        spawns.Value++;
    }

    private void OnClientUsernameChanged(FixedString128Bytes previousValue, FixedString128Bytes newValue)
    {
        m_UsernameLabel.text = newValue.ToString();
    }

    private void OnClientHealthChanged(int previousHealth, int newHealth)
    {
        m_HealthBarImage.rectTransform.localScale = new Vector3((float)newHealth / Health.MaxLife, 1);
        float healthPercent = (float)newHealth / Health.MaxLife;
        Color healthBarColor = new Color(1 - healthPercent, healthPercent, 0);
        m_HealthBarImage.color = healthBarColor;
        txtHealth.text = newHealth.ToString();
    }

    private void OnSpawnsChanged(int previousValue, int newValue)
    {
        txtSpawns.text = newValue.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsServer && collision.gameObject.CompareTag("Bullet"))
        {
            healthComponent.ApplyDamageRpc(BULLET_DAMAGE);
        }
    }

    private void HandleDeath()
    {
        Respawn();
    }

    private void Respawn()
    {
        if (!IsServer) return;

        transform.position = playerSpawner.GetComponent<SpawnPointManager>().GetRandomSpawnPoint();
        healthComponent.CurrentHealth.Value = Health.MaxLife;
        spawns.Value++;
    }
}
