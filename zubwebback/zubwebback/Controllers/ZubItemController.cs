using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using zubwebback.DataObjects;
using zubwebback.Models;

namespace zubwebback.Controllers
{
    public class ZubItemController : TableController<ZubItem>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            zubwebContext context = new zubwebContext();
            DomainManager = new EntityDomainManager<ZubItem>(context, Request);
        }

        // GET tables/
        public IQueryable<ZubItem> GetAllItems()
        {
            return Query();
        }

        // GET tables/
        public SingleResult<ZubItem> GetItem(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/
        public Task<ZubItem> PatchItem(string id, Delta<ZubItem> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/
        public async Task<IHttpActionResult> PostItem(ZubItem item)
        {
            ZubItem current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/
        public Task DeleteItem(string id)
        {
            return DeleteAsync(id);
        }
    }
}