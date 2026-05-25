using System.IO;

[System.Serializable]

public class Attributes 
{
    public int strength = 10;
    public int defense = 10;
    public int vitality = 10;

    public Attributes Clone()
    {
        return new Attributes
        {            
            strength = this.strength,
            defense = this.defense,
            vitality = this.vitality,

        };
    }

    public int Get(AttributeType type)
    {
        return type switch
        {
            AttributeType.Strength => strength,
            AttributeType.Defense => defense,
            AttributeType.Vitality => vitality,
            _ => 0
        };
    }

    public void Set (AttributeType type, int value)
    {
        switch (type)
        {
            case AttributeType.Strength : strength = value; break;
            case AttributeType.Defense : defense = value; break;
            case AttributeType.Vitality : vitality = value; break;
        }
    }

    public enum AttributeType {Strength, Defense, Vitality}
}
