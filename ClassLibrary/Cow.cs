using System;

namespace MyClassLibrary
{
    /// <summary>класс коровы</summary>
    [Comment("класс Cow — даёт молоко")]
    public class Cow : Animal
    {
        /// <summary>создаёт корову</summary>
        /// <param name="name">имя</param>
        /// <param name="country">страна</param>
        public Cow(string name, string country) : base(name, country, eClassificationAnimal.Herbivores) { }

        /// <summary>возвращает любимую еду коровы</summary>
        /// <returns>растения</returns>
        public override eFavoriteFood GetFavouriteFood() => eFavoriteFood.Plants;

        /// <summary>говорит привет от коровы</summary>
        public override void SayHello() => Console.WriteLine($"moo, я {Name}");
    }
}
