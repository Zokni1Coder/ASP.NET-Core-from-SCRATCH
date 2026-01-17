using MathApp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (HttpContext context) =>
{
    //Ez a rész benne van a korábbi magyarázatokban.
    StreamReader sr = new StreamReader(context.Request.Body);
    string body = await sr.ReadToEndAsync();
    Dictionary<string, StringValues> queryString = QueryHelpers.ParseQuery(body);

    //valueproblems tárolja az összes olyan Key-t ami nem létezik, de mi keressük.
    List<string> valueproblems = new List<string>();
    if (!queryString.ContainsKey("firstNumber"))
        valueproblems.Add("firstNumber");
    if (!queryString.ContainsKey("secondNumber"))
        valueproblems.Add("secondNumber");
    if (!queryString.ContainsKey("Operation"))
        valueproblems.Add("Operation");

    //Ha van olyan mező, mely nem létezik a fentiek közül, akkor a Státusz kód 400 lesz és kiírja az oldalra, hogy konkrétan melyik mező hiányzik.
    if(valueproblems.Count > 0)
    {
        context.Response.StatusCode = 400;
        foreach (var item in valueproblems)
        {
            context.Response.WriteAsync($"Invalid input for '{item}'.");
        }       
    }

    //Ha van QueryString az url-ben, akkor lefut a kalkulálás. 
    //Miért kell? Mert ha nincs itt, akkor a kezetekkor befutó localhost:xxxx nem tartalmaz QueryString-et és folyamatosan errorba fog futni. Tehát előbb le kell ellenőrizni, hogy egyáltalán adtunk-e vagy csak a nyitó oldalon vagyunk.
    if (queryString.Count() != 0)
    {
        operationTypes choosen;
        try
        {
            //Megpróbálja parse-olni a QueryStringet és ha sikerül, akkor az értéket a choosen mezőben menti el. Ha sikeres a konvertálás akkor true értéket ad vissza, he nem akkor false.
            if (Enum.TryParse(queryString["Operation"], out choosen) != false)
            {
                Calculating(choosen, int.Parse(queryString["firstNumber"]), int.Parse(queryString["secondNumber"]), context);
            }
            else
                throw new MathApp.InvalidOperationException();
        }
        //Itt a catch-ben fontos a példány átatáds (ex), mert ezzel tudjuk elérni az objektumon belüli nyilvános eljárást.
        catch (MathApp.InvalidOperationException ex)
        {
            ex.Outwriting(context);
        }
        catch (Exception)
        {
            throw;
        }
    }
});
app.Run();


//Ez maga a kalkulálást végző eljárás. A HttpContext-et azért kell átadni, mert az OutWritingClient-nek tovább kell adni. Más célt nem szolgál. Amennyiben valamilyen oknál fogva parse-olni tudtunk egy olyan értéket enumként, ami nincs itt (tehát bugos lenne), akkor is le van kezelve a default résszel egy státusz 400 és egy kiírással. 
static void Calculating(operationTypes operation, int firstNumber, int secondNumber, HttpContext context)
{
    int result;
    switch (operation)
    {
        case operationTypes.Addition:
            result = firstNumber + secondNumber;
            OutwritingToClient(operation, result, context);
            break;
        case operationTypes.Subtraction:
            result = firstNumber - secondNumber;
            OutwritingToClient(operation, result, context);
            break;
        case operationTypes.Multiplication:
            result = firstNumber * secondNumber;
            OutwritingToClient(operation, result, context);
            break;
        case operationTypes.Division:
            try
            {
                if (secondNumber == 0)
                {
                    throw new NullDivisor();
                }
                result = secondNumber / firstNumber;
                OutwritingToClient(operation, result, context);
            }
            catch (NullDivisor ex)
            {
                ex.Outwriting(context);
            }
            catch (Exception)
            {
                throw;
            }
            break;
        case operationTypes.Modulo:
            try
            {
                if (secondNumber == 0)
                {
                    throw new NullDivisor();
                }
                result = secondNumber % firstNumber;
                OutwritingToClient(operation, result, context);
            }
            catch (NullDivisor ex)
            {
                ex.Outwriting(context);
            }
            catch (Exception)
            {
                throw;
            }
            break;
        default:
            context.Response.StatusCode = 400;
            context.Response.WriteAsync("Something went wrong!");
            break;
    }
}

//Magát a kiírást elvégző eljárás.
static void OutwritingToClient(operationTypes type, int result, HttpContext context)
{
   context.Response.WriteAsync($"The result of {type} is: {result}.");
}

/// <summary>
/// Enumok arra jók, hogy a sűrűn használt értékeket ne kézzel kelljen begépelni, mert hiba forrása lehet. Ezért érdemesebb definiálni és csak folyamatosan meghívni a megfelelő értékkel. 
/// </summary>
enum operationTypes
{
    Addition,
    Subtraction,
    Multiplication,
    Division,
    Modulo
}


