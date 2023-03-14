using FGLogDog.Application.Features.Commands;
using MediatR;
using MongoDB.Bson;
using System.Threading;
using System.Threading.Tasks;
using Buffer = FGLogDog.Application.Models.Buffer;

namespace FGLogDog.Application.Features.Handlers
{
    internal class GetMessageCommandHandler : IRequestHandler<GetMessageCommand, BsonDocument>
    {
        async Task<BsonDocument> IRequestHandler<GetMessageCommand, BsonDocument>.Handle(GetMessageCommand request, CancellationToken cancellationToken)
        {
            BsonDocument result;

            while(!Buffer.buffer.IsCompleted)
            {
                if (Buffer.buffer.TryTake(out result))
                    return await Task.FromResult(result);
            }
            
            throw new System.Exception("Somthing get wrong witb Cuncurrect collection.");
        }
    }
}