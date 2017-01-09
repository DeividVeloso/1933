using SpaUserControl.Domain.Models.Contracts.Repositories;
using SpaUserControl.Domain.Models.Contracts.Services;
using System;
using SpaUserControl.Common.Validation;
using SpaUserControl.Common.Resources;
using SpaUserControl.Domain.Models;

namespace SpaUserControl.Business.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _repository;
        public UserService(IUserRepository repository)
        {
            this._repository = repository;
        }

        public Domain.Models.User Authenticate(string email, string password)
        {
            var user = GetByEmail(email);

            if (user.Password.Trim() != PasswordAssertionConcern.Encrypt(password))
            {
                throw new Exception(Errors.InvalidCredentials);
            }

            return user;
        }
       
        public void ChangeInformation(string email, string name)
        {
            var user = GetByEmail(email);

            user.ChangeName(name);
            user.Validate();

            _repository.Update(user);
        }

        public void ChangePassword(string email, string password, string newPassword, string confirmNewPassword)
        {
            var user = Authenticate(email, password);

            user.SetPassword(newPassword, confirmNewPassword);
            user.Validate();

            _repository.Update(user);
        }

        public Domain.Models.User GetByEmail(string email)
        {
            var user = _repository.Get(email);

            if (user == null)
            {
                throw new Exception(Errors.UserNotFound);
            }

            return user;
        }


        public void Register(string name, string email, string password, string confirmPassword)
        {
            var hasUser = GetByEmail(email);

            if (hasUser != null)
            {
                throw new Exception(Errors.DuplicateEmail);
            }

            var user = new User(name, email,password);
            user.SetPassword(password, confirmPassword);
            user.Validate();

            _repository.Create(user);

        }
        public string ResetPassword(string email)
        {
            var user = GetByEmail(email);

            var password = user.ResetPassword();
            user.Validate();

            _repository.Update(user);

            return password;
        }

        public void Dispose()
        {
            //para não deixar nenhuma conexão aberta
            _repository.Dispose();
        }
    }
}
