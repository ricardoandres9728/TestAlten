using TestDAL;
using TestDAL.Models;

namespace TestAPI.Services
{
    public class ClientService : IClientService
    {
        readonly TestBDContext TestBDContext;

        public ClientService(TestBDContext testBDContext)
        {
            //BDContext injection, defined in Program.cs
            TestBDContext = testBDContext;
        }

        //Gets all the clients
        public IEnumerable<Client> Get()
        {
            try
            {
                IEnumerable<Client>? clients = TestBDContext.Clients;
                if (clients != null)
                    return clients;
                else
                    return Enumerable.Empty<Client>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting clients. \n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace, ex);
            }
        }

        //Finds client by ID
        public Client Get(int id)
        {
            try
            {
                Client client = TestBDContext.Clients?.Find(id) ?? new Client();
                return client;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting client by ID. \n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace, ex);
            }
        }

        //Saves client in database
        public void Save(Client client)
        {
            TestBDContext.Add(client);
            TestBDContext.SaveChanges();
        }

        //Updates existing client, find by id
        public void Update(int id, Client client)
        {
            try
            {
                Client currentInfo = TestBDContext.Clients?.Find(id) ?? new Client();

                if (currentInfo.Id != 0)
                {
                    currentInfo.Identification = client.Identification;
                    currentInfo.Name = client.Name;
                    TestBDContext.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating client. \n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace, ex);
            }
        }

        //Deletes existing client
        public void Delete(int id)
        {
            try
            {
                Client client = TestBDContext.Clients?.Find(id) ?? new Client();

                if (client.Id != 0)
                {
                    TestBDContext.Remove(client);
                    TestBDContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting client by ID. \n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace, ex);
            }
        }
    }

    //Creates interface in order to expose methods to be consummed
    public interface IClientService
    {
        IEnumerable<Client> Get();
        Client Get(int id);
        void Save(Client client);
        void Update(int id, Client client);
        void Delete(int id);
    }
}
