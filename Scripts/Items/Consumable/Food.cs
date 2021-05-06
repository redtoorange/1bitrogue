namespace BitRoguelike.Scripts.Items.Consumable
{
    public class Food : Item, IConsumable
    {
        public string GetConsumeText()
        {
            return "Eat";
        }
    }
}