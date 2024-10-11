using ReflectionSerialization.Serializators;
using ReflectionSerialization.TestData;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ReflectionSerialization
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            DisplayMainMenu();
        }

        public static void DisplayMainMenu()
        {
            string[] menuOptions = {
                "Работа с Json",
                "\t- Сериализовать класс в файл",
                "\t- Сериализовать класс в строку",
                "\t- Сериализовать класс в строку и десериализовать в класс",
                "Работа с CSV",
                "\t- Сериализовать класс в файл",
                "\t- Сериализовать класс в строку",
                "\t- Сериализовать класс в строку и десериализовать в класс",
                "\t- Десериализовать CSV файл",
                "- Выход"
            };

            Action[] menuActions = {
                null,
                () => SelectSerializatorClass("json", true, true, false),
                () => SelectSerializatorClass("json", true, false, false),
                () => SelectSerializatorClass("json", true, false, true),
                null,
                () => SelectSerializatorClass("csv", true, true, false),
                () => SelectSerializatorClass("csv", true, false, false),
                () => SelectSerializatorClass("csv", true, false, true),
                () => SelectSerializatorClass("csv", false, false, false),
                Exit
            };

            int[] headerIndices = [0, 4];
            ShowMenu("Главное меню", menuOptions, menuActions, 0, headerIndices);
        }

        public static void ShowMenu(string title, string[] options, Action[] actions, int currentSelection, int[] headerIndices)
        {
            Console.Clear();

            DisplayMainLabel();

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

                DisplayMainLabel();

                Console.WriteLine(title);
                Console.WriteLine(new string('-', title.Length));
            }
        }

        static void DisplayMainLabel()
        {
            var title = "Программа сериализации в CSV и десериализации из CSV";
            Console.WriteLine(title);
            Console.WriteLine(new string('-', title.Length));
            Console.WriteLine("\n");
        }

        static void SelectSerializatorClass(string type, bool isSerialize, bool toFile, bool isReverse)
        {
            List<ISerializator> serializators = SerializatorsFactory.GetSerializatorsByType(type);
            List<string> menuOptions = new List<string>();
            List<Action> menuActions = new List<Action>();
            serializators.ForEach(serializator =>
            {
                menuOptions.Add($"- {serializator.GetType().Name}");
                if (isSerialize)
                {
                    menuActions.Add(() => Serialize(type, serializator, toFile, isReverse));
                }
                else
                {
                    menuActions.Add(() => FileWorkMenu(type, serializator));
                }
            });
            menuOptions.Add("- В главное меню");
            menuActions.Add(BackToMainMenu);

            ShowMenu("Выберите тип сериализатора", menuOptions.ToArray(), menuActions.ToArray(), 0, []);
        }

        static void FileWorkMenu(string type, ISerializator serializator)
        {
            string[] menuOptions = { "- Большой класс Person", "- Маленький класс F", "- В главное меню" };
            Action[] menuActions =
            {
                () => FileWorkSelectFile<Person>(type, serializator),
                () => FileWorkSelectFile<F>(type, serializator),
                BackToMainMenu
            };

            ShowMenu($"Сериализатор: {serializator.GetType().Name}\nРабота с файлами {type}", menuOptions, menuActions, 0, []);
        }

        static void FileWorkSelectFile<T>(string type, ISerializator serializator) where T : new()
        {
            List<string> fileNames = GetFileNamesFromInputFolder(type);
            List<string> menuOptions = new List<string>();
            List<Action> menuActions = new List<Action>();
            fileNames.ForEach(fileName =>
            {
                menuOptions.Add($"- {fileName}");
                menuActions.Add(() => Deserialize<T>(fileName, serializator));
            });

            menuOptions.Add("- В главное меню");
            menuActions.Add(BackToMainMenu);

            if (fileNames.Any())
            {
                ShowMenu("Выберите файл:", menuOptions.ToArray(), menuActions.ToArray(), 0, []);
            }
            else
            {
                ShowMenu("Подходящих файлов - нет :(", menuOptions.ToArray(), menuActions.ToArray(), 0, []);
            }
        }

        static void Serialize(string type, ISerializator serializator, bool toFile, bool isReverse)
        {
            string[] menuOptions = { "- Большой класс Person", "- Маленький класс F", "- В главное меню" };
            Action[] menuActions =
            {
                () => ClassWork<Person>(type, serializator, toFile, isReverse),
                () => ClassWork<F>(type, serializator, toFile, isReverse),
                BackToMainMenu
            };

            ShowMenu($"Сериализатор: {serializator.GetType().Name}\nРабота с классами {type}", menuOptions, menuActions, 0, []);
        }

        static void Deserialize<T>(string fileName, ISerializator serializator) where T : new()
        {
            string label = "\nРезультаты:";
            var fileContents = ReadFileContent(fileName);
            var result = serializator.CheckIEnumerableDeserializationTime<T>(fileContents);

            Console.WriteLine($"\n{label}");
            Console.WriteLine(new string('-', label.Length));
            Console.WriteLine($"\nВремя десериализации: {result.DeserializationTime} ms");

            WaitForUser();
        }

        static void ClassWork<T>(string type, ISerializator serializator, bool toFile, bool isReverse) where T : new()
        {
            int count = GetNumber();
            string[] menuOptions = { "Выполнить", "Назад" };
            Action[] menuActions = { () => Begin<T>(count, serializator, toFile, isReverse), BackToMainMenu };

            ShowMenu($"Сериализатор: {serializator.GetType().Name}\nРабота с классами {type}\nВыбранное количество элементов: {count}", menuOptions, menuActions, 0, []);
        }

        static void Begin<T>(int count, ISerializator serializator, bool toFile, bool isReverse) where T : new() 
        {
            switch (count)
            {
                case 0:
                    throw new Exception("Введен ноль, такого не может быть :(");
                case 1:
                    ExecuteSerializeOne<T>(serializator, count, toFile, isReverse);
                    break;
                default:
                    ExecuteSerializeMany<T>(serializator, count, toFile, isReverse);
                    break;
            }
            WaitForUser();
        }

        static void ExecuteSerializeOne<T>(ISerializator serializator, int count, bool toFile, bool isReverse) where T : new()
        {
            string label = $"Результаты по {count} элементу:";
            var obj = new T();
            var result = serializator.CheckSerializationTime(obj);
            if (toFile)
            {
                WriteStringToFile(result.SerializedString, $"serealized_{DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss")}_{obj.GetType().Name}", $".{serializator.SerializatorType}");
            }
            else
            {

                Console.WriteLine($"\n{label}");
                Console.WriteLine(new string('-', label.Length));
                Console.WriteLine("Сериализованная строка:");
                Console.WriteLine(result.SerializedString);
                Console.WriteLine($"\nВремя сериализации: {result.SerializationTime} ms");
            }

            if (isReverse)
            {
                label = "\nРезультаты обратного действия:";
                var reverseResult = serializator.CheckDeserializationTime<T>(result.SerializedString);

                Console.WriteLine($"\n{label}");
                Console.WriteLine(new string('-', label.Length));
                Console.WriteLine($"\nВремя десериализации: {reverseResult.DeserializationTime} ms");
            }
        }

        static void ExecuteSerializeMany<T>(ISerializator serializator, int count, bool toFile, bool isReverse) where T : new()
        {
            string label = $"Результаты по {count} элементам:";
            var objects = new List<T>();
            for (int i = 0; i < count; i++)
            {
                objects.Add(new T());
            }
            var result = serializator.CheckIEnumerableSerializationTime(objects);
            if (toFile)
            {
                WriteStringToFile(result.SerializedString, $"serealized_{DateTime.Now.ToString("yyyy.MM.ddHH.mm.ss")}_List_{objects.FirstOrDefault().GetType().Name}", $".{serializator.SerializatorType}");
            }
            else
            {
                Console.WriteLine($"\n{label}");
                Console.WriteLine(new string('-', label.Length));
                Console.WriteLine("Сериализованная строка:");
                Console.WriteLine(result.SerializedString);
                Console.WriteLine($"\nВремя сериализации: {result.SerializationTime} ms");
                Console.WriteLine($"\nСреднее время сериализации одного элемента: {result.AverageSerializationTime} ms");
            }

            if (isReverse)
            {
                label = "\nРезультаты обратного действия:";
                var reverseResult = serializator.CheckIEnumerableDeserializationTime<T>(result.SerializedString);

                Console.WriteLine($"\n{label}");
                Console.WriteLine(new string('-', label.Length));
                Console.WriteLine($"\nВремя десериализации: {reverseResult.DeserializationTime} ms");
            }
        }

        public static void WriteStringToFile(string content, string fileName, string fileExtension)
        {
            string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, fileName + fileExtension);

            File.WriteAllText(filePath, content);

            Console.WriteLine($"\nСтрока успешно записана в файл: \n{filePath}");
        }

        static List<string> GetFileNamesFromInputFolder(string type)
        {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Input");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            List<string> fileNames = new List<string>();
            try
            {
                fileNames = Directory.GetFiles(folderPath)
                    .Where(file => file.EndsWith($".{type}", StringComparison.OrdinalIgnoreCase))
                    .Select(Path.GetFileName)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении файлов: {ex.Message}");
            }

            return fileNames;
        }

        public static string ReadFileContent(string fileName)
        {
            string inputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Input");

            if (!Directory.Exists(inputDirectory))
            {
                Directory.CreateDirectory(inputDirectory);
            }

            string filePath = Path.Combine(inputDirectory, fileName);

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
                return string.Empty;
            }

            return File.ReadAllText(filePath);
        }

        public static int GetNumber()
        {
            int number;
            string input;

            do
            {
                Console.Write("\nВведите количество элементов: ");
                input = Console.ReadLine();

            } while (!int.TryParse(input, out number) || number <= 0);

            return number;
        }

        static void BackToMainMenu()
        {
            DisplayMainMenu();
        }

        static void Exit()
        {
            Console.WriteLine("\nВыход из программы.");
            Environment.Exit(0);
        }

        static void WaitForUser()
        {
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}
