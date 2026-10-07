using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SpaceInvaders.Classes
{
    internal class CanvasRenderer
    {
        private readonly Canvas canvas;
        private readonly Dictionary<Entity, Rectangle> visuals = new();

        public CanvasRenderer(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void Synchronize(GameWorld world, int animationFrame)
        {
            List<Entity> activeEntities = world.GetLiveEntities().ToList();

            foreach (Entity entity in activeEntities)
            {
                if (!visuals.ContainsKey(entity))
                    CreateVisual(entity);

                Rectangle rectangle = visuals[entity];

                Canvas.SetLeft(rectangle, entity.position.X);
                Canvas.SetTop(rectangle, entity.position.Y);
                ImageBrush frame = entity.skinFrames[animationFrame % entity.skinFrames.Length];
                if (!ReferenceEquals(rectangle.Fill, frame))
                    rectangle.Fill = frame;
            }

            List<Entity> removedEntities = visuals.Keys
                .Where(entity => !activeEntities.Contains(entity))
                .ToList();

            foreach (Entity entity in removedEntities)
                RemoveVisual(entity);
        }

        private void CreateVisual(Entity entity)
        {
            if (visuals.ContainsKey(entity))
                return;

            Rectangle rectangle = new Rectangle
            {
                Tag = entity.Id,
                Width = entity.dimensions.width,
                Height = entity.dimensions.height,
                Fill = entity.skinFrames[0]
            };

            Canvas.SetLeft(rectangle, entity.position.X);
            Canvas.SetTop(rectangle, entity.position.Y);

            visuals.Add(entity, rectangle);
            canvas.Children.Add(rectangle);
        }

        private void RemoveVisual(Entity entity)
        {
            if (!visuals.TryGetValue(entity, out Rectangle rectangle))
                return;

            canvas.Children.Remove(rectangle);
            visuals.Remove(entity);
        }

        public void Clear()
        {
            foreach (Rectangle rectangle in visuals.Values)
                canvas.Children.Remove(rectangle);

            visuals.Clear();
        }
    }
}