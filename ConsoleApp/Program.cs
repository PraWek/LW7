using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using MyClassLibrary;

namespace ReflectionGenerator
{
    /// <summary>
    /// приложение, создающее xml-представление классов библиотеки средствами рефлексии
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// точка входа программы
        /// </summary>
        /// <param name="args">аргументы командной строки (не используются)</param>
        public static void Main(string[] args)
        {
            try
            {
                var assembly = LoadLibraryAssembly("MyClassLibrary");
                var xmlPath = GetOutputPath("classes.xml");
                GenerateXml(assembly, xmlPath);
                Console.WriteLine($"Файл XML успешно создан: {xmlPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        /// <summary>
        /// загружает сборку библиотеки по имени
        /// </summary>
        /// <param name="assemblyName">имя сборки без расширения</param>
        /// <returns>загруженная сборка.</returns>
        /// <exception cref="FileNotFoundException">если сборка не найдена</exception>
        private static Assembly LoadLibraryAssembly(string assemblyName)
        {
            try
            {
                return Assembly.Load(assemblyName);
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException($"Сборка {assemblyName}.dll не найдена. Сначала соберите проект библиотеки.");
            }
        }

        /// <summary>
        /// формирует абсолютный путь для сохранения xml в корне решения
        /// </summary>
        /// <param name="fileName">имя xml-файла</param>
        /// <returns>полный путь к xml-файлу</returns>
        private static string GetOutputPath(string fileName)
        {
            var dir = AppContext.BaseDirectory;
            var root = Path.GetFullPath(Path.Combine(dir, @"..\..\.."));
            return Path.Combine(root, fileName);
        }

        /// <summary>
        /// создаёт xml-представление типов, свойств, методов и атрибутов сборки
        /// </summary>
        /// <param name="assembly">загруженная сборка библиотеки</param>
        /// <param name="outputPath">путь сохранения xml-файла</param>
        private static void GenerateXml(Assembly assembly, string outputPath)
        {
            var settings = new XmlWriterSettings { Indent = true, Encoding = System.Text.Encoding.UTF8 };
            using var writer = XmlWriter.Create(outputPath, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Library");
            writer.WriteAttributeString("Name", assembly.GetName().Name);

            foreach (var type in assembly.GetTypes().OrderBy(t => t.Name))
            {
                writer.WriteStartElement("Type");
                writer.WriteAttributeString("Name", type.Name);
                writer.WriteAttributeString("Namespace", type.Namespace ?? string.Empty);
                writer.WriteAttributeString("Kind", GetTypeKind(type));

                if (type.BaseType is not null && type.BaseType != typeof(object))
                    writer.WriteElementString("BaseType", type.BaseType.Name);

                WriteAttributes(writer, type);
                WriteProperties(writer, type);
                WriteMethods(writer, type);

                writer.WriteEndElement(); // Type
            }

            writer.WriteEndElement(); // Library
            writer.WriteEndDocument();
        }

        /// <summary>
        /// возвращает текстовое обозначение типа
        /// </summary>
        /// <param name="type">тип для анализа</param>
        /// <returns>строка с категорией (Class, Enum, Struct)</returns>
        private static string GetTypeKind(Type type) =>
            type.IsEnum ? "Enum" :
            type.IsClass ? "Class" :
            type.IsValueType ? "Struct" : "Unknown";

        /// <summary>
        /// записывает свойства типа в xml
        /// </summary>
        /// <param name="writer">xml-писатель</param>
        /// <param name="type">анализируемый тип</param>
        private static void WriteProperties(XmlWriter writer, Type type)
        {
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            if (props.Length == 0) return;

            writer.WriteStartElement("Properties");
            foreach (var p in props)
            {
                writer.WriteStartElement("Property");
                writer.WriteAttributeString("Name", p.Name);
                writer.WriteAttributeString("Type", p.PropertyType.Name);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        /// <summary>
        /// записывает методы типа в xml
        /// </summary>
        /// <param name="writer">xml-писатель</param>
        /// <param name="type">анализируемый тип</param>
        private static void WriteMethods(XmlWriter writer, Type type)
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                              .Where(m => !m.IsSpecialName)
                              .ToArray();
            if (methods.Length == 0) return;

            writer.WriteStartElement("Methods");
            foreach (var m in methods)
            {
                writer.WriteStartElement("Method");
                writer.WriteAttributeString("Name", m.Name);
                writer.WriteAttributeString("ReturnType", m.ReturnType.Name);
                if (m.GetParameters().Length > 0)
                {
                    writer.WriteStartElement("Parameters");
                    foreach (var p in m.GetParameters())
                    {
                        writer.WriteStartElement("Parameter");
                        writer.WriteAttributeString("Name", p.Name ?? string.Empty);
                        writer.WriteAttributeString("Type", p.ParameterType.Name);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        /// <summary>
        /// записывает пользовательские атрибуты типа
        /// </summary>
        /// <param name="writer">xml-писатель</param>
        /// <param name="type">анализируемый тип</param>
        private static void WriteAttributes(XmlWriter writer, Type type)
        {
            var attrs = type.GetCustomAttributes(false);
            if (attrs.Length == 0) return;

            writer.WriteStartElement("Attributes");
            foreach (var a in attrs)
            {
                writer.WriteStartElement("Attribute");
                writer.WriteAttributeString("Type", a.GetType().Name);
                if (a is CommentAttribute commentAttr)
                    writer.WriteElementString("Comment", commentAttr.Comment);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
    }
}
