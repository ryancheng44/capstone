// CLEARED

using UnityEngine;
using TMPro;

public class AntibodyManager : MonoBehaviour
{
    public static AntibodyManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI antibodyText;
    [SerializeField] private int startingAntibodies;

    private int currentAntibodies = 0;
    private int maxAntibodies = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        ChangeAntibodiesBy(startingAntibodies);
    }

    public void ChangeAntibodiesBy(int amount)
    {
        currentAntibodies += amount;

        if (currentAntibodies > maxAntibodies)
            maxAntibodies = currentAntibodies;

        EffectsManager.Instance.OnAntibodiesChange(Mathf.Min(currentAntibodies / maxAntibodies, 0.0f));

        antibodyText.text = currentAntibodies.ToString();
    }
}
