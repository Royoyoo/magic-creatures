using System;

[Serializable]
public class CreatureInfo
{
    public string Id;
    public int Level;
    public int CardsCount;
    public StatType statType;

    public int CardsToLevelUp => (Level + 2) * 2;
    public int StatBonus => Level * 3;
}
