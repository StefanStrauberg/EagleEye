using System;
using System.Net;
using System.Threading.Tasks;
using FGLogDog.FGLogDog.Application.Helper;

namespace FGLogDog.Application.Contracts
{
    public interface IUdpServer : IDisposable
    {
        Task Start(IPAddress iPAddress, int port, ParserDelegate parse);
    }
}