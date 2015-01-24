using System.Linq;
using FsCheck;

namespace FsCheckUtils
{
    public static class PropExtensions
    {
        public static Gen<Rose<Result>> And<TLeftTestable, TRightTestable>(TLeftTestable l, TRightTestable r)
        {
            return PropOperators.op_DotAmpDot(l, r);
        }

        public static Gen<Rose<Result>> AndAll<TTestable>(params TTestable[] assertions)
        {
            return assertions.Aggregate(Prop.ofTestable(true), And);
        }

        public static Gen<Rose<Result>> Or<TLeftTestable, TRightTestable>(TLeftTestable l, TRightTestable r)
        {
            return PropOperators.op_DotBarDot(l, r);
        }

        public static Gen<Rose<Result>> OrAll<TTestable>(params TTestable[] assertions)
        {
            return assertions.Aggregate(Prop.ofTestable(false), Or);
        }

        public static Gen<Rose<Result>> Label<TTestable>(TTestable assertion, string name)
        {
            return PropOperators.op_BarAt(assertion, name);
        }

        public static Gen<Rose<Result>> Implies<TTestable>(bool condition, TTestable assertion)
        {
            return PropOperators.op_EqualsEqualsGreater(condition, assertion);
        }
    }
}
