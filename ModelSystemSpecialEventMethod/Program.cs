using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ModelSystemSpecialEventMethod
{
    static class Program // var4
    {
        // параметры необходимо задать: 
        public static int amountSouce = 8; // количество источников
        public static int bufferSize = 4; // размер буфера
        public static int amountDevice = 4; // количество приборов       
        public static int totalRequestGenerate = 950; // количество заявок, которое хотим пропустить через систему
        public static int numberOfRequestsWereGenerated = 0;

        public static float lambda = 14.5f;  // интенсивность потока // нужно настраивать в теории
        public static float newLambda;

        public static float timeOfSourceGenerateNewRequest = 0.1f; // промежутки времени когда генерируется новая заявка источника       

        public static float time = 0; // текущее время системы

        public static LinkedList<Source> sources; // массив источников
        public static Buffer buffer;
        public static LinkedList<Device> devices;

        public static int pointerOnDevice = 0; // Д2П2

        public static List<int> prioritiesOfSources = new List<int>() { 1, 0, 3, 4, 7, 6, 5, 8, 9, 12, 11, 10 }; // можно задать приоритеты источников

        public static Random random = new Random();

        // вывод результатов
        public static List<Request> EndRequests = new List<Request>();
        public static List<float> averateTimeOfWorkingDevice = new List<float>();
        public static string[] finalResults;

        public static List<Request> allRequestInSystem = new List<Request>();

        public static string state = "Начало моделирования: " + time;      

        public static float Pfaiilure = 0;
        public static float WorkloadDevices = 0;
        public static float Pworkload = 0;

        public static void InitilizeInputParameters(int numOfSource, int bufSize, int numOfDeivce, int N)
        {
            amountSouce = numOfSource;
            bufferSize = bufSize;
            amountDevice = numOfDeivce;
            totalRequestGenerate = N;
        }

        public static void InitiliazeRequestsRange(float timeRangeNewRequests)
        {
            timeOfSourceGenerateNewRequest = timeRangeNewRequests;
        }

        public static float ExponentialTimeWorkingDevice() 
        {
            float temp = (float)((-1) * (1 / lambda) * Math.Log(random.NextDouble()));
            averateTimeOfWorkingDevice.Add(temp);

            return temp;
        }

        public static void ClearOutputParameters()
        {
            EndRequests = new List<Request>();
            averateTimeOfWorkingDevice = new List<float>();
            allRequestInSystem = new List<Request>();           
        }

        public static void InitializeInputData()
        {
            pointerOnDevice = 0;
            time = 0;
            numberOfRequestsWereGenerated = 0;
            sources = new LinkedList<Source>();
            for (int i = 0; i < amountSouce; i++)
            {
                sources.AddLast(new Source(i, default, prioritiesOfSources.ElementAt(i)));
            }

            buffer = new Buffer(bufferSize)
            {
                bufferSize = bufferSize // размер буфера
            };

            devices = new LinkedList<Device>(); // Инициализая N приборов
            for (int i = 0; i < amountDevice; i++) // i - номер прибора
            {
                devices.AddLast(new Device(i, false, ExponentialTimeWorkingDevice()));
            }
        }


        private static void FindNextSpecialEvent() // найти ближайшее особое событие 
        {
          
            if (buffer.Count > 0) // взять заявку из буфера если есть свободоные приборы
            {
                foreach(var device in devices)
                {
                    if (!device.isWorking)
                    {
                        device.takeRequest(buffer.takeFromBuffer(time), time);
                        time = time + 0.000001f;
                        return;
                    }                  
                }
            }

            // поиск ближайшего особого события
            float tempMinDevice = float.MaxValue;
            Device tempDevice = new Device();
            foreach(var device in devices)
            {
                if (device.isWorking)
                {
                    if (device.timeOfDeviceWillBeFree < tempMinDevice)
                    {
                        tempMinDevice = device.timeOfDeviceWillBeFree;
                        tempDevice = device;
                    }
                }
            }

            float tempMinRequest = float.MaxValue;
            Source tempSource = new Source();
            foreach(var source in sources)
            {
                if (source.sourceRequests != null)
                {
                    if(source.sourceRequests.dateTime < tempMinRequest)
                    {
                        tempMinRequest = source.sourceRequests.dateTime;
                        tempSource = source;
                    }
                }
            }

            if (tempMinDevice < tempMinRequest) // освобождаем прибор ближайший по времени
            {
                
                foreach (var device in devices) // необходимо запомнить какой прибор надо освободить
                {
                   if (device == tempDevice)
                   {
                        time = device.timeOfDeviceWillBeFree; // перемещаемся по времени
                        device.CheckIfDeviceFree(time);
                        break;
                   }
                }
            }
         
            if(tempMinDevice > tempMinRequest) // передаём заявку в систему ближайшую по времени
            {
                
                foreach(var source in sources)
                {
                    if (source.GetTimeRequest() == -1)
                    {
                        continue;
                    }
                    if (source == tempSource) // передать заявку на прибор либо положить в буфер
                    {
                        bool isFreeDevice = false;

                        for (int i = pointerOnDevice; i < devices.Count; i++) // с указателя до конца
                        {
                            if (!devices.ElementAt(i).isWorking)
                            {

                                isFreeDevice = true;

                                time = source.GetTimeRequest(); // перемещаемся по времени

                                devices.ElementAt(i).takeRequest(source.GetNewRequest(), time);

                                pointerOnDevice = i; // новый указатель
                                break;
                            }
                        }

                        if (!isFreeDevice)
                        {
                            for (int j = 0; j < pointerOnDevice; j++) // с начала и до указателя
                            {
                                if (!devices.ElementAt(j).isWorking)
                                {

                                    isFreeDevice = true;

                                    time = source.GetTimeRequest(); // перемещаемся по времени

                                    devices.ElementAt(j).takeRequest(source.GetNewRequest(), time);

                                    pointerOnDevice = j; // новый указатель
                                    break;
                                }
                            }
                        }

                        if (!isFreeDevice)
                        {
                            buffer.AddToBuffer(source.GetNewRequest()); // передаём в буфер если приборы заняты
                        }
                        break;
                    }
                }
            }
            
        }

        public static bool CheckIfAllDevicesAreFree() // true если все свободны
        {
            foreach (var device in devices)
            {
                if (device.isWorking)
                {
                    return false;
                }
            }
            return true;
        }

        public static void AutoMode()
        {
            InitializeInputData();

           
            while (true) // пока есть особые события или приборы не освобождены
            {
                if ((numberOfRequestsWereGenerated >= totalRequestGenerate) && CheckIfAllDevicesAreFree() && buffer.Count <= 0 && CheckIfAllSourcesEnd())
                {
                    Program.state = "Конец моделирования";// + time;
                    break;
                }
                FindNextSpecialEvent();
                // после особого события - это шаг - выводит состояние
            }

            CountResults();
        }

        public static void CountResults()
        {
            // считаем результаты

            // количество обработанных заявок
            int totalRequestsFinished = 0;
            foreach (var device in devices)
            {
                totalRequestsFinished += device.numberOfFinishedRequests;
            }

            newLambda = totalRequestGenerate / time;

            // вероятность отказа
            double ProbabilityOfFailure = (double)buffer.amountOfFailure / totalRequestGenerate;


            // T пребывания
            float SumOfTime = 0;
            List<float> timeInSystemOfRequsts = new List<float>();
            foreach (var req in EndRequests)
            {
                if (req.EndTime == 0)
                {
                    req.EndTime = req.dateTime; //в заявке было отказано
                }
                timeInSystemOfRequsts.Add(req.EndTime - req.dateTime);
            }
            foreach (var time in timeInSystemOfRequsts)
            {
                SumOfTime += time;
            }
            double averageTime = (double)SumOfTime / timeInSystemOfRequsts.Count();
            

            // P загруженности 
            float sumTimeOfWorkDevices = 0;
            foreach (var device in devices)
            {
                sumTimeOfWorkDevices += device.sumTimeOfWork;
            }
            double averageTimeOfWorkingDevice = (double)sumTimeOfWorkDevices / devices.Count();
            double workload = averageTimeOfWorkingDevice / time;

            // P загруженности
            float lambdai = sources.Count() * timeOfSourceGenerateNewRequest;
            float Ui = Program.EndRequests.Count();
            float p = lambdai / Ui;

            // дисперсия обслуживания
            double D = 1 / Math.Pow(lambda, 2);


            // среднее время работы прибора
            float sumTimeOfWorkingDeviceOnRequest = 0f;
            foreach (var time in averateTimeOfWorkingDevice)
            {
                sumTimeOfWorkingDeviceOnRequest += time;
            }
            sumTimeOfWorkingDeviceOnRequest = sumTimeOfWorkingDeviceOnRequest / averateTimeOfWorkingDevice.Count();


            Pfaiilure = (float)ProbabilityOfFailure;
            WorkloadDevices = (float)workload;
            Pworkload = p;

            finalResults = new string[] { 
             "Количество отказов: " + buffer.amountOfFailure,
              "Количество заявок: " + numberOfRequestsWereGenerated,
             "Вероятность отказа: " + ProbabilityOfFailure,
            "Т пребывания: " + averageTime,
            "Время работы прибора среднее: " + sumTimeOfWorkingDeviceOnRequest,
            "Загруженность приборов: " + workload.ToString(),
            "Дисперсия обслуживания: " + D
            ,"Новое Lambda значение:" + newLambda,
            "P загруженность: " + p,
            };
        }

        private static bool CheckIfAllSourcesEnd()
        {
            foreach(var source in sources)
            {
                if (source.GetTimeRequest() != -1)
                {
                    return false;
                }
            }
            return true;
        }


        public static void OneStep()
        {

            if ((numberOfRequestsWereGenerated >= totalRequestGenerate) && CheckIfAllDevicesAreFree() && buffer.Count == 0 && CheckIfAllSourcesEnd())
            {
                Program.state = "Конец моделирования";// + time;
                return;
            }
            FindNextSpecialEvent();
        }
        

        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Choose());
         
        }
    }
}
