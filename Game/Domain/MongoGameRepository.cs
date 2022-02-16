using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace Game.Domain
{
    public class MongoGameRepository : IGameRepository
    {
        public const string CollectionName = "games";
        
        private readonly IMongoCollection<GameEntity> gamesCollection;

        public MongoGameRepository(IMongoDatabase db)
        {
            gamesCollection = db.GetCollection<GameEntity>(CollectionName);
        }

        public GameEntity Insert(GameEntity game)
        {
            gamesCollection.InsertOne(game);
            return game;
        }

        public GameEntity FindById(Guid gameId)
        {
            return gamesCollection.Find(g => g.Id == gameId).SingleOrDefault();
        }

        public void Update(GameEntity game)
        {
            gamesCollection.ReplaceOne(g => g.Id == game.Id, game);
        }

        // Возвращает не более чем limit игр со статусом GameStatus.WaitingToStart
        public IList<GameEntity> FindWaitingToStart(int limit)
        {
            return gamesCollection.Find(g => g.Status == GameStatus.WaitingToStart).Limit(limit).ToList();
        }

        // Обновляет игру, если она находится в статусе GameStatus.WaitingToStart
        public bool TryUpdateWaitingToStart(GameEntity game)
        {
            var result = gamesCollection.ReplaceOne(g => g.Id == game.Id && g.Status == GameStatus.WaitingToStart, game);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}