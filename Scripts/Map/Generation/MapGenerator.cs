namespace BitRoguelike.Scripts.Map.Generation
{
    public class MapGenerator
    {
        private int mapWidth;
        private int mapHeight;

        public MapGenerator(int mapWidth, int mapHeight)
        {
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
            GenerateMap();
        }

        private void GenerateMap()
        {
        }
    }
}