using System.Text.RegularExpressions;
namespace RoutingExample.CustomConstraints;
//Először is implementálni kell az IRouteConstraint interfacet, amivel azonnal be is tudod húzni a Match() függvényt.

//Példa útvonal: employee/5/apr, aminek a felépítési sablonja: employee/{id:int:min(0)}/{month:regex}

//FONTOS: Amikor kész a classunk, buildeljük le, hogy a "builder" objektumba bekerüljön az osztály!
public class MonthsCustomConstraintClass : IRouteConstraint
{
    public bool Match(
        HttpContext? httpContext, //ez magát a Context-et tárolja, amivel el tudjuk érni a requestet és respondot.
        IRouter? route, //Ezt ASP.Net Core-ban szinte sosem használják. Legacy MVC maradvány
        string routeKey, // Annak a route paraméternek a neve, amelyhez EZ a constraint tartozik. 
        RouteValueDictionary values, // Ez a legfontosabb! Az URL-ből kinyert ÖSSZES route paraméter kulcs-érték párja.
        RouteDirection routeDirection //Haladó téma: request feldolgozás vs URL generálás.
        )
    {
        //Leellenőrizzük, hogy van-e megfelelő paraméterünk
        if (!values.ContainsKey(routeKey))
        {
            return false;
        }

        Regex monthRegex = new Regex($"^(jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec)$");
        string? monthValue = values[routeKey].ToString();
        
        //Leellenőrizzük, hogy a paraméter értéke megfelel-e a regexnek
        if (monthRegex.IsMatch(monthValue))
        {
             return true;
        }
        else
            return false;
    }
}
