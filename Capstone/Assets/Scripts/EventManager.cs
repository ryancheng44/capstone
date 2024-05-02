using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class EventManager : MonoBehaviour
{
    public static EventManager instance { get; private set; }

    [HideInInspector] public UnityEvent<Event, bool> onEventConclusion;
    [HideInInspector] public UnityEvent onNewEvent;

    [SerializeField] private Event[] events;
    [SerializeField] private Sprite[] operationSprites;

    [SerializeField] private GameObject eventPanel;
    [SerializeField] private GameObject problemPanel;

    [SerializeField] private TextMeshProUGUI eventNameText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI firstNumberText;
    [SerializeField] private TextMeshProUGUI secondNumberText;
    [SerializeField] private TextMeshProUGUI resultText;

    [SerializeField] private Image operationImage;
    [SerializeField] private TMP_InputField answerInputField;

    [SerializeField] private float minTimeBetweenEvents = 3.0f;
    [SerializeField] private float maxTimeBetweenEvents = 5.0f;

    [SerializeField] private Transform effects;
    [SerializeField] private TextMeshProUGUI effectTextPrefab;

    private Dictionary<string, Sprite> operationSpritesDict = new ();
    private List<TextMeshProUGUI> effectTexts = new ();

    private Event currentEvent;

    private float timeSinceLastEvent = 0.0f;
    private float timeToAnswer = 0.0f;

    private bool eventActive = false;
    private int correctAnswer;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Sprite sprite in operationSprites)
            operationSpritesDict.Add(sprite.name, sprite);

        foreach (Event e in events)
        {
            e.operationsAllowed = new HashSet<FourBasicOperations>(e.operationsAllowed).ToArray();
            e.effects = new float[] { e.effectOnGermSpeed, e.effectOnGermSpawnRate, e.effectOnGermHealth, e.effectOnGermDamage, e.effectOnTowerDamage };
        }

        FieldInfo[] fields = typeof(Event).GetFields();

        foreach (FieldInfo field in fields)
        {
            if (field.Name.StartsWith("effectOn"))
            {
                TextMeshProUGUI effectText = Instantiate(effectTextPrefab, effects);
                
                string temp = field.Name.Substring(8);
                string effectName = "" + temp[0];

                for (int i = 1; i < temp.Length; i++)
                {
                    if (char.IsUpper(temp[i]))
                        effectName += " ";

                    effectName += temp[i];
                }

                effectText.name = effectName;
                effectTexts.Add(effectText);
            }
        }

        timeSinceLastEvent = Random.Range(minTimeBetweenEvents, maxTimeBetweenEvents);
    }

    // Update is called once per frame
    void Update()
    {
        if (eventActive)
        {
            if (timeToAnswer <= 0.0f)
            {
                onEventConclusion.Invoke(currentEvent, false);
                DisplayResult(false);
            }
            else
            {
                timeToAnswer -= Time.deltaTime;
                timeText.text = timeToAnswer.ToString("F1");
            }

            if (Input.GetKeyDown(KeyCode.Return))
                SubmitAnswer();
        }
        else
        {
            if (timeSinceLastEvent <= 0.0f)
            {
                NewEvent();
                onNewEvent.Invoke();
                eventActive = true;
            }
            else
                timeSinceLastEvent -= Time.deltaTime;
        }
    }

    public void SubmitAnswer()
    {
        if (answerInputField.text == correctAnswer.ToString())
        {
            onEventConclusion.Invoke(currentEvent, true);
            DisplayResult(true);
        }
        else
            Debug.Log("Incorrect!");
    }

    private void DisplayResult(bool correct)
    {
        eventActive = false;
        timeSinceLastEvent = float.MaxValue;

        for (int i = 0; i < currentEvent.effects.Length; i++)
        {
            float effect = currentEvent.effects[i];
            TextMeshProUGUI effectText = effectTexts[i];

            if (effect != 0.0f)
            {
                effect *= correct ? 1.0f : -1.0f;

                effectText.text = effectText.name + ": " + (effect > 0.0f ? "+" : "") + effect * 100 + "%";
                effectText.gameObject.SetActive(true);
            }
            else
                effectText.gameObject.SetActive(false);
        }

        timeText.gameObject.SetActive(false);
        problemPanel.SetActive(false);

        resultText.gameObject.SetActive(true);
        resultText.text = "Due to " + (correct ? "good " : "bad ") + currentEvent.type.ToLower() + ", the following effects are applied until the next event:";

        Invoke("CloseEvent", 3.0f);
    }

    private void CloseEvent()
    {
        resultText.gameObject.SetActive(false);
        timeText.gameObject.SetActive(true);
        problemPanel.SetActive(true);
        answerInputField.text = string.Empty;
        eventPanel.SetActive(false);
        timeSinceLastEvent = Random.Range(minTimeBetweenEvents, maxTimeBetweenEvents);
    }

    private void NewEvent()
    {
        currentEvent = events[Random.Range(0, events.Length)];

        eventNameText.text = currentEvent.type + " " + currentEvent.level;
        timeToAnswer = currentEvent.time;

        FourBasicOperations operation = currentEvent.operationsAllowed[Random.Range(0, currentEvent.operationsAllowed.Length)];

        int minFirstNumber;

        if (currentEvent.firstNumberDigits == 1)
            minFirstNumber = 0;
        else
            minFirstNumber = (int)Mathf.Pow(10, currentEvent.firstNumberDigits - 1);
        
        int maxFirstNumber = (int)Mathf.Pow(10, currentEvent.firstNumberDigits);
        int firstNumber = Random.Range(minFirstNumber, maxFirstNumber);

        int minSecondNumber;

        if (currentEvent.secondNumberDigits == 1)
            minSecondNumber = 0;
        else
            minSecondNumber = (int)Mathf.Pow(10, currentEvent.secondNumberDigits - 1);

        int maxSecondNumber = (int)Mathf.Pow(10, currentEvent.secondNumberDigits);
        int secondNumber = Random.Range(minSecondNumber, maxSecondNumber);

        if (currentEvent.negativesAllowed)
        {
            if (Random.Range(0, 2) == 0)
                firstNumber *= -1;

            if (Random.Range(0, 2) == 0)
                secondNumber *= -1;
        }

        switch (operation)
        {
            case FourBasicOperations.Addition:
                correctAnswer = firstNumber + secondNumber;
                operationImage.sprite = operationSpritesDict["Addition"];
                break;
            case FourBasicOperations.Multiplication:
                correctAnswer = firstNumber * secondNumber;
                operationImage.sprite = operationSpritesDict["Multiplication"];
                break;
            case FourBasicOperations.Subtraction:
                if (!currentEvent.negativesAllowed)
                    secondNumber = Random.Range(minSecondNumber, firstNumber + 1);

                correctAnswer = firstNumber - secondNumber;
                operationImage.sprite = operationSpritesDict["Subtraction"];
                break;
            case FourBasicOperations.Division:
                if (firstNumber != 0)
                {
                    List<int> factors = FindFactors(firstNumber);

                    List<int> priorityFactors = factors.Where(factor => factor.ToString().Length == currentEvent.secondNumberDigits).ToList();
                    List<int> vipFactors = priorityFactors.Where(factor => factor != 1 && factor != firstNumber).ToList();

                    if (vipFactors.Count > 0)
                        secondNumber = vipFactors[Random.Range(0, vipFactors.Count)];
                    else if (priorityFactors.Count > 0)
                        secondNumber = priorityFactors[Random.Range(0, priorityFactors.Count)];
                    else
                        secondNumber = factors[Random.Range(0, factors.Count)];
                }

                correctAnswer = firstNumber / secondNumber;
                operationImage.sprite = operationSpritesDict["Division"];
                break;
        }

        firstNumberText.text = firstNumber.ToString();
        secondNumberText.text = secondNumber.ToString();

        eventPanel.SetActive(true);
    }

    private List<int> FindFactors(int n)
    {
        List<int> factors = new List<int>();

        int sqrt = (int)Mathf.Sqrt(n);
        for (int i = 1; i <= sqrt; i++)
        {
            if (n % i == 0)
            {
                factors.Add(i);
                int factor = n / i;
                if (factor != i)
                    factors.Add(factor);
            }
        }

        return factors;
    }
}
