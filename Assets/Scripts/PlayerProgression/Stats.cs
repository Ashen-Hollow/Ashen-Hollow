public static class Stats 
{
    public static int MaxHealth(Attributes attributes) => attributes.vitality * 5;
    public static int AttackDamage(Attributes attributes) => attributes.strength * 2;
    public static int DamageDefense(Attributes attributes) => attributes.defense * 2;
 
}
