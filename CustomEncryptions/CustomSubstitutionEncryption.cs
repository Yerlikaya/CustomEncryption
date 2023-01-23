namespace CustomEncryptions
{
    public class CustomSubstitutionEncryption
    {
        private const string _privateKey = "KA2ZENFX";
        private const string _secretKey = "ZPX9K7";
        private const string _charSet = "ACDEFGHKLMNPRTXYZ234579";

        public List<string> GenerateCodes()
        {
            List<string> permutations = new List<string>();
            char[] characters = _secretKey.ToCharArray();
            Permute(characters, 0, _secretKey.Length - 1, permutations);

            List<string> result = new List<string>();

            for (int i = 0; i < permutations.Count; i++)
            {
                int dynamicShiftValue = i % 100;
                string dynamicShiftValueCode = dynamicShiftValue.ToString();
                if (dynamicShiftValueCode.Length == 1)
                {
                    dynamicShiftValueCode = "0" + dynamicShiftValueCode;
                }

                permutations[i] += dynamicShiftValueCode;
                string encryptedCode = "";
                string codeStr = permutations[i];

                for (int y = 0; y < codeStr.Length; y++)
                {
                    if(y >= codeStr.Length - 2)
                    {
                        dynamicShiftValue = 0;
                    }

                    char validCharKey = ReplaceNonExistDigit(codeStr[y]);
                    int codeCharIndex = _charSet.IndexOf(validCharKey);
                    int privateKeyCharIndex = _charSet.IndexOf(_privateKey[y]);

                    int encryptedChValueIndex = (privateKeyCharIndex + codeCharIndex + dynamicShiftValue) % _charSet.Length;
                    encryptedCode += _charSet[encryptedChValueIndex];
                }
                result.Add(encryptedCode);
            }
            return result;
        }
        private static void Permute(char[] arr, int l, int r, List<string> permutations)
        {
            if (l == r)
            {
                permutations.Add(new string(arr));
            }
            else
            {
                for (int i = l; i <= r; i++)
                {
                    Swap(ref arr[l], ref arr[i]);
                    Permute(arr, l + 1, r, permutations);
                    Swap(ref arr[l], ref arr[i]);
                }
            }
        }
        private static void Swap(ref char a, ref char b)
        {
            if (a == b) return;
            a ^= b;
            b ^= a;
            a ^= b;
        }
        private static char ReplaceNonExistDigit(char ch)
        {
            int index = _charSet.IndexOf(ch);
            if (index == -1)
            {
                int chValue = Convert.ToInt32(ch.ToString());
                return _charSet[chValue];
            }
            return ch;
        }
        public bool CheckCode(string code)
        {
            if(code.Length != _privateKey.Length)
            {
                return false;
            }

            int dynamicShiftValue = GetEncryptDynamicShiftValue(code);
            if (dynamicShiftValue == -1)
            {
                return false;
            }

            string orginalCode = "";
            for (int i = 0; i < code.Length-2; i++)
            {
                int privateKeyCharIndex = _charSet.IndexOf(_privateKey[i]);
                int codeKeyCharIndex = _charSet.IndexOf(code[i]);
                int orginalCodeIndex = (codeKeyCharIndex - privateKeyCharIndex - dynamicShiftValue) % _charSet.Length;
                if(orginalCodeIndex < 0)
                {
                    orginalCodeIndex += _charSet.Length;
                }
                char orginalCodeChar = _charSet[orginalCodeIndex];
                //orginalCodeChar = ReplaceNonDigit(orginalCodeChar);
                orginalCode += orginalCodeChar;
            }
            if(orginalCode.Distinct().Count() != 6)
            {
                return false;
            }

            foreach(char ch in orginalCode)
            {
                if (_secretKey.IndexOf(ch) == -1)
                {
                    return false;
                }
            }
            
            return true;
        }

        private static char ReplaceNonDigit(char ch)
        {
            if(int.TryParse(ch.ToString(), out _))
            {
                return ch;
            }

            int digit = _charSet.IndexOf(ch);
            return digit.ToString()[0];
        }

        private static int GetEncryptDynamicShiftValue(string code)
        {
            string shiftValueStr = "";
            for(int i = code.Length - 2; i < code.Length; i++)
            {
                int privateKeyCharIndex = _charSet.IndexOf(_privateKey[i]);
                int codeKeyCharIndex = _charSet.IndexOf(code[i]);
                int shiftCodeIndex = (codeKeyCharIndex - privateKeyCharIndex) % _charSet.Length;
                if (shiftCodeIndex < 0)
                {
                    shiftCodeIndex += _charSet.Length;
                }
                char shiftCodeChar = _charSet[shiftCodeIndex];

                shiftCodeChar = ReplaceNonDigit(shiftCodeChar);

                if (!int.TryParse(shiftCodeChar.ToString(), out _))
                {
                    return -1;
                }
                shiftValueStr += shiftCodeChar;
            }
            return int.Parse(shiftValueStr);
        }
    }
}