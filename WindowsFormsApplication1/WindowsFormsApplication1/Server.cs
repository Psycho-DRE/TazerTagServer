using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Collections;
using System.IO;




namespace WindowsFormsApplication1
{
    class Server
    {
        public static String cmd = "läuft";
        static System.Net.IPAddress ip = System.Net.IPAddress.Parse("127.0.0.1");
        private static TcpListener listener = null;
        // Die Liste der laufenden Server-Threads
        private static ArrayList threads = new ArrayList();
        // Die Hauptfunktion des Servers
        public static void StartServer()
        {
            // Listener initialisieren und starten
            listener = new TcpListener(ip, 4711);
            listener.Start();
            // Haupt-Server-Thread initialisieren und starten
            Thread th = new Thread(new ThreadStart(Run));
            th.Start();
            // Benutzerbefehle entgegennehmen
            
            
            while (!cmd.ToLower().Equals("stop"))
            {
               // cmd = Console.ReadLine();
                //if (!cmd.ToLower().Equals("stop"))
                Console.WriteLine("Unbekannter Befehl: " + cmd);
            }
       
            // Alle Server-Threads stoppen

            foreach(ServerThread thread in threads)
            {
                thread.stop = true;
                while(thread.running)
                {
                    Thread.Sleep(1000);
                }
            }
            //for (IEnumerator e = threads.GetEnumerator(); e.MoveNext();)
            //{
            //    // Nächsten Server-Thread holen
            //    ServerThread st = (ServerThread)e.Current;
            //    // und stoppen
            //    st.stop = true;
            //    while (st.running)
            //        Thread.Sleep(1000);
            //}
            // Listener stoppen
            listener.Stop();

            // Haupt-Server-Thread stoppen
            th.Abort();
        }

        

        // Hauptthread des Servers
        // Nimmt die Verbindungswünsche von Clients entgegen
        // und startet die Server-Threads für die Clients
        public static void Run()
        {
            while (true)
            {
                // Wartet auf eingehenden Verbindungswunsch
                TcpClient c = listener.AcceptTcpClient();
                // Initialisiert und startet einen Server-Thread
                // und fügt ihn zur Liste der Server-Threads hinzu
                threads.Add(new ServerThread(c));
            }
        }

    }

    class ServerThread
    {
        // Stop-Flag
        public bool stop = false;
        // Flag für "Thread läuft"
        public bool running = false;
        // Die Verbindung zum Client
        private TcpClient connection = null;
        // Speichert die Verbindung zum Client und startet den Thread
        public ServerThread(TcpClient connection)
        {
            // Speichert die Verbindung zu Client,
            // um sie später schließen zu können
            this.connection = connection;
            // Initialisiert und startet den Thread
            new Thread(new ThreadStart(Run)).Start();
        }
        // Der eigentliche Thread
        public void Run()
        {
            // Setze Flag für "Thread läuft"
            this.running = true;
            // Hole den Stream für's schreiben
            Stream outStream = this.connection.GetStream();
            String buf = null;
            bool loop = true;
            while (loop)
            {
                try
                {
                    // Hole die aktuelle Zeit als String
                    String time = DateTime.Now.ToString();
                    // Sende Zeit nur wenn sie sich von der vorherigen unterscheidet
                    if (!time.Equals(buf))
                    {
                        // Wandele den Zeitstring in ein Byte-Array um
                        // Es wird noch ein Carriage-Return-Linefeed angefügt
                        // so daß das Lesen auf Client-Seite einfacher wird
                        Byte[] sendBytes = Encoding.ASCII.GetBytes(time + "\r\n");
                        // Sende die Bytes zum Client
                        outStream.Write(sendBytes, 0, sendBytes.Length);
                        // Merke die Zeit
                        buf = time;
                    }
                    // Wiederhole die Schleife so lange bis von außen der Stopwunsch kommt
                    loop = !this.stop;
                }
                catch (Exception)
                {
                    // oder bis ein Fehler aufgetreten ist
                    loop = false;
                }
            }
            // Schließe die Verbindung zum Client
            this.connection.Close();
            // Setze das Flag "Thread läuft" zurück
            this.running = false;
        }
    }
}
