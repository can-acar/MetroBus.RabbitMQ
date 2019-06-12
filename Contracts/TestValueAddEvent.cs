using System.Collections.Generic;

namespace Contracts
{
    public class TestValueAddEvent
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public Dictionary<string, string> EventKeys { get; set; }
    }
}