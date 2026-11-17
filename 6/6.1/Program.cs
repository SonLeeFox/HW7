using System;
using System.Text;

namespace FactoryMethod
{
    // Product
    abstract class Transport
    {
        public abstract void Deliver();
    }

    // ConcreteProduct
    class Truck : Transport
    {
        public override void Deliver()
        {
            Console.WriteLine("Вантажівка везе товар дорогою");
        }
    }

    // ConcreteProduct
    class Ship : Transport
    {
        public override void Deliver()
        {
            Console.WriteLine("Корабель везе товар морем");
        }
    }

    // ConcreteProduct
    class Plane : Transport
    {
        public override void Deliver()
        {
            Console.WriteLine("Літак везе товар повітрям");
        }
    }

    // Creator
    abstract class Logistics
    {
        public abstract Transport CreateTransport();

        public void PlanDelivery()
        {
            Transport transport = CreateTransport();
            transport.Deliver();
        }
    }

    // ConcreteCreator
    class RoadLogistics : Logistics
    {
        public override Transport CreateTransport()
        {
            return new Truck();
        }
    }

    // ConcreteCreator
    class SeaLogistics : Logistics
    {
        public override Transport CreateTransport()
        {
            return new Ship();
        }
    }

    // ConcreteCreator
    class AirLogistics : Logistics
    {
        public override Transport CreateTransport()
        {
            return new Plane();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Logistics[] logistics =
            {
                new RoadLogistics(),
                new SeaLogistics(),
                new AirLogistics()
            };

            foreach (Logistics l in logistics)
            {
                l.PlanDelivery();
            }

            Console.WriteLine();
            Console.WriteLine("Натисніть Enter для виходу...");
            Console.ReadLine();
        }
    }
}
