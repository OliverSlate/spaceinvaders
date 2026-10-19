using System;

namespace SpaceInvaders.Classes
{
    internal sealed class Explosion : Entity
    {
        private const double LifetimeSeconds = 1.0;
        public int CurrentFrame { get; private set; }
        public double RemainingLifetime { get; private set; } = LifetimeSeconds;

        public Explosion(Damageable source)
        {
            position = source.position;
            dimensions = source.dimensions;

            skinFrames =
            [
                LoadBrush("Resources/Images/explosion_00.png"),
                LoadBrush("Resources/Images/explosion_01.png"),
                LoadBrush("Resources/Images/explosion_02.png"),
                LoadBrush("Resources/Images/explosion_03.png"),
                LoadBrush("Resources/Images/explosion_04.png"),
                LoadBrush("Resources/Images/explosion_05.png"),
                LoadBrush("Resources/Images/explosion_06.png"),
                LoadBrush("Resources/Images/explosion_07.png"),
                LoadBrush("Resources/Images/explosion_08.png"),
                LoadBrush("Resources/Images/explosion_09.png"),
                LoadBrush("Resources/Images/explosion_10.png")
            ];
        }

        public void Update(double deltaTime)
        {
            if (!isAlive)
                return;

            RemainingLifetime = Math.Max(0, RemainingLifetime - deltaTime);

            double elapsedSeconds = LifetimeSeconds - RemainingLifetime;

            CurrentFrame = Math.Min(
                skinFrames.Length - 1,
                (int)(elapsedSeconds / LifetimeSeconds * skinFrames.Length));

            if (RemainingLifetime == 0)
                isAlive = false;
        }

        public override int GetAnimationFrame(int sharedAnimationFrame) => CurrentFrame;

    }
}
