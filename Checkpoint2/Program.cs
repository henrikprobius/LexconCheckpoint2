
var key = ConsoleKey.Q;
do
{
    Console.Clear();
    Console.WriteLine("Press F2 to add products"+"\n" + "Press F3 to print all products" + "\n" + "Press F4 to search" + "\n"+ "Press q to quit");
    key = Console.ReadKey().Key;
    if (key == ConsoleKey.F2) { EnterProducts(); }
    else if (key == ConsoleKey.F3) { ProductStore.PrintList(); }
    else if (key == ConsoleKey.F4) { SearchProduct(); }
        
} while (key != ConsoleKey.Q);

/************************************************ methods area **************************************/
void SearchProduct()
{
    String? indata;
    int padding = 20;
    Console.Clear();
    Console.Write("Enter searchstring: ");
    indata = Console.ReadLine();
    var list = ProductStore.Search(indata);
    if (list.Count() > 0)
    {
        foreach (var (key, product) in list)
            Console.WriteLine(product.Name.PadRight(padding) + product.Category.PadRight(padding) + product.Price.ToString().PadRight(padding));
            
    }
    else Console.WriteLine("The productlist is empty");
    Console.WriteLine("\n"+"Press any key to go back to menu");
    Console.ReadKey();
}

    void EnterProducts()
{
    bool doRun = true;
    (bool success, string msg) msg;
    String? indata;
    String[]? strings;

    Console.Clear();
    Console.WriteLine("Enter data, separate with comma like this: product1,category1,23. Enter q to quit");
    do { 
        indata = Console.ReadLine();
        if (indata is null) continue;
        if ((indata.Trim().ToLower() == "q")) doRun = false;
        else
        {   try
            {
                strings = StringExtensions.Split(indata, ",");
                msg = ProductStore.AddProduct(new(strings[0], strings[1], strings[2]));
                Console.WriteLine(msg.msg + "\n");
            }catch (Exception ex) { Console.WriteLine(ex.Message); }

        }
    } while (doRun) ;
}


/************************************************ class area **************************************/
class Product
{
    private string name;
    private string category;
    private int price; 

    public Product(string name, string category, int price)
    {
        Name = name;
        Category = category;
        Price = price;
    }

    public Product(string name, string category, string price)
    {
        Name = name;
        Category = category;
        Price = int.Parse(price);
    }


    public string Name 
    { 
        get { return name; }
        set {
            name = value.FirstCharToUpper(); 
        }
    }    
    public string? Category {
        get { return category; }
        set {category = value is null?"":value;}
    } 
    public int Price {
        get { return price; } 
        set {
            if (value < 0) throw new ArgumentException($"{value} is less than 0");
            price = value;

        } }


}//class Product

class ProductStore
{ 
    private static SortedList<String,Product> products = new();

    public static (bool,string) AddProduct(Product p)
    {
        if(!products.ContainsKey(p.Name))
        {
            products.Add(p.Name, p);
            return (true, "Product added successfully");
        }
        return (false, $"Product {p.Name} already exists, try again!");
    }

    public static void PrintList()
    {
        System.Console.Clear();
        if (products.Count > 0)
        {
            int padding = 20;
            Console.WriteLine("Name".PadRight(padding) + "Category".PadRight(padding) + "Price".PadRight(padding));
            foreach (var (key, product) in products)
            {
                Console.WriteLine(product.Name.PadRight(padding) +product.Category.PadRight(padding) +product.Price.ToString().PadRight(padding));
            }
            Console.WriteLine("\nSum Price: " + products.Values.Sum(x => x.Price).ToString()+ "\n");
        }
        else Console.WriteLine("The productlist is empty");
        Console.WriteLine("Press any key to go back to menu");
        Console.ReadKey();

    }

    public static IEnumerable<KeyValuePair<String, Product>> Search(string search)
    {   
        if(search is not null && search.Trim() != "")
            return products.Where(product => product.Key.StartsWith(search, true, null)).AsEnumerable();
        return new SortedList<String, Product>();

    }

    public static void TestData()
    {
          AddProduct(new Product("pridd1","Category1", 56));
          AddProduct(new Product("PRodd1", "Category1", 34));  //!!dubblett
          AddProduct(new Product("prod2", "Category1", 16));
          AddProduct(new Product("Prod3", "Category1", 19));
          AddProduct(new Product("bBBProd4", "Category2", 516));
          AddProduct(new Product("Produ5", "Category2", 544));
          AddProduct(new Product("CCProd6", "Category3", 516));
          AddProduct(new Product("Produkt7", "Category3", 5467));
          AddProduct(new Product("aAAProdukt8", "Category3", 110));
          AddProduct(new Product("Pridukt9", "Category2", 9999));
          AddProduct(new Product("dProdukt10jJj", "Category1", 1101));
          AddProduct(new Product("88dProdukt10jJj", "Category1", 55108));
        AddProduct(new Product(" Testar11trodukt1Jj ", "Category1", 1601));
        AddProduct(new Product("  Testar22trodujJj  ", "Category1", 1108));
        AddProduct(new Product("Testar22trodujJj", "Category4", 1108));  //dubblett


        /*Exceptions*/
        //nedan ej fixad
        //try { AddProduct(new Product("  ", "Category1", -34)); } catch (Exception e) { Console.WriteLine(e.Message); }
        //om Category är tomt eller null??

        // try { AddProduct(new Product("88dProdukt10jJj", "Category1", "")); } catch (Exception e) { Console.WriteLine(e.Message); }
        //try { AddProduct(new Product("88dProdukt10jJj", "Category1", -34)); } catch (Exception e) { Console.WriteLine(e.Message); }
    }
}// class ProductStore


public static class StringExtensions
{
    public static string FirstCharToUpper(this string input) =>
    input switch
    {
            null => throw new ArgumentNullException("Data cannot be empty"),
            "" => throw new ArgumentException($"Data entered in wrong format, try again", nameof(input)),
            _  => string.Concat(input.Trim()[0].ToString().ToUpper(), input.Trim().ToLower().AsSpan(1))
        };

    public static String[] Split(String value, String splitter)
    {
        var list = value.Split(splitter, StringSplitOptions.TrimEntries);
        if (list.Length != 3) throw new ArgumentNullException("Wrong format entered, try again");
        return list;    
    }
} //class StringExtensions



