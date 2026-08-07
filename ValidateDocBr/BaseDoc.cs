namespace ValidateDocBr
{
    public abstract class BaseDoc
    {
        public readonly List<int> Digits = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
        public abstract bool Validate(string doc = "");
        public abstract string Generate(bool mask = false, bool digitOnly = true);
        public abstract string Mask(ReadOnlySpan<char> doc);

        public List<bool> ValidateList(List<string> docList)
        {
            List<bool> results = new(docList.Count);

            foreach (string doc in docList)
            {
                results.Add(Validate(doc));
            }

            return results;
        }

        protected string OnlyDigits(string doc = "")
        {
            return string.Join("", doc.Where(char.IsDigit));
        }

        protected string OnlyDigitsAndLetters(string doc = "")
        {
            return string.Join("", doc.Where(char.IsLetterOrDigit));
        }

        /// <summary>
        /// This method will validate the input to check if is an valid document.
        /// </summary>
        /// <param name="input">the document number</param>
        /// <param name="valid_characters">the valid special characters</param>
        /// <param name="allowLetters">if the document can have letters or not</param>
        /// <returns>true if the document is valid, otherwise false</returns>
        protected bool ValidateInput(string input, List<char>? valid_characters = null, bool allowLetters = false)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            valid_characters ??= ['.', '-', '/', ' '];

            foreach (char c in input)
            {
                if (char.IsDigit(c))
                {
                    continue;
                }

                if (allowLetters && char.IsLetter(c))
                {
                    continue;
                }

                if (valid_characters.Contains(c))
                {
                    continue;
                }

                return false;
            }

            return true;
        } 
    } 
    
}
