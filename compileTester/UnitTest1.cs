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
            
            Assert.AreEqual(1,m.Functions.Count,"�֐����˂���");
            Assert.AreEqual("void func(){we}", m.Functions[0].Name, "�֐��s��v����");
        }
    }
}