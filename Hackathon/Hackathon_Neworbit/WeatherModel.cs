namespace Hackathon_Neworbit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WeatherModel
{
    public IEnumerable<WeatherDay> list {get; set;}
    //public string WeatherCondition { get; set; }
}


public class WeatherDay
{
    public IEnumerable<Weather> weather { get; set; }
}

public class Weather
{
    public string main { get; set; }
}
