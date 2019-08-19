using MAExtensions.CustomModel;
using Microsoft.Extensions.Configuration;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.WebApi;
using Sitecore.XConnect.Schema;
using Sitecore.XConnect.Serialization;
using Sitecore.Xdb.Common.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Orchestrator
{
    internal class Application
    {
        private IConfiguration _configuration = null;
        private readonly XdbModel _xDbModel = null;
        private XConnectClientConfiguration _xConnectClientConfiguration = null;
        private Contact _contact = null;

        public Application(IConfiguration configuration)
        {
            _configuration = configuration;
            _xDbModel = AkiraModel.Model;
        }

        public async Task Run()
        {
            var operation = "";

            do
            {
                PrintOptions();
                operation = Console.ReadLine();

                switch (operation)
                {
                    case "1":
                        await CreateContact();
                        break;

                    case "2":
                        await LoadContact();
                        break;

                    case "3":
                        ShowFacet();
                        break;

                    case "4":
                        await IncrementVesta();
                        break;

                    case "5":
                        await DecrementVesta();
                        break;

                    case "6":
                        await SetRigel();
                        break;

                    case "a":
                        await SerializeModel();
                        break;

                    case "x":
                        break;

                    default:
                        Console.WriteLine("Unknown operation");
                        break;
                }
            }
            while (operation != "x");
        }

        private void PrintOptions()
        {
            Console.WriteLine();
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("1. Create contact");
            Console.WriteLine("2. Load contact");
            Console.WriteLine("3. Show facet");
            Console.WriteLine("4. Increment Vesta");
            Console.WriteLine("5. Decrement Vesta");
            Console.WriteLine("6. Set Rigel");
            Console.WriteLine();
            Console.WriteLine("a. Serialize Model");
            Console.WriteLine();
            Console.WriteLine("x. Exit");
            Console.WriteLine();
        }

        private async Task CreateContact()
        {
            Console.WriteLine("Enter the contact's demo identifier");
            var identifier = Console.ReadLine();
            
            var contactIdentifier = new ContactIdentifier("demo", identifier, ContactIdentifierType.Known);
            _contact = new Contact(contactIdentifier);

            using (var client = await CreateXConnectClient())
            {
                client.AddContact(_contact);
                await client.SubmitAsync(CancellationToken.None);
            }

            Console.WriteLine($"Contact created with identifier (demo, {identifier})");
        }

        private async Task LoadContact()
        {
            Console.WriteLine("Enter the contact's demo identifier");
            var identifier = Console.ReadLine();
            var reference = new IdentifiedContactReference("demo", identifier);
            var expandOptions = new ContactExpandOptions(SurakFacet.DefaultFacetName);

            using (var client = await CreateXConnectClient())
            {
                _contact = await client.GetContactAsync(reference, expandOptions);
            }

            if (_contact == null)
                Console.WriteLine("Contact not found");
            else
                Console.WriteLine("Contact loaded");
        }

        private void ShowFacet()
        {
            if (_contact == null)
            {
                Console.WriteLine("No contact loaded");
                return;
            }

            var facet = _contact.GetFacet<SurakFacet>(SurakFacet.DefaultFacetName);

            if (facet == null)
            {
                Console.WriteLine("The facet has not been set yet");
                return;
            }

            Console.WriteLine($"Vesta: {facet.Vesta}");
            Console.WriteLine($"Rigel: {facet.Rigel}");
        }

        private async Task IncrementVesta()
        {
            if(_contact == null)
            {
                Console.WriteLine("No contact loaded");
                return;
            }

            var facet = _contact.GetFacet<SurakFacet>(SurakFacet.DefaultFacetName);
            if (facet == null)
                facet = new SurakFacet();

            facet.Vesta++;
            Console.WriteLine(facet.Vesta);
            await UpdateFacet(facet);
        }

        private async Task DecrementVesta()
        {
            if (_contact == null)
            {
                Console.WriteLine("No contact loaded");
                return;
            }

            var facet = _contact.GetFacet<SurakFacet>(SurakFacet.DefaultFacetName);
            if (facet == null)
                facet = new SurakFacet();

            facet.Vesta--;
            Console.WriteLine(facet.Vesta);
            await UpdateFacet(facet);
        }

        private async Task SetRigel()
        {
            if (_contact == null)
            {
                Console.WriteLine("No contact loaded");
                return;
            }

            Console.WriteLine("Enter new value for Rigel");
            var input = Console.ReadLine();

            var facet = _contact.GetFacet<SurakFacet>(SurakFacet.DefaultFacetName);
            if (facet == null)
                facet = new SurakFacet();

            facet.Rigel = input;
            Console.WriteLine(facet.Rigel);
            await UpdateFacet(facet);
        }

        private async Task SerializeModel()
        {
            var json = XdbModelWriter.Serialize(_xDbModel);
            var filename = _xDbModel.ToString() + ".json";
            await File.WriteAllTextAsync(filename, json);

            Console.WriteLine($"Model has been written to {filename}");
        }

        private async Task<XConnectClient> CreateXConnectClient()
        {
            if (_xConnectClientConfiguration == null)
                _xConnectClientConfiguration = await CreateXConnectClientConfiguration();

            return new XConnectClient(_xConnectClientConfiguration);
        }

        private async Task<XConnectClientConfiguration> CreateXConnectClientConfiguration()
        {
            Console.WriteLine("Initializing xConnect configuration...");

            var uri = new Uri(_configuration.GetValue<string>("xconnect:uri"));
            var certificateSection = _configuration.GetSection("xconnect:certificate");
            var handlerModifiers = new List<IHttpClientHandlerModifier>();

            if (certificateSection.GetChildren().Any())
            {
                var certificateModifier = new CertificateHttpClientHandlerModifier(certificateSection);
                handlerModifiers.Add(certificateModifier);
            }

            var xConnectConfigurationClient = new ConfigurationWebApiClient(new Uri(uri + "configuration"), null, handlerModifiers);
            var xConnectCollectionClient = new CollectionWebApiClient(new Uri(uri + "odata"), null, handlerModifiers);
            var xConnectSearchClient = new SearchWebApiClient(new Uri(uri + "odata"), null, handlerModifiers);

            var xConnectClientConfig = new XConnectClientConfiguration(_xDbModel, xConnectCollectionClient, xConnectSearchClient, xConnectConfigurationClient);
            await xConnectClientConfig.InitializeAsync();

            Console.WriteLine("xConnect configuration has been initialized");
            return xConnectClientConfig;
        }

        private async Task UpdateFacet<T>(T facet) where T : Facet
        {
            using(var client = await CreateXConnectClient())
            {
                client.SetFacet(_contact, facet);
                await client.SubmitAsync();
            }

            Console.WriteLine("Facet updated");
        }
    }
}
