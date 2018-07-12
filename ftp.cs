using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using iSpyApplication.Utilities;
using Renci.SshNet;

namespace iSpyApplication
{
    public class AsynchronousFtpUpLoader
    {
        public bool FTP(string server, int port, bool passive, string username, string password, string filename, int counter, string path, out string error, bool rename, bool useSftp, byte[] contents)
        {
            bool failed = false;
            if (contents == null)
            {
                var fi = new FileInfo(path);
                contents = Helper.ReadBytesWithRetry(fi);
            }

            if (useSftp)
            {
                return Sftp(server, port, username, password, filename, counter, contents, out error, rename);
            }
            try
            {
                var urib = new UriBuilder(server);
                urib.Port = port;

                var target = urib.ToString();
                int i = 0;
                filename = filename.Replace("{C}", counter.ToString(CultureInfo.InvariantCulture));
                if (rename)
                    filename += ".tmp";

                while (filename.IndexOf("{", StringComparison.Ordinal) != -1 && i < 20)
                {
                    filename = String.Format(CultureInfo.InvariantCulture, filename, Helper.Now);
                    i++;
                }

                //try uploading
                //directory tree
                var filepath = filename.Trim('/').Split('/');
                var rpath = "";
                FtpWebRequest request;
                for (var iDir = 0; iDir < filepath.Length - 1; iDir++)
                {
                    rpath += filepath[iDir] + "/";
                    request = (FtpWebRequest)WebRequest.Create(target + rpath);
                    request.Credentials = new NetworkCredential(username, password);
                    request.Method = WebRequestMethods.Ftp.MakeDirectory;
                    try { request.GetResponse(); }
                    catch
                    {
                        //directory exists
                    }
                }

                request = (FtpWebRequest)WebRequest.Create(target + filename);
                request.Credentials = new NetworkCredential(username, password);
                request.UsePassive = passive;
                //request.UseBinary = true;
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.ContentLength = contents.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(contents, 0, contents.Length);
                requestStream.Close();

                var response = (FtpWebResponse)request.GetResponse();
                if (response.StatusCode != FtpStatusCode.ClosingData)
                {
                    Logger.LogError("FTP Failed: " + response.StatusDescription, "FTP");
                    failed = true;
                }

                response.Close();

                if (rename && !failed)
                {
                    //delete existing
                    request = (FtpWebRequest)WebRequest.Create(target + filename.Substring(0, filename.Length - 4));
                    request.Credentials = new NetworkCredential(username, password);
                    request.UsePassive = passive;
                    //request.UseBinary = true;
                    request.Method = WebRequestMethods.Ftp.DeleteFile;
                    filename = "/" + filename;

                    try
                    {
                        response = (FtpWebResponse)request.GetResponse();
                        if (response.StatusCode != FtpStatusCode.ActionNotTakenFileUnavailable &&
                            response.StatusCode != FtpStatusCode.FileActionOK)
                        {
                            Logger.LogError("FTP Delete Failed: " + response.StatusDescription, "FTP");
                            failed = true;
                        }

                        response.Close();
                    }
                    catch
                    {
                        //Logger.LogException(ex, "FTP");
                        //ignore
                    }

                    //rename file
                    if (!failed)
                    {
                        request = (FtpWebRequest)WebRequest.Create(target + filename);
                        request.Credentials = new NetworkCredential(username, password);
                        request.UsePassive = passive;
                        //request.UseBinary = true;
                        request.Method = WebRequestMethods.Ftp.Rename;
                        filename = "/" + filename;

                        request.RenameTo = filename.Substring(0, filename.Length - 4);

                        response = (FtpWebResponse)request.GetResponse();
                        if (response.StatusCode != FtpStatusCode.FileActionOK)
                        {
                            Logger.LogError("FTP Rename Failed: " + response.StatusDescription, "FTP");
                            failed = true;
                        }
                        response.Close();
                    }
                }
                if (!failed)
                {
                    Logger.LogMessage("FTP'd " + filename + " to " + server + ":" + port, "FTP");
                }
                error = failed ? "FTP Failed. Check Log" : "";
            }
            catch (Exception ex)
            {
                error = ex.Message;
                failed = true;
            }
            return !failed;
        }

        private bool Sftp(string server, int port, string username, string password, string filename, int counter, byte[] contents, out string error, bool rename)
        {
            bool failed = false;
            error = "";
            try
            {
                int i = 0;
                filename = filename.Replace("{C}", counter.ToString(CultureInfo.InvariantCulture));
                if (rename)
                    filename += ".tmp";

                while (filename.IndexOf("{", StringComparison.Ordinal) != -1 && i < 20)
                {
                    filename = String.Format(CultureInfo.InvariantCulture, filename, Helper.Now);
                    i++;
                }

                var methods = new List<AuthenticationMethod> { new PasswordAuthenticationMethod(username, password) };

                var con = new ConnectionInfo(server, port, username, methods.ToArray());
                using (var client = new SftpClient(con))
                {
                    client.Connect();

                    var filepath = filename.Trim('/').Split('/');
                    var path = "";
                    for (var iDir = 0; iDir < filepath.Length - 1; iDir++)
                    {
                        path += filepath[iDir] + "/";
                        try
                        {
                            client.CreateDirectory(path);
                        }
                        catch
                        {
                            //directory exists
                        }
                    }
                    if (path != "")
                    {
                        client.ChangeDirectory(path);
                    }

                    filename = filepath[filepath.Length - 1];

                    using (Stream stream = new MemoryStream(contents))
                    {
                        client.UploadFile(stream, filename);
                        if (rename)
                        {
                            try
                            {
                                //delete target file?
                                client.DeleteFile(filename.Substring(0, filename.Length - 4));
                            }
                            catch (Exception)
                            {
                                // ignored
                            }
                            client.RenameFile(filename, filename.Substring(0, filename.Length - 4));
                        }
                    }

                    client.Disconnect();
                }


                Logger.LogMessage("SFTP'd " + filename + " to " + server + " port " + port, "SFTP");
            }
            catch (Exception ex)
            {
                error = ex.Message;
                failed = true;
            }
            return !failed;
        }

        public void FTP(object taskObject)
        {
            var task = (FTPTask)taskObject;
            int i = 0;
            while (task.FileName.IndexOf("{", StringComparison.Ordinal) != -1 && i < 20)
            {
                task.FileName = String.Format(CultureInfo.InvariantCulture, task.FileName, Helper.Now);
                i++;
            }
            string error;
            FTP(task.Server, task.Port, task.UsePassive, task.Username, task.Password, task.FileName, task.Counter, task.Path, out error, task.Rename, task.UseSftp, task.Content);

            if (error != "")
            {
                Logger.LogError(error, "FTP");
            }

            objectsCamera oc = MainForm.Cameras.SingleOrDefault(p => p.id == task.CameraId);
            if (oc != null)
            {
                oc.ftp.ready = true;
            }
        }

    }

    public struct FTPTask
    {
        public int CameraId;
        public string Path;
        public string FileName;
        public bool IsError;
        public string Password;
        public string Server;
        public bool UsePassive;
        public string Username;
        public int Counter;
        public bool Rename;
        public bool UseSftp;
        public int Port;
        public byte[] Content;

        public FTPTask(string server, int port, bool usePassive, string username, string password, string fileName,
                       string path, int cameraId, int counter, bool rename, bool useSftp, byte[] content = null)
        {
            Server = server;
            Port = port;
            UsePassive = usePassive;
            Username = username;
            Password = password;
            FileName = fileName;
            Path = path;
            CameraId = cameraId;
            IsError = false;
            Counter = counter;
            Rename = rename;
            UseSftp = useSftp;
            Content = content;
        }
    }
}