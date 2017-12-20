using System;
using System.IO;
using System.Text;
using _1Lab;

namespace _2Lab
{
    public class TripleDes : BaseDes, ICipher
    {
        public sealed override int SizeOfBlock { get; set; }
        private const string Key = "akgsfqwgjyfgbhjsbmnasbfwqjk";

        public TripleDes()
        {
            SizeOfBlock = 64;
        }

        public string Encryption(string text)
        {
            var key1 = Key.Substring(0, Key.Length * 2 / 3);
            var key2 = Key?.Substring(Key.Length * 1 / 3, Key.Length * 1 / 3);
            var key3 = Key?.Substring(Key.Length * 2 / 3, Key.Length * 1 / 3);
            var it1 = Encript(text, key1);
            it1 = Encript(it1, key2);
            it1 = Encript(it1, key3);
            return it1;
        }

        public string Decryption(string text)
        {
            var key1 = Key?.Substring(0, Key.Length * 2 / 3);
            var key2 = Key?.Substring(Key.Length * 1 / 3, Key.Length * 1 / 3);
            var key3 = Key?.Substring(Key.Length * 2 / 3, Key.Length * 1 / 3);
            var it1 = Decript(text, key3);
            it1 = Decript(it1, key2);
            it1 = Decript(it1, key1);
            return it1;
        }
    }
}
