using BMMCompiler.Parts;
namespace compileTester
{
    [TestClass]
    public class FunctionTest
    {
        [TestMethod]
        public void Name()
        {
            Module m = new("func BrokeGoogle(){}");
            Assert.AreEqual(1,m.Functions.Count,"�֐����˂���");
            Assert.AreEqual("BrokeGoogle", m.Functions[0].name, "�֐����s��v����");
            Assert.AreEqual(false, m.Functions[0].isVoid, "�߂�l�s��v����");
        }
        [TestMethod]
        public void Argument()
        {
            Module m = new("func BrokeGoogle(Count,isFill){}");
            Assert.AreEqual(1, m.Functions.Count, "�֐����˂���");
            Assert.AreEqual(2, m.Functions[0].arguments.Count, "��������������");
            Assert.AreEqual("Count", m.Functions[0].arguments[0], "��������������[0]");
            Assert.AreEqual("isFill", m.Functions[0].arguments[1], "��������������[1]");
        }
        [TestMethod]
        public void Statements()
        {
            Module m = new("func BrokeGoogle(Count,isFill){\n    Detect();\n    Broke();\n}");
            Assert.AreEqual(1, m.Functions.Count, "�֐����˂���");
            Assert.AreEqual(2, m.Functions[0].statements.Count, "���e���˂���");
            Assert.AreEqual("Detect()", m.Functions[0].statements[0].Compile([], [], []), "���e������������[0]");
            Assert.AreEqual("Broke()", m.Functions[0].statements[1].Compile([], [], []), "���e������������[1]");
        }
    }
}