using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class House : MonoBehaviour
{
    [SerializeField] private Color taxableColor;
    [SerializeField] private Color notTaxableColor;
    
    [HideInInspector] public int numAdults = 0;
    [HideInInspector] public int numChildren = 0;
    [HideInInspector] public bool isMarried = false;

    public DateTime nextTaxDate { get; private set; }
    private SpriteRenderer spriteRenderer;
    private bool isTaxable = true;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = notTaxableColor;
        nextTaxDate = DateManager.instance.date.AddMonths(1);
    }

    public void MakeTaxable()
    {
        spriteRenderer.color = taxableColor;
        isTaxable = true;
    }

    private void OnMouseDown()
    {
        Debug.Log($"This house is home to {numAdults} adults and {numChildren} children.");

        if (!isTaxable)
            return;

        BalanceManager.instance.EditBalanceBy(numAdults * 100f + numChildren * 50f);
        nextTaxDate = DateManager.instance.date.AddMonths(1);
        
        spriteRenderer.color = notTaxableColor;
        isTaxable = false;
    }
}
