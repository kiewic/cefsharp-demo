using CefSharp;

namespace CefSharpApp
{
    internal class CustomSchemeHandlerFactory : ISchemeHandlerFactory
    {
        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            return new CustomResourceHandler();
        }
    }
}