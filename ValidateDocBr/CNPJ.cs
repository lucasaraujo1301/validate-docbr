namespace ValidateDocBr
{
    public class CNPJ : BaseDoc
    {
        public readonly List<int> WeightsFirst = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        public readonly List<int> WeightsSecond = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        public readonly List<char> DigitsAndLetters =
        [
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        ];
        private readonly Random Random = new();

        public override bool Validate(string doc = "")
        {
            if (string.IsNullOrEmpty(doc))
            {
                return false;
            }

            Span<char> characters = stackalloc char[14];
            int characterCount = 0;

            foreach (char character in doc.Trim())
            {
                if (char.IsLetterOrDigit(character))
                {
                    if (characterCount == characters.Length)
                    {
                        return false;
                    }

                    characters[characterCount++] = char.ToUpperInvariant(character);
                    continue;
                }

                if (character is not ('.' or '/' or '-'))
                {
                    return false;
                }
            }

            if (characterCount != characters.Length)
            {
                return false;
            }

            return GenerateDigit(characters) == characters[12] && GenerateDigit(characters, true) == characters[13];
        }

        public override string Generate(bool mask = false, bool digitOnly = true)
        {
            Span<char> cnpjChars = stackalloc char[14];

            for (int i = 0; i < 12; i++)
            {
                if (digitOnly)
                {
                    int randomIndex = Random.Next(Digits.Count);
                    cnpjChars[i] = (char)('0' + Digits[randomIndex]);
                }
                else
                {
                    int randomIndex = Random.Next(DigitsAndLetters.Count);
                    cnpjChars[i] = DigitsAndLetters[randomIndex];
                }
            }

            cnpjChars[12] = GenerateDigit(cnpjChars);
            cnpjChars[13] = GenerateDigit(cnpjChars, true);

            return mask ? Mask(cnpjChars) : new string(cnpjChars);
        }

        public override string Mask(ReadOnlySpan<char> doc)
        {
            if (doc.Length != 14)
            {
                throw new ArgumentException("The length must be 14 for this document");
            }
            return $"{doc[..2]}.{doc[2..5]}.{doc[5..8]}/{doc[8..12]}-{doc[12..]}";
        }

        private char GenerateDigit(ReadOnlySpan<char> doc, bool isSecondDigit = false)
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
