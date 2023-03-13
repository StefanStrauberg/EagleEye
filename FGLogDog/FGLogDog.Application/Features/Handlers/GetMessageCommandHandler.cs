using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using FGLogDog.Application.Features.Commands;
using MediatR;
using Buffer = FGLogDog.Application.Models.Buffer;

namespace FGLogDog.Application.Features.Handlers
{
    public class GetMessageCommandHandler : IRequestHandler<GetMessageCommand, JsonObject>
    {
        async Task<JsonObject> IRequestHandler<GetMessageCommand, JsonObject>.Handle(GetMessageCommand request, CancellationToken cancellationToken)
        {
            JsonObject result;
            while(!Buffer.buffer.IsCompleted)
            {
                if (Buffer.buffer.TryTake(out result))
                    return await Task.FromResult(result);
            }
            throw new System.Exception("Somthing get wrong witb Cuncurrect collection.");
        }
    }
}