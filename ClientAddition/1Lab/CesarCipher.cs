using System;
using System.IO;
using System.Text;

namespace _1Lab
{
    public class CesarCipher : ICipher
    {
        private string Alph { get; set; }
        private string AlphLower { get; } = "abcdefjhigklmnopqrstuvwxyz";
        private string AlphUpper { get; } = "ABCDEFJHIGKLMNOPQRSTUVWXYZ";
        private string Encrypt(string text, int shift = 3)
        {
            Alph = AlphLower;
            var res = new StringBuilder();
            foreach (var t in text)
            {
                var j = Alph.IndexOf(t);
                if (j < 0)
                {
                    Alph = Alph == AlphLower ? AlphUpper : AlphLower;
                    j = Alph.IndexOf(t);
                }
                res.Append(j >= 0 ? Alph[(j + shift) % Alph.Length] : t);
            }
            return res.ToString();
        }
        private string Decrypt(string text, int shift = 3)
        {
            Alph = AlphLower;
            var res = new StringBuilder();
            foreach (var t in text)
            {
                var j = Alph.IndexOf(t);
                if (j < 0)
                {
                    Alph = Alph == AlphLower ? AlphUpper : AlphLower;
                    j = Alph.IndexOf(t);
                }
                res.Append(j >= 0 ? Alph[(j - shift + Alph.Length) % Alph.Length] : t);
            }
            return res.ToString();
        }

        public string Encryption(string text)
        {
            return Encrypt(text);
        }

        public string Decryption(string text)
        {
            return Decrypt(text);
        }
    }
}
