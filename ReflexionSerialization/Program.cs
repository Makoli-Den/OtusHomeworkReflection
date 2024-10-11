using ReflectionSerialization.Serializators;
using ReflectionSerialization.TestData;

namespace ReflectionSerialization
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            string[] mainMenuOptions = { "Работа с Json", "Работа с CSV", "Выход" };
            Action[] mainMenuActions = 
            {
                () => DisplayTypeBasedMenu("Json"),
                () => DisplayTypeBasedMenu("CSV"),
                Exit
            };

            ShowMenu("Главное меню", mainMenuOptions, mainMenuActions);
        }

        static void ShowMenu(string menuTitle, string[] options, Action[] actions)
        {
            if (options.Length != actions.Length)
            {
                throw new ArgumentException("Количество опций должно совпадать с количеством действий.");
            }

            int selectedIndex = 0;

            while (true)
            {
                DisplayOptions(menuTitle, options, selectedIndex);
                var key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex == 0) ? options.Length - 1 : selectedIndex - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex == options.Length - 1) ? 0 : selectedIndex + 1;
                        break;
                    case ConsoleKey.Enter:
                        actions[selectedIndex]();
                        break;
                }
            }
        }

        static void DisplayMainLabel()
        {
            Console.WriteLine("Программа сериализации в CSV и десериализации из CSV\n\n");
        }

        // Метод для отображения опций меню
        static void DisplayOptions(string menuTitle, string[] options, int selectedIndex)
        {
            Console.Clear();
            DisplayMainLabel();
            Console.WriteLine(menuTitle + ":\n");

            for (int i = 0; i < options.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }

                Console.WriteLine(options[i]);
                Console.ResetColor();
            }
        }

        // Действие для "Опция 1"
        static void Option1()
        {
            Console.WriteLine("Вы выбрали 'Опция 1'.");
            WaitForUser();
        }

        // Действие для "Опция 2"
        static void Option2()
        {
            Console.WriteLine("Вы выбрали 'Опция 2'.");
            WaitForUser();
        }

        static void DisplayTypeBasedMenu(string type)
        {
            string[] subMenuOptions = { "Работа с файлами", "Работа с классами", "Назад" };
            Action[] subMenuActions =
            {
                () => FileWorkMenu(type),
                () => ClassWorkMenu(type),
                BackToMainMenu
            };

            ShowMenu("Работа с Json", subMenuOptions, subMenuActions);
        }

        static void FileWorkMenu(string type)
        {

        }

        static void ClassWorkMenu(string type)
        {
            string[] subMenuOptions = { "Большой класс TestData", "Маленький класс F", "Назад" };
            Action[] subMenuActions =
            {
                () => ClassWork<F>(type),
                () => ClassWork<F>(type),
                BackToMainMenu
            };

            ShowMenu("Работа с классами Json", subMenuOptions, subMenuActions);
        }

        static void ClassWork<T>(string type)
        {

        }

        // Действие для "Подопция 2"
        static void SimpleClass(string type)
        {
            int count = GetNumber();
            string[] subMenuOptions = { "Начать", "Назад" };
            Action[] subMenuActions = { () => Begin(count), BackToMainMenu };

            ShowMenu($"Выбранное количество элементов: {count}", subMenuOptions, subMenuActions);
        }

        static void Begin(int count)
        {
            switch (count)
            {
                case 0:
                    throw new Exception("Введен ноль, такого не может быть :(");
                case 1:

                    break;
                default:
                    break;
            }

        }

        static void DisplayCSVMenu()
        {
            string[] subMenuOptions = { "Работа с файлами", "Работа с классами", "Назад" };
            Action[] subMenuActions = { SubOption1, SubOption1, BackToMainMenu };

            ShowMenu("Дополнительное меню", subMenuOptions, subMenuActions);
        }

        // Действие для "Подопция 1"
        static void SubOption1()
        {
            Console.WriteLine("Вы выбрали 'Подопция 1'.");
            WaitForUser();
        }

        public static int GetNumber()
        {
            int number;
            string input;

            do
            {
                Console.Write("Введите количество элементов: ");
                input = Console.ReadLine();

            } while (!int.TryParse(input, out number) || number <= 0);

            return number;
        }

        static void BackToMainMenu()
        {
            Console.Clear();
            Console.WriteLine("Возвращаемся в главное меню...");
        }

        static void Exit()
        {
            Console.WriteLine("Выход из программы.");
            Environment.Exit(0);
        }

        static void WaitForUser()
        {
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}
