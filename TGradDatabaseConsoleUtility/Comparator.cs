using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGradDatabaseConsoleUtility
{
    static class Comparator
    {
        public static void Compare(Metabase metabase, Database database)
        {
            foreach (var command in metabase.Commands)
            {
                if (command.ServerName.ToLower() == "get_service")
                    Console.WriteLine("Hello");
                if (!IsCommandInDatabase(command, database))
                {
                    Log.Error("Command doesn't exist in database or doesn't match any overload. ",
                        $"Command {command.FullName}");
                }
                else if (Options.Instance.Verbose)
                {
                    Log.PositiveResult($"MATCHED: ", $"{command.FullName}, Package={command.Package}, ServerName={command.ServerName}");
                }
            }
        }

        private static bool IsCommandInDatabase(Command command, Database database)
        {
            var lowerPckName = command.Package.ToLower();
            Package foundPackage = null;
            foreach (var package in database.Packages)
            {
                if (lowerPckName == package.Name.ToLower())
                {
                    foundPackage = package;
                    break;
                }
            }
            if (foundPackage == null)
            {
                Log.Error($"package {command.Package} not found. ", $"Command Name={command.Name}.");
                return false;
            }
            var isFucntion = command.Parameters.Any(param => param.Direction == ParameterDirection.ReturnValue);
            var exists = false;
            if (isFucntion)
            {
                if (FindFunctionsByName(command, foundPackage).Any(function => CheckFunction(command, function)))
                    exists = true;
            }
            else
            {
                if (FindProceduresByName(command, foundPackage).Any(procedure => CheckProcedure(command, procedure)))
                    exists = true;
            }
            return exists;
        }

        private static List<Procedure> FindProceduresByName(Command command, Package package)
        {
            var lowerServerName = command.ServerName.ToLower();
            return package.Procedures.Where(p => p.Name.ToLower() == lowerServerName).ToList();
        }

        private static List<Function> FindFunctionsByName(Command command, Package package)
        {
            var lowerServerName = command.ServerName.ToLower();
            return package.Functions.Where(f => f.Name.ToLower() == lowerServerName).ToList();
        }

        private static bool CheckFunction(Command command, Function function)
        {
            if (command.Parameters.Count != function.Arguments.Count + 1)
                return false;
            var res = true;
            var returnValueEquals = true;
            foreach (var parameter in command.Parameters)
            {
                var exists = false;
                if (parameter.Direction != ParameterDirection.ReturnValue ||
                    ParameterTypeToArgumentType[parameter.Type] != function.ReturnValue.Type)
                {
                    returnValueEquals = false;
                    break;
                }

                foreach (var argument in function.Arguments)
                {
                    if (string.Equals(parameter.ServerName, argument.Name, StringComparison.CurrentCultureIgnoreCase) &&
                        ParameterTypeToArgumentType[parameter.Type] == argument.Type &&
                        (int)parameter.Direction == (int)argument.Direction)
                        exists = true;

                }
                if (exists) continue;
                res = false;
                break;
            }
            return res && returnValueEquals;
        }

        private static bool CheckProcedure(Command command, Procedure procedure)
        {
            if (command.Parameters.Count != procedure.Arguments.Count)
                return false;
            var res = true;
            foreach (var parameter in command.Parameters)
            {
                var exists = false;
                foreach (var argument in procedure.Arguments)
                {
                    if (!string.Equals(parameter.ServerName, argument.Name,
                            StringComparison.CurrentCultureIgnoreCase) ||
                        ParameterTypeToArgumentType[parameter.Type] != argument.Type ||
                        (int) parameter.Direction != (int) argument.Direction) continue;
                    exists = true;
                    break;
                }
                if (exists) continue;
                res = false;
                break;
            }
            return res;
        }
        
        private static Dictionary<ParameterType, ArgumentType> ParameterTypeToArgumentType { get; } = new Dictionary<ParameterType, ArgumentType>()
        {
            { ParameterType.Number, ArgumentType.NUMBER },
            { ParameterType.Blob, ArgumentType.BLOB },
            { ParameterType.Clob, ArgumentType.CLOB },
            { ParameterType.Date, ArgumentType.DATE },
            { ParameterType.DateTime, ArgumentType.DATE },
            { ParameterType.Decimal, ArgumentType.DECIMAL },
            { ParameterType.Timestamp, ArgumentType.TIMESTAMP },
            { ParameterType.AnsiString, ArgumentType.VARCHAR2 },
            { ParameterType.String, ArgumentType.STRING },
            { ParameterType.Cursor, ArgumentType.CURSOR }
        };
    }
}
