using BMMCompiler.Token;
namespace compileTester
{
    [TestClass]
    public class CompilerTest
    {
        [TestMethod]
        public void FunctionTest()
        {
            Module m = new("//func aaaa(){}\nvoid func(){//Sample}^o^\nwe}");
            
            Assert.AreEqual(1,m.Functions.Count,"ŠÖ”‚ª‚Ë‚¦‚¨");
            Assert.AreEqual("void func(){we}", m.Functions[0].Name, "ŠÖ”•sˆê’v‚¾‚¨");
        }
    }
}