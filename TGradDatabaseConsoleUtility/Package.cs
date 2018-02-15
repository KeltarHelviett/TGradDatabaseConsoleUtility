using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TGradDatabaseConsoleUtility
{
    class Package
    {
        #region Ctor
        
        public Package(string filePath)
        {
            var package = File.ReadAllText(filePath);
            Name = Path.GetFileNameWithoutExtension(filePath).ToLower();
            package = Regex.Replace(package, @"(--(.*?)\r?\n|/\*(.*?)\*/)", "");
            package = package.Substring(0, package.IndexOf("package body", StringComparison.OrdinalIgnoreCase));
            GetProcedures(package);
            GetFunctions(package);
        }

        #endregion


        #region PublicProperties

        public Dictionary<string, List<Procedure>> Procedures { get; } = new Dictionary<string, List<Procedure>>();

        public  Dictionary<string, List<Function>> Functions { get; } = new Dictionary<string, List<Function>>();

        public string Name { set; get; }

        #endregion

        private void GetFunctions(string package)
        {
            var matches = GetSubroutines(package, @"(?s)function ([A-Za-z0-9_]+?)\s*\((.*?)\)(\s*)return\s*(.+?);");
            foreach (Match match in matches)
            {
                var function = new Function() { Name = match.Groups[1].Value };
                bool correctArguments;
                ArgumentType returnValueType;
                var correctReturnType = Argument.StringToArgumentType.TryGetValue(
                    match.Groups[4].Value.Trim().ToLower(),
                    out returnValueType);
                ParseAgruments(match.Groups[2].Value, function, out correctArguments);
                if (correctReturnType && correctArguments)
                {
                    function.ReturnValue = new Argument() { Direction = ArgumentDirection.ReturnValue, Type = returnValueType};
                    List<Function> lf;
                    if (Functions.TryGetValue(function.Name, out lf))
                        lf.Add(function);
                    else
                        Functions.Add(function.Name, new List<Function>() {function});
                }
                else
                {
                    if (Options.Instance.Verbose && !correctReturnType)
                        Log.Warn("Unknown return value type. ",
                            $"Package {this.Name}, function {function.Name}, return type {match.Groups[4].Value}");
                }
            }
        }

        private void GetProcedures(string package)
        {
            
            var matches = GetSubroutines(package, @"(?s)procedure\s*([A-Za-z0-9_]+?)\s*\((.*?)\);");
            foreach (Match match in matches)
            {
                var procedure = new Procedure {Name = match.Groups[1].Value};
                bool correct;
                ParseAgruments(match.Groups[2].Value, procedure, out correct);
                if (correct)
                {
                    List<Procedure> pl;
                    if (Procedures.TryGetValue(procedure.Name, out pl))
                        pl.Add(procedure);
                    else
                        Procedures.Add(procedure.Name, new List<Procedure>() {procedure});
                }
            }
        }

        private MatchCollection GetSubroutines(string package, string regex)
        {
            var matches = Regex.Matches(package, regex, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return matches;
        }

        private void ParseAgruments(string argsStr, Subroutine subroutine, out bool correct)
        {
            var args = argsStr.Split(',');
            correct = false;
            foreach (var arg in args)
            {
                var argument = new Argument();
                var components = arg.Trim().Split(' ');
                argument.Name = components[0];
                var _in = false;
                var _out = false;

                try
                {
                    for (var i = 1; i < components.Length; ++i)
                    {
                        var component = components[i].Trim().ToLower();
                        if (component == "" || component == " ")
                            continue;
                        if (component == "in")
                            _in = true;
                        else if (component == "out")
                            _out = true;
                        else if (component == "default")
                        {
                            argument.IsDefault = true;
                            break;
                        }
                        else
                            argument.Type = Argument.StringToArgumentType[component];
                    }
                    if (_in && _out)
                        argument.Direction = ArgumentDirection.InputOutput;
                    else if (_out)
                        argument.Direction = ArgumentDirection.Output;
                    else
                        argument.Direction = ArgumentDirection.Input;
                    subroutine.Arguments.Add(argument.Name, argument);
                    correct = true;
                }
                catch (Exception e)
                {
                    if (Options.Instance.Verbose)
                        Log.Warn(string.Join(" ", components), $"  {this.Name} package, subroutine {subroutine.Name}");
                    correct = false;
                    return;
                }
                
            }

        }

    }
}
