using System;

namespace ReflectionSerialization
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            // Опции главного меню с заголовками и подменю
            string[] menuOptions = {
                "- Работа с Json",  // Заголовок 1
                "\tСериализация", // Подэлемент 1
                "\tДесериализация", // Подэлемент 2
                "- Работа с CSV", // Заголовок 2
                "\tИмпорт", // Подэлемент 3
                "\tЭкспорт", // Подэлемент 4
                "Выход" // Заголовок 3
            };

            // Действия, соответствующие каждому элементу меню
            Action[] menuActions = {
                null, // Заголовок 1 (недоступно)
                Option1, // Подэлемент 1
                Option2, // Подэлемент 2
                null, // Заголовок 2 (недоступно)
                Option3, // Подэлемент 3
                Option4, // Подэлемент 4
                Exit // Заголовок 3 (недоступно)
            };

            // Определяем индексы заголовков
            int[] headerIndices = new int[] { 0, 3 };
            ShowMenu("Главное меню", menuOptions, menuActions, 0, headerIndices);
        }

        public static void ShowMenu(string title, string[] options, Action[] actions, int currentSelection, int[] headerIndices)
        {
            Console.Clear();
            Console.WriteLine(title);
            Console.WriteLine(new string('-', title.Length));

            while (true)
            {
                for (int i = 0; i < options.Length; i++)
                {
                    string formattedOption = $"{options[i]}";
                    bool isHeader = Array.Exists(headerIndices, index => index == i);

                    if (isHeader)
                    {
                        Console.ResetColor();
                    }
                    else if (i == currentSelection)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.ResetColor();
                    }

                    if (!isHeader)
                    {
                        formattedOption = formattedOption;
                    }

                    Console.WriteLine(formattedOption);
                }

                Console.ResetColor();

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    currentSelection = (currentSelection + 1) % options.Length; 
                    while (Array.Exists(headerIndices, index => index == currentSelection))
                    {
                        currentSelection = (currentSelection + 1) % options.Length;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    currentSelection = (currentSelection - 1 + options.Length) % options.Length;
                    while (Array.Exists(headerIndices, index => index == currentSelection))
                    {
                        currentSelection = (currentSelection - 1 + options.Length) % options.Length;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    if (actions[currentSelection] != null)
                    {
                        actions[currentSelection].Invoke();
                        Console.Clear();
                    }
                    else
                    {
                        Console.WriteLine("Этот пункт меню недоступен. Нажмите любую клавишу для продолжения...");
                        Console.ReadKey(true);
                        Console.Clear();
                    }
                }

                Console.Clear();
            }
        }

        public static void Option1()
        {
            Console.WriteLine("Вы выбрали 'Сериализация'.");
            // Логика работы с сериализацией...
            Console.WriteLine("Нажмите любую клавишу для возврата в меню.");
            Console.ReadKey(true);
        }

        public static void Option2()
        {
            Console.WriteLine("Вы выбрали 'Десериализация'.");
            // Логика работы с десериализацией...
            Console.WriteLine("Нажмите любую клавишу для возврата в меню.");
            Console.ReadKey(true);
        }

        public static void Option3()
        {
            Console.WriteLine("Вы выбрали 'Импорт'.");
            // Логика работы с импортом...
            Console.WriteLine("Нажмите любую клавишу для возврата в меню.");
            Console.ReadKey(true);
        }

        public static void Option4()
        {
            Console.WriteLine("Вы выбрали 'Экспорт'.");
            // Логика работы с экспортом...
            Console.WriteLine("Нажмите любую клавишу для возврата в меню.");
            Console.ReadKey(true);
        }

        public static void Exit()
        {
            Console.WriteLine("Выход из программы.");
        }
    }
}
