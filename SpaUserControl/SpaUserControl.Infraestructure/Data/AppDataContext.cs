using SpaUserControl.Domain.Models;
using SpaUserControl.Infraestructure.Data.Map;
using System.Data.Entity;

namespace SpaUserControl.Infraestructure.Data
{
    public class AppDataContext : DbContext
    {

        public AppDataContext()
            : base("AppConnectionString")
        {
            //Desabilita o carregamento de objetos compostos
            //Por exemplo se eu tenho uma Classe User e dentro eu relaciono com Endereço
            //Dessa forma ele só vai me retornar o User
            //Só quando eu der um User.Endereco que ele ira recuperar o endereço
            //Carrega os dados por parte
            Configuration.LazyLoadingEnabled = false;

            //Serve para quando eu usar o .ToList() para executar a consulta no banco ao invés de ele me retornar um objeto proxy
            //Ele me retorna um objeto concreto, pois o serializador JSON não consegue serializar um Proxy 
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<User> Users { get; set; }


        //Adiciono meu mapeamento do meu User para que não seja criado automatico do EF e sim do que eu defini para o banco de dados
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());
        }
    }
}
