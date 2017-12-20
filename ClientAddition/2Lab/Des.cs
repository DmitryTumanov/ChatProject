using System;
using System.IO;
using System.Linq;
using System.Text;
using _1Lab;

namespace _2Lab
{
    public class DesCipher : BaseDes, ICipher
    {
        public sealed override int SizeOfBlock { get; set; }
        private const string Key = "akgsfqwgjyfgbhjsbmnasbfwqjk";

        public DesCipher()
        {
            SizeOfBlock = 64;
        }

        public string Encryption(string text)
        {
            return Encript(text, Key);
        }
        
        public string Decryption(string text)
        {
            return Decript(text, Key);
        }
    }
}