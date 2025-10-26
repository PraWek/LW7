using System;

namespace MyClassLibrary
{
    /// <summary>
    /// абстрактный базовый класс для животных
    /// </summary>
    [Comment("базовый абстрактный класс Animal")]
    public abstract class Animal
    {
        /// <summary>страна происхождения</summary>
        public string Country { get; init; }

        /// <summary>имя животного</summary>
        public string Name { get; init; }

        /// <summary>прятать ли от других животных</summary>
        public bool HideFromOtherAnimals { get; set; }

        /// <summary>классификация животного</summary>
        public eClassificationAnimal Classification { get; init; }

        /// <summary>создаёт экземпляр животного</summary>
        /// <param name="name">имя</param>
        /// <param name="country">страна</param>
        /// <param name="classification">классификация</param>
        protected Animal(string name, string country, eClassificationAnimal classification)
        {
            Name = name;
            Country = country;
            Classification = classification;
            HideFromOtherAnimals = false;
        }

        /// <summary>разбирает объект на составляющие</summary>
        public void Deconstruct(out string name, out string country) { name = Name; country = Country; }

        /// <summary>возвращает классификацию животного</summary>
        /// <returns>значение перечисления eClassificationAnimal</returns>
        public eClassificationAnimal GetClassificationAnimal() => Classification;

        /// <summary>возвращает любимую еду</summary>
        /// <returns>значение перечисления eFavoriteFood</returns>
        public abstract eFavoriteFood GetFavouriteFood();

        /// <summary>произносит приветствие</summary>
        public abstract void SayHello();
    }
}
