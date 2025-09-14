using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearenaApp
{
    public class Weather
    {
        private int weatherId;
        private float temperature;
        private string condition;
        private string recommendation;
        public int WeatherId
        {
            get { return weatherId; }
            set { weatherId = value; }
        }
        public float Temperature
        {
            get { return temperature; }
            set { temperature = value; }
        }
        public string Condition
        {
            get { return condition; }
            set { condition = value; }
        }
        public string Recommendation
        {
            get { return recommendation; }
            set { recommendation = value; }
        }
        public static Weather GetWeather(string location)
        {
            throw new NotImplementedException();
        }
        public void UpdateWeather()
        {
            throw new NotImplementedException();
        }
    }
}
