using System;
using System.Collections.Generic;
using Klarna.Rest.Core.Common;
using Klarna.Rest.Core.Communication;
using Klarna.Rest.Core.Model;
using Klarna.Rest.Core.Model.Enum;

namespace Klarna.Rest.Core.Examples
{
    /// <summary>
    /// Create a checkout order
    /// </summary>
    public class CreateExample
    {
        /// <summary>
        /// Run the example code.
        /// Remember to replace username and password with valid Klarna credentials. 
        /// </summary>
        //public static void Main()
        //public static void OldCreate()
        public static String OldCreate()
        {
            //var username = "0_abc";
            var username = "PK52760_e9fd580e033a";
            //var password = "sharedsecret";
            var password = "TC1KwODjPpwpFRPj";
            Console.WriteLine("username: " + username + "\nPass: " + password);

            var client = new Klarna(username, password, KlarnaEnvironment.TestingEurope);

            var order = new CheckoutOrder
            {
                PurchaseCountry = "se",
                PurchaseCurrency = "sek",
                Locale = "sv-se",
                OrderAmount = 10000,
                OrderTaxAmount = 2000,
                MerchantUrls = new CheckoutMerchantUrls
                {
                    Terms = "https://www.example.com/terms.html",
                    Checkout = "https://www.example.com/checkout.html",
                    Confirmation = "https://www.example.com/confirmation.html",
                    Push = "https://www.example.com/push.html"
                },
                OrderLines = new List<OrderLine>()
                {
                    new OrderLine
                    {
                        Type = OrderLineType.physical,
                        Name = "Foo",
                        Quantity = 1,
                        UnitPrice = 10000,
                        TaxRate = 2500,
                        TotalAmount = 10000,
                        TotalTaxAmount = 2000,
                        TotalDiscountAmount = 0,
                    }
                },
                CheckoutOptions = new CheckoutOptions
                {
                    AllowSeparateShippingAddress = false,
                    ColorButton = "#FF9900",
                    ColorButtonText = "#FF9900",
                    ColorCheckbox = "#FF9900",
                    ColorCheckboxCheckmark = "#FF9900",
                    ColorHeader = "#FF9900",
                    ColorLink = "#FF9900",
                    DateOfBirthMandatory = false,
                    ShippingDetails = "bar",
                    AllowedCustomerTypes = new List<string>(){"person", "organization"}
                }
            };

            try
            {
                var createdOrder = client.Checkout.CreateOrder(order).Result;
                var orderId = createdOrder.OrderId;
                Console.WriteLine($"Order ID: {orderId}");
                return orderId;
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions) {
                    if (e is ApiException)
                    {
                        var apiException = (ApiException) e;
                        Console.WriteLine("Status code: " + apiException.StatusCode);
                        Console.WriteLine("Error: " + string.Join("; ", apiException.ErrorMessage.ErrorMessages));
                    }
                    else {
                        // Rethrow any other exception or process it
                        Console.WriteLine(e.Message);
                        throw;
                    }
                }
            }
            return "0";
        }
    }

    public class DiscountsExample
    {
        /// <summary>
        /// Run the example code.
        /// Remember to replace username and password with valid Klarna credentials. 
        /// </summary>
        //static void Main()
        static void Old()
        {
            var username = "0_abc";
            var password = "sharedsecret";

            var client = new Klarna(username, password, KlarnaEnvironment.TestingEurope);

            var order = new CheckoutOrder
            {
                PurchaseCountry = "gb",
                PurchaseCurrency = "gbp",
                Locale = "en-gb",
                OrderAmount = 9000,
                OrderTaxAmount = 818,
                MerchantUrls = new CheckoutMerchantUrls
                {
                    Terms = "https://www.example.com/terms.html",
                    Checkout = "https://www.example.com/checkout.html",
                    Confirmation = "https://www.example.com/confirmation.html",
                    Push = "https://www.example.com/push.html"
                },
                OrderLines = new List<OrderLine>()
                {
                    new OrderLine
                    {
                        Type = OrderLineType.physical,
                        Reference = "19-402-USA",
                        Name = "Red T-Shirt",
                        Quantity = 1,
                        QuantityUnit = "pcs",
                        UnitPrice = 10000,
                        TaxRate = 1000,
                        TotalAmount = 10000,
                        TotalTaxAmount = 909
                    },

                    // Set a discount
                    new OrderLine
                    {
                        Type = OrderLineType.discount,
                        Reference = "10-gbp-order-discount",
                        Name = "Discount",
                        Quantity = 1,
                        UnitPrice = -1000,
                        TaxRate = 1000,
                        TotalAmount = -1000,
                        TotalTaxAmount = -91
                    }
                },
            };

            try
            {
                var createdOrder = client.Checkout.CreateOrder(order).Result;
                Console.WriteLine(createdOrder.HtmlSnippet);
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions) {
                    if (e is ApiException)
                    {
                        var apiException = (ApiException) e;
                        Console.WriteLine("Status code: " + apiException.StatusCode);
                        Console.WriteLine("Error: " + string.Join("; ", apiException.ErrorMessage.ErrorMessages));
                    }
                    else {
                        // Rethrow any other exception or process it
                        Console.WriteLine(e.Message);
                        throw;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Retrieves a checkout order
    /// </summary>
    public class FetchExample
    {
        /// <summary>
        /// Run the example code.
        /// Remember to replace username and password with valid Klarna credentials. 
        /// </summary>
        public static void Main()
        //public static void Old2()
        {
            //CreateExample.OldCreate();
            var orderId = CreateExample.OldCreate();
            var username = "PK52760_e9fd580e033a";
            var password = "TC1KwODjPpwpFRPj";
            //var orderId = "85dae756-a98d-6248-9a9f-1b4a93c0d095";
            //var orderId = "8b675da7-aa4e-4334-82b5-99abd7b3d77b";
            
            Console.WriteLine("TEXTX");

            var client = new Klarna(username, password, KlarnaEnvironment.TestingEurope);

            try
            {
                var order = client.Checkout.GetOrder(orderId).Result;
                Console.WriteLine(order.OrderId);
                Console.WriteLine(order.OrderAmount);
                //Console.WriteLine(order.OrderLines.GetEnumerator().Current.Quantity);
                var iterator = order.OrderLines.GetEnumerator();
                while(iterator.MoveNext())
                {
                   var item = iterator.Current;
                   Console.WriteLine(item.Name);
                }
                Console.WriteLine("\n------------------------------------\n\n");
                Console.WriteLine(order.HtmlSnippet);
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions) {
                    if (e is ApiException)
                    {
                        var apiException = (ApiException) e;
                        Console.WriteLine("Status code: " + apiException.StatusCode);
                        Console.WriteLine("Error: " + string.Join("; ", apiException.ErrorMessage.ErrorMessages));
                    }
                    else {
                        // Rethrow any other exception or process it
                        Console.WriteLine(e.Message);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Updates a checkout order
        /// </summary>
        public class FetchAndUpdateExample
        {
            /// <summary>
            /// Run the example code.
            /// Remember to replace username and password with valid Klarna credentials. 
            /// </summary>
            //public static void Main()
            public static void Old3()
            {
                var username = "0_abc";
                var password = "sharedsecret";
                var orderId = "1234";

                var client = new Klarna(username, password, KlarnaEnvironment.TestingEurope);
                try
                {
                    var order = client.Checkout.GetOrder(orderId).Result;
                    order.OrderAmount = 20000;
                    order.OrderTaxAmount = 4000;
                    order.OrderLines = new List<OrderLine>
                    {
                        new OrderLine
                        {
                            Type = OrderLineType.physical,
                            Name = "Foo",
                            Quantity = 1,
                            UnitPrice = 10000,
                            TaxRate = 2500,
                            TotalAmount = 10000,
                            TotalTaxAmount = 2000,
                            TotalDiscountAmount = 0,
                        },
                        new OrderLine
                        {
                            Type = OrderLineType.physical,
                            Name = "Foo2",
                            Quantity = 1,
                            UnitPrice = 10000,
                            TaxRate = 2500,
                            TotalAmount = 10000,
                            TotalTaxAmount = 2000,
                            TotalDiscountAmount = 0,
                        }
                    };

                    var updatedOrder = client.Checkout.UpdateOrder(order).Result;
                }
                catch (AggregateException ae)
                {
                    foreach (var e in ae.InnerExceptions) {
                        if (e is ApiException)
                        {
                            var apiException = (ApiException) e;
                            Console.WriteLine("Status code: " + apiException.StatusCode);
                            Console.WriteLine("Error: " + string.Join("; ", apiException.ErrorMessage.ErrorMessages));
                        }
                        else {
                            // Rethrow any other exception or process it
                            Console.WriteLine(e.Message);
                            throw;
                        }
                    }
                }
            }
        }
    }
}
