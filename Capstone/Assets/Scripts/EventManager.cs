// CLEARED

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    [SerializeField] private Event[] events;
    [SerializeField] private Sprite[] operationSprites;

    [SerializeField] private GameObject eventPanel;
    [SerializeField] private TextMeshProUGUI eventNameText;
    [SerializeField] private TextMeshProUGUI timeText;

    [SerializeField] private GameObject problemPanel;
    [SerializeField] private TextMeshProUGUI firstNumberText;
    [SerializeField] private Image operationImage;
    [SerializeField] private TextMeshProUGUI secondNumberText;
    [SerializeField] private TMP_InputField answerInputField;

    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI germAntibodiesAwardedEffectText;
    [SerializeField] private TextMeshProUGUI germDamagePerSecondEffectText;
    [SerializeField] private TextMeshProUGUI germHealthEffectText;
    [SerializeField] private TextMeshProUGUI germSpawnRateEffectText;
    [SerializeField] private TextMeshProUGUI germSpeedEffectText;
    [SerializeField] private TextMeshProUGUI towerAttackRateEffectText;
    [SerializeField] private TextMeshProUGUI towerDamageEffectText;
    [SerializeField] private TextMeshProUGUI towerRangeEffectText;

    [SerializeField] private float minTimeBetweenEvents;
    [SerializeField] private float maxTimeBetweenEvents;

    private Dictionary<string, Sprite> operationSpritesDict = new();

    private Level currentLevel;
    private Event currentEvent;

    private float timeSinceLastEvent = 0.0f;
    private float timeToAnswer = 0.0f;

    private bool eventActive = false;
    private int correctAnswer;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        currentLevel = LevelManager.Instance.currentLevel;

        foreach (Sprite sprite in operationSprites)
            operationSpritesDict.Add(sprite.name, sprite);

        timeSinceLastEvent = Random.Range(minTimeBetweenEvents, maxTimeBetweenEvents);
    }

    // Update is called once per frame
    void Update()
    {
        if (eventActive)
        {
            if (timeToAnswer <= 0.0f)
                HandleEventConclusion(false);
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
                eventActive = true;
            }
            else
                timeSinceLastEvent -= Time.deltaTime;
        }
    }

    public void SubmitAnswer()
    {
        if (answerInputField.text == correctAnswer.ToString())
            HandleEventConclusion(true);
        else
            Debug.Log("Incorrect!");

        // TODO: Incorrect animation
    }

    private void HandleEventConclusion(bool success)
    {
        eventActive = false;
        timeSinceLastEvent = float.MaxValue;

        resultText.text = "Due to " + (success ? currentEvent.successDescription : currentEvent.failureDescription) + ", the following effects have been applied until the next event:";

        Dictionary<string, float> effectsDict = new();

        void AddEffectIfAffecting(bool affecting, string key, TextMeshProUGUI text, bool invert = false)
        {
            if (affecting)
            {
                int temp = (int)(Random.Range(currentLevel.minEffect, currentLevel.maxEffect) * ((invert ? !success : success) ? 1.0f : -1.0f) * 100.0f);
                float effect = temp / 100.0f;
                effectsDict.Add(key, effect);

                text.text = key + ": " + (effect > 0.0f ? "+" : "") + effect * 100 + "%";
                text.gameObject.SetActive(true);
            }
            else
            {
                effectsDict.Add(key, 0.0f);
                text.gameObject.SetActive(false);
            }
        }

        AddEffectIfAffecting(currentEvent.affectGermAntibodiesAwarded, "Germ Antibodies Awarded", germAntibodiesAwardedEffectText);
        AddEffectIfAffecting(currentEvent.affectGermDamagePerSecond, "Germ Damage Per Second", germDamagePerSecondEffectText, true);
        AddEffectIfAffecting(currentEvent.affectGermHealth, "Germ Health", germHealthEffectText, true);
        AddEffectIfAffecting(currentEvent.affectGermSpawnRate, "Germ Spawn Rate", germSpawnRateEffectText, true);
        AddEffectIfAffecting(currentEvent.affectGermSpeed, "Germ Speed", germSpeedEffectText);
        AddEffectIfAffecting(currentEvent.affectTowerDamage, "Tower Damage", towerDamageEffectText);
        AddEffectIfAffecting(currentEvent.affectTowerAttackRate, "Tower Attack Rate", towerAttackRateEffectText);
        AddEffectIfAffecting(currentEvent.affectTowerRange, "Tower Range", towerRangeEffectText);

        EffectsManager.Instance.OnEventConclusion(effectsDict);

        problemPanel.SetActive(false);
        resultText.gameObject.SetActive(true);

        Invoke("CloseEvent", 5.0f);
    }

    private void CloseEvent()
    {
        resultText.gameObject.SetActive(false);

        answerInputField.text = string.Empty;
        problemPanel.SetActive(true);

        eventPanel.SetActive(false);

        timeSinceLastEvent = Random.Range(minTimeBetweenEvents, maxTimeBetweenEvents);
    }

    private void NewEvent()
    {
        EffectsManager.Instance.OnNewEvent();

        currentEvent = events[Random.Range(0, events.Length)];

        eventNameText.text = currentEvent.name;
        timeToAnswer = currentLevel.time;

        FourBasicOperations operation = currentLevel.operationsAllowed[Random.Range(0, currentLevel.operationsAllowed.Length)];

        int minFirstNumber;

        if (currentLevel.firstNumberDigits == 1)
            minFirstNumber = 0;
        else
            minFirstNumber = (int)Mathf.Pow(10, currentLevel.firstNumberDigits - 1);

        int maxFirstNumber = (int)Mathf.Pow(10, currentLevel.firstNumberDigits);
        int firstNumber = Random.Range(minFirstNumber, maxFirstNumber);

        int minSecondNumber;

        if (currentLevel.secondNumberDigits == 1)
            minSecondNumber = 0;
        else
            minSecondNumber = (int)Mathf.Pow(10, currentLevel.secondNumberDigits - 1);

        int maxSecondNumber = (int)Mathf.Pow(10, currentLevel.secondNumberDigits);
        int secondNumber = Random.Range(minSecondNumber, maxSecondNumber);

        if (currentLevel.useNegatives)
        {
            if (Random.Range(0, 2) == 0)
            {
                firstNumber *= -1;

                if (Random.Range(0, 2) == 0)
                    secondNumber *= -1;
            }
            else
            {
                secondNumber *= -1;

                if (Random.Range(0, 2) == 0)
                    firstNumber *= -1;
            }
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
                if (!currentLevel.useNegatives && firstNumber < secondNumber)
                    secondNumber = Random.Range(minSecondNumber, firstNumber + 1);

                correctAnswer = firstNumber - secondNumber;
                operationImage.sprite = operationSpritesDict["Subtraction"];
                break;
            case FourBasicOperations.Division:
                if (firstNumber != 0)
                {
                    List<int> factors = FindFactors(firstNumber);

                    List<int> priorityFactors = factors.Where(factor => factor.ToString().Length == currentLevel.secondNumberDigits).ToList();
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
