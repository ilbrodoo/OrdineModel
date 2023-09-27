using System;
using System.Linq;

namespace OrdineModel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var ctx = new ordersEntities1();

            bool Login = false;


            while (!Login)
            {

                Console.WriteLine("Salve , Scegli azione da fare : \n (1) - Login \n (2) - Registrati ");
                var x = Console.ReadLine();
                Login = Menu(ctx, Login, x);
            }
            while (true)
            {
                MenuPrincipale(ctx);

            }
        }

        private static void MenuPrincipale(ordersEntities1 ctx)
        {
            Console.WriteLine("Premi invio per continuare ");
            Console.ReadLine();
            Console.Clear(); var prezzoTot = 0;
            Console.WriteLine("Login Effettuato , Cosa desidera?\n (1) Visualizza Lista Ordini (2) Dettaglio Ordine (3) Creazione nuovo ordine  (4) Esci ");
            var p = Console.ReadLine();

            switch (p)
            {
                case "1":
                    foreach (order order in ctx.orders)
                        prezzoTot = SelectOrdini(ctx, order);

                    break;
                case "2":
                    DettagliOrdine(ctx);

                    break;
                case "3":
                    Console.WriteLine("Quale Customer ha ordinato ?");
                    foreach (customer c in ctx.customers)
                    {
                        Console.WriteLine($"\t -> {c.customer1}");
                    }
                    string customerChoice = Console.ReadLine();

                    var selectedCustomer = ctx.customers.FirstOrDefault(c => c.customer1 == customerChoice);

                    while (selectedCustomer == null)
                    {
                        Console.WriteLine("Customer non esistente. Inserisci nuovamente:");
                        customerChoice = Console.ReadLine();
                        selectedCustomer = ctx.customers.FirstOrDefault(c => c.customer1 == customerChoice);
                    }

                    Console.WriteLine("-------------------------------------------------------------------------------------------------------");
                    Console.WriteLine("Inserisci Quantità");
                    int qty = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("-------------------------------------------------------------------------------------------------------");
                    Console.WriteLine("Inserisci Prezzo");
                    int price = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("-------------------------------------------------------------------------------------------------------");

                    Console.ReadLine();
                    break;
                case "4":
                    Environment.Exit(0);
                    break;
            }
        }

        private static int SelectOrdini(ordersEntities1 ctx, order order)
        {
            int prezzoTot;
            {
                prezzoTot = 0;
                Console.WriteLine($"Id  : {order.orderid}  - Data : {order.orderdate} - Customer {order.customer}");
                foreach (orderitem oi in ctx.orderitems)
                {
                    if (order.orderid == oi.orderid)
                    {
                        Console.WriteLine($" - Item : {oi.item} - Prezzo {oi.price}");
                        prezzoTot = prezzoTot + oi.price;

                    }

                }
                Console.WriteLine($"------------- Prezzo Totale : {prezzoTot}  -----------------");


            }

            return prezzoTot;
        }

        private static void DettagliOrdine(ordersEntities1 ctx)
        {
            Console.WriteLine("Inserisci id dell'ordine ");
            int id = Convert.ToInt32(Console.ReadLine());
            foreach (orderitem oi in ctx.orderitems)
            {
                if (oi.orderid == id)
                {
                    Console.WriteLine($" Id : {oi.orderid} - Item : {oi.item} - Quantità e prezzo: {oi.qty}x{oi.price}");
                    foreach (order order in ctx.orders)
                    {
                        if (order.orderid == oi.orderid)
                        {
                            Console.WriteLine($"\t Customer - {order.customer} \t- data : {order.orderdate}");
                        }
                    }
                }
            }
        }

        private static bool Menu(ordersEntities1 ctx, bool Login, string x)
        {
            bool userexits = false;
            switch (x)
            {
                case "1":



                    Console.WriteLine("Inserisci Username e Password ");
                    string user = Console.ReadLine();
                    string passw = Console.ReadLine();


                    foreach (Utenti utenti in ctx.Utentis)
                    {
                        if (utenti.username.Equals(user) && utenti.password.Equals(passw))
                        {
                            Login = true;

                            break;
                        }
                        else
                        {

                            Login = false;
                        }
                    }

                    break;

                case "2":

                    string pw;
                    bool userExists = false; // Inizializza la variabile a false
                    Console.WriteLine("Inserisci username e password ");
                    user = Console.ReadLine();
                    Console.WriteLine("password");
                    pw = Console.ReadLine();

                    foreach (Utenti utenti in ctx.Utentis)
                    {
                        if (utenti.username == user && utenti.password.Equals(pw))
                        {
                            Console.WriteLine("Utente già esistente, Registrazione non possibile");
                            userExists = true;
                            break; // Esci dal ciclo una volta che hai trovato un utente corrispondente
                        }
                    }

                    if (!userExists) // Verifica se l'utente non esiste
                    {
                        Register(user, pw, ctx);
                    }



                    break;

                default:

                    Console.WriteLine("Azione non esistente");
                    break;
            }

            return Login;
        }

        private static void Register(string user, string pw, ordersEntities1 ctx)
        {

            var newL = new Utenti()
            {
                username = user,
                password = pw
            };
            ctx.Utentis.Add(newL);
            ctx.SaveChanges();
        }
    }
}



