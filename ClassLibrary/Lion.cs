using System;

namespace MyClassLibrary
{
    /// <summary>класс льва</summary>
    [Comment("класс Lion - царь зверей")]
    public class Lion : Animal
    {
        /// <summary>создаёт льва</summary>
        /// <param name="name">имя</param>
        /// <param name="country">страна</param>
        public Lion(string name, string country) : base(name, country, eClassificationAnimal.Carnivores) { }

        /// <summary>возвращает любимую еду льва</summary>
        /// <returns>мясо</returns>
        public override eFavoriteFood GetFavouriteFood() => eFavoriteFood.Meat;

        /// <summary>говорит привет от льва</summary>
        public override void SayHello() => Console.WriteLine($"roar, я {Name}");
    }
}
