namespace ModelSystemSpecialEventMethod
{
    class Device
    {
        public int numOfDevice { get; set; }
        public bool isWorking { get; set; } = false;

        public float timeWorkingOnRequest { get; set; } // TODO

        public float startTime;

        public Request requestInDevice { get; set; } = null;

        public float sumTimeOfWork { get; set; } = 0;

        public int numberOfFinishedRequests { get; set; } = 0;

        public float timeOfDeviceWillBeFree { get; set; }

        public Device(int numOfDevice, bool isWorking, float timeWorking)
        {
            this.numOfDevice = numOfDevice;
            this.isWorking = isWorking;
            this.timeWorkingOnRequest = timeWorking;
        }

        public Device() { }

        public bool CheckIfDeviceFree(float currentTime) // освобождаем прибор
        {

            if (timeOfDeviceWillBeFree <= currentTime)
            {
                isWorking = false;
             
                Program.state = "освобождён прибор:" + "№ " + numOfDevice + " T : " + currentTime;


                sumTimeOfWork += currentTime - startTime; // считаем загруженность 

                requestInDevice.EndTime = currentTime; // считаем время пребывания заявки в системе

                int i = 0;
                foreach (var source in Program.sources) // учитываем ушедшую заявку источника
                {
                    if (i == requestInDevice.numberSource)
                    {
                        source.Tpreb = Program.time - requestInDevice.dateTime;
                        source.TObcl = Program.time - startTime;
                    }
                    i++;
                }

                Program.EndRequests.Add(requestInDevice);

                requestInDevice = null;
                numberOfFinishedRequests++;

                timeOfDeviceWillBeFree = float.MaxValue;

                return true; // прибор освободился
            }

            return false; // прибор ещё занят
        }

        public void takeRequest(Request req, float currentTime) // взять заявку на обслуживание
        {
            if (req == null || isWorking)
            {
                return;
            }

            if (isWorking == false)
            {
                requestInDevice = req; // помещаем заявку в прибор

                startTime = currentTime; //  время начало обработки
                isWorking = true;

                timeOfDeviceWillBeFree = startTime + Program.ExponentialTimeWorkingDevice(); // время конца обработки
            }

            Program.state = "Заявка передана на прибор " + req.uniqueID;
        }
    }
}
