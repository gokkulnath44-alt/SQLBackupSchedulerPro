using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLBackupSchedulerPro.Services
{
    public class DiskSpaceService
    {
        public long GetFreeSpaceGB(string folderPath)
        {
            var root = Path.GetPathRoot(folderPath);
            var drive = new DriveInfo(root!);

            return drive.AvailableFreeSpace / (1024 * 1024 * 1024);
        }

        public bool HasEnoughSpace(string folderPath, long requiredGB)
        {
            return GetFreeSpaceGB(folderPath) >= requiredGB;
        }
    }
}
