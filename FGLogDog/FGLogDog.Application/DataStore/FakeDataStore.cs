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
            var data = File.ReadAllLines("input.log");
            foreach (var item in data)
                _inputLogs.Add(item);
        }

        public async Task<string> GetLog()
            => await Task.FromResult(_inputLogs[_rnd.Next(0,_inputLogs.Count)]);
    }
}