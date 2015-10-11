using System;
using System.Linq;
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
        /// TODO
        /// </summary>
        /// <typeparam name="TTestable">TODO</typeparam>
        /// <param name="condition">TODO</param>
        /// <param name="ifTrue">TODO</param>
        /// <param name="ifFalse">TODO</param>
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <returns></returns>
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
    }
}
