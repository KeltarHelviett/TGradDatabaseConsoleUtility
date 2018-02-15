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
            if (!database.Packages.TryGetValue(lowerPckName, out foundPackage))
            {
                Log.Error($"package {command.Package} not found. ", $"Command Name={command.Name}.");
                return false;
            }
            if (command.ReturnValue != null)
                return FindFunctionsByName(command, foundPackage).Any(function => CheckFunction(command, function));
            return FindProceduresByName(command, foundPackage).Any(procedure => CheckProcedure(command, procedure));
        }

        private static List<Procedure> FindProceduresByName(Command command, Package package)
        {
            var key = command.ServerName.ToLower();
            return package.Procedures.ContainsKey(key) ? package.Procedures[key] : new List<Procedure>();
        }

        private static List<Function> FindFunctionsByName(Command command, Package package)
        {
            var key = command.ServerName.ToLower();
            return package.Functions.ContainsKey(key) ? package.Functions[key] : new List<Function>();
        }

        private static bool CheckFunction(Command command, Function function)
        {
            var unmatchedParamsCount = command.Parameters.Count;
            var unmatchedArgumentsCount = function.Arguments.Count;
            if (function.ReturnValue.Type != ParameterTypeToArgumentType[command.ReturnValue.Type])
                return false;
            var parameters = command.Parameters;
            foreach (var argument in function.Arguments)
            {
                var exists = false;
                var key = argument.Key;
                var arg = argument.Value;
                Parameter p;
                if (parameters.TryGetValue(key, out p) && ParameterTypeToArgumentType[p.Type] == arg.Type
                    && (int) p.Direction == (int) arg.Direction)
                {
                    unmatchedParamsCount--;
                    unmatchedArgumentsCount--;
                    continue;
                }
                else if (arg.IsDefault)
                {
                    unmatchedArgumentsCount--;
                    continue;
                }
                return false;
            }

            return unmatchedParamsCount == 0 && unmatchedArgumentsCount == 0;
        }

        private static bool CheckProcedure(Command command, Procedure procedure)
        {
            var unmatchedParamsCount = command.Parameters.Count;
            var unmatchedArgumentsCount = procedure.Arguments.Count;
            var parameters = command.Parameters;
            foreach (var argument in procedure.Arguments)
            {
                var key = argument.Key;
                var arg = argument.Value;
                Parameter p;
                if (parameters.TryGetValue(key, out p) && ParameterTypeToArgumentType[p.Type] == arg.Type
                                                       && (int)p.Direction == (int)arg.Direction)
                {
                    unmatchedParamsCount--;
                    unmatchedArgumentsCount--;
                    continue;
                }
                else if (arg.IsDefault)
                {
                    unmatchedArgumentsCount--;
                    continue;
                }
                return false;
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
