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
            var unmatched = 0;
            foreach (var command in metabase.Commands)
            {
                if (!IsCommandInDatabase(command, database))
                {
                    unmatched++;
                    Log.Error("Command doesn't exist in database or doesn't match any overload. ",
                        $"Command {command.FullName}");
                }
                else if (Options.Instance.Verbose)
                {
                    Log.PositiveResult($"MATCHED: ", $"{command.FullName}, Package={command.Package}, ServerName={command.ServerName}");
                }
            }
            Log.LogText(ConsoleColor.Yellow, "", $"{metabase.Commands.Count - unmatched}/{metabase.Commands.Count} matched");
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
            var correctReturnValue = false;
            var unmatchedParamsCount = command.Parameters.Count;
            var unmatchedArgumentsCount = function.Arguments.Count;
            foreach (var argument in function.Arguments)
            {
                var exists = false;
                foreach (var parameter in command.Parameters)
                {
                    if (!correctReturnValue && parameter.Direction == ParameterDirection.ReturnValue &&
                        ParameterTypeToArgumentType[parameter.Type] == function.ReturnValue.Type)
                    {
                        correctReturnValue = true;
                        unmatchedParamsCount--;
                    }



                    if (string.Equals(parameter.ServerName, argument.Name, StringComparison.CurrentCultureIgnoreCase)
                        && ParameterTypeToArgumentType[parameter.Type] == argument.Type
                        && (int) parameter.Direction == (int) argument.Direction)
                    {
                        exists = true;
                        unmatchedParamsCount--;
                        unmatchedArgumentsCount--;
                        break;
                    }
                }

                if (exists) continue;
                if (argument.IsDefault)
                    unmatchedArgumentsCount--;
                else
                    break;
            }

            return correctReturnValue && unmatchedParamsCount == 0 && unmatchedArgumentsCount == 0;
        }

        private static bool CheckProcedure(Command command, Procedure procedure)
        {
            var unmatchedParamsCount = command.Parameters.Count;
            var unmatchedArgumentsCount = procedure.Arguments.Count;
            foreach (var argument in procedure.Arguments)
            {
                var exists = false;
                foreach (var parameter in command.Parameters)
                {
                    if (string.Equals(parameter.ServerName, argument.Name, StringComparison.CurrentCultureIgnoreCase)
                        && ParameterTypeToArgumentType[parameter.Type] == argument.Type
                        && (int)parameter.Direction == (int)argument.Direction)
                    {
                        exists = true;
                        unmatchedParamsCount--;
                        unmatchedArgumentsCount--;
                        break;
                    }
                }

                if (exists) continue;
                if (argument.IsDefault)
                    unmatchedArgumentsCount--;
                else
                    break;
            }

            return unmatchedParamsCount == 0 && unmatchedArgumentsCount == 0;
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
