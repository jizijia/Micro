using Micro.Core;
using Micro.Core.Domain;
using Micro.Core.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Data.MySQL
{
    public class ObjectContext : DbContext, IDbContext
    {
        public DbContext DbContext => this;
        public virtual string DomainFilePatten { get; } = " *.Domain.dll";
        public ObjectContext(DbContextOptions<ObjectContext> options) : base(options) { }
        public ObjectContext(DbContextOptions<ObjectContext> options, string domainFilePatten) : base(options)
        {
            DomainFilePatten = domainFilePatten;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var loggerFactory = IocResolver.Resolve<ILoggerFactory>();
            if (loggerFactory != null)
            {
                optionsBuilder.UseLoggerFactory(loggerFactory);
            }
            base.OnConfiguring(optionsBuilder);
        }
        /// <summary>
        /// On model creating
        /// </summary>
        /// <param name="modelBuilder">Model builder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            string path = System.IO.Directory.GetCurrentDirectory();
            var typeFinder = IocResolver.Resolve<ITypeFinder>();

            var assemblies = typeFinder.GetAssemblies("~/", DomainFilePatten, false);
            List<Type> typesToRegister = new List<Type>();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes().Where(type => !string.IsNullOrEmpty(type.Namespace));
                types = types.Where(type => !string.IsNullOrEmpty(type.Namespace)).ToArray();
                types = types.Where(x => x.BaseType != null).ToArray();
                types = types.Where(x => x.BaseType == typeof(IEntity)).ToArray();
                typesToRegister.AddRange(types);
            }
            foreach (var item in typesToRegister)
            {
                if (item is ISoftDelete)
                {
                    modelBuilder.Entity(item).ToTable(ToPlural(item.Name));
                    modelBuilder.Entity(item).Property(nameof(ISoftDelete.IsDeleted)).HasDefaultValue(false);
                }
            }
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// 单词变成复数形式
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private static string ToPlural(string word)
        {
            Regex plural1 = new Regex("(?<keep>[^aeiou])y$");
            Regex plural2 = new Regex("(?<keep>[aeiou]y)$");
            Regex plural3 = new Regex("(?<keep>[sxzh])$");
            Regex plural4 = new Regex("(?<keep>[^sxzhy])$");

            if (plural1.IsMatch(word))
                return plural1.Replace(word, "${keep}ies");
            else if (plural2.IsMatch(word))
                return plural2.Replace(word, "${keep}s");
            else if (plural3.IsMatch(word))
                return plural3.Replace(word, "${keep}es");
            else if (plural4.IsMatch(word))
                return plural4.Replace(word, "${keep}s");
            return word;
        }
    }

}
