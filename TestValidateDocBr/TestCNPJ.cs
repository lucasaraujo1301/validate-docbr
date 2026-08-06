using ValidateDocBr;

namespace TestValidateDocBr
{
    public class TestCNPJ
    {
        private readonly CNPJ _cnpj = new();

        [Test]
        public void TestValidateCNPJ_Success()
        {
            Assert.That(_cnpj.Validate("37.124.385/0001-09"), Is.True);
        }

        [TestCase("37124385000109")]
        [TestCase("37.124.385/0001-09")]
        public void TestValidateCNPJ_Formatted_And_Unformatted(string cnpj)
        {
            Assert.That(_cnpj.Validate(cnpj), Is.True);
        }

        [TestCase("12.ABC.345/01DE-35")]
        [TestCase("12.abc.345/01de-35")]
        public void TestValidateCNPJ_Alphanumeric_Success(string cnpj)
        {
            Assert.That(_cnpj.Validate(cnpj), Is.True);
        }

        [Test]
        public void TestValidateCNPJ_Alphanumeric_Invalid_Check_Digits()
        {
            Assert.That(_cnpj.Validate("12.ABC.345/01DE-36"), Is.False);
        }

        [TestCase("12.ABC.345/01DE-3A")]
        [TestCase("12.ABC.345/01DE-A5")]
        public void TestValidateCNPJ_Alphanumeric_Check_Digits_Must_Be_Numeric(string cnpj)
        {
            Assert.That(_cnpj.Validate(cnpj), Is.False);
        }

        [Test]
        public void TestValidateCNPJ_Unsuccess_Invalid_Doc()
        {
            Assert.That(_cnpj.Validate("37.124.385/0001-11"), Is.False);
        }

        [Test]
        public void TestValidateCNPJ_Unsuccess_Doc_Length_Different_14()
        {
            Assert.That(_cnpj.Validate("37.124.385/0001-091"), Is.False);
        }

        [Test]
        public void TestValidateCNPJ_Unsuccess_Invalid_Char()
        {
            Assert.That(_cnpj.Validate("37.124.385@0001-09"), Is.False);
        }

        [Test]
        public void TestMaskCNPJ_Success()
        {
            string cnpj = "12345678000134";

            Assert.That(_cnpj.Mask(cnpj), Is.EqualTo("12.345.678/0001-34"));
        }

        [Test]
        public void TestMaskCNPJ_Unsuccess()
        {
            string cnpj = "1234567800013";

            ArgumentException ex = Assert.Throws<ArgumentException>(() => _cnpj.Mask(cnpj));

            Assert.That(ex.Message, Is.EqualTo("The length must be 14 for this document"));
        }

        [Test]
        public void TestGenerateCNPJ_Only_Digits_Success()
        {
            string cnpj = _cnpj.Generate();

            Assert.That(cnpj, Is.Not.Null);
            Assert.That(_cnpj.Validate(cnpj), Is.True);
        }

        [Test]
        public void TestGenerateCNPJ_Letters_And_Digits_Success()
        {
            string cnpj = _cnpj.Generate(digitOnly: false);

            Assert.That(cnpj, Is.Not.Null);
            Assert.That(_cnpj.Validate(cnpj), Is.True);
        }

        [Test]
        public void TestGenerateCNPJ_With_Mask_Success()
        {
            string cnpj = _cnpj.Generate(mask: true);

            Assert.That(cnpj, Does.Contain('.').And.Contain('/').And.Contain('-'));
            Assert.That(_cnpj.Validate(cnpj), Is.True);
        }
    }
}
