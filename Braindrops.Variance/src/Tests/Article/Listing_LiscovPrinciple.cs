using System;

namespace Braindrops.Variance.Tests.Article
{
    abstract class Basisklasse
    {
        public abstract object NehmenUndGeben(string irgendwas);
    }

    class Subklasse : Basisklasse
    {
        /* Variant parameters and return types
         * theoretically possible, but not in .NET
         *
        public override string NehmenUndGeben(object irgendwas)
        {
            throw new NotImplementedException();
        }
         *
         */

        public override object NehmenUndGeben(string irgendwas)
        {
            throw new NotImplementedException();
        }
    }
}