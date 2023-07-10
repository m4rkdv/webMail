﻿using Mails.ConsoleApp;
using Mails.Entities;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

Console.WriteLine("Ingrese su email: ");
var email = Console.ReadLine();
Console.WriteLine("Ingrese su contraseña: ");
var password = Console.ReadLine();

var loginRequest = new LogInRequest()
{
    Email = email,
    PasswordHash = password
};
using (var client = new HttpClient())
{
    string data = JsonConvert.SerializeObject(loginRequest);
    StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
    HttpResponseMessage response = client.PostAsync("https://localhost:5050/api/users/login", content).Result;
    if (response.IsSuccessStatusCode && response.Content.ReadAsStringAsync().Result.Equals("true"))
    {
        Console.WriteLine("Ingreso exitoso!");
        int key;
        do
        {
            Console.WriteLine("---MENU---");
            Console.WriteLine("1 - BANDEJA DE ENTRADA\n2 - BANDEJA DE SALIDA\n0 - SALIR");
            key = int.Parse(Console.ReadLine());
            switch (key)
            {
                case 1:
                    response = client.GetAsync("https://localhost:5050/api/mails/all/inbox/" + email).Result;
                    data = response.Content.ReadAsStringAsync().Result;
                    var resultList = JsonConvert.DeserializeObject<List<Mail>>(data);
                    Console.WriteLine("---BANDEJA DE ENTRADA---");
                    foreach (var item in resultList)
                    {
                        Console.WriteLine("------Mail------");
                        Console.WriteLine(item);
                        Console.WriteLine("----------------");
                    }
                    break;
                case 2:
                    response = client.GetAsync("https://localhost:5050/api/mails/all/outbox/" + email).Result;
                    data = response.Content.ReadAsStringAsync().Result;
                    resultList = JsonConvert.DeserializeObject<List<Mail>>(data);
                    Console.WriteLine("---BANDEJA DE SALIDA---");
                    foreach (var item in resultList)
                    {
                        Console.WriteLine("-------Mail------");
                        Console.WriteLine(item);
                        Console.WriteLine("-----------------");
                    }
                    break;
                case 0:
                    break;
            }
        } while (key != 0);


    }
    else
    {
        Console.WriteLine("Email o contraseña incorrecta!");
    }
}



