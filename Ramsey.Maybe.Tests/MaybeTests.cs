using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ramsey.Maybe;

namespace Ramsey.Maybe.Tests
{
    [TestClass]
    public class MaybeTests
    {
        [TestMethod]
        public void Test_ToMaybe1()
        {
            var something = "".ToMaybe();
            Assert.IsNotNull(something);
        }

        [TestMethod]
        public void Test_ToMaybe2()
        {
            var nothing = ((string)null).ToMaybe();
            Assert.IsNull(nothing);
        }

        [TestMethod]
        public void Test_ValueOrDefault1()
        {
            var something = "".ToMaybe();
            Assert.AreEqual(something.ValueOrDefault(), "");
            Assert.AreEqual(something.ValueOrDefault("a"), "");
        }

        [TestMethod]
        public void Test_IsNothing()
        {
            var something = 1.ToMaybe();
            Assert.IsFalse(something.IsNothing());

            var nothing = ((string)null).ToMaybe();
            Assert.IsTrue(nothing.IsNothing());
        }

        [TestMethod]
        public void Test_ValueOrDefault2()
        {
            var nothing = ((string)null).ToMaybe();
            Assert.AreEqual(nothing.ValueOrDefault("a"), "a");
            Assert.AreEqual(nothing.ValueOrDefault(), default(string));
        }

        [TestMethod]
        public void Test_Select1()
        {
            var newSomething = "a".ToMaybe()
                                .Select(x => x + "a")
                                .ValueOrDefault();

            Assert.AreEqual(newSomething, "aa");
        }

        [TestMethod]
        public void Test_Select2()
        {
            var nothing = ((string)null).ToMaybe();
            var stillNothing = nothing
                                .Select(x => x + "a")
                                .ValueOrDefault();

            Assert.AreEqual(stillNothing, null);
        }

        [TestMethod]
        public void Test_Where1()
        {
            var something = "a".ToMaybe();
            var passFilter = something
                                .Where(x => x.Length == 1)
                                .ValueOrDefault();

            Assert.AreEqual(passFilter, "a");

            var failFilter = something
                                .Where(x => x.Length > 1)
                                .ValueOrDefault();

            Assert.AreEqual(failFilter, null);
        }

        [TestMethod]
        public void Test_Where2()
        {
            var nothing = ((string)null).ToMaybe();
            var stillNothing = nothing
                                .Where(x => x.Length == 1)
                                .ValueOrDefault();

            Assert.AreEqual(stillNothing, null);
        }

        [TestMethod]
        public void Test_WhereNot1()
        {
            var something = "a".ToMaybe();
            var passFilter = something
                                .WhereNot(x => x == "b")
                                .ValueOrDefault();

            Assert.AreEqual(passFilter, "a");

            var failFilter = something
                                .WhereNot(x => x.Length == 1)
                                .ValueOrDefault();

            Assert.AreEqual(failFilter, null);
        }

        [TestMethod]
        public void Test_WhereNot2()
        {
            var nothing = ((string)null).ToMaybe();
            var stillNothing = nothing
                                .WhereNot(x => x.Length == 1)
                                .ValueOrDefault();

            Assert.AreEqual(stillNothing, null);
        }

        [TestMethod]
        public void Test_SelectMany1()
        {
            var thing1 = "1".ToMaybe();
            var thing2 = "2".ToMaybe();

            string combine()
            {
                return thing1
                        .SelectMany(_ => thing2, (x, y) => x + y)
                        .ValueOrDefault();
            }

            Assert.AreEqual(combine(), "12");

            thing1 = null;
            thing2 = "2".ToMaybe();
            Assert.AreEqual(combine(), null);

            thing1 = "1".ToMaybe();
            thing2 = null;
            Assert.AreEqual(combine(), null);

            thing1 = null;
            thing2 = null;
            Assert.AreEqual(combine(), null);
        }

        [TestMethod]
        public void Test_QuerySyntax()
        {
            var x =
                from val in "a".ToMaybe()
                where !string.IsNullOrWhiteSpace(val)
                where val.Length == 1
                from val2 in "b".ToMaybe()
                select val + val2;

            Assert.AreEqual(x.ValueOrDefault(), "ab");
        }

        [TestMethod]
        public void Test_ShortCircuitsOnNothings()
        {
            bool calledFunciton = false;

            string fun(string x)
            {
                calledFunciton = true;
                return x;
            }


            string s = null;

            var newS =
                from val in s.ToMaybe()
                select fun(s);

            Assert.AreEqual(calledFunciton, false);

            s = "";
            newS = s.ToMaybe().Select(fun);

            Assert.AreEqual(calledFunciton, true);
        }
    }
}
