namespace SpaceInvaders.Classes
{
    public enum Flags {
        FORCE_BOSS = 0x00000001,
        INVINCIBLE = 0x00000010,
        SUPER_SPEED = 0x00000100,
        FAST_ATTACK = 0x00001000
    }
    public enum State
    {
        Menu,
        Wave,
        BossAlert,
        BossWave,
        Win,
        Loss
    }
    public enum SFX
    {
        Click,
        Explosion,
        Hover,
        Pickup,
        Shoot
    }
    public enum PickUpType
    {
        FastShoot,
        BonusPoints,
        BonusHealth,
        BonusShield
    }
    public enum Direction
    {
        Up = -1,
        Down = 1,
        Right = 1,
        Left = -1
    }
    public enum Faction
    {
        Player,
        Enemy
    }
    public struct Position(double x, double y)
    {
        public double x = x, y = y;
    }
    public struct Movement
    {
        public double x, y;
    }
    public struct Velocity
    {
        public double x, y;
    }
    public struct Dimensions(float width, float height)
    {
        public float width = width, height = height;
    }
}
