using FGLogDog.Application.Models;
using FGLogDog.FGLogDog.Application.Helper;

namespace FGLogDog.FGLogDog.Application.Models.ParametersOfReceivers
{
    public class UdpReceiverParams : ReceiverParameters
    {
        public UdpReceiverParams(ParserDelegate parser)
            : base(parser)
        {
        }
    }
}