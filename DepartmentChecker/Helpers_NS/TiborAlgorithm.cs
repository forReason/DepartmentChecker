using DepartmentChecker.Objects_NS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Helpers_NS
{
    public class TiborAlgorithm
    {
        public static async Task<List<CatalogItem>> SetupCatalogState(string department, bool isCadre, List<CatalogItem> items)
        {

            // this is a generation for all tags in the dataset. presumably to fill the filter list
            HashSet<string> tags = new HashSet<string>();
            department = WebUtility.HtmlDecode(department).Replace(' ', '\u00A0');

            // normalize all relevant columns of the dataset entry
            async Task ProcessItem(CatalogItem item)
            {
                foreach (var tag in item.Tags)
                {
                    tags.Add(WebUtility.HtmlDecode(tag));
                }

                for (int i = 0; i < item.TargetAudience.Count; i++)
                {
                    item.TargetAudience[i] = WebUtility.HtmlDecode(item.TargetAudience[i]);
                }

                for (int i = 0; i < item.TargetAudienceMandatory.Count; i++)
                {
                    item.TargetAudienceMandatory[i] = WebUtility.HtmlDecode(item.TargetAudienceMandatory[i]);
                }
            }

            // normalize all relevant columns of all records
            await Task.WhenAll(items.Select(item => ProcessItem(item))); // single threaded (!)

            // filter dataset
            var cadreItems = items.Where(item => item.IsCadre).ToList();
            var catalogItems = items.Where(item => !item.IsCadre).ToList();

            var filteredItems = catalogItems.Where(item =>
                item.ForAllMandatory || item.TargetAudienceMandatory.Any(audience =>
                    String.Equals(audience, department, StringComparison.InvariantCulture))
            ).ToList();

            /// set state (not included)

            return filteredItems;
        }
    }
}
