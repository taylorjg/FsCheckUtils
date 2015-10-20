using System;
using System.Linq;
using System.Net.NetworkInformation;
using FsCheck;
using FsCheck.Fluent;
using Microsoft.FSharp.Core;

namespace FsCheckUtils
{
    using Property = Gen<Rose<Result>>;

    /// <summary>
    /// Wrappers around FsCheck's PropOperators.
    /// </summary>
    public static class PropExtensions
    {
        /// <summary>
        /// Wrapper method around FsCheck's .&amp;. operator that succeeds if both properties succeed.
        /// </summary>
        /// <typeparam name="TLeftTestable">Type of the left property.</typeparam>
        /// <typeparam name="TRightTestable">Type of the right property.</typeparam>
        /// <param name="l">The left property.</param>
        /// <param name="r">The right property.</param>
        /// <returns>A property that succeeds if both properties succeed.</returns>
        public static Property And<TLeftTestable, TRightTestable>(TLeftTestable l, TRightTestable r)
        {
            return PropOperators.op_DotAmpDot(l, r);
        }

        /// <summary>
        /// Convenience method that applies FsCheck's .&amp;. operator to a collection of properties and succeeds if all properties succeed.
        /// </summary>
        /// <typeparam name="TTestable">Type of the properties in the collection. They must all be of the same type.</typeparam>
        /// <param name="assertions">The collection of properties.</param>
        /// <returns>A property that succeeds if all properties succeed.</returns>
        public static Property AndAll<TTestable>(params TTestable[] assertions)
        {
            return assertions.Aggregate(Prop.ofTestable(true), And);
        }

        /// <summary>
        /// Wrapper method around FsCheck's .|. operator that fails if both properties fail.
        /// </summary>
        /// <typeparam name="TLeftTestable">Type of the left property.</typeparam>
        /// <typeparam name="TRightTestable">Type of the right property.</typeparam>
        /// <param name="l">The left property.</param>
        /// <param name="r">The right property.</param>
        /// <returns>A property that fails if both properties fail.</returns>
        public static Property Or<TLeftTestable, TRightTestable>(TLeftTestable l, TRightTestable r)
        {
            return PropOperators.op_DotBarDot(l, r);
        }

        /// <summary>
        /// Convenience method that applies FsCheck's .|. operator to a collection of properties and fails if all properties fail.
        /// </summary>
        /// <typeparam name="TTestable">Type of the properties in the collection. They must all be of the same type.</typeparam>
        /// <param name="assertions">The collection of properties.</param>
        /// <returns>A property that fails if all properties fail.</returns>
        public static Property OrAll<TTestable>(params TTestable[] assertions)
        {
            return assertions.Aggregate(Prop.ofTestable(false), Or);
        }

        /// <summary>
        /// Wrapper method around FsCheck's |@ property operator.
        /// </summary>
        /// <typeparam name="TTestable">Type of the property.</typeparam>
        /// <param name="assertion">The property to be labelled.</param>
        /// <param name="name">The label to apply to the property.</param>
        /// <returns>A property</returns>
        public static Property Label<TTestable>(TTestable assertion, string name)
        {
            return PropOperators.op_BarAt(assertion, name);
        }

        /// <summary>
        /// Wrapper method around FsCheck's ==> implication operator.
        /// </summary>
        /// <typeparam name="TTestable">Type of the property.</typeparam>
        /// <param name="condition">The condition which, if true, implies that the property should be tested.</param>
        /// <param name="assertion">The property to test if the condition is true.</param>
        /// <returns>A property that does or doesn't test the assertion depending on the condition</returns>
        public static Property Implies<TTestable>(bool condition, TTestable assertion)
        {
            return PropOperators.op_EqualsEqualsGreater(condition, assertion);
        }

        /// <summary>
        /// Classify test cases combinator with true ansd false options.
        /// </summary>
        /// <typeparam name="TTestable">Type of the property.</typeparam>
        /// <param name="condition">If the condition is true then the test case is assigned the classification <paramref name="ifTrue" /> otherwise <paramref name="ifFalse" />.</param>
        /// <param name="ifTrue">TODO</param>
        /// <param name="ifFalse">TODO</param>
        /// <returns>TODO</returns>
        public static FSharpFunc<TTestable, Property> Classify<TTestable>(bool condition, string ifTrue, string ifFalse)
        {
            return Prop.classify<TTestable>(true, condition ? ifTrue : ifFalse);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <typeparam name="TA">TODO</typeparam>
        /// <param name="specBuilder">TODO</param>
        /// <param name="filter">TODO</param>
        /// <param name="ifTrue">TODO</param>
        /// <param name="ifFalse">TODO</param>
        /// <returns>TODO</returns>
        public static SpecBuilder<TA> Classify<TA>(
            this SpecBuilder<TA> specBuilder,
            Func<TA, bool> filter,
            string ifTrue,
            string ifFalse)
        {
            return specBuilder
                .Classify(filter, ifTrue)
                .Classify(a => !filter(a), ifFalse);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <typeparam name="TA">TODO</typeparam>
        /// <typeparam name="TB">TODO</typeparam>
        /// <param name="specBuilder">TODO</param>
        /// <param name="filter">TODO</param>
        /// <param name="ifTrue">TODO</param>
        /// <param name="ifFalse">TODO</param>
        /// <returns>TODO</returns>
        public static SpecBuilder<TA, TB> Classify<TA, TB>(
            this SpecBuilder<TA, TB> specBuilder,
            Func<TA, TB, bool> filter,
            string ifTrue,
            string ifFalse)
        {
            return specBuilder
                .Classify(filter, ifTrue)
                .Classify((a, b) => !filter(a, b), ifFalse);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <typeparam name="TA">TODO</typeparam>
        /// <typeparam name="TB">TODO</typeparam>
        /// <typeparam name="TC">TODO</typeparam>
        /// <param name="specBuilder">TODO</param>
        /// <param name="filter">TODO</param>
        /// <param name="ifTrue">TODO</param>
        /// <param name="ifFalse">TODO</param>
        /// <returns>TODO</returns>
        public static SpecBuilder<TA, TB, TC> Classify<TA, TB, TC>(
            this SpecBuilder<TA, TB, TC> specBuilder,
            Func<TA, TB, TC, bool> filter,
            string ifTrue,
            string ifFalse)
        {
            return specBuilder
                .Classify(filter, ifTrue)
                .Classify((a, b, c) => !filter(a, b, c), ifFalse);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="ps">TODO</param>
        /// <returns>TODO</returns>
        public static Property Chain(params FSharpFunc<Property, Property>[] ps)
        {
            return Chain(true, ps);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="b">TODO</param>
        /// <param name="ps">TODO</param>
        /// <returns>TODO</returns>
        public static Property Chain(bool b, params FSharpFunc<Property, Property>[] ps)
        {
            return Chain(Prop.ofTestable(b), ps);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="p0">TODO</param>
        /// <param name="ps">TODO</param>
        /// <returns>TODO</returns>
        public static Property Chain(Property p0, params FSharpFunc<Property, Property>[] ps)
        {
            return ps.Aggregate(p0, (acc, p) => p.Invoke(acc));
        }

        public static Property ForAll<T1, TTestable>(Arbitrary<T1> arb1, Func<T1, TTestable> p)
        {
            return Prop.forAll(arb1, FSharpFunc<T1, TTestable>.FromConverter(v1 => p(v1)));
        }

        public static Property ForAll<T1, TTestable>(Gen<T1> g1, Func<T1, TTestable> p)
        {
            return ForAll(Arb.fromGen(g1), p);
        }

        public static Property ForAll<T1, T2, TTestable>(Arbitrary<T1> arb1, Arbitrary<T2> arb2, Func<T1, T2, TTestable> p)
        {
            return
                Prop.forAll(arb1, FSharpFunc<T1, Property>.FromConverter(v1 =>
                    Prop.forAll(arb2, FSharpFunc<T2, TTestable>.FromConverter(v2 =>
                        p(v1, v2)))));
        }

        public static Property ForAll<T1, T2, TTestable>(Gen<T1> g1, Gen<T2> g2, Func<T1, T2, TTestable> p)
        {
            return ForAll(
                Arb.fromGen(g1),
                Arb.fromGen(g2),
                p);
        }

        public static Property ForAll<T1, T2, T3, TTestable>(Arbitrary<T1> arb1, Arbitrary<T2> arb2, Arbitrary<T3> arb3, Func<T1, T2, T3, TTestable> p)
        {
            return
                Prop.forAll(arb1, FSharpFunc<T1, Property>.FromConverter(v1 =>
                    Prop.forAll(arb2, FSharpFunc<T2, Property>.FromConverter(v2 =>
                        Prop.forAll(arb3, FSharpFunc<T3, TTestable>.FromConverter(v3 =>
                            p(v1, v2, v3)))))));
        }

        public static Property ForAll<T1, T2, T3, TTestable>(Gen<T1> g1, Gen<T2> g2, Gen<T3> g3, Func<T1, T2, T3, TTestable> p)
        {
            return ForAll(
                Arb.fromGen(g1),
                Arb.fromGen(g2),
                Arb.fromGen(g3),
                p);
        }

        public static Property ForAll<T1, T2, T3, T4, TTestable>(Arbitrary<T1> arb1, Arbitrary<T2> arb2, Arbitrary<T3> arb3, Arbitrary<T4> arb4, Func<T1, T2, T3, T4, TTestable> p)
        {
            return
                Prop.forAll(arb1, FSharpFunc<T1, Property>.FromConverter(v1 =>
                    Prop.forAll(arb2, FSharpFunc<T2, Property>.FromConverter(v2 =>
                        Prop.forAll(arb3, FSharpFunc<T3, Property>.FromConverter(v3 =>
                            Prop.forAll(arb4, FSharpFunc<T4, TTestable>.FromConverter(v4 =>
                                p(v1, v2, v3, v4)))))))));
        }

        public static Property ForAll<T1, T2, T3, T4, TTestable>(Gen<T1> g1, Gen<T2> g2, Gen<T3> g3, Gen<T4> g4, Func<T1, T2, T3, T4, TTestable> p)
        {
            return ForAll(
                Arb.fromGen(g1),
                Arb.fromGen(g2),
                Arb.fromGen(g3),
                Arb.fromGen(g4),
                p);
        }

        public static Property ForAll<T1, T2, T3, TTestable>(Func<T1, T2, T3, TTestable> p)
        {
            var g1 = Arb.generate<T1>();
            var g2 = Arb.generate<T2>();
            var g3 = Arb.generate<T3>();
            return ForAll(g1, g2, g3, p);
        }
    }
}
