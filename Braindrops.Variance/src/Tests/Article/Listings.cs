using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Braindrops.Variance.Tests.Model;
using NUnit.Framework;

namespace Braindrops.Variance.Tests.Article
{
    [TestFixture]
    public class Listings
    {
        [Test]
        public void CovariantImplicitCast()
        {
object irgendwas = "Hallo, Welt";
        }

        [Test]
        public void UnsafeContravariantExplicitCast()
        {
object irgendwas = "Hallo, Welt";
string abc = (string) irgendwas;
        }

        [Test, ExpectedException]
        public void FailingContravariantExplicitCast()
        {
object irgendwas = 5;
string abc = (string)irgendwas;
        }

        [Test]
        public void CovariantArray()
        {
Reptile[] reptiles = new Reptile[]
  {
    new Snake("Leo"), 
    new Snake("Lucy")
  };

Animal[] animals = reptiles;
Assert.True(animals[0].Name == "Leo");

animals[0] = new Turtle("Platschi");

Assert.Throws
  <ArrayTypeMismatchException>(() =>
    animals[1] = new Tiger("Mieze"));
        }

        [Test]
        public void InvariantValueTypeArray()
        {

//object[] objects = new DateTime[0];
// error CS0029: Cannot implicitly 
// convert type 'DateTime[]' 
// to 'object[]'

Assert.True(typeof(object)
 .IsAssignableFrom(typeof(DateTime))
);
Assert.False(typeof(object[])
 .IsAssignableFrom(typeof(DateTime[]))
);
        }

        [Test]
        public void MakeIEnumerableT()
        {
Func<Type, Type> x = 
(Type t) => typeof (IEnumerable<>)
              .MakeGenericType(t);
           
        }

        delegate Animal AnimalMethod();
delegate Mammal MammalMethod();
//delegate Giraffe GiraffeMethod();

delegate void TakeAnimalMethod
    (Animal animal);

delegate void TakeMammalMethod
    (Mammal mammal);

delegate void TakeGiraffeMethod
    (Giraffe giraffe);

        [Test]
        public void VariantDelegates()
        {
AnimalMethod a = animal;
MammalMethod m = mammal;
// Leo, das Säugetier
Assert.True(m().Name == "Leo");

AnimalMethod a2 = mammal;
// immer noch Leo, das Säugertier
Assert.True(a2().Name == "Leo");

TakeAnimalMethod ta = takeAnimal;
TakeMammalMethod tm = takeMammal;

// reicht jegliches Tier an
// takeAnimal durch
ta(new Turtle("Platschi"));


//TakeGiraffeMethod tg = takeGiraffe;

// TakeAnimalMethod tm2 = takeMammal;
// error CS0123: No overload for 
// 'takeMammal' matches delegate...

TakeGiraffeMethod tg = takeMammal;
// Reicht Giraffen als Säugetier
// an takeMammal durch
tg(new Giraffe("Halsi"));

MammalMethod m3 = mammal;
// AnimalMethod a5 = m3;
// error CS0029: Cannot implicitly 
// convert type 'MammalMethod' 
// to 'AnimalMethod'

        }

Animal animal()
{
  // irgendein Tier
  return new Turtle("Platschi");
}
Mammal mammal()
{
  // irgendein Säugetier
  return new Tiger("Leo");
}
void takeAnimal(Animal animal) {}
void takeMammal(Mammal mammal) {}
void takeGiraffe(Giraffe giraffe){}

        [Test]
        public void Enumerables()
        {
IEnumerable<Reptile> reptiles =
    new List<Reptile>
        {
            new Turtle("Platschi"),
            new Snake("Lucy")
        };

foreach(Animal a in reptiles)
    Console.WriteLine(a.Name);

#if (NET4)
printAnimals(reptiles);
// error CS0266: Cannot implicitly 
// convert type 'IEnumerable<Reptile>'
// to 'IEnumerable<Animal>'.

//IList<Animal> animals = 
//    new List<Reptile>();
// error CS0029: Cannot implicitly 
// convert type 'List<Reptile>' to 
// 'List<Animal>'
#endif
        }

void printAnimals(
    IEnumerable<Animal> animals)
{
    foreach (Animal a in animals)
        Console.WriteLine(a.Name);
}

        [Test]
        public void GenericFuncAndAction()
        {

Func<Snake> erzeugeSchlange = 
    () => new Snake("Lucy");

#if (NET4)
Func<Reptile> erzeugeReptil
    = erzeugeSchlange;

var lucy = erzeugeReptil();
Assert.True(lucy.Name == "Lucy");
#endif

Action<Reptile> killReptile = 
    r => r.Name = null;

#if (NET4)
Action<Snake> killSnake
    = killReptile;

var leo = new Snake("leo");
killSnake(leo);
Assert.True(leo.Name == null);
#endif
        }

        [Test]
        public void MakeArray()
        {
Assert.True(typeof(int[]) ==
typeof (int).MakeArrayType());
        }

        [Test]
        public void InvariantValueTypes()
        {
// IEnumerable<object> objects 
//    = new List<int>();
// error CS0266: Cannot implicitly
// convert type 'List<int>' to 
// 'IEnumerable<object>'.

IEnumerable<object> objects
    = new List<int>().Cast<object>();
        }

        [Test]
        public void GenericDelegates()
        {
Func<Snake> createSnake =
    () => new Snake("Lucy");
Func<Reptile> createReptile
    = createSnake;
// error CS0029: Cannot implicitly
// convert type 'Func<Snake>'
// to 'Func<Reptile>'

Action<Reptile> killReptile =
    r => r.Name = null;
Action<Snake> killSnake
    = killReptile;
// error CS0029: Cannot implicitly
// convert type 'Action<Reptile>' 
// to 'Action<Snake>'

        }
    }
}
