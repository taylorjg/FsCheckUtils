using System.Linq;
using FsCheck;

namespace FsCheckUtils
{
    public static class PropExtensions
    {
        /// <summary>
        /// Wrapper method around FsCheck's .&. operator that succeeds if both properties succeed.
        /// </summary>
        /// <typeparam name="TLeftTestable">Type of the left property.</typeparam>
        /// <typeparam name="TRightTestable">Type of the right property.</typeparam>
        /// <param name="l">The left property.</param>
        /// <param name="r">The right property.</param>
        /// <returns>A property that succeeds if both properties succeed.</returns>
        public static Gen<Rose<Result>> And<TLeftTestable, TRightTestable>(TLeftTestable l, TRightTestable r)
        {
            return PropOperators.op_DotAmpDot(l, r);
        }

        /// <summary>
        /// Convenience method that applies FsCheck's .&. operator to a collection of properties and succeeds if all properties succeed.
        /// </summary>
        /// <typeparam name="TTestable">Type of the properties in the collection. They must all be of the same type.</typeparam>
        /// <param name="assertions">The collection of properties.</param>
        /// <returns>A property that succeeds if all properties succeed.</returns>
        public static Gen<Rose<Result>> AndAll<TTestable>(params TTestable[] assertions)
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
        public static Gen<Rose<Result>> Or<TLeftTestable, TRightTestable>(TLeftTestable l, TRightTestable r)
        {
            return PropOperators.op_DotBarDot(l, r);
        }

        /// <summary>
        /// Convenience method that applies FsCheck's .|. operator to a collection of properties and fails if all properties fail.
        /// </summary>
        /// <typeparam name="TTestable">Type of the properties in the collection. They must all be of the same type.</typeparam>
        /// <param name="assertions">The collection of properties.</param>
        /// <returns>A property that fails if all properties fail.</returns>
        public static Gen<Rose<Result>> OrAll<TTestable>(params TTestable[] assertions)
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
        public static Gen<Rose<Result>> Label<TTestable>(TTestable assertion, string name)
        {
            return PropOperators.op_BarAt(assertion, name);
        }

        /// <summary>
        /// Wrapper method around FsCheck's ==> implication operator.
        /// </summary>
        /// <typeparam name="TTestable">Type of the property.</typeparam>
        /// <param name="condition">The condition which, if true, implies that the property should be tested.</param>
        /// <param name="assertion">The property to test if the condition is true.</param>
        /// <returns></returns>
        public static Gen<Rose<Result>> Implies<TTestable>(bool condition, TTestable assertion)
        {
            return PropOperators.op_EqualsEqualsGreater(condition, assertion);
        }
    }
}
