using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Timers;
using System.Linq;
using System.Net;

//for server beastlords.strivetechies.com

namespace RunningGoogleDriveUpdateArk
{
    class Program
    {
        //for ark update
        static Timer messageTimer;
        static DateTime CurrTime = DateTime.Now;
        static string arkrcon = "/home/akira777/rcon/rcon -Pxxxxxxx -a127.0.0.1 -p32330";
        //private static string STEAMDIR = "/home/akira777/Steam";
        private static string STEAMCMDDIR = "/home/akira777/steamcmd";

        private static int CountDownMinutes = 10;
        //STEAMCMDSCRIPT=update.txt
        private static string SERVERDIR = "/home/akira777/Ark";


        /*----------*/
        static string appPathError = Environment.CurrentDirectory + "/logCopyGoogleDrive.txt";

        static void Main(string[] args)
        {

            // Console.WriteLine();


            //var result = ReturnConsoleData("echo \"hello\";");

            string processResult = string.Empty;


                     if (returnCountDirectoryGoogleDrive() > 1)
                     {
                         processResult +=  ReturnConsoleData("sudo cp -ru /home/akira777/Ark/ShooterGame/Saved/ /media/GoogleDrive/ArkGame_Backup",true);

                     }

                     else
                     {
                         processResult += ReturnConsoleData("sudo google-drive-ocamlfuse /media/GoogleDrive/",true);
                         processResult += ReturnConsoleData("sudo cp -ru /home/akira777/Ark/ShooterGame/Saved/ /media/GoogleDrive/ArkGame_Backup",true);

            }

            logData(processResult);

            //update ark
               UpdateArk();

          Console.WriteLine("Press \'q\' to quit the app.");
          while (Console.Read() != 'q');

            //Console.Read();

          //  ReturnConsoleData("arkrcon broadcast hello there",true);


		}


        static int returnCountDirectoryGoogleDrive() {
            var resultInt = 0;

            try
            {

                resultInt = Convert.ToInt32(ReturnConsoleData("sudo find /media/GoogleDrive -maxdepth 1 -type d | wc -l", true));
				return resultInt;
            }

            catch(Exception e) {
                Console.WriteLine("cannot return integer for google drive: " +e.Message);
                logData(e.Message);
                return resultInt;
            }





        }


        static string ReturnConsoleData(string command, bool WaitToExit) {
			
			string result = "";
			using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
			{
				proc.StartInfo.FileName = "/bin/bash";
				proc.StartInfo.Arguments = "-c \" " + command + " \"";
                proc.StartInfo.UseShellExecute = false;
				proc.StartInfo.RedirectStandardOutput = true;
				proc.StartInfo.RedirectStandardError = true;
				proc.Start();

				result += proc.StandardOutput.ReadToEnd();
				result += proc.StandardError.ReadToEnd();

                if (WaitToExit)
                {

                    proc.WaitForExit();
                }
			}
            return ReplacenewLinesInString(result);

        }

        static string ReplacenewLinesInString(string result) {

            string replacement = Regex.Replace(result, @"\t|\n|\r", "");

            return replacement;

        }



		#region for Ark update run

        //write a function, check version of ark server, and then if it is higher than the one in the text file 
        //create updateprogress.dat
        //then update with steamcmd and then update the current version after upgrade on the textfile and then 
        //remove updateprogress.dat

        static void UpdateArk() {

            try
            {
                var InstallVersion = File.ReadLines("" + SERVERDIR + "/LatestInstalledUpdate").First();

                var LatestAvailableUpdate = GetLatestAvailableUpdate();

                if (File.Exists(SERVERDIR + "/updateinprogress.dat")) 
                {
                    Console.WriteLine("update in progress or delete the file "+SERVERDIR+"/updateinprogress.dat");
                    Environment.Exit(0);
                }

                else if (Convert.ToDouble(InstallVersion) < Convert.ToDouble(LatestAvailableUpdate) && !File.Exists(SERVERDIR + "/updateinprogress.dat"))
                {



                    Console.WriteLine("latest version is " + LatestAvailableUpdate + ", install version is: " + InstallVersion + "Server will now update");

                    ReturnConsoleData("touch " + SERVERDIR + "/updateinprogress.dat", true);


                    //broadcast message

                    Console.WriteLine(CurrTime + " :begin broadcast message");

                    ReturnConsoleData(""+arkrcon+" broadcast new Ark update is available. Get to a safe area and save your game. Server will go down in " + CountDownMinutes + " minutes. The new update will be version: v." + LatestAvailableUpdate + "", true);

                    messageTimer = new Timer();
                    messageTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                    messageTimer.Interval = 300000;
                    messageTimer.Enabled = true;

                }

                else
                {
                    Console.WriteLine("your version is up to date");
					Environment.Exit(0);
                }

            }

            catch (Exception e)
            {
                Console.WriteLine("Something is wrong: "+e.Message);
                logData(e.Message);
            }


		//	Console.ReadLine();

        }

        static string GetLatestAvailableUpdate() {

            string latestVersion = string.Empty;

            using(WebClient client = new WebClient()) {

                latestVersion = client.DownloadString("http://arkdedicated.com/version");
			}

            return latestVersion;

        }


        static void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            CountDownMinutes = CountDownMinutes - 5;

            if (DateTime.Now > CurrTime.AddMinutes(10) || CountDownMinutes <= 0)
            {
                Console.WriteLine("broadcast Server is going down now for an update");
                ReturnConsoleData(""+arkrcon+" broadcast Server is going down now for an update.", true);
                messageTimer.Stop();
                messageTimer.Dispose();



                //   Console.WriteLine("other actions here");

                SavingWorldUpdateStart();



          
            }

            else
            {
                //Console.WriteLine(e.SignalTime);
                Console.WriteLine("broadcast new Ark update is available. Get to a safe area and save your game. Server will go down in " + CountDownMinutes + " minutes.");

                ReturnConsoleData(""+arkrcon+" broadcast new Ark update is available. Get to a safe area and save your game. Server will go down in " + CountDownMinutes + " minutes.", true);
            }



            //throw new NotImplementedException();
        }




        static void SavingWorldUpdateStart() {


			ReturnConsoleData(""+arkrcon+" saveworld", true);
            System.Threading.Thread.Sleep(10000);


			ReturnConsoleData(""+arkrcon+" doexit", true);
            System.Threading.Thread.Sleep(60000);

            //doSteamUpdate

            ReturnConsoleData(STEAMCMDDIR+"/steamcmd.sh +login anonymous +force_install_dir \""+SERVERDIR+"\" +app_update 376030 +quit", true);

			//dete updateinprogress file
			if (File.Exists(SERVERDIR + "/updateinprogress.dat"))
			{
				File.Delete(SERVERDIR + "/updateinprogress.dat");
			}

			//write latest installedupdate to file
			File.WriteAllText(SERVERDIR + "/LatestInstalledUpdate", GetLatestAvailableUpdate());
            System.Threading.Thread.Sleep(10000);



            //start server backup
            ReturnConsoleData("screen -wipe", true);
			ReturnConsoleData("screen -dmS ark_start sudo " + SERVERDIR + "/ShooterGame/Binaries/Linux/ShooterGameServer Ragnarok?listen?QueryPort=27015?MaxPlayers=10?Port=7777 -servergamelog -server -log -UseBattleEye", true);

            System.Threading.Thread.Sleep(10000);
            Console.WriteLine("exiting Application");
            Environment.Exit(0);



          
        
        }





		#endregion region





		static void logData(string error) {

            File.WriteAllText(appPathError, error);

        }





    }
}
