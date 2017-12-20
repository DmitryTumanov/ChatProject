using System;
using System.Globalization;

namespace ChatClient
{
    public class Message
    {
        public int Id { get; set; }
        public string FirstConnectionId { get; set; }
        public string SecondConnectionId { get; set; }
        public DateTime Date { get; set; }
        public string MessageText { get; set; }

        public string StringDate => Date.ToString(CultureInfo.InvariantCulture);
        public int? CipherType { get; set; }
    }
}