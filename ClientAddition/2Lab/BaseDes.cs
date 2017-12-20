using System;
using System.Linq;

namespace _2Lab
{
    public class BaseDes
    {
        public virtual int SizeOfBlock { get; set; }
        protected const int SizeOfChar = 16;
        protected const int ShiftKey = 2;
        protected const int QuantityOfRounds = 16;
        protected string[] Blocks;

        protected string Encript(string s, string key)
        {
            s = StringToRightLength(s);
            CutStringIntoBlocks(s);
            key = CorrectKeyWord(key, s.Length / (2 * Blocks.Length));
            key = StringToBinaryFormat(key);
            for (var j = 0; j < QuantityOfRounds; j++)
            {
                for (var i = 0; i < Blocks.Length; i++)
                    Blocks[i] = EncodeDES_One_Round(Blocks[i], key);
                key = KeyToNextRound(key);
            }
            return StringFromBinaryToNormalFormat(Blocks.Aggregate("", (current, t) => current + t));
        }

        protected string Decript(string s, string key)
        {
            key = CorrectKeyWord(key, s.Length / (2 * s.Length * SizeOfChar / SizeOfBlock));
            key = StringToBinaryFormat(key);
            for (var j = 0; j < QuantityOfRounds; j++)
                key = KeyToNextRound(key);
            key = KeyToPrevRound(key);
            s = StringToBinaryFormat(s);
            CutBinaryStringIntoBlocks(s);
            for (var j = 0; j < QuantityOfRounds; j++)
            {
                for (var i = 0; i < Blocks.Length; i++)
                    Blocks[i] = DecodeDES_One_Round(Blocks[i], key);
                key = KeyToPrevRound(key);
            }
            return StringFromBinaryToNormalFormat(Blocks.Aggregate("", (current, t) => current + t));
        }

        protected string StringToRightLength(string input)
        {
            while (input.Length * SizeOfChar % SizeOfBlock != 0)
                input += " ";
            return input;
        }

        protected void CutStringIntoBlocks(string input)
        {
            Blocks = new string[input.Length * SizeOfChar / SizeOfBlock];
            var lengthOfBlock = input.Length / Blocks.Length;
            for (var i = 0; i < Blocks.Length; i++)
            {
                Blocks[i] = input.Substring(i * lengthOfBlock, lengthOfBlock);
                Blocks[i] = StringToBinaryFormat(Blocks[i]);
            }
        }

        protected void CutBinaryStringIntoBlocks(string input)
        {
            Blocks = new string[input.Length / SizeOfBlock];
            var lengthOfBlock = input.Length / Blocks.Length;
            for (var i = 0; i < Blocks.Length; i++)
                Blocks[i] = input.Substring(i * lengthOfBlock, lengthOfBlock);
        }

        protected static string StringToBinaryFormat(string input)
        {
            var output = "";
            foreach (var t in input)
            {
                var charBinary = Convert.ToString(t, 2);

                while (charBinary.Length < SizeOfChar)
                    charBinary = "0" + charBinary;

                output += charBinary;
            }
            return output;
        }

        protected static string CorrectKeyWord(string input, int lengthKey)
        {
            if (input.Length > lengthKey)
                input = input.Substring(0, lengthKey);
            else
                while (input.Length < lengthKey)
                    input = "0" + input;
            return input;
        }

        protected string EncodeDES_One_Round(string input, string key)
        {
            var l = input.Substring(0, input.Length / 2);
            var r = input.Substring(input.Length / 2, input.Length / 2);
            return r + Xor(l, F(r, key));
        }

        protected string DecodeDES_One_Round(string input, string key)
        {
            var l = input.Substring(0, input.Length / 2);
            var r = input.Substring(input.Length / 2, input.Length / 2);
            return Xor(F(l, key), r) + l;
        }

        protected static string Xor(string s1, string s2)
        {
            var result = "";

            for (var i = 0; i < s1.Length; i++)
            {
                var a = Convert.ToBoolean(Convert.ToInt32(s1[i].ToString()));
                var b = Convert.ToBoolean(Convert.ToInt32(s2[i].ToString()));

                if (a ^ b)
                    result += "1";
                else
                    result += "0";
            }
            return result;
        }

        protected static string F(string s1, string s2)
        {
            return Xor(s1, s2);
        }

        protected static string KeyToNextRound(string key)
        {
            for (var i = 0; i < ShiftKey; i++)
            {
                key = key[key.Length - 1] + key;
                key = key.Remove(key.Length - 1);
            }
            return key;
        }

        protected static string KeyToPrevRound(string key)
        {
            for (var i = 0; i < ShiftKey; i++)
            {
                key = key + key[0];
                key = key.Remove(0, 1);
            }

            return key;
        }

        protected static string StringFromBinaryToNormalFormat(string input)
        {
            var output = "";
            while (input.Length > 0)
            {
                var charBinary = input.Substring(0, SizeOfChar);
                input = input.Remove(0, SizeOfChar);

                var degree = charBinary.Length - 1;

                var a = charBinary.Sum(c => Convert.ToInt32(c.ToString()) * (int)Math.Pow(2, degree--));

                output += ((char)a).ToString();
            }
            return output;
        }
    }
}
