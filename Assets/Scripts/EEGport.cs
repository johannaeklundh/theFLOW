using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;

using libStreamSDK; //Externt library för ThinkGear

namespace EEG
{
    public class EEGport: MonoBehaviour
    {
        public static int numPlayers = 0;
        public int connectionID;

        //public string comPortName = "\\\\.\\"; om comport > "COM9"
        public string comPortName;
        public int errCodeConnect;
        public int errCodeAutoRead;
        public static int enable = -1;
        public static int disable = 0;
        public float att = 0;
        public float med = 0;

        public static GameObject PlayerObject1;
        public static GameObject PlayerObject2;
        public static GameObject PlayerObject3;
        public static GameObject PlayerObject4;
        public float barStatus;

        //public static GameObject[] playerObjects;
        //public static EEGport[] ports;

        public EEGport(string s)
        {
            comPortName = s;
            connectionID = NativeThinkgear.TG_GetNewConnectionId();
        }
        //Funktioner på connection sidan
        public void connect()
        {
            if(errCodeConnect != 0){
                errCodeConnect = NativeThinkgear.TG_Connect(connectionID,
                        comPortName,
                        NativeThinkgear.Baudrate.TG_BAUD_57600,
                        NativeThinkgear.SerialDataFormat.TG_STREAM_PACKETS);
                    
                errCodeAutoRead = NativeThinkgear.TG_EnableAutoRead(connectionID, enable);
            }

            if(errCodeAutoRead == 0){
            numPlayers++;
            }
            else{
                NativeThinkgear.TG_EnableAutoRead(connectionID, disable);
                errCodeAutoRead = 2;
            }
        }

        public void disconnect()
        {
            
            NativeThinkgear.TG_EnableAutoRead(connectionID, disable);
            errCodeAutoRead = 2;

            NativeThinkgear.TG_Disconnect(connectionID);
            errCodeConnect = 2;
            numPlayers--;
            
        }

        public void deleteConnection() // När applikationen ska stängas ner
        {
            //Kollar att Auto Read inte redan blivit 
            if(errCodeAutoRead == -3 || errCodeAutoRead == 0){
            NativeThinkgear.TG_EnableAutoRead(connectionID, disable);
            }
            
            //Utför automatiskt TG_disconnect funktionen vid detta kommand
            NativeThinkgear.TG_FreeConnection(connectionID); 
        }

        // Kan endast uföras om connectionID existerar, TG_connect och TG_EnableAutoRead redan utförts
        public float getAttention()
        {
            return NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.TG_DATA_ATTENTION);
        }
        public float getMeditation()
        {
            return NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.TG_DATA_MEDITATION);
        }

        void Awake(){

                        /*AVKOMMENTERA OM ANDRA KODSTYCKET INTE FUNGERAR*/

            if(PlayerObject4 == null){
                PlayerObject4 = new GameObject("PlayerObject4");
                EEGport p4 = PlayerObject4.AddComponent<EEGport>();
                p4.comPortName = "COM3";
                p4.connectionID = NativeThinkgear.TG_GetNewConnectionId();
                p4.errCodeConnect = 2;
                p4.errCodeAutoRead = 2;
                DontDestroyOnLoad(PlayerObject4);
            }
            if(PlayerObject3 == null){
                PlayerObject3 = new GameObject("PlayerObject3");
                EEGport p3 = PlayerObject3.AddComponent<EEGport>();
                p3.comPortName = "COM1";
                p3.connectionID = NativeThinkgear.TG_GetNewConnectionId();
                p3.errCodeConnect = 2;
                p3.errCodeAutoRead = 2;
                DontDestroyOnLoad(PlayerObject3);
            }
            if(PlayerObject2 == null){
                PlayerObject2 = new GameObject("PlayerObject2");
                EEGport p2 = PlayerObject2.AddComponent<EEGport>();
                p2.comPortName = "COM4";
                p2.connectionID = NativeThinkgear.TG_GetNewConnectionId();
                p2.errCodeConnect = 2;
                p2.errCodeAutoRead = 2;
                DontDestroyOnLoad(PlayerObject2);
            }
            if(PlayerObject1 == null){
                PlayerObject1 = new GameObject("PlayerObject1");
                EEGport p1 = PlayerObject1.AddComponent<EEGport>();
                p1.comPortName = "COM7";
                p1.connectionID = NativeThinkgear.TG_GetNewConnectionId();
                p1.errCodeConnect = 2;
                p1.errCodeAutoRead = 2;
                DontDestroyOnLoad(PlayerObject1);
            }

        //     playerObjects = new GameObject[4];
        //     ports = new EEGport[4];

        //     for(int index = 0; index < playerObjects.Length; index++){
        //         if(playerObjects[index]  == null){
        //             GameObject tempObject = new GameObject("PlayerObject" + (index + 1).ToString()); //?
        //             if(ports[index] == null){
        //                 ports[index] = tempObject.AddComponent<EEGport>();                        //?
        //                 ports[index].connectionID = NativeThinkgear.TG_GetNewConnectionId();
        //                 ports[index].errCodeConnect = 2;
        //                 ports[index].errCodeAutoRead = 2;
        //                 playerObjects[index] = tempObject;
        //                 DontDestroyOnLoad(tempObject);
        //             }
        //         }
        //     }
        //     ports[0].comPortName = "COM7";
        //     ports[1].comPortName = "COM3";
        //     ports[2].comPortName = "COM5";
        //     ports[3].comPortName = "COM9";  
        }

        void Start()
        {  
        }

        void Update()
        { 
            att = NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.TG_DATA_ATTENTION);
            med = NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.TG_DATA_MEDITATION);
        }

    }
}

