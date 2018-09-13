using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Micro.ConsolePanel
{
    class Program
    {
        static void Main(string[] args)
        {
            var _roleService = new RoleService();
            var _userService = new UserService();

            var transaction = new Transaction();
            var role = transaction.Do<Role>(new RoleTransationUnit()
            {
                Continue = () =>
                {
                    return _roleService.AddRoleAsync(new Role()
                    {
                        Name = "Administrator"
                    }).Result;
                },
                Rollback = x =>
               {
                   return _roleService.DeleteRoleAsync((Role)x).Result;
               }
            });
            var user = transaction.Do<User>(new RoleTransationUnit()
            {
                Continue = () =>
               {
                   return _userService.AddUserAsync(new User()
                   {
                       Name = "Gavin"
                   });
               },
                Rollback = x =>
               {
                   return _userService.DeleteUserAsync((User)x).Result;
               }
            });
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }

    public class Transaction
    {
        private Stack<TransactionHistory> transactionHistories = new Stack<TransactionHistory>();

        public T Do<T>(RoleTransationUnit transactionUnit)
        {
            try
            {
                var result = transactionUnit.Continue.Invoke();
                transactionHistories.Push(new TransactionHistory()
                {
                    Index = transactionHistories.Count + 1,
                    Paremter = result,
                    TransationUnit = transactionUnit
                });
                return (T)result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Occus an error, try to rollback.");
                TransactionHistory transactionHistory = null;
                if (transactionHistories.TryPop(out transactionHistory))
                {
                    transactionHistory.TransationUnit.Rollback.Invoke(transactionHistory.Paremter);
                }
                Console.WriteLine(ex.Message);
                return default(T);
            }
        }
    }
    public class TransactionHistory
    {
        public RoleTransationUnit TransationUnit { get; set; }
        public object Paremter { get; set; }
        public int Index { get; set; }
    }

    public class RoleTransationUnit
    {
        public virtual Func<object> Continue { get; set; }

        public virtual Func<object, bool> Rollback { get; set; }
    }

    public class RoleService
    {
        public Task<Role> AddRoleAsync(Role role)
        {
            role.Id = Guid.NewGuid();
            Console.WriteLine($"Save role succeed[{role.Id}:{role.Name}]");
            return Task.FromResult(role);
        }

        public Task<bool> DeleteRoleAsync(Role role)
        {
            Console.WriteLine($"Delete role succeed[{role.Id}:{role.Name}]");
            return Task.FromResult(true);
        }
    }
    public class UserService
    {
        public Task<User> AddUserAsync(User user)
        {
            user.Id = Guid.NewGuid();
            //Console.WriteLine($"Save User succeed[{user.Id}:{user.Name}]");

            throw new NotImplementedException("Test throw exception");
            //return user;
        }

        public Task<bool> DeleteUserAsync(User user)
        {
            Console.WriteLine($"Save User succeed[{user.Id}:{user.Name}]");
            return Task.FromResult(true);
        }
    }
}
