public partial class Card
{
    public enum Type
    {
        Curse = 1,
        Damage = 2,
        Equip = 3,
        Event = 4,
        None = 0,
        Spell = 5
    }

    public enum Rarity
    {
        Common = 1,
        None = 0,
        Legendary = 3,
        Rare = 2
    }
}
