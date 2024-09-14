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
            Assert.AreEqual(1,m.Functions.Count,"ŠÖ”‚ª‚È‚¢");
            Assert.AreEqual("BrokeGoogle", m.Functions[0].name, "ŠÖ”–¼•sˆê’v");
            Assert.AreEqual(false, m.Functions[0].isVoid, "–ß‚è’l•sˆê’v");
        }
        [TestMethod]
        public void Argument()
        {
            Module m = new("func BrokeGoogle(Count,isFill){}");
            Assert.AreEqual(1, m.Functions.Count, "ŠÖ”‚ª‚È‚¢");
            Assert.AreEqual(2, m.Functions[0].arguments.Count, "ˆø”‚¨‚©‚µ‚¢");
            Assert.AreEqual("Count", m.Functions[0].arguments[0], "ˆø”‚¨‚©‚µ‚¢[0]");
            Assert.AreEqual("isFill", m.Functions[0].arguments[1], "ˆø”‚¨‚©‚µ‚¢[1]");
        }
        [TestMethod]
        public void Statements()
        {
            BMMCompiler.Parts.Expressions.Expression exp = new("Broke(aaa,123)");
            Assert.AreEqual("Broke", exp.func, "ŠÖ”–¼•sˆê’v");
            Assert.AreEqual(2, exp.pushes.Count, "ˆø”’·•sˆê’v");
            Assert.AreEqual("aaa", exp.pushes[0].func, "ˆø”[0]•sˆê’v");
            Assert.AreEqual(123, exp.pushes[1].num, "ˆø”[1]•sˆê’v");
        }
    }
}