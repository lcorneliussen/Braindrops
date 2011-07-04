using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Braindrops.Variance.Tests.Article
{
interface IOutAndRefParams
    <out TOut, in TIn>
{
    //void Out(out TOut o, ref TIn i);
    // error CS1961: Invalid variance: 
    // The type parameter 'TOut' must 
    // be invariantly valid on '...'. 
    // 'TOut' is covariant.
    //void Ref(ref TOut o, ref TIn i);
    // error CS1961: Invalid variance:
    // The type parameter 'TIn' must 
    // be invariantly valid on '...'. 
    // 'TIn' is contravariant.
}
}
