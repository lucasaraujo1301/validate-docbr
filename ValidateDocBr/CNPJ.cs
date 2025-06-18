namespace ValidateDocBr
{
    public class CNPJ : BaseDoc
    {
        public readonly List<int> WeightsFirst;
        public readonly List<int> WeightsSecond;
        public readonly List<char> DigitsAndLetters;
        private readonly Random Random = new();

        public CNPJ()
        {
            // Initiating the List<T> with a capacity.
            WeightsFirst = new(12);
            WeightsSecond = new(13);
            DigitsAndLetters = new(36);
            List<int> secondRange = [.. Enumerable.Range(2, 8).Reverse()];
            IEnumerable<char> asciiUpperCase = Enumerable.Range('A', 'Z' - 'A' + 1).Select(i => (char)i);
            IEnumerable<char> asciiDigits = Enumerable.Range('0', '9' - '0' + 1).Select(i => (char)i);

            // Concat two List<T>
            WeightsFirst.AddRange([.. Enumerable.Range(2, 4).Reverse()]);
            WeightsFirst.AddRange(secondRange);
            WeightsSecond.AddRange([.. Enumerable.Range(2, 5).Reverse()]);
            WeightsSecond.AddRange(secondRange);
            DigitsAndLetters.AddRange(asciiUpperCase);
            DigitsAndLetters.AddRange(asciiDigits);
        }

        public override bool Validate(string doc = "")
        {
            if (!ValidateInput(doc, ['.', '/', '-'], true))
            {
                return false;
            }

            doc = doc.Trim().ToUpper();
            doc = OnlyDigitsAndLetters(doc);

            if (doc.Length != 14)
            {
                return false;
            }

            List<char> charsdigits = [.. doc];

            return GenerateDigit(charsdigits) == charsdigits[12] && GenerateDigit(charsdigits, true) == charsdigits[13];
        }

        protected List<char> GetLettersAndDigits()
        {
            List<char> chars = [];

            for (int i = 0; i < 12; i++)
            {
                int randomIndex = Random.Next(DigitsAndLetters.Count);
                char randomAlphaNum = DigitsAndLetters[randomIndex];

                chars.Add(randomAlphaNum);
            }

            return chars;
        }

        protected List<char> GetDigits()
        {
            List<char> chars = [];

            for (int i = 0; i < 12; i++)
            {
                int randomIndex = Random.Next(Digits.Count);
                int randomDigit = Digits[randomIndex];

                chars.Add((char)('0' + randomDigit));
            }

            return chars;
        }

        public override string Generate(bool mask = false, bool digitOnly = true)
        {
            List<char> cnpjChars = new(14);

            if (digitOnly)
            {
                cnpjChars.AddRange(GetDigits());
            }
            else
            {
                cnpjChars.AddRange(GetLettersAndDigits());
            }
            char firstDigit = GenerateDigit(cnpjChars);
            cnpjChars.Add(firstDigit);
            char secondDigit = GenerateDigit(cnpjChars, true);
            cnpjChars.Add(secondDigit);

            string cnpj = string.Join("", cnpjChars);

            return mask ? Mask(cnpj) : cnpj;
        }

        public override string Mask(string doc = "")
        {
            if (doc.Length != 14)
            {
                throw new ArgumentException("The length must be 14 for this document");
            }
            return $"{doc[..2]}.{doc[2..5]}.{doc[5..8]}/{doc[8..12]}-{doc[^2..]}";
        }

        private char GenerateDigit(List<char> doc, bool isSecondDigit = false)
        {
            int length = isSecondDigit ? 13 : 12;
            List<int> wheights = isSecondDigit ? WeightsSecond : WeightsFirst;

            int sum = 0;

            for (int i = 0; i < length; i++)
            {

                sum += ((int)doc[i] - 48) * wheights[i];
            }

            sum %= 11;

            sum = sum < 2 ? 0 : 11 - sum;

            return (char)('0' + sum);
        }
    }
}