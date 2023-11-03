using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasyArchitect.Web.Blazor.Components
{
    public partial class GridView
    {
        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public object DataSource { get; set; }

        protected object _dataSource = null;

        protected override void OnParametersSet()
        {
            _dataSource = DataSource;

            base.OnParametersSet();
        }

        /// <summary>
        /// 取得在 Response OutputStream 中的資料列（傳入的資料應該是強型別）
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 根據 startURL 取得 Response OutputStream
        /// </summary>
        /// <param name="_startUrl"></param>
        /// <returns></returns>
        public static async Task<object> GetData<T>(string _startUrl)
        {
            object result = null;

            var request = new HttpRequestMessage(HttpMethod.Get, _startUrl);

            HttpClient _client = new HttpClient();

            _client.BaseAddress = new Uri(_startUrl);

            var client = _client;

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();

                result = await JsonSerializer.DeserializeAsync<IEnumerable<T>>(
                    responseStream,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }

            return result;
        }
    }
}
