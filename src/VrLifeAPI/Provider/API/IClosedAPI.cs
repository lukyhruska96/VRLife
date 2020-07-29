
using VrLifeAPI.Provider.API.APIs;

namespace VrLifeAPI.Provider.API
{

    /// <summary>
    /// Interface ClosedAPI na straně Providera.
    /// </summary>
    public interface IClosedAPI
    {

        /// <summary>
        /// Instance OpenAPI na straně Providera.
        /// </summary>
        IOpenAPI OpenAPI { get; }

        /// <summary>
        /// Instance poskytovatele služeb na straně Providera.
        /// </summary>
        IServiceProvider Services { get; }
    }
}
