using System;
using System.Collections.Generic;
using System.Linq;
using CommandAPI.Models;

namespace CommandAPI.Data
{
    public class SqlCommandAPIRepo : ICommandAPIRepo
    {
        private readonly CommandContext context;
        public SqlCommandAPIRepo(CommandContext context)
        {
            this.context = context;
        }
        public void CreateCommand(Command command)
        {
            if(command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            context.CommandItems.Add(command);
        }

        public void DeleteCommand(Command command)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Command> GetAllCommands()
        {
            return context.CommandItems.ToList();
        }

        public Command GetCommandById(int id)
        {
            return context.CommandItems.FirstOrDefault(x => x.Id == id);
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() >= 0;
        }

        public void UpdateCommand(Command command)
        {
            
        }
    }
}