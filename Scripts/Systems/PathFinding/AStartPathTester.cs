using System.Collections.Generic;
using Godot;

namespace BitRoguelike.Scripts.Systems.PathFinding
{
    public class AStartPathTester : Node2D
    {
        [Export] private NodePath mapControllerPath;
        private MapController mapController;

        private Vector2 startClick;
        private Vector2 endClick;
        private List<Vector2> currentPath = new List<Vector2>();

        public override void _Ready()
        {
            mapController = GetNode<MapController>(mapControllerPath);
        }
        
        public override void _Process(float delta)
        {
            if (Input.IsActionJustPressed("LeftClick"))
            {
                startClick = GetGlobalMousePosition();
            }
            
            if (Input.IsActionPressed("RightClick"))
            {
                endClick = GetGlobalMousePosition();
                UpdatePath();
            }
        }

        private void UpdatePath()
        {
            uint start = OS.GetTicksMsec();
            currentPath = mapController.GeneratePath(startClick, endClick);
            GD.Print("Took " + (OS.GetTicksMsec() - start));
            Update();
        }

        public override void _Draw()
        {
            if (currentPath != null && currentPath.Count > 1)
            {
                for (int i = 0; i < currentPath.Count - 1; i++)
                {
                    DrawLine(
                        currentPath[i] + new Vector2(8, 8), 
                        currentPath[i+1] + new Vector2(8, 8), 
                        Colors.Red,
                        3f
                        );
                }
            }
        }
    }
}