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
            Assert.AreEqual(1,m.Functions.Count,"関数がねえお");
            Assert.AreEqual("BrokeGoogle", m.Functions[0].name, "関数名不一致だお");
            Assert.AreEqual(false, m.Functions[0].isVoid, "戻り値不一致だお");
        }
        [TestMethod]
        public void Argument()
        {
            Module m = new("func BrokeGoogle(Count,isFill){}");
            Assert.AreEqual(1, m.Functions.Count, "関数がねえお");
            Assert.AreEqual(2, m.Functions[0].arguments.Count, "引数おかしいお");
            Assert.AreEqual("Count", m.Functions[0].arguments[0], "引数おかしいお[0]");
            Assert.AreEqual("isFill", m.Functions[0].arguments[1], "引数おかしいお[1]");
        }
        [TestMethod]
        public void Statements()
        {
            Module m = new("func BrokeGoogle(Count,isFill){\n    Detect();\n    Broke();\n}");
            Assert.AreEqual(1, m.Functions.Count, "関数がねえお");
            Assert.AreEqual(2, m.Functions[0].statements.Count, "内容がねえお");
            Assert.AreEqual("Detect()", m.Functions[0].statements[0].Compile([], [], []), "内容がおかしいお[0]");
            Assert.AreEqual("Broke()", m.Functions[0].statements[1].Compile([], [], []), "内容がおかしいお[1]");
        }
    }
}