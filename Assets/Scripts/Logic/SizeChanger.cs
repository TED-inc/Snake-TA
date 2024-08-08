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
}
