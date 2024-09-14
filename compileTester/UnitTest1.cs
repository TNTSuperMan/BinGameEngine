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
            Assert.AreEqual(1,m.Functions.Count,"関数がない");
            Assert.AreEqual("BrokeGoogle", m.Functions[0].name, "関数名不一致");
            Assert.AreEqual(false, m.Functions[0].isVoid, "戻り値不一致");
        }
        [TestMethod]
        public void Argument()
        {
            Module m = new("func BrokeGoogle(Count,isFill){}");
            Assert.AreEqual(1, m.Functions.Count, "関数がない");
            Assert.AreEqual(2, m.Functions[0].arguments.Count, "引数おかしい");
            Assert.AreEqual("Count", m.Functions[0].arguments[0], "引数おかしい[0]");
            Assert.AreEqual("isFill", m.Functions[0].arguments[1], "引数おかしい[1]");
        }
        [TestMethod]
        public void Statements()
        {
            BMMCompiler.Parts.Expressions.Expression exp = new("Broke(aaa,123)");
            Assert.AreEqual("Broke", exp.func, "関数名不一致");
            Assert.AreEqual(2, exp.pushes.Count, "引数長不一致");
            Assert.AreEqual("aaa", exp.pushes[0].func, "引数[0]不一致");
            Assert.AreEqual(123, exp.pushes[1].num, "引数[1]不一致");
        }
    }
}