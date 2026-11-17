using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChristmasTreeDecorator
{
    // Component
    abstract class Tree
    {
        public abstract string GetDescription();
        public abstract void Shine();
    }

    // ConcreteComponent
    class ChristmasTree : Tree
    {
        public override string GetDescription()
        {
            return "Ялинка";
        }

        public override void Shine()
        {
            Console.WriteLine("Ялинка стоїть у кімнаті");
        }
    }

    // Decorator
    abstract class TreeDecorator : Tree
    {
        protected Tree tree;

        public TreeDecorator(Tree tree)
        {
            if (tree == null)
            {
                throw new ArgumentNullException("tree");
            }
            this.tree = tree;
        }
    }

    // ConcreteDecorator
    class OrnamentDecorator : TreeDecorator
    {
        private string ornament;

        public OrnamentDecorator(Tree tree, string ornament) : base(tree)
        {
            if (string.IsNullOrWhiteSpace(ornament))
            {
                throw new ArgumentException("Назва прикраси не може бути порожньою.");
            }
            this.ornament = ornament;
        }

        public override string GetDescription()
        {
            return tree.GetDescription() + ", прикраса: " + ornament;
        }

        public override void Shine()
        {
            tree.Shine();
        }
    }

    // ConcreteDecorator
    class GarlandDecorator : TreeDecorator
    {
        private string color;
        private bool isShining;

        public GarlandDecorator(Tree tree, string color) : base(tree)
        {
            if (string.IsNullOrWhiteSpace(color))
            {
                throw new ArgumentException("Колір гірлянди не може бути порожнім.");
            }
            this.color = color;
            this.isShining = Random.Shared.Next(2) == 1;
        }

        public override string GetDescription()
        {
            return tree.GetDescription() + ", гірлянда: " + color;
        }

        public override void Shine()
        {
            tree.Shine();
            if (isShining)
            {
                Console.WriteLine("Гірлянда (" + color + ") світиться");
            }
            else
            {
                Console.WriteLine("Гірлянда (" + color + ") не світиться");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Tree tree = new ChristmasTree();

            Console.WriteLine("Оберіть режим:");
            Console.WriteLine("1 - Наряджати вручну");
            Console.WriteLine("2 - Випадково дістати з коробки");
            Console.Write("Ваш вибір: ");
            string mode = Console.ReadLine()?.Trim();

            if (mode == "2")
            {
                tree = DecorateFromBox(tree);
            }
            else
            {
                tree = DecorateManually(tree);
            }

            Console.WriteLine();
            Console.WriteLine(tree.GetDescription());
            tree.Shine();

            Console.WriteLine();
            Console.WriteLine("Натисніть Enter для виходу...");
            Console.ReadLine();
        }

        // Наряджання випадковими елементами зі списків користувача
        static Tree DecorateFromBox(Tree tree)
        {
            Console.WriteLine("\nВведіть прикраси в коробку через кому (наприклад: кулька, зірка, шишка, бурулька):");
            List<string> ornamentBox = ReadItemsList();

            Console.WriteLine("Введіть кольори гірлянд у коробку через кому (наприклад: червона, синя, жовта, біла):");
            List<string> garlandBox = ReadItemsList();

            int ornamentsToHang = ReadCount($"Скільки прикрас повісити? (у коробці є {ornamentBox.Count}): ", ornamentBox.Count);
            int garlandsToHang = ReadCount($"Скільки гірлянд повісити? (у коробці є {garlandBox.Count}): ", garlandBox.Count);

            // Перемішуємо коробку та беремо потрібну кількість без дублікатів
            var selectedOrnaments = ornamentBox.OrderBy(_ => Random.Shared.Next()).Take(ornamentsToHang);
            foreach (var ornament in selectedOrnaments)
            {
                tree = new OrnamentDecorator(tree, ornament);
            }

            var selectedGarlands = garlandBox.OrderBy(_ => Random.Shared.Next()).Take(garlandsToHang);
            foreach (var color in selectedGarlands)
            {
                tree = new GarlandDecorator(tree, color);
            }

            return tree;
        }

        // Парсинг рядка в список (роздільник - кома)
        static List<string> ReadItemsList()
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    var items = input.Split(',')
                                     .Select(x => x.Trim())
                                     .Where(x => !string.IsNullOrEmpty(x))
                                     .ToList();
                    if (items.Count > 0)
                        return items;
                }
                Console.Write("Список не може бути порожнім. Спробуйте ще раз: ");
            }
        }

        // Валідація кількості в межах наявного в коробці
        static int ReadCount(string prompt, int maxAvailable)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int count) && count >= 0 && count <= maxAvailable)
                {
                    return count;
                }
                Console.WriteLine($"Помилка: введіть число від 0 до {maxAvailable}.");
            }
        }

        // Ручне додавання декораторів
        static Tree DecorateManually(Tree tree)
        {
            while (true)
            {
                Console.WriteLine("1 - додати прикрасу, 2 - додати гірлянду, 0 - завершити");
                Console.Write("Ваш вибір: ");
                string choice = Console.ReadLine();

                if (choice == null || choice.Trim() == "0")
                {
                    break;
                }

                if (choice.Trim() == "1")
                {
                    Console.Write("Яка прикраса (наприклад, кулька): ");
                    string name = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        Console.WriteLine("Помилка: назва не може бути порожньою.");
                        continue;
                    }
                    tree = new OrnamentDecorator(tree, name.Trim());
                }
                else if (choice.Trim() == "2")
                {
                    Console.Write("Колір гірлянди: ");
                    string color = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(color))
                    {
                        Console.WriteLine("Помилка: колір не може бути порожнім.");
                        continue;
                    }
                    tree = new GarlandDecorator(tree, color.Trim());
                }
                else
                {
                    Console.WriteLine("Помилка: оберіть 0, 1 або 2.");
                }
            }
            return tree;
        }
    }
}