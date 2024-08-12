using System;
using UnityEngine;

namespace TEDinc.SnakeTA.Logic
{
    public sealed class FieldScoreService : IDisposable
    {
        public event Action ScoreChanged;
        public int Score 
        { 
            get => _score; 
            private set 
            {
                if (value < 0 || _score == value)
                    return;
                _score = value;
                ScoreChanged?.Invoke();
            } 
        }
        private int _score;
        private readonly FieldService _fieldService;

        public FieldScoreService()
        {
            AllServices.Get(out _fieldService);
            _fieldService.OnStart += ResetScore;
            _fieldService.Field.CellSet += TryIncreaseScore;
        }

        private void ResetScore() => 
            Score = 0;

        private void TryIncreaseScore(Vector2Int pos, ICellable removed, ICellable added)
        {
            if (removed is not null && FieldUtils.IsEatable(removed) && added is Snake)
                Score++;
        }

        public void Dispose()
        {
            _fieldService.OnStart -= ResetScore;
            _fieldService.Field.CellSet -= TryIncreaseScore;
        }
    }
}
