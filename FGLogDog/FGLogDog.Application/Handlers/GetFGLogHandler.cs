using System.Threading;
using System.Threading.Tasks;
using FGLogDog.Application.DataStore;
using FGLogDog.Application.Queries;
using MediatR;

namespace FGLogDog.Application.Handlers
{
    public class GetFGLogHandler : IRequestHandler<GetFGLogQuery, string>
    {
        private FakeDataStore _fakeData;

        public GetFGLogHandler(FakeDataStore fakeData)
            => _fakeData = fakeData;

        public async Task<string> Handle(GetFGLogQuery request,
                                         CancellationToken cancellationToken)
            => await _fakeData.GetLog();
    }
}