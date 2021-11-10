using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EasyArchitect.Web.Razor
{
    public partial class GridView: ComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public object DataSource { get; set; }

        protected object _dataSource = null;

        protected override void OnParametersSet()
        {
            _dataSource = DataSource;

            base.OnParametersSet();
        }

        string GetDataRow(object row)
        {
            string result = string.Empty;

            PropertyInfo[] ps = row.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Default | BindingFlags.Public);

            result += "<tr>";
            foreach (var p in ps)
            {
                result += $"<td>{p.GetValue(row)}</td>";
            }
            result += "</tr>";

            return result;
        }
    }
}
