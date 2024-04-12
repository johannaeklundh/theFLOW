using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;

using libStreamSDK; //Externt library för ThinkGear

namespace EEGport
{
    public class EEGport: MonoBehaviour
    {
        public int connectionID = 0;
        
        //public string comPortName = "\\\\.\\";
        public string comPortName;
        public int errCode = 0;

        public float attention;
        public float meditation;

        int enable = -1;
        int disable = 0;

        DateTime startTime;
        DateTime lastTimeTrue;

        void Start()
        {
            //Vilken COM port pannbandet ligger på
            comPortName += "COM4";
            Debug.Log(comPortName);
            
            //Skapar ett ThinkGear objekt och länkar det med ett ConnectionID (int)
            connectionID = NativeThinkgear.TG_GetNewConnectionId();
            Debug.Log("TG_GetNewConnectionId returned: " + connectionID);
            
            //Kopplar ConnectionID/objektet med COM porten där pannbandet ligger
            errCode = NativeThinkgear.TG_Connect(connectionID,
                comPortName,
                NativeThinkgear.Baudrate.TG_BAUD_57600,
                NativeThinkgear.SerialDataFormat.TG_STREAM_PACKETS);
            
            //Annat värde än 0 betyder error
            Debug.Log("TG_Connect returned: " + errCode);

            //Skapar en auto read stream som kontinuerligt läser av värden från pannbandet
            errCode = NativeThinkgear.TG_EnableAutoRead(connectionID, enable);
            Debug.Log("TG_EnableAutoRead: "+ errCode);

            startTime = DateTime.Now;
            lastTimeTrue = DateTime.MinValue;
        }

        void Update()
        { 
            //Programmet körs i 10 sekunder
            while ((DateTime.Now - startTime).TotalSeconds < 10)
            {
                //Output skrivs ut varje sekund och ej varje millisekund
                if ((DateTime.Now - lastTimeTrue).TotalSeconds >= 1)
                {
                    //Hämtar ett värde från auto read stream, TG_DATA_ATTENTION är en int med ett värde som signalerar vilken plats värdet ska hämtas ifrån
                    attention = NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.TG_DATA_ATTENTION);
                   
                    Debug.Log("Attention: " + attention);
                
                    //Kollar om det är dålig koppling och skriver ut info, kan användas för exempelvis uppvisning av dålig uppkoppling
                    if (NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.BMD200_DATA_POOR_QUALITY) != 0)
                    {

                        Debug.Log("BMD200_DATA_POOR_QUALITY: : " + (int)NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.BMD200_DATA_POOR_QUALITY));
                    }

                    lastTimeTrue = DateTime.Now;
                }
            }
                //Stoppar auto read
                NativeThinkgear.TG_EnableAutoRead(connectionID, disable);
               
                //Disconnectar och tar bort ConnectionID (frigör minne) inför varje körning. 
                //Måste göras när programmets ska avslutas annars kan det bli oförutsägbara beteenden, t.ex kan ej hitta COM port nästa körning
                NativeThinkgear.TG_FreeConnection(connectionID);    
        }

    }
}

