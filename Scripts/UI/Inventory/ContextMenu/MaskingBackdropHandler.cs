using Godot;

namespace GameboyRoguelike.Scripts.UI.Inventory.ContextMenu
{
    public class MaskingBackdropHandler : ColorRect
    {
        private static readonly string AREA = "area";

        private ShaderMaterial shaderMaterial;

        public override void _Ready()
        {
            shaderMaterial = Material as ShaderMaterial;
        }

        public void SetMaskLocation(Vector2 location)
        {
            shaderMaterial.SetShaderParam(AREA, location);
        }
    }
}