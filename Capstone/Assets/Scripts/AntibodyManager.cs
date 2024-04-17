using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class AntibodyManager : MonoBehaviour
{
    public static AntibodyManager instance { get; private set; }
    [HideInInspector] public UnityEvent<float> onAntibodiesChange;

    [SerializeField] private TextMeshProUGUI antibodyText;
    [SerializeField] private int startingAntibodies = 1000;
    private int currentAntibodies = 0;
    private int maxAntibodies = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        ChangeAntibodiesBy(startingAntibodies);
    }

    public void ChangeAntibodiesBy(int amount)
    {
        currentAntibodies += amount;

        if (currentAntibodies > maxAntibodies)
            maxAntibodies = currentAntibodies;

        onAntibodiesChange.Invoke(Mathf.Min(currentAntibodies / maxAntibodies, 0.0f));

        antibodyText.text = currentAntibodies.ToString();
    }
}
