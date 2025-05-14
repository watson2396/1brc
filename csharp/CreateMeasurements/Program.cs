public class Program
{

    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        // TODO: parsing logic for setting create/recreate from command line
        CreateWeatherStationData(true);
        Console.WriteLine("GoodBye, World!");
    }

    static int CreateWeatherStationData(bool recreate)
    {

        if (!recreate)
            return 1;

        var stations = CreateUniqueWeatherStations(recreate);

        /*
         * The text file contains temperature values for a range of weather stations. 
         * Each row is one measurement in the format <string: station name>;<double: measurement>, 
         * with the measurement value having exactly one fractional digit.
         */

        // Loop through stations and create a temp each
        //  iteration and go until hitting 1 billion ?

        string weatherDataFile = "Data/station_data.csv";
        int fractionalNumberSize = 1;
        int maxTempInt = 99;
        int minTempInt = -99;
        int recordMax = 1_000_000_000;
        int records = 0;

        if (File.Exists(weatherDataFile))
            File.Delete(weatherDataFile);

        using (var stream = new StreamWriter(weatherDataFile))
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

    static List<string> CreateUniqueWeatherStations(bool recreate)
    {
        string stationNameFile = "Data/weather_station_names.csv";
        List<string> stations = new List<string>();

        if (File.Exists(stationNameFile) && !recreate)
        {
            using (var fileStream = File.OpenRead(stationNameFile))
            {
                using (var stream = new StreamReader(fileStream, System.Text.Encoding.UTF8, true))
                {
                    String? line;

                    while ((line = stream.ReadLine()) != null)
                    {
                        var station = line;

                        if (!station.StartsWith("#"))
                        {
                            stations.Add(station);
                        }
                    }
                }
            }

            return stations;
        }

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

        if (File.Exists(stationNameFile))
            File.Delete(stationNameFile);

        using (var stream = new StreamWriter(stationNameFile))
        {
            foreach (var station in stations)
            {
                stream.WriteLine($"{station}");
            }
            Console.WriteLine($"File: {stationNameFile} has been created");
        }

        return stations;
    }
}
