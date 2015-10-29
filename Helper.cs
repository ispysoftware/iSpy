using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace iSpyApplication
{
    public static class Helper
    {
        public static double CalculateTrigger(double percent)
        {
            const double minimum = 0.00000001;
            const double maximum = 1;
            return minimum + ((maximum - minimum)/100)*Convert.ToDouble(percent);
        }

        public static DateTime Now => DateTime.UtcNow;

        public static bool HasFeature(Enums.Features feature)
        {
            return ((1L & FeatureSet) != 0) || (((long)feature & FeatureSet) != 0);
        }

        private static long FeatureSet
        {
            get
            {
                var o = MainForm.Conf.Permissions.FirstOrDefault(p => p.name == MainForm.Group);
                if (o == null)
                    return 1; //group missing - assign all permissions
                return o.featureset;
            }
        }
        public static string ZeroPad(int i)
        {
            if (i < 10)
                return "0" + i;
            return i.ToString(CultureInfo.InvariantCulture);
        }

        public static Dictionary<string, string> GetDictionary(string cfg, char delim)
        {
            var d = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(cfg))
            {
                var l = cfg.Split(delim);
                foreach (var t in l)
                {
                    var nv = t.Split('=');
                    if (nv.Length == 2)
                    {
                        if (!d.ContainsKey(nv[0]))
                            d.Add(nv[0], nv[1]);
                    }
                }
            }
            return d;
        }

        static public void CopyFolder(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                if (name != null)
                {
                    string dest = Path.Combine(destFolder, name);
                    File.Copy(file, dest, true);
                }
            }
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                if (name != null)
                {
                    string dest = Path.Combine(destFolder, name);
                    CopyFolder(folder, dest);
                }
            }
        }

        public static void SetTitle(Form f)
        {
            string ttl = $"iSpy v{Application.ProductVersion}";
            if (Program.Platform != "x86")
                ttl = $"iSpy 64 v{Application.ProductVersion}";

            if (MainForm.Conf.WSUsername != "")
            {
                ttl += $" ({MainForm.Conf.WSUsername})";
            }

            if (!string.IsNullOrEmpty(MainForm.Conf.Reseller))
            {
                ttl += $" Powered by {MainForm.Conf.Reseller.Split('|')[0]}";
            }
            else
            {
                if (!string.IsNullOrEmpty(MainForm.Conf.Vendor))
                {
                    ttl += $" with {MainForm.Conf.Vendor}";
                }
            }
            f.Text = ttl;
        }

        public static string GetMotionDataPoints(StringBuilder  motionData)
        {
            var elements = motionData.ToString().Trim(',').Split(',');
            if (elements.Length <= 1200)
                return String.Join(",", elements);
            
            var interval = (elements.Length / 1200d);
            var newdata = new StringBuilder(motionData.Length);
            var iIndex = 0;
            double dMax = 0;
            var tMult = 1;
            double target = 0;

            for(var i=0;i<elements.Length;i++)
            {
                try
                {
                    var dTemp = Convert.ToDouble(elements[i]);
                    if (dTemp > dMax)
                    {
                        dMax = dTemp;
                        iIndex = i;
                    }
                    if (i > target)
                    {
                        newdata.Append(elements[iIndex] + ",");
                        tMult++;
                        target = tMult*interval;
                        dMax = 0;

                    }
                }
                catch (Exception)
                {
                    //extremely long recordings can break
                    break;
                }
            }
            string r = newdata.ToString().Trim(',');
            newdata.Clear();
            return r;

        }

        internal static bool ArchiveFile(string filename)
        {

            if (!string.IsNullOrEmpty(MainForm.Conf.Archive) && Directory.Exists(MainForm.Conf.Archive))
            {
                string fn = filename.Substring(filename.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                if (File.Exists(filename))
                {
                    try
                    {
                        if (!File.Exists(MainForm.Conf.Archive + fn))
                            File.Copy(filename, MainForm.Conf.Archive + fn);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        MainForm.LogExceptionToFile(ex);
                    }
                }
            }
            return false;

        }

        internal static bool ArchiveAndDelete(string filename)
        {

            if (!string.IsNullOrEmpty(MainForm.Conf.Archive) && Directory.Exists(MainForm.Conf.Archive))
            {
                string fn = filename.Substring(filename.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                if (File.Exists(filename))
                {
                    try
                    {
                        if (!File.Exists(MainForm.Conf.Archive + fn))
                            File.Copy(filename, MainForm.Conf.Archive + fn);
                        File.Delete(filename);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        MainForm.LogExceptionToFile(ex);
                    }
                }
            }
            return false;

        }

        internal static string GetMediaDirectory(int ot, int oid)
        {
            int i = 0;
            switch (ot)
            {
                case 1:
                    {
                        var o = MainForm.Microphones.FirstOrDefault(p => p.id == oid);
                        if (o != null)
                            i = o.settings.directoryIndex;
                    }
                    break;
                case 2:
                    {
                        var o = MainForm.Cameras.FirstOrDefault(p => p.id == oid);
                        if (o != null)
                            i = o.settings.directoryIndex;
                    }
                    break;
            }
            var o2 = MainForm.Conf.MediaDirectories.FirstOrDefault(p => p.ID == i);
            if (o2 != null)
                return o2.Entry;
            return MainForm.Conf.MediaDirectories[0].Entry;
        }

        internal static string GetMediaDirectory(int directoryIndex)
        {
            var o2 = MainForm.Conf.MediaDirectories.FirstOrDefault(p => p.ID == directoryIndex);
            if (o2 != null)
                return o2.Entry;
            return MainForm.Conf.MediaDirectories[0].Entry;
        }

        public static string GetFullPath(int ot, int oid)
        {
            string d = GetMediaDirectory(ot, oid);
            if (!d.EndsWith("\\"))
                d += "\\";
            return  d+ (ot==1?"audio":"video")+"\\"+GetDirectory(ot, oid) + "\\";
        }

        public static string GetDirectory(int objectTypeId, int objectId)
        {
            if (objectTypeId == 1)
            {
                var m = MainForm.Microphones.SingleOrDefault(p => p.id == objectId);
                if (m != null)
                    return m.directory;
                throw new Exception("could not find directory for mic " + objectId);
            }
            var c = MainForm.Cameras.SingleOrDefault(p => p.id == objectId);
            if (c != null)
                return c.directory;
            throw new Exception("could not find directory for cam " + objectId);
        }

        public static void DeleteAllContent(int objectTypeId, int objectid)
        {
            var dir = GetMediaDirectory(objectTypeId, objectid);
            var dirName = GetDirectory(objectTypeId, objectid);
            if (objectTypeId == 1)
            {
                var lFi = new List<FileInfo>();
                var dirinfo = new DirectoryInfo(dir + "audio\\" +
                                              dirName + "\\");

                lFi.AddRange(dirinfo.GetFiles());
                lFi = lFi.FindAll(f => f.Extension.ToLower() == ".mp3");

                foreach (FileInfo fi in lFi)
                {
                    try
                    {
                        FileOperations.Delete(fi.FullName);
                    }
                    catch(Exception ex)
                    {
                        MainForm.LogExceptionToFile(ex);
                    }
                }

            }
            if (objectTypeId == 2)
            {
                var lFi = new List<FileInfo>();
                var dirinfo = new DirectoryInfo(dir + "video\\" +
                                              dirName + "\\");

                lFi.AddRange(dirinfo.GetFiles());
                lFi = lFi.FindAll(f => f.Extension.ToLower() == ".mp4" || f.Extension.ToLower() == ".avi");

                foreach (FileInfo fi in lFi)
                {
                    try
                    {
                        FileOperations.Delete(fi.FullName);
                    }
                    catch(Exception ex)
                    {
                        MainForm.LogExceptionToFile(ex);
                    }
                }
                System.Array.ForEach(Directory.GetFiles(dir + "video\\" +
                                              dirName + "\\thumbs\\"), delegate(string path)
                                              {
                                                  try
                                                  {
                                                      FileOperations.Delete(path);
                                                  }
                                                  catch
                                                  {
                                                      // ignored
                                                  }
                                              });

            }

        }
        // returns the number of milliseconds since Jan 1, 1970 (useful for converting C# dates to JS dates)
        public static double UnixTicks(this DateTime dt)
        {
            var d1 = new DateTime(1970, 1, 1);
            var d2 = dt.ToUniversalTime();
            var ts = new TimeSpan(d2.Ticks - d1.Ticks);
            return ts.TotalMilliseconds;
        }

        public static double UnixTicks(this long ticks)
        {
            var d1 = new DateTime(1970, 1, 1);
            var d2 = new DateTime(ticks);
            var ts = new TimeSpan(d2.Ticks - d1.Ticks);
            return ts.TotalMilliseconds;
        }


        public static bool CanAlert(string groupname, int resetInterval)
        {
            if (string.IsNullOrEmpty(groupname) || resetInterval == 0)
                return true;

            var ag = AlertGroups.FirstOrDefault(p => p.Name == groupname);
            if (ag == null)
            {
                ag = new AlertGroup(groupname);
                AlertGroups.Add(ag);
                return true;
            }
            if ((Now - ag.LastReset).TotalSeconds >= resetInterval)
            {
                ag.LastReset = Now;
                return true;
            }
            ag.LastReset = Now;
            return false;

        }

        public static readonly List<AlertGroup> AlertGroups = new List<AlertGroup>();

        public static MachineType GetDllMachineType(string dllPath)
        {
            //see http://www.microsoft.com/whdc/system/platform/firmware/PECOFF.mspx
            //offset to PE header is always at 0x3C
            //PE header starts with "PE\0\0" =  0x50 0x45 0x00 0x00
            //followed by 2-byte machine type field (see document above for enum)
            var fs = new FileStream(dllPath, FileMode.Open, FileAccess.Read);
            var br = new BinaryReader(fs);
            fs.Seek(0x3c, SeekOrigin.Begin);
            Int32 peOffset = br.ReadInt32();
            fs.Seek(peOffset, SeekOrigin.Begin);
            UInt32 peHead = br.ReadUInt32();
            if (peHead != 0x00004550) // "PE\0\0", little-endian
                throw new Exception("Can't find PE header");
            var machineType = (MachineType)br.ReadUInt16();
            br.Close();
            fs.Close();
            return machineType;
        }

        public enum MachineType : ushort
        {
            IMAGE_FILE_MACHINE_UNKNOWN = 0x0,
            IMAGE_FILE_MACHINE_AM33 = 0x1d3,
            IMAGE_FILE_MACHINE_AMD64 = 0x8664,
            IMAGE_FILE_MACHINE_ARM = 0x1c0,
            IMAGE_FILE_MACHINE_EBC = 0xebc,
            IMAGE_FILE_MACHINE_I386 = 0x14c,
            IMAGE_FILE_MACHINE_IA64 = 0x200,
            IMAGE_FILE_MACHINE_M32R = 0x9041,
            IMAGE_FILE_MACHINE_MIPS16 = 0x266,
            IMAGE_FILE_MACHINE_MIPSFPU = 0x366,
            IMAGE_FILE_MACHINE_MIPSFPU16 = 0x466,
            IMAGE_FILE_MACHINE_POWERPC = 0x1f0,
            IMAGE_FILE_MACHINE_POWERPCFP = 0x1f1,
            IMAGE_FILE_MACHINE_R4000 = 0x166,
            IMAGE_FILE_MACHINE_SH3 = 0x1a2,
            IMAGE_FILE_MACHINE_SH3DSP = 0x1a3,
            IMAGE_FILE_MACHINE_SH4 = 0x1a6,
            IMAGE_FILE_MACHINE_SH5 = 0x1a8,
            IMAGE_FILE_MACHINE_THUMB = 0x1c2,
            IMAGE_FILE_MACHINE_WCEMIPSV2 = 0x169,
        }

        public static bool UnmanagedDllIs64Bit(string dllPath)
        {
            try
            {
                switch (GetDllMachineType(dllPath))
                {
                    case MachineType.IMAGE_FILE_MACHINE_AMD64:
                    case MachineType.IMAGE_FILE_MACHINE_IA64:
                        return true;
                    case MachineType.IMAGE_FILE_MACHINE_I386:
                        return false;
                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public class AlertGroup
        {
            public DateTime LastReset;
            public readonly String Name;

            public AlertGroup(string name)
            {
                LastReset = Now;
                Name = name;
            }
        }

        #region Nested type: FrameAction

        public struct FrameAction
        {
            public byte[] Content;
            public int DataLength;
            public readonly double Level;
            public readonly DateTime TimeStamp;
            public readonly Enums.FrameType FrameType;

            public FrameAction(Bitmap frame, double level, DateTime timeStamp)
            {
                Level = level;
                TimeStamp = timeStamp;
                using (var ms = new MemoryStream())
                {
                    frame.Save(ms, MainForm.Encoder, MainForm.EncoderParams);
                    Content = ms.GetBuffer();
                }
                FrameType = Enums.FrameType.Video;
                DataLength = Content.Length;
            }

            public FrameAction(byte[] frame,  int bytesRecorded, double level, DateTime timeStamp)
            {
                Content = frame;
                Level = level;
                TimeStamp = timeStamp;
                FrameType = Enums.FrameType.Audio;
                DataLength = bytesRecorded;
            }

            public void Nullify()
            {
                Content = null;
                DataLength = 0;
            }

        }

       
        #endregion
    }
}