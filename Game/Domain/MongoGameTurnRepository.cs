using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace Game.Domain
{
    public class MongoGameTurnRepository : IGameTurnRepository
    {
        public const string CollectionName = "turns";

        private readonly IMongoCollection<GameTurnEntity> turnsCollection;

        public MongoGameTurnRepository(IMongoDatabase db)
        {
            turnsCollection = db.GetCollection<GameTurnEntity>(CollectionName);
            turnsCollection.Indexes.CreateOne(
                new CreateIndexModel<GameTurnEntity>(
                    Builders<GameTurnEntity>.IndexKeys
                        .Ascending(t => t.GameId).Ascending(t => t.TurnIndex))
            );
        }

        public GameTurnEntity Insert(GameTurnEntity gameTurnEntity)
        {
            turnsCollection.InsertOne(gameTurnEntity);
            return gameTurnEntity;
        }

        public IList<GameTurnEntity> GetLast(Guid gameId, int limit)
        {
            var result =  turnsCollection.Find(t => t.GameId == gameId)
                .SortByDescending(t => t.TurnIndex)
                .Limit(limit)
                .ToList();
            result.Reverse();
            return result;
        }
    }
}