using System;
using System.Collections.Generic;

namespace Game.Domain
{
    public interface IGameTurnRepository
    {
        public GameTurnEntity Insert(GameTurnEntity gameTurnEntity);

        public IList<GameTurnEntity> GetLast(Guid gameId, int limit);
    }
}