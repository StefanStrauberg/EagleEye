using System.Net;
using System;
using System.Threading.Tasks;
using FGLogDog.FGLogDog.Application.Helper;

namespace FGLogDog.Application.Contracts
{
    public interface ITcpServer : IDisposable
    {
        Task Start(IPAddress iPAddress, int port, ParserDelegate parse);
    }
}