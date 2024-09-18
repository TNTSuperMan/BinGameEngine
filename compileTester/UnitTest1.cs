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
            Assert.AreEqual(1,m.Functions.Count,"関数がない");
            Assert.AreEqual("BrokeGoogle", m.Functions[0].Name, "関数名不一致");
            Assert.AreEqual(false, m.Functions[0].IsVoid, "戻り値不一致");
        }
        [TestMethod]
        public void Argument()
        {
            Module m = new("func BrokeGoogle(Count,isFill){}");
            Assert.AreEqual(1, m.Functions.Count, "関数がない");
            Assert.AreEqual(2, m.Functions[0].Arguments.Count, "引数おかしい");
            Assert.AreEqual("Count", m.Functions[0].Arguments[0], "引数おかしい[0]");
            Assert.AreEqual("isFill", m.Functions[0].Arguments[1], "引数おかしい[1]");
        }
        [TestMethod]
        public void Statements()
        {
            BMMCompiler.Parts.Expressions.Expression exp = new("Broke(aaa,123,gen())");
            Assert.AreEqual("Broke", exp.func, "関数名不一致");
            Assert.AreEqual(3, exp.pushes.Count, "引数長不一致");
            Assert.AreEqual("aaa", exp.pushes[0].func, "引数[0]不一致");
            Assert.AreEqual(123, exp.pushes[1].num, "引数[1]不一致");
            Assert.AreEqual("gen", exp.pushes[2].func, "引数[1]不一致");
        }
        [TestMethod]
        public void Include()
        {
            Module m = new("#include \"File.bmm\"\n#include \"libstd.bmm\"");
            Assert.AreEqual(2, m.Imports.Count, "読込ファイル数不一致");
            Assert.AreEqual("File.bmm", m.Imports[0], "読込ファイル名[0]不一致");
            Assert.AreEqual("libstd.bmm", m.Imports[1], "読込ファイル名[1]不一致");
        }
        [TestMethod]
        public void Extern()
        {
            Module m = new("extern Tent\nextern  Ne;extern Temp\n");
            Assert.AreEqual(3, m.ExportVariables.Count, "変数数不一致");
            Assert.AreEqual("Tent", m.ExportVariables[0].Name, "変数[0]不一致");
            Assert.AreEqual("Ne", m.ExportVariables[1].Name, "変数[1]不一致");
            Assert.AreEqual("Temp", m.ExportVariables[2].Name, "変数[2]不一致");
        }
    }
}