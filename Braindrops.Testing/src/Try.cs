using System;
using System.Collections.Generic;
using System.Linq;

namespace Braindrops.Testing
{
    public static class Try
    {
        public static ResultType Get<ResultType>(Func<ResultType> func)
        {
            return Get<ResultType, Exception>(func);
        }

        public static ResultType Get<ResultType, CatchType>(Func<ResultType> func)
            where CatchType : Exception
        {
            try
            {
                return func();
            }
            catch (CatchType)
            {
                return default(ResultType);
            }
        }

        public static Exception Catch(Action action)
        {
            return Catch<Exception>(action);
        }

        public static CatchType Catch<CatchType>(Action action)
            where CatchType : Exception
        {
            try
            {
                action();
            }
            catch (CatchType exception)
            {
                return exception;
            }

            return null;
        }

        public static Exception[] CatchAll(params Action[] actions)
        {
            return actions.Select(a => Catch(a)).ToArray();
        }

        public static Exception[] CatchAll(IEnumerable<Action> actions)
        {
            return CatchAll(actions.ToArray());
        }
    }
}