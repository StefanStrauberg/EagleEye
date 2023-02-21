using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FGLogDog.Application.DataStore
{
    public class FakeDataStore
    {
        private static List<string> _inputLogs;
        private readonly Random _rnd;

        public FakeDataStore()
        {
            _rnd = new Random();
            _inputLogs = new List<string>();
            var dirDatabaseScripts = Path.Combine(Directory.GetCurrentDirectory(), "input.log");
            var data = File.ReadAllLines(dirDatabaseScripts);
            foreach (var item in data)
                _inputLogs.Add(item);
        }

        public async Task<string> GetLog()
            => await Task.FromResult(_inputLogs[_rnd.Next(0,_inputLogs.Count)]);
    }
}