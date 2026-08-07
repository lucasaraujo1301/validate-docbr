namespace ValidateDocBr
{
    public class CPF(bool repeatedDigits = false) : BaseDoc
    {
        public bool RepeatedDigits = repeatedDigits;

        private readonly Random Random = new();

        public override bool Validate(string doc = "")
        {
            if (string.IsNullOrEmpty(doc))
            {
                return false;
            }

            Span<char> digits = stackalloc char[11];
            int digitCount = 0;

            foreach (char character in doc)
            {
                if (char.IsDigit(character))
                {
                    if (digitCount == digits.Length)
                    {
                        return false;
                    }

                    digits[digitCount++] = character;
                    continue;
                }

                if (character is not ('.' or '-'))
                {
                    return false;
                }
            }

            if (digitCount < digits.Length)
            {
                digits[..digitCount].CopyTo(digits[(digits.Length - digitCount)..]);
                digits[..(digits.Length - digitCount)].Fill('0');
            }

            bool repeatedDigits = CheckRepeatedDigits(digits);

            if (!RepeatedDigits && repeatedDigits)
            {
                return false;
            }

            if (RepeatedDigits && repeatedDigits)
            {
                return true;
            }

            return GenerateDigit(digits) == digits[9] && GenerateDigit(digits, true) == digits[10];
        }

        public override string Generate(bool mask = false, bool digitOnly = true)
        {
            Span<char> cpfDigits = stackalloc char[11];

            for (int i = 0; i < 9; i++)
            {
                int randomIndex = Random.Next(Digits.Count);
                int randomDigit = Digits[randomIndex];

                cpfDigits[i] = (char)('0' + randomDigit);
            }

            cpfDigits[9] = GenerateDigit(cpfDigits);
            cpfDigits[10] = GenerateDigit(cpfDigits, true);

            return mask ? Mask(cpfDigits) : new string(cpfDigits);
        }

        public override string Mask(ReadOnlySpan<char> doc)
        {
            if (doc.Length != 11)
            {
                throw new ArgumentException("The length must be 11 for this document");
            }
            return $"{doc[..3]}.{doc[3..6]}.{doc[6..9]}-{doc[9..]}";
        }

        private static char GenerateDigit(ReadOnlySpan<char> doc, bool isSecondDigit = false)
        {
            int length = isSecondDigit ? 11 : 10;

            int sum = 0;

            for (int i = length; i > 1; i--)
            {
                int charIndex = length - i;

                int digit = doc[charIndex] - '0';

                sum += digit * i;
            }

            sum = sum * 10 % 11;

            if (sum == 10)
            {
                sum = 0;
            }

            return (char)('0' + sum);
        }

        private static bool CheckRepeatedDigits(ReadOnlySpan<char> doc)
        {
            char firstDigit = doc[0];

            foreach (char digit in doc[1..])
            {
                if (digit != firstDigit)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
