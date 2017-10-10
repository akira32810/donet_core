- Ark and Google auto update

Note: I installed my Ark with sudo, but you do not have to. You may remove sudo from any part of the code.
I have also added dotnet as visosudo for my user (not root) to by able to run my script. You will need to install "dotnet core" to your server machine first of course. 

Google auto update (optional)

You may install google-drive-ocamlfuse to your linux distro and have it run with cronjob. You may comment ou tthe following code from below in Program.cs

---------------------

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


------------------------------
1. You must compile rcon first for Ark and before you compile the rcon.c file, you will need to change the password within it to use it with this app.  You will then change it in the source code in Program.cs the location to your compiled rcon file and the password you set.
2. Change the path in the code to your own preference and then use "dotnet publish [code directory]"
3. If you  want to run it in a schedule time like myself, set cron job (linux) to run "dotnet [/path/to/your/compile.dll] file after you run the command on step 2.
4. This should work on Linux, Mac and Windows. You will just have to adjust your path accordingly.
5. Shoot me an email at akira32810@gmail.com with any questions.

And yes, I know there are other methods to write this much more efficiently depending on your setup. But I am glad to take any advice.
