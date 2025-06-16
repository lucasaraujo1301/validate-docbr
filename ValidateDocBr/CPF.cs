namespace ValidateDocBr
{
    public class CPF(bool repeatedDigits = false) : BaseDoc
    {
        public List<int> Digits = Enumerable.Range(0, 10).ToList();

        public bool RepeatedDigits = repeatedDigits;

        private readonly Random Random = new();

        public override bool Validate(string doc = "")
        {
            if (!ValidateInput(doc, ['.', '-']))
            {
                return false;
            }

            doc = OnlyDigits(doc);

            if (doc.Length < 11)
            {
                doc = CompleteWithZero(doc);
            }

            List<char> digits = doc.ToList();

            if (!RepeatedDigits && CheckRepatedDigits(digits))
            {
                return false;
            }

            if (RepeatedDigits && CheckRepatedDigits(digits))
            {
                return true;
            }

            return GenerateDigit(digits) == digits[9] && GenerateDigit(digits, true) == digits[10];
        }

        public override string Generate(bool mask = false)
        {
            List<char> cpfDigits = [];

            for (int i = 0; i < 9; i++)
            {
                int randomIndex = Random.Next(Digits.Count);
                int randomDigit = Digits[randomIndex];

                cpfDigits.Add((char)('0' + randomDigit));
            }

            char firstDigit = GenerateDigit(cpfDigits);
            cpfDigits.Add(firstDigit);
            char secondDigit = GenerateDigit(cpfDigits, true);
            cpfDigits.Add(secondDigit);

            string cpf = string.Join("", cpfDigits);

            if (mask)
            {
                cpf = Mask(cpf);
            }

            return cpf;
        }

        public override string Mask(string doc = "")
        {
            if (doc.Length != 11)
            {
                throw new ArgumentException("The length must be 11 for this document");
            }
            return $"{doc[..3]}.{doc[3..6]}.{doc[6..9]}-{doc[^2..]}";
        }

        private char GenerateDigit(List<char> doc, bool isSecondDigit = false)
        {
            int length = isSecondDigit ? 11 : 10;

            int sum = 0;

            for (int i = length; i > 1; i--)
            {
                int charIndex = length - i;

                string charAsString = doc[charIndex].ToString();

                int digit = int.Parse(charAsString);

                sum += digit * i;
            }

            sum = sum * 10 % 11;

            if (sum == 10)
            {
                sum = 0;
            }

            return (char)('0' + sum);
        }

        private bool CheckRepatedDigits(List<char> doc)
        {
            HashSet<char> digits = [.. doc];

            return digits.Count == 1;
        }

        private string CompleteWithZero(string doc)
        {
            int totalLength = 11;
            return doc.PadLeft(totalLength, '0');
        }
    }
}