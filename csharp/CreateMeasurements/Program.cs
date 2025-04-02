public class Program
{
    public static readonly int MAX_NAME_LEN = 100;
    public static readonly int KEYSET_SIZE = 10_000;

    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        CreateUniqueWeatherStations(true);
    }

    static int CreateUniqueWeatherStations(bool recreate)
    {
        if (File.Exists("Data/weather_station_names.csv") & !recreate)
            return 1;

        if (!File.Exists("Data/weather_stations.csv"))
        {
            Console.WriteLine("ERR: weather stations file not found");
            return -1;
        }

        // process weather_stations.csv and pull out all unique names
        // and put into file for generating random values

        HashSet<string> stations = new HashSet<string>();

        using (var fileStream = File.OpenRead("Data/weather_stations.csv"))
        {
            using (var stream = new StreamReader(fileStream, System.Text.Encoding.UTF8, true))
            {
                String? line;

                while ((line = stream.ReadLine()) != null)
                {
                    var station = line.Split(';').First();

                    stations.Add(station);
                }
            }
        }

        if (stations.Count == 0)
        {
            Console.WriteLine("Data/weather_station.csv is empty or something else went wrong");
            return -1;
        }

        using (var outputFile = File.Create("Data/weather_station_names.csv"))
        {
            using (var stream = new StreamWriter(outputFile)) {


                foreach (var station in stations)
                {
                    stream.WriteLine($"{station}");
                }
            }
        }

        return 1;
    }
}
