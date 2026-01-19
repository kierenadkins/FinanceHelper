namespace ApplicationTest.Tools
{
    using FinanceHelper.Application.Tools;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StringToolsTests
    {
        [TestMethod]
        public void IsValidEmail_ShouldReturnTrue_ForValidEmail()
        {
            var result = StringTools.IsValidEmail("test.user@example.com");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidEmail_ShouldReturnFalse_ForInvalidEmail()
        {
            var result = StringTools.IsValidEmail("invalid-email");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidEmail_ShouldReturnFalse_ForEmptyString()
        {
            var result = StringTools.IsValidEmail("");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidEmail_ShouldReturnFalse_ForNull()
        {
            var result = StringTools.IsValidEmail(null);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidPassword_ShouldReturnTrue_ForValidPassword()
        {
            //  lowercase, uppercase, number, special char, min 8 chars
            var result = StringTools.IsValidPassword("Aa1!aaaa");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidPassword_ShouldReturnFalse_WhenMissingUppercase()
        {
            var result = StringTools.IsValidPassword("aa1!aaaa");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidPassword_ShouldReturnFalse_WhenMissingLowercase()
        {
            var result = StringTools.IsValidPassword("AA1!AAAA");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidPassword_ShouldReturnFalse_WhenMissingNumber()
        {
            var result = StringTools.IsValidPassword("Aa!aaaaa");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidPassword_ShouldReturnFalse_WhenMissingSpecialCharacter()
        {
            var result = StringTools.IsValidPassword("Aa1aaaaa");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidPassword_ShouldReturnFalse_WhenTooShort()
        {
            var result = StringTools.IsValidPassword("Aa1!");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidPassword_ShouldReturnFalse_ForNull()
        {
            var result = StringTools.IsValidPassword(null);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidPassword_ShouldReturnFalse_ForEmptyString()
        {
            var result = StringTools.IsValidPassword("");
            Assert.IsFalse(result);
        }
    }
}

