using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VrLifeClient.API
{
    class ClosedAPI
    {
        private OpenAPI _openApi;
        public OpenAPI OpenAPI { get => _openApi; }

        public ClosedAPI(OpenAPI openAPI)
        {
            this._openApi = openAPI;
        }
    }
}
