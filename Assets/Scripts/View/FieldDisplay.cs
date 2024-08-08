using UnityEngine;
using TEDinc.SnakeTA.Logic;

namespace TEDinc.SnakeTA.View
{
    internal sealed class FieldDisplay : MonoBehaviour
    {
        [SerializeField] private Transform _root;
        [SerializeField] private ItemPool<FieldDisplayCell> _cellsPool;
        [SerializeField] private FieldDisplaySnakeHead _snakeHead;
        private FieldService _fieldService;

        private Field Field => _fieldService.Field;

        private void Awake()
        {
            AllServices.Get(out _fieldService);
            _fieldService.OnStart += SetupField;
        }

        private void Update()
        {
            foreach (FieldDisplayCell cell in _cellsPool.EnumerateActiveItems())
                cell.Tick();
            _snakeHead.Tick();
        }

        private void OnDestroy() => 
            _fieldService.OnStart -= SetupField;

        private void SetupField()
        {
            for (int x = 0; x < Field.Size.x; x++)
                for (int y = 0; y < Field.Size.y; y++)
                    _cellsPool.Next().Setup(new (x, y), Field);
            _cellsPool.FinishIterating();
            _snakeHead.Setup(_fieldService.Snake);
            _root.localPosition = (Field.Size - Vector2.one) * -0.5f;
        }
    }
}
