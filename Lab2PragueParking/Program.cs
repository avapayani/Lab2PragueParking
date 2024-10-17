//Det är denna koden vi använder nu

using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        ParkingGarage garage = new ParkingGarage();
        bool exit = false;

        while (!exit)
        {
            ShowMenu();
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Ange fordonstyp (CAR/MC):");
                    string vehicleType = Console.ReadLine().ToUpper();

                    Console.WriteLine("Ange registreringsnummer:");
                    string registrationNumber = Console.ReadLine().ToUpper();

                    garage.ParkVehicle(vehicleType, registrationNumber);
                    break;

                case "2":
                    garage.ShowParkingLot();
                    break;

                case "3":
                    Console.WriteLine("Ange registreringsnummer för fordonet som ska flyttas:");
                    string regToMove = Console.ReadLine().ToUpper();

                    Console.WriteLine("Ange ny plats att flytta fordonet till:");
                    int newSpot = int.Parse(Console.ReadLine());

                    garage.MoveVehicle(regToMove, newSpot);
                    break;

                case "4":
                    Console.WriteLine("Ange registreringsnummer för fordonet du söker:");
                    string regToFind = Console.ReadLine().ToUpper();

                    garage.FindVehicle(regToFind);
                    break;

                case "5":
                    Console.WriteLine("Ange registreringsnummer för fordonet som ska tas bort:");
                    string regToRemove = Console.ReadLine().ToUpper();
                    garage.RemoveVehicle(regToRemove);
                    break;

                case "6":
                    exit = true;
                    Console.WriteLine("Avslutar programmet...");
                    break;

                default:
                    Console.WriteLine("Felaktigt val, försök igen.");
                    break;
            }
        }
    }

    // Metod för att visa menyn
    static void ShowMenu()
    {
        Console.WriteLine("\n--- PARKERINGSGARAGE MENY ---");
        Console.WriteLine("1. Parkera ett fordon");
        Console.WriteLine("2. Visa parkeringsplatser");
        Console.WriteLine("3. Flytta ett fordon");
        Console.WriteLine("4. Leta efter ett fordon");
        Console.WriteLine("5. Ta bort ett fordon");
        Console.WriteLine("6. Avsluta");
        Console.WriteLine("Välj ett alternativ:");

    }
}

public class ParkingGarage
{
    private string[] parkingLot;

    public ParkingGarage()
    {
        parkingLot = new string[100]; // Skapa en array med 100 element
                                      
    }


    // Metod för att parkera fordon
    public void ParkVehicle(string vehicleType, string registrationNumber)
    {
        int spot = FindAvailableSpot(vehicleType);
        if (spot == -1)
        {
            Console.WriteLine("Ingen ledig plats för fordonet.");
            return;
        }

        string vehicle = $"{vehicleType}#{registrationNumber}";

        if (vehicleType == "CAR")
        {
            if (string.IsNullOrEmpty(parkingLot[spot])) // Kontrollera om platsen är tom
            {
                parkingLot[spot] = vehicle;
                Console.WriteLine($"Bil {registrationNumber} har parkerats på plats {spot + 1}.");
            }
            else
            {
                Console.WriteLine("Platsen är redan upptagen av ett annat fordon.");
            }
        }
        else if (vehicleType == "MC")
        {
            if (string.IsNullOrEmpty(parkingLot[spot])) // Tom plats för en MC
            {
                parkingLot[spot] = vehicle;
                Console.WriteLine($"MC {registrationNumber} har parkerats på plats {spot + 1}.");
            }
            else if (parkingLot[spot].StartsWith("MC") && !parkingLot[spot].Contains("|")) // Redan en MC, men inte full
            {
                parkingLot[spot] += $"|{vehicle}"; // Lägg till den andra MC:n
                Console.WriteLine($"MC {registrationNumber} har dubbelparkerats på plats {spot + 1}.");
            }
            else
            {
                Console.WriteLine("Platsen är full för motorcyklar.");
            }
        }
    }


    // Metod för att visa parkeringsplatser
    public void ShowParkingLot()
    {
        Console.WriteLine("\n--- Parkeringsöversikt ---");
        for (int i = 0; i < parkingLot.Length; i++)
        {
            if (string.IsNullOrEmpty(parkingLot[i]))
            {
                Console.WriteLine($"Plats {i + 1}: [TOM]");
            }
            else
            {
                Console.WriteLine($"Plats {i + 1}: {parkingLot[i]}");
            }
        }
    }


    // Metod för att flytta fordon
    public void MoveVehicle(string registrationNumber, int newSpot)
    {
        if (newSpot < 1 || newSpot > parkingLot.Length)
        {
            Console.WriteLine("Ogiltigt platsnummer.");
            return;
        }

        newSpot--; // Justera för 0-indexerad array

        // Leta efter fordonet
        for (int i = 0; i < parkingLot.Length; i++)
        {
            if (!string.IsNullOrEmpty(parkingLot[i]) && parkingLot[i].Contains(registrationNumber))
            {
                // Flytta fordonet om den nya platsen är ledig
                if (string.IsNullOrEmpty(parkingLot[newSpot]) || (parkingLot[newSpot].StartsWith("MC") && !parkingLot[newSpot].Contains("|")))
                {
                    parkingLot[newSpot] = parkingLot[i]; // Flytta fordonet till den nya platsen
                    parkingLot[i] = ""; // Ta bort fordonet från den gamla platsen
                    Console.WriteLine($"Fordon {registrationNumber} har flyttats till plats {newSpot + 1}.");
                }
                else
                {
                    Console.WriteLine("Den nya platsen är inte tillgänglig.");
                }
                return;
            }
        }

        Console.WriteLine("Fordonet kunde inte hittas.");
    }


    // Metod för att leta efter ett fordon
    public void FindVehicle(string registrationNumber)
    {
        for (int i = 0; i < parkingLot.Length; i++)
        {
            if (!string.IsNullOrEmpty(parkingLot[i]) && parkingLot[i].Contains(registrationNumber))
            {
                Console.WriteLine($"Fordon {registrationNumber} hittades på plats {i + 1}.");
                return;
            }
        }

        Console.WriteLine("Fordonet kunde inte hittas.");
    }


    // Metod för att hitta en ledig plats
    private int FindAvailableSpot(string vehicleType)
    {
        // Först leta efter en plats med en MC som inte är full
        if (vehicleType == "MC")
        {
            for (int i = 0; i < parkingLot.Length; i++)
            {
                if (parkingLot[i]?.StartsWith("MC") == true && !parkingLot[i].Contains("|"))
                {
                    return i; // Finns plats för en till MC
                }
            }
        }

        // Leta efter en tom plats
        for (int i = 0; i < parkingLot.Length; i++)
        {
            if (string.IsNullOrEmpty(parkingLot[i]))
            {
                return i; // Tom plats
            }
        }

        return -1; // Ingen ledig plats
    }

    // Metod för att ta bort fordon
    public void RemoveVehicle(string registrationNumber)
    {
        for (int i = 0; i < parkingLot.Length; i++)
        {
            if (!string.IsNullOrEmpty(parkingLot[i]) && parkingLot[i].Contains(registrationNumber))
            {
                parkingLot[i] = ""; // Töm platsen
                Console.WriteLine($"Fordon {registrationNumber} har tagits bort från plats {i + 1}.");
                return;
            }
        }
        Console.WriteLine("Fordonet kunde inte hittas.");
    }


}

