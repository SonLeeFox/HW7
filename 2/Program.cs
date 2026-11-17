using System;
using System.Text;

namespace Builder
{
    class Pizza
    {
        string dough;
        string sauce;
        string topping;
        string extraTopping;

        public Pizza() { }
        public void SetDough(string d) { dough = d; }
        public void SetSauce(string s) { sauce = s; }
        public void SetTopping(string t) { topping = t; }
        public void SetExtraTopping(string et) { extraTopping = et; }

        public void Info()
        {
            Console.WriteLine("Dough: {0}\nSauce: {1}\nTopping: {2}\nExtra Topping: {3}", dough, sauce, topping, extraTopping);
        }
    }

    // Abstract Builder
    abstract class PizzaBuilder
    {
        protected Pizza pizza;

        public PizzaBuilder() { }
        public Pizza GetPizza() { return pizza; }
        public void CreateNewPizza() { pizza = new Pizza(); }

        public abstract void BuildDough();
        public abstract void BuildSauce();
        public abstract void BuildTopping();
        public virtual void BuildExtraTopping() { }
    }

    // Concrete Builder
    class HawaiianPizzaBuilder : PizzaBuilder
    {
        public override void BuildDough() { pizza.SetDough("cross"); }
        public override void BuildSauce() { pizza.SetSauce("mild"); }
        public override void BuildTopping() { pizza.SetTopping("ham+pineapple"); }
    }

    // Concrete Builder
    class SpicyPizzaBuilder : PizzaBuilder
    {
        public override void BuildDough() { pizza.SetDough("pan baked"); }
        public override void BuildSauce() { pizza.SetSauce("hot"); }
        public override void BuildTopping() { pizza.SetTopping("pepperoni+salami"); }
    }

    // Concrete Builder
    class MargaritaPizzaBuilder : PizzaBuilder
    {
        public override void BuildDough() { pizza.SetDough("thin"); }
        public override void BuildSauce() { pizza.SetSauce("tomato"); }
        public override void BuildTopping() { pizza.SetTopping("mozzarella+basil"); }
        public override void BuildExtraTopping() { pizza.SetExtraTopping("mint"); }
    }

    // Director
    class Waiter
    {
        private PizzaBuilder pizzaBuilder;

        public void SetPizzaBuilder(PizzaBuilder pb)
        {
            if (pb == null)
            {
                throw new ArgumentNullException("pb");
            }
            pizzaBuilder = pb;
        }

        public Pizza GetPizza() { return pizzaBuilder.GetPizza(); }

        public void ConstructPizza()
        {
            if (pizzaBuilder == null)
            {
                throw new InvalidOperationException("Спочатку потрібно задати будівельника.");
            }
            pizzaBuilder.CreateNewPizza();
            pizzaBuilder.BuildDough();
            pizzaBuilder.BuildSauce();
            pizzaBuilder.BuildTopping();
            pizzaBuilder.BuildExtraTopping();
        }
    }

    // A customer ordering a pizza
    class BuilderExample
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Waiter waiter = new Waiter();
            PizzaBuilder[] builders =
            {
                new HawaiianPizzaBuilder(),
                new SpicyPizzaBuilder(),
                new MargaritaPizzaBuilder(),
            };

            foreach (PizzaBuilder builder in builders)
            {
                waiter.SetPizzaBuilder(builder);
                waiter.ConstructPizza();
                Pizza pizza = waiter.GetPizza();
                pizza.Info();
                Console.WriteLine();
            }

            Console.WriteLine("Натисніть Enter для виходу...");
            Console.ReadLine();
        }
    }
}
