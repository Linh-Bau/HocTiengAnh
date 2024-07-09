using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABI.System;
using HocTiengAnh.Helpers;

namespace TestProject1.Helpers
{
    [TestClass]
    public class StringHelperTests
    {
        [TestMethod]
        public void ConvertStringToEngWord_ReturnAEngWordWhenInputFormatIsCorrect()
        {
            string input = "activity (n)\thoạt động";
            var word = StringHelper.ConvertStringToEngWord(input);
            bool check = word.Word == "activity" &&
               word.Type == "(n)" &&
               word.Meaning == "hoạt động";
            Assert.IsTrue(check);
        }

        [TestMethod]
        public void ConvertStringToEngWord_ThrowExceptionWhenInputFormatIsIncorrect()
        {
            string input = "activity hoạt động";
            Assert.ThrowsException<ArgumentException>(() =>
            {
                StringHelper.ConvertStringToEngWord(input);
            });
        }

    }
}
