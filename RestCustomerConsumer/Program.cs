using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RestCustomerConsumer
{
    class Program
    {

        private static string CustomersUri = "https://localhost:44316/api/Customer";
        public static async Task<IList<Customer>> GetCustomersAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync(CustomersUri);
                IList<Customer> cList = JsonConvert.DeserializeObject<IList<Customer>>(content);
                return cList;
            }
        }

        public static async Task<Customer> GetOneCustomerAsync(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                
                string content = await client.GetStringAsync("https://localhost:44316/api/Customer/" + id);
                Customer customer = JsonConvert.DeserializeObject<Customer>(content);
                return customer;
            }
        }

        public static async Task<Customer> PostCustomerAsync(Customer newCustomer)
        {
            using (HttpClient client = new HttpClient())
            {
                var jsonString = JsonConvert.SerializeObject(newCustomer);
                Console.WriteLine("JSON: " + jsonString);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("https://localhost:44316/api/Customer/", content);
                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    throw new Exception("Customer already exists. Try another id");
                }

                response.EnsureSuccessStatusCode();
                string str = await response.Content.ReadAsStringAsync();
                Customer copyOfNewCustomer = JsonConvert.DeserializeObject<Customer>(str);
                return copyOfNewCustomer;
            }
        }

        static void Main(string[] args)
        {
            Customer customer = new Customer(5, "Mark", "Hamil", 1953);
            Task<IList<Customer>> t = GetCustomersAsync();
            foreach (var item in t.Result)
            {
                Console.WriteLine($"test: {item.FirstName}");
            }

            Console.WriteLine(GetOneCustomerAsync(1).Result);
            Console.ReadKey();

        }      
            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }


