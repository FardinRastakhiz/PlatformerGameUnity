using TMPro;
using UnityEngine;

public class CharacterSheetManager : MonoBehaviour
{
    public TextMeshProUGUI pointsAvailableText;
    public TextMeshProUGUI strText;
    public TextMeshProUGUI dexText;
    public TextMeshProUGUI intText;
    public int pointsAvailable = 10;
    public int strPoints = 10;
    public int dexPoints = 10;
    public int intPoints = 10;

    private void UpdateTextFields()
    {
        pointsAvailableText.text = pointsAvailable.ToString();
        strText.text = strPoints.ToString();
        dexText.text = dexPoints.ToString();
        intText.text = intPoints.ToString();
    }

    private void AdjustAbilityScore(AbilityScore abilityScore, Operator opr)
    {
        if (opr == Operator.Add)
        {
            if (pointsAvailable <= 0) return;
            switch (abilityScore)
            {
                case AbilityScore.STR:
                    strPoints += 1;
                    break;
                case AbilityScore.DEX:
                    dexPoints += 1;
                    break;
                case AbilityScore.INT:
                    intPoints += 1;
                    break;
            }
            pointsAvailable -= 1;
        }
        else
        {
            switch (abilityScore)
            {
                case AbilityScore.STR:
                    if (strPoints <= 6) return; // Minimum ability score of 6
                    strPoints -= 1;
                    break;
                case AbilityScore.DEX:
                    if (dexPoints <= 6) return; // Minimum ability score of 6
                    dexPoints -= 1;
                    break;
                case AbilityScore.INT:
                    if (intPoints <= 6) return; // Minimum ability score of 6
                    intPoints -= 1;
                    break;
            }
            pointsAvailable += 1;
        }
    }

    public void STRAdd()
    {
        AdjustAbilityScore(AbilityScore.STR, Operator.Add);
        UpdateTextFields();
    }

    public void STRSub()
    {
        AdjustAbilityScore(AbilityScore.STR, Operator.Subtract);
        UpdateTextFields();
    }

    public void DEXAdd()
    {
        AdjustAbilityScore(AbilityScore.DEX, Operator.Add);
        UpdateTextFields();
    }

    public void DEXSub()
    {
        AdjustAbilityScore(AbilityScore.DEX, Operator.Subtract);
        UpdateTextFields();
    }

    public void INTAdd()
    {
        AdjustAbilityScore(AbilityScore.INT, Operator.Add);
        UpdateTextFields();
    }

    public void INTSub()
    {
        AdjustAbilityScore(AbilityScore.INT, Operator.Subtract);
        UpdateTextFields();
    }

    private enum AbilityScore
    {
        STR,
        DEX,
        INT
    }

    private enum Operator
    {
        Add,
        Subtract
    }
}
