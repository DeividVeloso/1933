using SpaUserControl.Domain.Models;
using SpaUserControl.Domain.Models.Contracts.Repositories;
using SpaUserControl.Infraestructure.Data;
using System;
using System.Linq;

namespace SpaUserControl.Infraestructure.Repositories
{
    public class UserRepository : IUserRepository
    {

        private AppDataContext _context = new AppDataContext();

        public User Get(string email)
        {
            //Recupera o primeiro ou User = null
            return _context.Users.Where(x => x.Email.ToLower() == email.ToLower()).FirstOrDefault();
        }

        public User Get(Guid id)
        {
            return _context.Users.Where(x => x.Id == id).FirstOrDefault();
        }

        public void Create(User user)
        {
            //Está na Sessão
            _context.Users.Add(user);
            //Commit no banco
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            //Eu Seto a propriedade do Objeto State como Modificado e no bacground ele faz um de para. E verifica o que mudou
            _context.Entry<User>(user).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
