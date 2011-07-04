using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Braindrops.Variance.Tests.Article
{
interface IIn<in TIn>
{
  void In(TIn value);
  
  //TIn Out();
  // error CS1961: Invalid variance: 
  // The type parameter 'TInAndOut' 
  // must be covariantly valid on 
  // 'IIn<TInAndOut>.Read()'. 
  // 'TInAndOut' is contravariant.

  IIn<TIn> Out();
}

interface IOut<out TOut>
{
  TOut Out();

  //void In(TOut value);
  // error CS1961: Invalid variance:
  // The type parameter 'TOut' must 
  // be contravariantly valid on 
  // 'IOut<TOut>.In(TOut)'.
  // 'TOut' is covariant.
}

// funktioniert wunderbar
interface IInAndOut<in TIn, out TOut>
{
  void In(TIn value);
  TOut Out();
}
}
