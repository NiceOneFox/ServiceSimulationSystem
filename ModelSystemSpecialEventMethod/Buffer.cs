using System.Collections.Generic;
using System.Linq;

namespace ModelSystemSpecialEventMethod
{
    class Buffer
    {
       
        public LinkedList<Request> buffer;
        public int bufferSize { get; set; } = 5;

        public int amountOfFailure { get; set; } = 0; // количество отказов
        public Buffer(int Size)
        {
            this.buffer = new LinkedList<Request>();
            bufferSize = Size;
        }

        public int Count { get; set; } = 0;


        public void AddToBuffer(Request request) 
        {

            if(Count >= bufferSize)
            {
                Request tempReq = buffer.Last();
                buffer.RemoveLast();

                buffer.AddLast(request);

                Program.EndRequests.Add(tempReq); // учитываем ушедшую заявку

                int i = 0;
                foreach (var source in Program.sources) // учитываем ушедшую заявку источника
                {
                    if (i == tempReq.numberSource)
                    {
                        source.declined += 1;
                        source.Tpreb = Program.time - tempReq.dateTime;                        
                    }
                    i++;
                }

                Program.state = "в заявке отказано: " + tempReq.uniqueID;
                amountOfFailure++;
                return;
            }
           
            Program.state = "заявка передана в буфер: " + request.uniqueID;
            buffer.AddLast(request);
            Count++;

            return;
        }


        public Request takeFromBuffer(float currentTime) 
        {

            if (this.Count == 0) // буфер пуст
            {
                return null;
            }

            int temp = int.MinValue;
            //Request tempReq = new Request();
            int numberOfSource = -1;

            List<Request> indexesOfSamePriority = new List<Request>();

            foreach (var req in this.buffer)
            {

                numberOfSource = req.numberSource; // получаем номер источника

                int priority = Program.sources.ElementAt(numberOfSource).priority; // получаем приоритет источника

                if (priority > temp)
                {
                    indexesOfSamePriority.Clear();

                    temp = priority;
                    indexesOfSamePriority.Add(req);
                    //tempReq = req; // возможно эта заявка самая приоритетная, запоминаем её
                }

                if (priority == temp)
                {
                    indexesOfSamePriority.Add(req); // номер источника с высшем приоритетом
                }

            }

            float maxTime = float.MinValue;
            Request tempReq = new Request();

            foreach (var req in indexesOfSamePriority) // ищем последнюю поступившую из заявок с наивысшем приоритетом
            {
                if (req.dateTime >= maxTime)
                {
                    maxTime = req.dateTime;
                    tempReq = req;
                }
            }
            Count--;
            buffer.Remove(tempReq); // забираем из буфера          

            int i = 0;
            foreach (var source in Program.sources)
            {
                if (i == tempReq.numberSource)
                {
                    source.Tbp = Program.time - tempReq.dateTime;
                    source.TbpAmount++;
                    source.allTbp.Add(Program.time - tempReq.dateTime);
                }
                i++;
            }

            Program.state = "Заявка передана на прибор" + tempReq.uniqueID;

            return tempReq;
        
        }
    }
}
