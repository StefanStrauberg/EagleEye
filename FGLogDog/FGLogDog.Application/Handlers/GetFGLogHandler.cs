using System.Threading;
using System.Threading.Tasks;
using FGLogDog.Application.DataStore;
using FGLogDog.Application.Queries;
using MediatR;

namespace FGLogDog.Application.Handlers
{
    public class GetFGLogHandler : IRequestHandler<GetFGLogQuery, Unit>
    {
        private FakeDataStore _fakeData;

        public GetFGLogHandler(FakeDataStore fakeData)
            => _fakeData = fakeData;

        public async Task<Unit> Handle(GetFGLogQuery request,
                                         CancellationToken cancellationToken)
        {
            
            return await Task.FromResult(Unit.Value);
        }
    }
}