
namespace Countries_App.RouteConstraintClasses
{
    public class countryIDConstraint : IRouteConstraint
    {
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.ContainsKey(routeKey))
            {
                return false;
            }

            int countryID = Convert.ToInt32(values[routeKey]);

            if (countryID > 5 || countryID < 1)
            {
                return false;
            }
            return true;
        }
    }
}
