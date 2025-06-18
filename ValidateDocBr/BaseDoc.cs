namespace ValidateDocBr
{
    public abstract class BaseDoc
    {
        public readonly List<int> Digits = [.. Enumerable.Range(0, 10)];
        public abstract bool Validate(string doc = "");
        public abstract string Generate(bool mask = false, bool digitOnly = true);
        public abstract string Mask(string doc = "");

        public List<bool> ValidateList(List<string> docList)
        {
            return [.. docList.Select(doc => Validate(doc))];
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

            HashSet<char> setValidCharacters = [.. valid_characters];
            HashSet<char> setNonPermittedCharacters = [];

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

                if (setValidCharacters.Contains(c))
                {
                    continue;
                }

                setNonPermittedCharacters.Add(c);
            }

            return setNonPermittedCharacters.Count == 0;
        } 
    } 
    
}