using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
    public static Statistics instance { get; private set; }

    [SerializeField] private Transform topBorder;
    [SerializeField] private Transform bottomBorder;
    [SerializeField] private Transform leftBorder;
    [SerializeField] private Transform rightBorder;

    [SerializeField] private TextMeshProUGUI populationText;

    public float xMin { get; private set; }
    public float XMax { get; private set; }
    public float yMin { get; private set; }
    public float yMax { get; private set; }

    private int population = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        xMin = leftBorder.position.x;
        XMax = rightBorder.position.x;
        yMin = bottomBorder.position.y;
        yMax = topBorder.position.y;
    }

    public void AddPopulation() {
        population++;
        populationText.text = "Population: " + population;
    }
}
