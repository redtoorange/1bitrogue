namespace BitRoguelike.Scripts.Items.Consumable
{
    public class Scroll : Item, IConsumable
    {
        public string GetConsumeText()
        {
            return "Read";
        }
    }
}
