using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageWizard.Filters
{
    public class FilterAction
    {
        public Regex Regex { get; set; }

        public IFilter Filter { get; set; }

        public MethodInfo TargetMethod { get; set; }

        public IEnumerable<Type> ParameterTypes { get; set; }

        public bool TryExecute(string input, FilterContext filterContext)
        {
            Match match = Regex.Match(input);

            if(match.Success == false)
            {
                return false;
            }

            List<object> parameterValues = new List<object>();

            foreach(ParameterInfo pi in TargetMethod.GetParameters())
            {
                string value = match.Groups[pi.Name].Value;

                if (pi.ParameterType == typeof(int))
                {
                    parameterValues.Add(int.Parse(value));
                }
                else if(pi.ParameterType.IsEnum)
                {
                    parameterValues.Add(Enum.Parse(pi.ParameterType, value, true));
                }
            }

            parameterValues.Add(filterContext);

            TargetMethod.Invoke(Filter, parameterValues.ToArray());

            return true;
        }
    }
}
