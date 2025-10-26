using System;

namespace MyClassLibrary
{
    /// <summary>класс свиньи</summary>
    [Comment("класс Pig — всёядная")]
    public class Pig : Animal
    {
        /// <summary>создаёт свинью</summary>
        /// <param name="name">имя</param>
        /// <param name="country">страна</param>
        public Pig(string name, string country) : base(name, country, eClassificationAnimal.Omnivores) { }

        /// <summary>возвращает любимую еду свиньи</summary>
        /// <returns>всё подряд</returns>
        public override eFavoriteFood GetFavouriteFood() => eFavoriteFood.Everything;

        /// <summary>говорит привет от свиньи</summary>
        public override void SayHello() => Console.WriteLine($"oink, я {Name}");
    }
}
