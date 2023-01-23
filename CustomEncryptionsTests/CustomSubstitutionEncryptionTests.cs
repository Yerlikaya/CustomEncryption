using Microsoft.VisualStudio.TestTools.UnitTesting;
using CustomEncryptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomEncryptions.Tests
{
    [TestClass()]
    public class CustomSubstitutionEncryptionTests
    {
        [TestMethod()]
        public void GenerateCodesTest()
        {
            CustomSubstitutionEncryption encryption = new CustomSubstitutionEncryption();
            List<string> codes = encryption.GenerateCodes();
            int count = codes.Count;
            codes = codes.Distinct().ToList();
            Assert.IsTrue(codes.Count == count);
        }

        [TestMethod()]
        public void CheckCodeTest()
        {
            CustomSubstitutionEncryption encryption = new CustomSubstitutionEncryption();
            List<string> codes = encryption.GenerateCodes();
            foreach (string code in codes)
            {
                if(encryption.CheckCode(code) == false)
                {
                    Assert.Fail();
                }
            }
            Assert.IsTrue(true);
        }
    }
}