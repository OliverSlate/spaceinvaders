using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvaders.Classes
{
    internal class Pickup : Entity
    {
        public PickUpType type;
        public event Action<Pickup> PickedUp;
        private Random random = new();
        public Pickup(double canvasWidth, double canvasHeight)
        {
            double verticalMargin = canvasHeight / 4;
            position.X = random.Next(12, (int)canvasWidth - (int)dimensions.width - 12);
            position.Y = random.Next((int)verticalMargin, (int)canvasHeight - (int)verticalMargin);
            int rnd = random.Next(1, 5);
            switch (rnd)
            {
                case 1:
                    type = PickUpType.FastShoot;
                    skinFrames = new[] 
                    { 
                        LoadBrush("Resources/Images/pickup_shoot_0.png"),
                        LoadBrush("Resources/Images/pickup_shoot_1.png"),
                        LoadBrush("Resources/Images/pickup_shoot_2.png"),
                        LoadBrush("Resources/Images/pickup_shoot_3.png")
                    };
                    break;
                case 2:
                    type = PickUpType.BonusPoints;
                    skinFrames = new[]
                  {
                        LoadBrush("Resources/Images/pickup_points_0.png"),
                        LoadBrush("Resources/Images/pickup_points_1.png")
                   };
                    break;
                case 3:
                    type = PickUpType.BonusHealth;
                    skinFrames = new[]
                   {
                        LoadBrush("Resources/Images/pickup_heart_0.png"),
                        LoadBrush("Resources/Images/pickup_heart_1.png")
                   };
                    break;
                case 4:
                    type = PickUpType.BonusShield;
                    skinFrames = new[]
                  {
                        LoadBrush("Resources/Images/pickup_shield_0.png"),
                        LoadBrush("Resources/Images/pickup_shield_1.png")
                   };
                    break;
                default:
                    break;
            }
        }
        public void PickUp()
        {
            if (!isAlive) return;
            PickedUp?.Invoke(this);
            isAlive = false;
        }
    }
}
