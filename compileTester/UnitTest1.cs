using BMMCompiler.Parts;
using System.Text.RegularExpressions;
namespace compileTester
{
    [TestClass]
    public class FunctionTest
    {
        [TestMethod]
        public void Name()
        {
            Module m = new("func BrokeGoogle(){}");
            Assert.AreEqual(1,m.Functions.Count,"�֐����Ȃ�");
            Assert.AreEqual("BrokeGoogle", m.Functions[0].name, "�֐����s��v");
            Assert.AreEqual(false, m.Functions[0].isVoid, "�߂�l�s��v");
        }
        [TestMethod]
        public void Argument()
        {
            Module m = new("func BrokeGoogle(Count,isFill){}");
            Assert.AreEqual(1, m.Functions.Count, "�֐����Ȃ�");
            Assert.AreEqual(2, m.Functions[0].arguments.Count, "������������");
            Assert.AreEqual("Count", m.Functions[0].arguments[0], "������������[0]");
            Assert.AreEqual("isFill", m.Functions[0].arguments[1], "������������[1]");
        }
        [TestMethod]
        public void Statements()
        {
            BMMCompiler.Parts.Expressions.Expression exp = new("Broke(aaa,123,gen())");
            Assert.AreEqual("Broke", exp.func, "�֐����s��v");
            Assert.AreEqual(3, exp.pushes.Count, "�������s��v");
            Assert.AreEqual("aaa", exp.pushes[0].func, "����[0]�s��v");
            Assert.AreEqual(123, exp.pushes[1].num, "����[1]�s��v");
            Assert.AreEqual("gen", exp.pushes[2].func, "����[1]�s��v");
        }
        [TestMethod]
        public void Include()
        {
            Module m = new("#include \"File.bmm\"\n#include \"libstd.bmm\"");
            Assert.AreEqual(2, m.Imports.Count, "�Ǎ��t�@�C�����s��v");
            Assert.AreEqual("File.bmm", m.Imports[0], "�Ǎ��t�@�C����[0]�s��v");
            Assert.AreEqual("libstd.bmm", m.Imports[1], "�Ǎ��t�@�C����[1]�s��v");
        }
    }
}