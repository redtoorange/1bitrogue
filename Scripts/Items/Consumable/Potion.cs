namespace BitRoguelike.Scripts.Items.Consumable
{
    public class Potion : Item, IConsumable
    {
        public string GetConsumeText()
        {
            return "Drink";
        }
    }
}