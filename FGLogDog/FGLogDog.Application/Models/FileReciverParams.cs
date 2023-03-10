using FGLogDog.FGLogDog.Application.Helper;

namespace FGLogDog.Application.Models
{
    public class FileReciverParams : Parameters
    {
        private readonly string _path;

        public FileReciverParams(string configuration, ParserDelegate parser)
            : base(parser)
        {
            _path = "Hello World!";
        }

        public string path { get => _path; }
    }
}