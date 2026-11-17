using System;
using System.Collections.Generic;
using System.Text;

namespace Composite
{
    // Component
    abstract class Component
    {
        protected string name;

        public Component(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Назва не може бути порожньою.");
            }
            this.name = name;
        }

        public abstract int GetSize();
        public abstract void Show(int level);
    }

    // Leaf
    class FileItem : Component
    {
        private int size;

        public FileItem(string name, int size) : base(name)
        {
            if (size < 0)
            {
                throw new ArgumentException("Розмір не може бути від'ємним.");
            }
            this.size = size;
        }

        public override int GetSize()
        {
            return size;
        }

        public override void Show(int level)
        {
            Console.WriteLine("{0}{1} ({2} КБ)", new string(' ', level * 2), name, size);
        }
    }

    // Leaf
    class Shortcut : Component
    {
        public Shortcut(string name) : base(name)
        {
        }

        public override int GetSize()
        {
            return 1;
        }

        public override void Show(int level)
        {
            Console.WriteLine("{0}{1} (ярлик, 1 КБ)", new string(' ', level * 2), name);
        }
    }

    // Composite
    class Folder : Component
    {
        private List<Component> items = new List<Component>();

        public Folder(string name) : base(name)
        {
        }

        public void Add(Component component)
        {
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }
            if (component == this)
            {
                throw new ArgumentException("Папку не можна додати саму в себе.");
            }
            items.Add(component);
        }

        public void Remove(Component component)
        {
            items.Remove(component);
        }

        public override int GetSize()
        {
            int sum = 0;
            foreach (Component c in items)
            {
                sum += c.GetSize();
            }
            return sum;
        }

        public override void Show(int level)
        {
            Console.WriteLine("{0}[{1}] ({2} КБ)", new string(' ', level * 2), name, GetSize());
            foreach (Component c in items)
            {
                c.Show(level + 1);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Folder disk = new Folder("Диск D");

            Folder docs = new Folder("Документи");
            docs.Add(new FileItem("lab1.docx", 120));
            docs.Add(new FileItem("report.pdf", 340));

            Folder photos = new Folder("Фото");
            photos.Add(new FileItem("photo1.jpg", 2048));
            photos.Add(new Shortcut("Ярлик на Документи"));

            disk.Add(docs);
            disk.Add(photos);

            disk.Show(0);
            Console.WriteLine("Загальний розмір: {0} КБ", disk.GetSize());

            Console.WriteLine();
            Console.WriteLine("Натисніть Enter для виходу...");
            Console.ReadLine();
        }
    }
}
