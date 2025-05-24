public class Program
{
    public static string _weatherDataFile = "Data/station_data.csv";

    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        // TODO: parsing logic for setting create/recreate from command line
        if (true) 
        {
            CreateWeatherStationData();
        }
        Challenge();
        Console.WriteLine("GoodBye, World!");
    }

    static int Challenge()
    {
    var stations = new Dictionary<string, StationData>();

        using (var fileStream = File.OpenRead(_weatherDataFile))
        {
            using (var stream = new StreamReader(fileStream, System.Text.Encoding.UTF8, true))
            {
                String? line;

                while ((line = stream.ReadLine()) != null)
                {
                    var lineArray = line.Split(';');
                    var station = lineArray[0];
                    var reading = lineArray[1];

                    StationData data = new StationData();
                    if (stations.ContainsKey(station)) 
                    {
                        data = stations[station];
                    } else 
                    {
                        data.Sum = 0;
                        data.Count = 1;
                        data.Min = 0;
                        data.Max = 0;
                    }


                }
            }
        }

        return 1;
    }

    static int CreateWeatherStationData()
    {
        HashSet<string> stations = new HashSet<string>();

        if (!File.Exists("Data/weather_stations.csv"))
        {
            Console.WriteLine("ERR: weather stations file not found");
            throw new InvalidDataException("Weather station file not found");
        }

        using (var fileStream = File.OpenRead("Data/weather_stations.csv"))
        {
            using (var stream = new StreamReader(fileStream, System.Text.Encoding.UTF8, true))
            {
                String? line;

                while ((line = stream.ReadLine()) != null)
                {
                    var station = line.Split(';').First();

                    if (!station.StartsWith("#"))
                    {
                        stations.Add(station);
                    }
                }
            }
        }

        if (stations.Count == 0)
        {
            Console.WriteLine("Data/weather_station.csv is empty or something else went wrong");
            throw new InvalidDataException("Weather station file was empty");
        }

        /*
         * The text file contains temperature values for a range of weather stations. 
         * Each row is one measurement in the format <string: station name>;<double: measurement>, 
         * with the measurement value having exactly one fractional digit.
         */

        int fractionalNumberSize = 1;
        int maxTempInt = 99;
        int minTempInt = -99;
        int recordMax = 1_000_000_000;
        int records = 0;

        if (File.Exists(_weatherDataFile))
            File.Delete(_weatherDataFile);

        using (var stream = new StreamWriter(_weatherDataFile))
        {
            while (records < recordMax)
            {
                var rand = new Random();
                foreach (var station in stations)
                {
                    if (records >= recordMax)
                    {
                        break;
                    }

                    var randInt = rand.Next(minTempInt, maxTempInt);
                    var randFractional = Double.Round(rand.NextDouble(), fractionalNumberSize);
                    var temp = randInt + randFractional;

                    stream.WriteLine(station + ";" + temp.ToString());
                    records++;
                }
            }
        }
        return 1;
    }
}

public struct StationData
{
    public double Min;
    public double Max;
    public double Sum;
    public int Count;
}
