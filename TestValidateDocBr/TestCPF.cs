using ValidateDocBr;

namespace TestValidateDocBr
{
    public class TestCPF
    {
        private readonly CPF _cpf = new();

        [Test]
        public void TestValidateCpfDoc_Success()
        {
            string validDoc = "947.299.670-11";

            Assert.That(_cpf.Validate(validDoc), Is.True);
        }

        [Test]
        public void TestValidateCpfDoc_InvalidDoc()
        {
            string invalidDoc = "123.456.789-10";

            Assert.That(_cpf.Validate(invalidDoc), Is.False);
        }

        [Test]
        public void TestValidateCpfDoc_RepeatedDigit_Invalid()
        {
            string invalidDoc = "111.111.111-11";

            Assert.That(_cpf.Validate(invalidDoc), Is.False);
        }

        [Test]
        public void TestValidateCpfDoc_RepeatedDigit_Valid()
        {
            string invalidDoc = "111.111.111-11";

            CPF cpf = new(true);

            Assert.That(cpf.Validate(invalidDoc), Is.True);
        }

        [Test]
        public void TestMaskCpfDoc_Success()
        {
            string validDoc = "12345678910";

            string maskedValidDoc = _cpf.Mask(validDoc);

            Assert.That(maskedValidDoc, Is.EqualTo("123.456.789-10"));
        }

        [Test]
        public void TestValidateCpfDoc_Success_Complete_Zeros()
        {
            string validDoc = "9235789001";

            Assert.That(_cpf.Validate(validDoc), Is.True);
        }

        [Test]
        public void TestValidateList_Success()
        {
            List<string> validsDoc = ["9235789001", "947.299.670-11"];

            List<bool> valids = _cpf.ValidateList(validsDoc);

            foreach (bool item in valids)
            {
                Assert.That(item, Is.True);
            }
        }

        [Test]
        public void TestGenerateCpfDoc_Without_Mask()
        {
            string validDoc = _cpf.Generate();

            Assert.That(validDoc, Is.Not.Null);
        }

        [Test]
        public void TestGenerateCpfDoc_With_Mask()
        {
            string validDoc = _cpf.Generate(true);

            Assert.That(validDoc, Does.Contain('.').And.Contain('-'));
        }
    }
}
