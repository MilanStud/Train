using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Train1;

class Locomotive 
{
    private Person driver;
    private Engine engine;
    public Engine Engine
    {
        get { return engine; }
        set { engine = value; }
    }

    public Locomotive(){}

    public Locomotive(Person driver, Engine engine)
    {
        this.driver = driver;
        this.engine = engine;
    }
    public override string ToString()
    {
        return "Lokomotiva: masinfuhrer: "+ driver.ToString() +", "+ engine.ToString() +"\n"; 
    }
}

class Engine 
{
    private string type; //diesel, eletric, steam
    public string Type   
    {
        get { return type; }
        set { type = value; }
    }

    public Engine(string type)
    {
        this.type = type;
    }

    public override string ToString()
    {
        return "trakce: "+ type ;
    }
}

class Person
{
    private string firstName;
    private string lastName;

    public string FirstName
    {
        get { return firstName; }
        set { firstName = value; }
    }

    public string LastName
    {
        get { return lastName; }
        set { lastName = value; }
    }

    public Person(string firstName, string lastName)
    {
        this.firstName = firstName;
        this.lastName = lastName;
    }
    
    public override string ToString()
    {
        return FirstName +" " + LastName ;
    }
}

class Chair
{
    private bool nearWindow;
    private int number;
    private bool reserved;

    public bool NearWindow
    {
        get { return nearWindow; }
        set { nearWindow = value; }
    }

    public int Number
    {
        get { return number; }
        set { number = value; }
    }

    public bool Reserved
    {
        get { return reserved; }
        set { reserved = value; }
    }

    public Chair(bool reserved, int number)
    {
        this.reserved = reserved;
        this.number = number;
    }
}

class Bed
{
    private int number;
    private bool reserved;

    public int Number
    {
        get { return number; }
        set { number = value; }
    }

    public bool Reserved
    {
        get { return reserved; }
        set { reserved = value; }
    }

    public Bed()
    {
    }
}

class Door
{
    private double height;
    private double width;

    public double Height
    {
        get { return height; }
        set { height = value; }
    }
    public double Width
    {
        get { return width; }
        set { width = value; }
    }

    public Door(double height, double width)
    {
        this.height = height;
        this.width = width;
    }
}

interface IWagon {
   // public void wagonType();
    public void ConnectToTrain(Train train);
    public void DisconnectFromTrain(Train train);
}

 class PersonalWagon
{
    private List<Door> doors = new List<Door>();
    public List<Chair> listChairs = new List<Chair>();
    //public List<Chair> Chairs
    //{
    //    get { return chairs; }
    //    set { chairs = value; }
    //}

    private int numberOfChairs;
    public int NumberOfChairs
    {
        get { return numberOfChairs; }
        set { numberOfChairs = value; }
    }
    public PersonalWagon(int numberOfChairs)
    {
        this.numberOfChairs = numberOfChairs;
        for(int i = 0;i < numberOfChairs; i++) {
            listChairs.Add(new Chair(false,i));
        }
    }
   
}

class Economy : PersonalWagon, IWagon
{
    public Train train;
    public Economy(int numberOfChairs) : base(numberOfChairs)
    {
    }

    public void ConnectToTrain(Train readyTrain) {
        this.train = readyTrain;
        this.train.wagons.Add(this);
        
    }

    public void DisconnectFromTrain(Train readyTrain) {
        this.train = readyTrain;
        this.train.wagons.Remove(this);
    }

    public override string ToString()
    {
        return "trida Economy, pocet sedadel:" + NumberOfChairs;
    }

}

class Business : PersonalWagon, IWagon
{
    public Train train;
    private Person steward;

    public Business(Person steward,int numberOfChairs) : base(numberOfChairs)
    {
        this.steward = steward;
    }

    public void ConnectToTrain(Train readyTrain) {
        this.train = readyTrain;
        this.train.wagons.Add(this);

    }

    public void DisconnectFromTrain(Train readyTrain) {
        this.train = readyTrain;
        this.train.wagons.Remove(this);
    }

    public override string ToString()
    {
        return "Trida Business, steward: " + steward + " pocet sedadel:" + NumberOfChairs;
    }


}

class NightWagon : PersonalWagon, IWagon
{
    public Train train;
    private Bed[] beds;
    private int numberOfBeds;

    public Bed[] Beds
    {
        get { return beds; }
        set { beds = value; }
    }

    public int NumberOfBeds
    {
        get { return numberOfBeds; }
        set { numberOfBeds = value; }
    }

    public NightWagon(int numberOfBeds, int numberOfChairs) : base(numberOfChairs)
    {
        this.numberOfBeds = numberOfBeds;   
    }

    public override string ToString()
    {
        return "Spaci vuz, pocet luzek: "+ numberOfBeds +" pocet sedadel:" + NumberOfChairs;
    }

    public void ConnectToTrain(Train readyTrain) {
        this.train = readyTrain;
        this.train.wagons.Add(this);

    }

    public void DisconnectFromTrain(Train readyTrain) {
        this.train = readyTrain;
        this.train.wagons.Remove(this);
    }
}

class Hopper : IWagon
{
    public Train train;
    private double loadingCapacity;
    public double LoadingCapacity
    {
        get { return loadingCapacity; }
        set { loadingCapacity = value; }
    }

    public Hopper(double loadingCapacity)
    {
        this.loadingCapacity = loadingCapacity;
    }
    public override string ToString()
    {
        return "Nakladni vuz typu hooper, kapacita: "+ loadingCapacity;
    }

    public void ConnectToTrain(Train readyTrain) {
        this.train = readyTrain;
        this.train.wagons.Add(this);
         
    }

    public void DisconnectFromTrain(Train readyTrain) {
        this.train = readyTrain;
        this.train.wagons.Remove(this);
    }
}

class Train
{
    public Locomotive locomotive;
    public List<IWagon> wagons = new List<IWagon>();
    
    public Train(){}

    public Train(Locomotive locomotive)
    {this.locomotive = locomotive;}

    public Train(Locomotive locomotive, List<IWagon> wagons) : this(locomotive) {this.wagons = wagons;}

    public void ConnectWagon(IWagon wagon)
    {
        string s = locomotive.Engine.Type;
        try {
            if (s == "parni" && wagons.Count >= 5) {
                throw new Exception("Parni lokomotiva nesmi mit vice nez 5 vagonu");
            }
            else {
                wagons.Add(wagon);
            }

        }
        catch (Exception e) {
            Console.WriteLine(e.ToString());
        }

    }
    public void DisconnectWagon(IWagon wagon) {wagons.Remove(wagon);}

    public void ReserveChair(int wagonNumber, int chairNumber) {
       
        if(wagons[wagonNumber].GetType().Name != "Hopper") {
            bool freeChair = true;
            for (int i = 0; i < (wagons[wagonNumber] as PersonalWagon).listChairs.Count; i++) {

                if ((wagons[wagonNumber] as PersonalWagon).listChairs[i].Reserved && (wagons[wagonNumber] as PersonalWagon).listChairs[i].Number == chairNumber) {
                    freeChair = false;  
                }
            }

            if(freeChair) {
                (wagons[wagonNumber] as PersonalWagon).listChairs[chairNumber].Reserved = true;
            }
            else {
                Console.WriteLine("! ! ! Misto cislo "+ chairNumber  + " vozu " + wagonNumber + " je jiz rezervovano ! ! !");
            }   
        }
        else {
            Console.WriteLine("! ! ! Do vozu cislo " + wagonNumber + " typu hopper neni mozne rezervovat miston ! ! !");
        }
    }

    public void ListReservedChairs()
    {
        string s;

        for(int j=0; j < wagons.Count; j++) {
            if(wagons[j].GetType().Name != "Hopper") 
                {
                Console.WriteLine("Cislo vozu: " + j + " " + wagons[j].GetType().Name +  "\n");
                for (int i = 0; i < (wagons[j] as PersonalWagon).listChairs.Count; i++) {
                    
                    if((wagons[j] as PersonalWagon).listChairs[i].Reserved) {
                        Console.WriteLine((wagons[j] as PersonalWagon).listChairs[i].Number);
                    }
 
                }
                Console.Write("\n");
            }
        }
    }

    public override string ToString()
    {
        String s = locomotive.ToString();
        foreach (IWagon wagon in wagons) 
        { 
            s = s + wagon.ToString() +"\n"; 
        }
        return s;
    }
}
class TrainRun
{
    public static void Main()
    {
        Person steward1 = new Person("Lenka", "Kozáková");
        Economy economy1 = new Economy(10);
        Economy economy2 = new Economy(10);
        Economy economy3 = new Economy(10);
        Economy economy4 = new Economy(10);
        Economy economy5 = new Economy(10);
        Economy economy6 = new Economy(10);
        Business business1 = new Business(steward1,10);
        NightWagon nightWagon1 = new NightWagon(10, 10);
        Hopper hooper1 = new Hopper(150.0);

        Person maschinFuhrer1 = new Person("Karel", "Novak");
        Engine engine1 = new Engine("diesel"); //parni
        Locomotive locomotive1 = new Locomotive(maschinFuhrer1, engine1);
         
        Train train1 = new Train(locomotive1) ;
        //tady bude pridavani vagonu k vlaku
        economy1.ConnectToTrain(train1);
        economy2.ConnectToTrain(train1);
        business1.ConnectToTrain(train1);
        economy4.ConnectToTrain(train1);
        nightWagon1.ConnectToTrain(train1);
        hooper1.ConnectToTrain(train1);
        Console.WriteLine(train1);
        //Console.WriteLine("-----------------------------");
        //train1.ConnectWagon(nightWagon1);
        //Console.WriteLine(train1);
        //Console.WriteLine("-----------------------------");
        //train1.DisconnectWagon(economy1);
        //Console.WriteLine(train1);
        Console.WriteLine("---------------------------");
        Console.WriteLine("Rezervace mist vlaku");
        Console.WriteLine("---------------------------");
        train1.ReserveChair(0, 1);
        train1.ReserveChair(1, 8);
        train1.ReserveChair(2, 5);
        train1.ReserveChair(2, 4);
        train1.ReserveChair(3, 7);
        train1.ReserveChair(4, 3);
        train1.ReserveChair(5, 4);
        train1.ReserveChair(0, 1);
        train1.ListReservedChairs();
    }
}
