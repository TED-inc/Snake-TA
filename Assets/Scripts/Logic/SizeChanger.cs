namespace TEDinc.SnakeTA.Logic
{
    public sealed class SizeChanger : ICellable
    {
        public readonly int Value;

        public SizeChanger(int value) => 
            Value = value;

        IFieldAction ICellable.Tick(float deltaTime) =>
            null;
    }

    public sealed class SpeedChanger : ICellable
    {
        public readonly float Multiplicator;
        public readonly float Duration;

        public SpeedChanger(float multiplicator, float duration)
        {
            Multiplicator = multiplicator;
            Duration = duration;
        }

        IFieldAction ICellable.Tick(float deltaTime) =>
            null;
    }
}
